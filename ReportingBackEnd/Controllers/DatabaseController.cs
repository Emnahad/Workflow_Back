using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Reporting.Core.IService;

namespace ReportingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public DatabaseController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet("collections")]
        public async Task<ActionResult<List<string>>> GetCollections()
        {
            try
            {
                var collections = await _databaseService.GetCollectionsAsync();
                return Ok(collections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("collections/{collectionName}/columns")]
        public async Task<ActionResult<List<string>>> GetCollectionColumns(string collectionName)
        {
            try
            {
                var columns = await _databaseService.GetCollectionColumnsAsync(collectionName);
                return Ok(columns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}
