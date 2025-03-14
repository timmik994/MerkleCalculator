using MerkleCalculator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleCalculatorTest;

[TestClass]
public class MerkleCalculatorServiceTests
{
    [TestMethod]
    public void CalculateRootOnArrayTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string tag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        var results = service.GetMerkleRoot(elements, tag, tag);
        Assert.IsNotNull(results);
        Assert.AreEqual("4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5", "4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5");
    }

    [TestMethod]
    public void ElementsEmptyTest()
    {
        string[] elements = new string[0];
        string tag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleRoot(elements, tag, tag));
    }

    [TestMethod]
    public void LeafTagEmptyTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "";
        string branchTag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleRoot(elements, leafTag, branchTag));
    }

    [TestMethod]
    public void BranchTagEmpty()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "Bitcoin_Transaction";
        string branchTag = "";

        var service = new MerkleCalculatorService();

        Assert.ThrowsException<ArgumentException>(() => service.GetMerkleRoot(elements, leafTag, branchTag));
    }

    [TestMethod]
    public async Task AsyncCalculationTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string tag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        var results = await service.GetMerkleRootAsync(elements, tag, tag, 4);
        Assert.IsNotNull(results);
        Assert.AreEqual("4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5", "4AA906745F72053498ECC74F79813370A4FE04F85E09421DF2D5EF760DFA94B5");
    }

    [TestMethod]
    public async Task AsyncMethodElementsEmptyTest()
    {
        string[] elements = new string[0];
        string tag = "Bitcoin_Transaction";

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await service.GetMerkleRootAsync(elements, tag, tag, 4));
    }

    [TestMethod]
    public async Task AsyncCalculationLeafTagEmptyTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "Bitcoin_Transaction";
        string branchTag = "";

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await service.GetMerkleRootAsync(elements, leafTag, branchTag, 4));
    }

    [TestMethod]
    public async Task AsyncCalculationBranchTagEmptyTest()
    {
        string[] elements = { "aaa", "bbb", "ccc", "ddd", "eee" };
        string leafTag = "Bitcoin_Transaction";
        string branchTag = "";

        var service = new MerkleCalculatorService();

        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await service.GetMerkleRootAsync(elements, leafTag, branchTag, 4));
    }
}
