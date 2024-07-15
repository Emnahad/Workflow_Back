using Microsoft.AspNetCore.Mvc;
using Reporting.Application.Services;
using Reporting.Core.Entities;
using Reporting.Core.IService;

namespace ReportingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : ControllerBase
    {
        private readonly IExecutionService _executionService;

        public ExecutionController(IExecutionService executionService)
        {
            _executionService = executionService;
        }

        [HttpGet]
        public async Task<List<Execution>> GetList() =>
            await _executionService.GetListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Execution>> Get(string id)
        {
            var execution = await _executionService.GetAsync(id);
            if (execution == null)
            {
                return NotFound();
            }
            return Ok(execution);
        }

        [HttpPost]
        public async Task<ActionResult<Execution>> Create(Execution execution)
        {
            await _executionService.InitializeAndCreateExecutionAsync(execution);
            return CreatedAtAction(nameof(Get), new { id = execution.Id }, execution);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var execution = await _executionService.GetAsync(id);
            if (execution == null)
            {
                return NotFound();
            }

            await _executionService.RemoveAsync(id);
            return NoContent();
        }
        /*
        [HttpPost("initialize")]
        public async Task<ActionResult> InitializeDatabase()
        {
            await _executionService.InitializeDatabase();
            return Ok();
        }*/

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Execution updatedExecution)
        {
            var existingExecution = await _executionService.GetAsync(id);
            if (existingExecution == null)
            {
                return NotFound();
            }

            updatedExecution.Id = existingExecution.Id;
            await _executionService.UpdateAsync(id, updatedExecution);
            return NoContent();
        }
    }

}
