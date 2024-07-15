using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Core.Entities
{
    public class Execution
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TBId { get; set; }
        public string ExecutionResult { get; set; }
        public string ExecutionLog { get; set; }
        public string ExecutionLink { get; set; }
        public string ExecutedAt { get; set; }
        public string SW { get; set; }
        public string HW { get; set; }
        public string ScopeName { get; set; }
        public string ScopeType { get; set; }
        public List<string> ReportIds { get; set; } = new List<string>();
    }
}
