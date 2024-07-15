using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Reporting.Core.Entities;
using Reporting.Core.IService;
using Reporting.Infrastracture.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reporting.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IMongoCollection<Report> _reportingCollection;
        private readonly IMongoDatabase _DataBase;
        private readonly IMongoCollection<Execution> _executionCollection;
        private readonly IMongoCollection<ValidationResult> _validationResultsCollection;

        public ReportService(IOptions<ReportingDatabaseSettings> ReportingDatabaseSettings)
        {
            var mongoClient = new MongoClient(ReportingDatabaseSettings.Value.ConnectionString);
            _DataBase = mongoClient.GetDatabase(ReportingDatabaseSettings.Value.DatabaseName);

            _reportingCollection = _DataBase.GetCollection<Report>(ReportingDatabaseSettings.Value.ReportingCollectionName);
            _executionCollection = _DataBase.GetCollection<Execution>("Executions");
            _validationResultsCollection = _DataBase.GetCollection<ValidationResult>("ValidationResults");
        }

        public async Task InitializeAndCreateReportAsync(Report newReport)
        {
            bool isCollectionEmpty = await _reportingCollection.CountDocumentsAsync(_ => true) == 0;

            if (isCollectionEmpty)
            {
                var initialReport = new Report
                {
                    ExecutionId = "66867a21c27c47015b8fdd88",
                    TBId = "test",
                    Ticket = "test",
                    Reason = "test",
                    Description = "This is a test document to initialize the database",
                    TesterComment = "No comment",
                    ReportedAt = DateTime.Now.ToString(),
                    ReportedBy = "System"
                    
                };

                await _reportingCollection.InsertOneAsync(initialReport);
            }

            if (newReport != null)
            {
                await _reportingCollection.InsertOneAsync(newReport);
                var update = Builders<Execution>.Update.Push(e => e.ReportIds, newReport._id);
                await _executionCollection.UpdateOneAsync(e => e.Id == newReport.ExecutionId, update);
            }
        }


        public async Task<Report?> GetAsync(string id)
        {
            return await _reportingCollection.Find(p => p._id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Report>> GetListAsync()
        {
            return await _reportingCollection.Find(_ => true).ToListAsync();
        }

        public async Task RemoveAsync(string id)
        {
            var report = await GetAsync(id);
            if (report != null)
            {
                await _reportingCollection.DeleteOneAsync(p => p._id == id);
                var update = Builders<Execution>.Update.Pull(e => e.ReportIds, id);
                await _executionCollection.UpdateOneAsync(e => e.Id == report.ExecutionId, update);
            }
        }

        /*public async Task UpdateAsync(string id, Report updatedReport)
        {
            await _reportingCollection.ReplaceOneAsync(p => p._id == id, updatedReport);
        }*/

        public async Task UpdateAsync(string id, Report updatedReport)
        {
            var existingReport = await GetAsync(id);
            if (existingReport != null)
            {
                var update = Builders<Report>.Update
                    .Set(r => r.TBId, updatedReport.TBId ?? existingReport.TBId)
                    .Set(r => r.Ticket, updatedReport.Ticket ?? existingReport.Ticket)
                    .Set(r => r.Reason, updatedReport.Reason ?? existingReport.Reason)
                    .Set(r => r.Description, updatedReport.Description ?? existingReport.Description)
                    .Set(r => r.TesterComment, updatedReport.TesterComment ?? existingReport.TesterComment)
                    .Set(r => r.ReportedAt, updatedReport.ReportedAt ?? existingReport.ReportedAt)
                    .Set(r => r.ReportedBy, updatedReport.ReportedBy ?? existingReport.ReportedBy);
                await _reportingCollection.UpdateOneAsync(r => r._id == id, update);
            }
        }

        public async Task<List<Report>> GetReportsByExecutionIdAsync(string executionId)
        {
            return await _reportingCollection.Find(r => r.ExecutionId == executionId).ToListAsync();
        }

        public async Task<string> ValidateAndSaveReportAsync(string reportId)
        {
            var report = await GetAsync(reportId);
            if (report == null)
            {
                return "Invalid Report: Report not found.";
            }
            if (report.ExecutionId == null)
            {
                return "Invalid Report: ExecutionId is null.";
            }

            var execution = await _executionCollection.Find(p => p.Id == report.ExecutionId).FirstOrDefaultAsync();
            if (execution == null)
            {
                return "Invalid Report: Execution not found.";
            }

            string validationMessage;
            bool isValid;

            if (report.TesterComment == "Passed")
            {
                if (execution.ExecutionResult == "Success")
                {
                    validationMessage = "Valid Report: No further checks needed.";
                    isValid = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(report.Ticket) && !string.IsNullOrEmpty(report.Reason))
                    {
                        validationMessage = "Valid Report: Ticket and Description are not empty.";
                        isValid = true;
                    }
                    else
                    {
                        validationMessage = "Invalid Report: Either Ticket or Description is empty.";
                        isValid = false;
                    }
                }
            }
            else
            {
                validationMessage = "Report validation criteria not met.";
                isValid = false;
            }

            var validationResult = new ValidationResult
            {
                ReportId = reportId,
                IsValid = isValid,
                ValidationMessage = validationMessage,
                ValidationDate = DateTime.Now
            };

            await _validationResultsCollection.InsertOneAsync(validationResult);

            return validationMessage;
        }

        public async Task<List<ValidationResult>> GetValidationResultsByReportIdAsync(string reportId)
        {
            return await _validationResultsCollection.Find(vr => vr.ReportId == reportId).ToListAsync();
        }
    }
}
