namespace ProofOfReserveApi.Models;

public class MerkleRootValue
{
    public MerkleRootValue(string merkleRoot)
    {
        MerkleRoot = merkleRoot;
    }

    public string MerkleRoot {  get; set; }
}
