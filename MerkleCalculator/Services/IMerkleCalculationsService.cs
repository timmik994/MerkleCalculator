namespace MerkleCalculator.Services;

/// <summary>
/// Service that calculates merkle tree related values.
/// </summary>
public interface IMerkleCalculationsService
{
    /// <summary>
    /// Gets merkle root.
    /// </summary>
    /// <param name="elements">The leaf values array.</param>
    /// <param name="leaftag">The tag to use for leaf hash</param>
    /// <param name="branchTag">The tag to use for branch hash</param>
    /// <returns></returns>
    string GetMerkleRoot(string[] elements, string leaftag, string branchTag);

    /// <summary>
    /// Gets merkle root using multiple threads.
    /// </summary>
    /// <param name="elements">The leaf values array.</param>
    /// <param name="leaftag">The tag to use for leaf hash</param>
    /// <param name="branchTag">The tag to use for branch hash</param>
    /// <param name="threadCount">The count of threads we can use. Default: 4</param>
    /// <returns></returns>
    Task<string> GetMerkleRootAsync(string[] elements, string leaftag, string branchTag, int threadCount = 4);

    /// <summary>
    /// Gets merkle proof for specific array element.
    /// </summary>
    /// <param name="elements">The leafs data.</param>
    /// <param name="targetElementIndex">The index of the element we want to proof for.</param>
    /// <param name="leaftag">The leaf tag.</param>
    /// <param name="branchTag">The branch tag.</param>
    /// <returns>The merkle proof data.</returns>
    MerkleProofData GetMerkleProof(string[] elements, int targetElementIndex, string leaftag, string branchTag);

    /// <summary>
    /// Gets merkle proof and merkle root using multiple threads.
    /// </summary>
    /// <param name="elements">The leafs data.</param>
    /// <param name="targetElementIndex">The index of the element we want to proof for.</param>
    /// <param name="leaftag">The leaf tag.</param>
    /// <param name="branchTag">The branch tag.</param>
    /// <param name="threadCount">The count of threads we can use. Default: 4</param>
    /// <returns></returns>
    Task<MerkleProofData> GetMerkleProofAsync(string[] elements, int targetElementIndex, string leaftag, string branchTag, int threadCount = 4);
}

public record MerkleProofItem(bool IsRight, string HashValue);

public record MerkleProofData(List<MerkleProofItem> Path, string MerkleRoot);
