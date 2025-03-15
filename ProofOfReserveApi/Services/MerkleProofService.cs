using MerkleCalculator.Services;
using ProofOfReserveApi.Models;
using System;

namespace ProofOfReserveApi.Services;

public class MerkleProofService : IMerkleProofService
{
    private const int UsedThreadsCount = 4;
    private const string LeafTag = "ProofOfReserve_Leaf";
    private const string BranchTag = "ProofOfReserve_Branch";

    private readonly MerkleCalculatorService merkleCalculatorService;
    private readonly IUserBalanceStorage storage;
    private readonly ILogger logger;

    public MerkleProofService(MerkleCalculatorService merkleCalculatorService, IUserBalanceStorage storage, ILogger<MerkleProofService> logger)
    {
        this.merkleCalculatorService = merkleCalculatorService;
        this.storage = storage;
        this.logger = logger;
    }

    public async Task<ProofOfReserveApiModel?> GetMerkleProof(int userId)
    {
        var userBalance = storage.GetUser(userId);
        if(userBalance == null)
        {
            logger.LogWarning("Trying to calculate proof for user {0}, but this user not exists.", userId);
            return null;
        }

        var userBalances = storage.GetAllUsers().Select(u => u.ToString()).ToArray();
        var userIndex = Array.IndexOf(userBalances, userBalance.ToString());
        if (userIndex == -1)
        {
            logger.LogError("User {0} serialized as {1} index was not found in users array.", userId, userBalance.ToString());
            return null;
        }

        try
        {
            var proofData = await merkleCalculatorService.GetMerkleProofAsync(userBalances, userIndex, LeafTag, BranchTag, UsedThreadsCount);
            var proofPath = proofData.Path.Select(p => SerializeMerklePathItem(p)).ToList();
            return new(userId, userBalance.Balance, userBalance.ToString(), proofPath, proofData.MerkleRoot);
        }
        catch (Exception ex) 
        {
            logger.LogError(ex, "Failed to calculate merkle proof for user {0}", userId);
            return null;
        }

    }

    public async Task<MerkleRootValue?> GetMerkleRoot()
    {
        var userBalances = storage.GetAllUsers().Select(u => u.ToString()).ToArray();
        if (!userBalances.Any()) 
        {
            logger.LogWarning("No users in the database. Can not calculate merkle root.");
            return null;
        }

        try
        {
            var rootValue = await merkleCalculatorService.GetMerkleRootAsync(userBalances, LeafTag, BranchTag, UsedThreadsCount);
            return new(rootValue);
        }
        catch (Exception ex) 
        {
            logger.LogError(ex, "Failed to calculate merkle root for users database.");
            return null;
        }
    }

    private string SerializeMerklePathItem(MerkleProofItem merkleProofItem)
    {
        string serializedDirectionByte = merkleProofItem.IsRight ? "1" : "0";
        return $"({merkleProofItem.HashValue},{serializedDirectionByte})";
    }
}
