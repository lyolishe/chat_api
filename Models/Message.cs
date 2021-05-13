using System;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace chat_api.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; } = new Guid();
        
        public Guid ChatId { get; set; }
        
        public Guid UserId { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        public string Text { get; set; }
    }
}