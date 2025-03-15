using ProofOfReserveApi.Models;

namespace ProofOfReserveApi.Services;

/// <summary>
/// Service that handles calculation of merkle values from users storage.
/// </summary>
public interface IMerkleProofService
{
    /// <summary>
    /// Gets merkle root from all users in database.
    /// </summary>
    /// <returns>The calculated merkle root.</returns>
    Task<MerkleRootApiModel?> GetMerkleRoot();

    /// <summary>
    /// Calculates merkle proof for specific user.
    /// </summary>
    /// <returns>The merkle proof data.</returns>
    Task<ProofOfReserveApiModel?> GetMerkleProof(int userId);
}
