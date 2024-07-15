using Reporting.Core.Entities;

namespace Reporting.Core.IService
{
    public interface IReportService
    {
        Task InitializeAndCreateReportAsync(Report newReport);
        Task<Report?> GetAsync(string id);
        Task<List<Report>> GetListAsync();
        Task RemoveAsync(string id);
        Task UpdateAsync(string id, Report updatedReport);
        Task<List<Report>> GetReportsByExecutionIdAsync(string executionId);
        Task<string> ValidateAndSaveReportAsync(string reportId);
        Task<List<ValidationResult>> GetValidationResultsByReportIdAsync(string reportId);
    }
}
