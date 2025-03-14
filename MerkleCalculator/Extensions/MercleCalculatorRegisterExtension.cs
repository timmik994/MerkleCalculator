using MerkleCalculator.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleCalculator.Extensions;

/// <summary>
/// Extension to add merkle calculations to service collection
/// </summary>
public static class MercleCalculatorRegisterExtension
{
    /// <summary>
    /// Registers merkle calculation service to service collection.
    /// </summary>
    public static void  AddMerkleCalculationService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IMerkleCalculationsService, MerkleCalculatorService>();
    }
}
