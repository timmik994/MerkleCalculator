namespace ProofOfReserveApi.Models;

/// <summary>
/// The API about user reserve data.
/// </summary>
public class ProofOfReserveApiModel
{
    public ProofOfReserveApiModel(int userId, int userBalance, string initialLeafValue, List<string> merklePath, string merkleRoot)
    {
        UserId = userId;
        UserBalance = userBalance;
        InitialLeafValue = initialLeafValue;
        MerklePath = merklePath;
        MerkleRoot = merkleRoot;
    }

    /// <summary>
    /// The user ID.
    /// </summary>
    public int UserId {  get; set; }

    /// <summary>
    /// The user balance.
    /// </summary>
    public int UserBalance {  get; set; }

    /// <summary>
    /// The initial user string used to calculate hash.
    /// </summary>
    public string InitialLeafValue {  get; set; }

    /// <summary>
    /// The The merkle proof.
    /// </summary>
    public List<string> MerklePath { get; set; }

    /// <summary>
    /// The merkle root value.
    /// </summary>
    public string MerkleRoot {  get; set; }
}
