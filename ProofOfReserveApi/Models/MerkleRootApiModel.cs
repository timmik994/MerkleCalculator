namespace ProofOfReserveApi.Models;

/// <summary>
/// The API model with merkle root.
/// </summary>
public class MerkleRootApiModel
{
    public MerkleRootApiModel(string merkleRoot)
    {
        MerkleRoot = merkleRoot;
    }

    /// <summary>
    /// The merkle root value.
    /// </summary>
    public string MerkleRoot {  get; set; }
}
