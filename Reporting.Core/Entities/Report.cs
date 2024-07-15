using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Reporting.Core.Entities
{
    [Collection("Reporting")]
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ExecutionId { get; set; }
        public string TBId { get; set; }
        public string Ticket { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public string TesterComment { get; set; }
        public string ReportedAt {  get; set; }
        public string ReportedBy { get; set; }

    }
}
