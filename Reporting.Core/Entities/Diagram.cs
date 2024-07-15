using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Reporting.Core.Entities
{
    public class Diagram
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<FlowNode> Nodes { get; set; } = new List<FlowNode>();

        public List<FlowEdge> Edges { get; set; } = new List<FlowEdge>();
    }

    public class FlowNode
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }
    }

    public class FlowEdge
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FromId { get; set; }

        public string ToId { get; set; }

        public string Text { get; set; }
    }
}
