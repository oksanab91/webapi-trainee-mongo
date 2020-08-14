using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace TraineesApi.Models
{
    public class Trainee
    {        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
    }
}
