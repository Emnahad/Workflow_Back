using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reporting.Core.Entities;
using Reporting.Core.IService;
using System;
using System.Threading.Tasks;

namespace ReportingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagramController : ControllerBase
    {
        private readonly IDiagramService _diagramService;
        private readonly ILogger<DiagramController> _logger;

        public DiagramController(IDiagramService diagramService, ILogger<DiagramController> logger)
        {
            _diagramService = diagramService;
            _logger = logger;
        }

        [HttpPost("saveDiagram")]
        public async Task<IActionResult> SaveDiagram([FromBody] Diagram diagram)
        {
            if (diagram == null)
            {
                return BadRequest("Diagram data is null.");
            }

            try
            {
                // Convert diagram ID if necessary
                if (!string.IsNullOrEmpty(diagram.Id) && !ObjectId.TryParse(diagram.Id, out _))
                {
                    diagram.Id = ObjectId.GenerateNewId().ToString();
                }

                foreach (var node in diagram.Nodes)
                {
                    if (!ObjectId.TryParse(node.Id, out _))
                    {
                        node.Id = ObjectId.GenerateNewId().ToString();
                    }
                }

                foreach (var edge in diagram.Edges)
                {
                    if (!string.IsNullOrEmpty(edge.Id) && !ObjectId.TryParse(edge.Id, out _))
                    {
                        edge.Id = ObjectId.GenerateNewId().ToString();
                    }

                    if (!string.IsNullOrEmpty(edge.FromId) && !ObjectId.TryParse(edge.FromId, out _))
                    {
                        edge.FromId = ObjectId.GenerateNewId().ToString();
                    }

                    if (!string.IsNullOrEmpty(edge.ToId) && !ObjectId.TryParse(edge.ToId, out _))
                    {
                        edge.ToId = ObjectId.GenerateNewId().ToString();
                    }
                }

                await _diagramService.SaveDiagramAsync(diagram);
                return Ok("Diagram saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the diagram.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("getDiagrams")]
        public async Task<IActionResult> GetDiagrams()
        {
            try
            {
                var diagrams = await _diagramService.GetDiagramsAsync();
                return Ok(diagrams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the diagrams.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
