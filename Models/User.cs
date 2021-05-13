using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace chat_api.Models
{
    public class User
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [BsonElement("CreatedAt")]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        public string Login { get; set; }
        
        public string Password { get; set; }
        
        #nullable enable
        public string? DisplayName { get; set; }
        
        public string? Phone { get; set; }
    }
}