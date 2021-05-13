using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace chat_api.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}