using Microsoft.AspNetCore.Mvc;
using Reporting.Application.Services;
using Reporting.Core.Entities;
using Reporting.Core.IService;


namespace ReportingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService) =>
            _reportService = reportService;
        [HttpGet]
        public async Task<List<Report>> GetList() =>
            await _reportService.GetListAsync();
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> Get(string id)
        {
            var report = await _reportService.GetAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }
        [HttpPost]
        public async Task<ActionResult<Report>> Create(Report report)
        {
            await _reportService.InitializeAndCreateReportAsync(report);
            return CreatedAtAction(nameof(Get), new { id = report._id }, report);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var report = await _reportService.GetAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            await _reportService.RemoveAsync(id);
            return NoContent();
        }
        
        [HttpPut("{id}")]
       
        public async Task<IActionResult> Update(string id, [FromBody] Report updatedReport)
        {
            var existingReport = await _reportService.GetAsync(id);
            if (existingReport == null)
            {
                return NotFound();
            }

            existingReport.TBId = updatedReport.TBId;
            existingReport.Ticket = updatedReport.Ticket;
            existingReport.Reason = updatedReport.Reason;
            existingReport.Description = updatedReport.Description;
            existingReport.TesterComment = updatedReport.TesterComment;

            await _reportService.UpdateAsync(id, existingReport);
            return NoContent();
        }
   
    [HttpGet("byexecution/{executionId}")]
        public async Task<ActionResult<List<Report>>> GetByExecutionId(string executionId)
        {
            var reports = await _reportService.GetReportsByExecutionIdAsync(executionId);
            return Ok(reports);
        }

        [HttpGet("validate/{id}")]
        public async Task<ActionResult<string>> ValidateReport(string id)
        {
            var result = await _reportService.ValidateAndSaveReportAsync(id);
            return Ok(result);
        }

        [HttpGet("validationResults/{reportId}")]
        public async Task<ActionResult<List<ValidationResult>>> GetValidationResults(string reportId)
        {
            var results = await _reportService.GetValidationResultsByReportIdAsync(reportId);
            return Ok(results);
        }

    }
    
}
