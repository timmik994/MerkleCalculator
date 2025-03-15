namespace ProofOfReserveApi.Models;

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

    public int UserId {  get; set; }
    public int UserBalance {  get; set; }

    public string InitialLeafValue {  get; set; }

    public List<string> MerklePath { get; set; }

    public string MerkleRoot {  get; set; }
}
