using MongoDB.Driver;
using Reporting.Core.Entities;
using Reporting.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Application.Services
{
    public class DiagramService : IDiagramService
    {
        private readonly IMongoCollection<Diagram> _diagramCollection;
        public DiagramService(IMongoDatabase database)
        {
            _diagramCollection = database.GetCollection<Diagram>("Diagrams");
        }
        public async Task SaveDiagramAsync(Diagram diagram)
        {
            await _diagramCollection.InsertOneAsync(diagram);
        }
        public async Task<List<Diagram>> GetDiagramsAsync()
        {
            return await _diagramCollection.Find(_ => true).ToListAsync();
        }
    }
}
