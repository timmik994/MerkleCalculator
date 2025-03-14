using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleCalculator.Services;

interface IMerkleCalculationsService
{
    string GetMerkleRoot(string[] elements, string leaftag, string branchTag);

    Task<string> GetMerkleRootAsync(string[] elements, string leaftag, string branchTag, int threadCount = 4);
}
