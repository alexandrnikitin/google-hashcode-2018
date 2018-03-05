using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Rides.GrainInterfaces
{
    public interface INodeGrain : IGrainWithGuidKey
    {
        Task Build();
        Task<IEnumerable<INodeGrain>> GetChildren();
    }
}