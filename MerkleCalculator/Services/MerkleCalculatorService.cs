﻿using MerkleCalculator.Helpers;
using System.Threading.Tasks.Dataflow;

namespace MerkleCalculator.Services;

public class MerkleCalculatorService : IMerkleCalculationsService
{
    public string GetMerkleRoot(string[] elements, string leaftag, string branchTag)
    {
        ValidateMerkleRootParameters(elements, leaftag, branchTag);

        var treeNodes = CalculateLeafs(elements, leaftag);

        while (treeNodes.Count > 1)
        {
            treeNodes = CalculateNextTreeLevel(treeNodes, branchTag);
        }

        return ConvertHelper.ShowHashAsString(treeNodes.First());
    }

    public async Task<string> GetMerkleRootAsync(string[] elements, string leaftag, string branchTag, int threadCount)
    {
        ValidateMerkleRootParameters(elements, leaftag, branchTag);

        var treeNodes = await CalculateLeafsAsync(elements, leaftag, threadCount);

        while (treeNodes.Count > 1)
        {
            treeNodes = await CalculateNextTreeLevelAsync(treeNodes, branchTag, threadCount);
        }

        return ConvertHelper.ShowHashAsString(treeNodes.First());
    }

    public MerkleProofData GetMerkleProof(string[] elements, int targetElementIndex, string leaftag, string branchTag)
    {
        ValidateMerkleProofParameters(elements, targetElementIndex, leaftag, branchTag);
        var treeNodes = CalculateLeafs(elements, leaftag);

        List<MerkleProofItem> merkleProofPath = new()
        {
            CreateProofItem(targetElementIndex, treeNodes)
        };

        targetElementIndex = targetElementIndex / 2;
        while (treeNodes.Count > 1)
        {
            treeNodes = CalculateNextTreeLevel(treeNodes, branchTag);

            if (treeNodes.Count == 1)
            {
                //We do not merkle root in merkle proof path. it will be returned separetly.
                break;
            }

            var proofPathItem = CreateProofItem(targetElementIndex, treeNodes);
            merkleProofPath.Add(proofPathItem);
            targetElementIndex = targetElementIndex / 2;
        }

        return new(merkleProofPath, ConvertHelper.ShowHashAsString(treeNodes.First()));
    }

    public async Task<MerkleProofData> GetMerkleProofAsync(string[] elements, int targetElementIndex, string leaftag, string branchTag, int threadCount = 4)
    {
        ValidateMerkleProofParameters(elements, targetElementIndex, leaftag, branchTag);
        var treeNodes = await CalculateLeafsAsync(elements, leaftag, threadCount);

        List<MerkleProofItem> merkleProofPath = new()
        {
            CreateProofItem(targetElementIndex, treeNodes)
        };

        targetElementIndex = targetElementIndex / 2;
        while (treeNodes.Count > 1)
        {
            treeNodes = await CalculateNextTreeLevelAsync(treeNodes, branchTag, threadCount);

            if (treeNodes.Count == 1)
            {
                //We do not merkle root in merkle proof path. it will be returned separetly.
                break;
            }

            var proofPathItem = CreateProofItem(targetElementIndex, treeNodes);
            merkleProofPath.Add(proofPathItem);
            targetElementIndex = targetElementIndex / 2;
        }

        return new(merkleProofPath, ConvertHelper.ShowHashAsString(treeNodes.First()));
    }

    private void ValidateMerkleProofParameters(string[] elements, int targetElementIndex, string leaftag, string branchTag)
    {
        if (!elements.Any())
        {
            throw new ArgumentException("elements Array can not be empty");
        }

        if (targetElementIndex >= elements.Length)
        {
            throw new ArgumentException("Target index is out of range");
        }

        if (string.IsNullOrEmpty(leaftag))
        {
            throw new ArgumentException("You need to provide tag to create hash for leaves");
        }

        if (string.IsNullOrEmpty(branchTag))
        {
            throw new ArgumentException("You need to provide hash for branches");
        }
    }

