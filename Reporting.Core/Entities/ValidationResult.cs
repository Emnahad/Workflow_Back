using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Core.Entities
{
    public class ValidationResult
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ReportId { get; set; }

        public bool IsValid { get; set; }

        public string ValidationMessage { get; set; }

        public DateTime ValidationDate { get; set; }
    }
}
