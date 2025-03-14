using MerkleCalculator.Helpers;
using MerkleCalculator.Services;

namespace MerkleCalculatorTest;

[TestClass]
public class MerkleCalculatorServiceTests
{
    [TestMethod]
    public void GetMerkleRootOnArrayTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string tag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        var results = service.GetMerkleRoot(elements, tag, tag);
        Assert.IsNotNull(results);
        Assert.AreEqual("4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5", "4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5");
    }

    [TestMethod]
    public void GetMerkleRootElementsEmptyTest()
    {
        string[] elements = new string[0];
        string tag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleRoot(elements, tag, tag));
    }

    [TestMethod]
    public void GetMerkleRootLeafTagEmptyTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "";
        string branchTag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleRoot(elements, leafTag, branchTag));
    }

    [TestMethod]
    public void GetMerkleRootBranchTagEmpty()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "Bitcoin_Transaction";
        string branchTag = "";

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleRoot(elements, leafTag, branchTag));
    }

    [TestMethod]
    public async Task GetMerkleRootAsyncCalculationTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string tag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        var result = await service.GetMerkleRootAsync(elements, tag, tag, 4);
        Assert.IsNotNull(result);
        Assert.AreEqual("4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5", result);
    }

    [TestMethod]
    public async Task GetMerkleRootAsyncElementsEmptyTest()
    {
        string[] elements = new string[0];
        string tag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await service.GetMerkleRootAsync(elements, tag, tag, 4));
    }

    [TestMethod]
    public async Task GetMerkleRootAsyncLeafTagEmptyTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "Bitcoin_Transaction";
        string branchTag = "";

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await service.GetMerkleRootAsync(elements, leafTag, branchTag, 4));
    }

    [TestMethod]
    public async Task GetMerkleRootAsyncBranchTagEmptyTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "Bitcoin_Transaction";
        string branchTag = "";

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await service.GetMerkleRootAsync(elements, leafTag, branchTag, 4));
    }

    [TestMethod]
    public void GetMerkleProofTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string tag = "Bitcoin_Transaction";
        int targetIndex = 1;

        var service = new MerkleCalculatorService();

        var merkleProof = service.GetMerkleProof(elements, targetIndex, tag, tag);

        Assert.AreEqual("4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5", merkleProof.MerkleRoot);
        Assert.IsTrue(ValidateMerkleProof(elements[targetIndex], tag, tag, merkleProof));
    }

    [TestMethod]
    public void GetMerkleProofElementsEmptyTest()
    {
        string[] elements = new string[0];
        string tag = "Bitcoin_Transaction";
        int targetIndex = 0;

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleProof(elements, targetIndex, tag, tag));
    }

    [TestMethod]
    public void GetMerkleProofTargetElementOutOfRangeTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "";
        string branchTag = "Bitcoin_Transaction";
        int targetIndex = elements.Length; //minimal out of range index.

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleProof(elements, targetIndex, leafTag, branchTag));
    }

    [TestMethod]
    public void GetMerkleProofLeafTagEmptyTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "";
        string branchTag = "Bitcoin_Transaction";
        int targetIndex = 0;

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleProof(elements, targetIndex, leafTag, branchTag));
    }

    [TestMethod]
    public void GetMerkleProofBranchTagEmpty()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "Bitcoin_Transaction";
        string branchTag = "";
        int targetIndex = 0;

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleProof(elements, targetIndex, leafTag, branchTag));
    }

    [TestMethod]
    public async Task GetMerkleProofAsyncTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string tag = "Bitcoin_Transaction";
        int targetIndex = 1;

        var service = new MerkleCalculatorService();

        var merkleProof = await service.GetMerkleProofAsync(elements, targetIndex, tag, tag);

        Assert.AreEqual("4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5", merkleProof.MerkleRoot);
        Assert.IsTrue(ValidateMerkleProof(elements[targetIndex], tag, tag, merkleProof));
    }

    [TestMethod]
    public async Task GetMerkleProofAsyncElementsEmptyTest()
    {
        string[] elements = new string[0];
        string tag = "Bitcoin_Transaction";
        int targetIndex = 0;

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(() => service.GetMerkleProofAsync(elements, targetIndex, tag, tag));
    }

    [TestMethod]
    public async Task GetMerkleProofAsyncTargetElementOutOfRangeTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "";
        string branchTag = "Bitcoin_Transaction";
        int targetIndex = elements.Length; //minimal out of range index.

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(() => service.GetMerkleProofAsync(elements, targetIndex, leafTag, branchTag));
    }

    [TestMethod]
    public async Task GetMerkleProofAsyncLeafTagEmptyTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "";
        string branchTag = "Bitcoin_Transaction";
        int targetIndex = 0;

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(() => service.GetMerkleProofAsync(elements, targetIndex, leafTag, branchTag));
    }

    [TestMethod]
    public async Task GetMerkleProofAsyncBranchTagEmpty()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "Bitcoin_Transaction";
        string branchTag = "";
        int targetIndex = 0;

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(() => service.GetMerkleProofAsync(elements, targetIndex, leafTag, branchTag));
    }

    private bool ValidateMerkleProof(string initialValue, string leafTag, string branchTag, MerkleProofData merkleProofData)
    {
        byte[] hashBytes = HashHelper.CalculateTaggedHash(initialValue, leafTag);
        
        foreach(var merklePathItem in merkleProofData.Path)
        {
            byte[] nextHashInput = merklePathItem.IsRight
                ? hashBytes.Concat(Convert.FromHexString(merklePathItem.HashValue)).ToArray()
                : Convert.FromHexString(merklePathItem.HashValue).Concat(hashBytes).ToArray();

            hashBytes = HashHelper.CalculateTaggedHash(nextHashInput, branchTag);
        }

        string calculatedRoot = Convert.ToHexString(hashBytes);
        return string.Equals(calculatedRoot, merkleProofData.MerkleRoot);
    }
}
