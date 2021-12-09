using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.NoSql
{
    public  class LogDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string User { get; set; }

        [BsonElement("Intent")]
        public bool intent { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        public LogDto(string user, bool intent, DateTime date)
        {
            User = user;
            this.intent = intent;
            Date = date;
        }
    }
}
