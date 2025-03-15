namespace ProofOfReserveApi.Models;

public class MerkleRootApiModel
{
    public MerkleRootApiModel(string merkleRoot)
    {
        MerkleRoot = merkleRoot;
    }

    public string MerkleRoot {  get; set; }
}
