using System.Threading.Tasks;
using Dapr.Actors;
using DotNetBa.Dapr.Common.Models;

namespace DotNetBa.Dapr.Common
{
    public interface IFancyActor : IActor
    {
        Task<FancyActorState> PerformComplexCalculation(FancyActorState state);
    }
}
