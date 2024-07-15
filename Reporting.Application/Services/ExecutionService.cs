using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Reporting.Core.Entities;
using Reporting.Core.IService;
using Reporting.Infrastracture.Configuration;

namespace Reporting.Application.Services
{
    public class ExecutionService : IExecutionService
    {
        
        private readonly IMongoCollection<Execution> _executionCollection;
        private readonly IMongoDatabase _DataBase;

        public ExecutionService(
            IOptions<ReportingDatabaseSettings> ReportingDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ReportingDatabaseSettings.Value.ConnectionString);

            _DataBase = mongoClient.GetDatabase(
                ReportingDatabaseSettings.Value.DatabaseName);

            _executionCollection = _DataBase.GetCollection<Execution>(
                "Executions"); 
        }

        public async Task InitializeAndCreateExecutionAsync(Execution newExecution)
        {
            bool isCollectionEmpty = await _executionCollection.CountDocumentsAsync(_ => true) == 0;

            if (isCollectionEmpty)
            {
                var initialExecution = new Execution
                {
                    TBId = "test",
                    ExecutionResult = "test",
                    ExecutionLog = "test",
                    ExecutionLink = "test",
                    ExecutedAt = "test",
                    SW = "test",
                    HW = "test",
                    ScopeName = "test",
                    ScopeType = "test"
                };

                await _executionCollection.InsertOneAsync(initialExecution);
            }

            if (newExecution != null)
            {
                await _executionCollection.InsertOneAsync(newExecution);
            }
        }


        public async Task<Execution?> GetAsync(string id)
        {
            return await _executionCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Execution>> GetListAsync()
        {
            return await _executionCollection.Find(_ => true).ToListAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await _executionCollection.DeleteOneAsync(p => p.Id == id);
        }

   

        public async Task UpdateAsync(string id, Execution updatedExecution)
        {
            await _executionCollection.ReplaceOneAsync(p => p.Id == id, updatedExecution);
        }
    }
}

        

   
