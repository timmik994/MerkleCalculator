namespace ProofOfReserveApi.Models;

public class UserBalance
{
    public UserBalance(int id, int balance)
    {
        Id = id;
        Balance = balance;
    }

    public int Id {  get; set; }
    public int Balance {  get; set; }

    public override string ToString()
    {
        return $"({Id},{Balance})"; 
    }
}