    private void ValidateMerkleRootParameters(string[] elements, string leaftag, string branchTag)
    {
        if (!elements.Any())
        {
            throw new ArgumentException("elements Array can not be empty");
        }

        if (string.IsNullOrEmpty(leaftag))
        {
            throw new ArgumentException("You need to provide tag to create hash for leaves");
        }

        if (string.IsNullOrEmpty(branchTag))
        {
            throw new ArgumentException("You need to provide hash for branches");
        }
    }

    private MerkleProofItem CreateProofItem(int targetIndex, List<byte[]> elements)
    {
        bool isElementRight = targetIndex % 2 == 1;

        if (isElementRight)
        {
            return new(false, ConvertHelper.ShowHashAsString(elements[targetIndex - 1]));
        }
        else
        {
            var pathElement = targetIndex + 1 >= elements.Count ? elements[targetIndex] : elements[targetIndex + 1];
            return new(true, ConvertHelper.ShowHashAsString(pathElement));
        }
    }

    private List<byte[]> CalculateNextTreeLevel(List<byte[]> elements, string branchTag)
    {
        List<byte[]> resultNodes = new();
        for (int i = 0; i < elements.Count; i += 2)
        {
            byte[] leftNode = elements[i];
            byte[]? rightNode = i + 1 < elements.Count ? elements[i + 1] : null;
            byte[] parentHash = CalculateTreeBranch(leftNode, rightNode, branchTag);
            resultNodes.Add(parentHash);
        }

        return resultNodes;
    }

    private byte[] CalculateTreeBranch(byte[] leftNode, byte[]? rightNode, string branchTag)
    {
        byte[] inputString = rightNode != null
            ? leftNode.Concat(rightNode).ToArray()
            : leftNode.Concat(leftNode).ToArray();

        return HashHelper.CalculateTaggedHash(inputString, branchTag);
    }

    private List<byte[]> CalculateLeafs(string[] elements, string leafTag)
    {
        return elements.Select(el => HashHelper.CalculateTaggedHash(el, leafTag)).ToList();
    }

    private async Task<List<byte[]>> CalculateLeafsAsync(string[] elements, string leafTag, int maxThreadsCount)
    {
        ExecutionDataflowBlockOptions blockOptions = new()
        {
            EnsureOrdered = true,
            MaxDegreeOfParallelism = maxThreadsCount
        };

        TransformBlock<string, byte[]> calculateHashBlock = new(el => HashHelper.CalculateTaggedHash(el, leafTag), blockOptions);
        BufferBlock<byte[]> results = new BufferBlock<byte[]>();
        calculateHashBlock.LinkTo(results);

        foreach (var element in elements)
        {
            calculateHashBlock.Post(element);
        }

        calculateHashBlock.Complete();
        await calculateHashBlock.Completion.ConfigureAwait(false);

        bool received = results.TryReceiveAll(out IList<byte[]>? leafs);

        if (!received)
        {
            throw new Exception("Leafs calculations failed due to block error");
        }

        return leafs!.ToList();
    }

    private async Task<List<byte[]>> CalculateNextTreeLevelAsync(List<byte[]> elements, string branchTag, int maxThreadsCount)
    {
        ExecutionDataflowBlockOptions blockOptions = new()
        {
            EnsureOrdered = true,
            MaxDegreeOfParallelism = maxThreadsCount
        };

        BatchBlock<byte[]> batchBlock = new BatchBlock<byte[]>(2);
        TransformBlock<byte[][], byte[]> calculateHashBlock =
            new TransformBlock<byte[][], byte[]>(el => CalculateTreeBranch(el[0], el.Length > 1 ? el[1] : null, branchTag), blockOptions);
        BufferBlock<byte[]> results = new();

        batchBlock.LinkTo(calculateHashBlock, new DataflowLinkOptions() { PropagateCompletion = true });
        calculateHashBlock.LinkTo(results);

        foreach (var element in elements)
        {
            batchBlock.Post(element);
        }
        batchBlock.Complete();
        await calculateHashBlock.Completion;

        bool received = results.TryReceiveAll(out IList<byte[]>? leafs);

        if (!received)
        {
            throw new Exception("Leafs calculations failed due to block error");
        }

        return leafs!.ToList();
    }
}
