namespace ProofOfReserveApi.Models;

public class ProofOfReserveApiModel
{
    public ProofOfReserveApiModel(string userBalance, List<string> merklePath, string merkleRoot)
    {
        UserBalance = userBalance;
        MerklePath = merklePath;
        MerkleRoot = merkleRoot;
    }

    public string UserBalance {  get; set; }

    public List<string> MerklePath { get; set; }

    public string MerkleRoot {  get; set; }
}
