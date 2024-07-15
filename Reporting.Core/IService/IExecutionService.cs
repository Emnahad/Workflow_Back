using Reporting.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reporting.Core.IService
{
    public interface IExecutionService
    {
        Task InitializeAndCreateExecutionAsync(Execution newExecution);
        Task<Execution?> GetAsync(string id);
        Task<List<Execution>> GetListAsync();
        Task RemoveAsync(string id);
        Task UpdateAsync(string id, Execution updatedExecution);
    }
}
