using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mongo
{
    [BsonIgnoreExtraElements]
    public class Location
    {
        public ObjectId id { get; set; }
        public string CreatedDate { get; set; } = DateTime.Now.ToString();
        public double CreatedTimeStamp { get; set; } = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmss"));
        public string Creator { get; set; }
        public string LastmodDate { get; set; } = DateTime.Now.ToString();
        public string creatorId { get; set; }
        public string description { get; set; } = "";
        public string name { get; set; } = "";
        public string number { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        public string parent { get; set; }
        public string profileId { get; set; }
        public BsonDocument profileFields { get; set; } = new BsonDocument { { "location", "null" } };
        public string setup { get; set; } = "";
        public string setupname { get; set; } = "";
        public bool system_status { get; set; } = true;
        public string tipo { get; set; } = "1";

        public Location()
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
        }
        public Location(string _id)
        {
            if (_id != "0")
                id = new ObjectId(_id);
            else
                ObjectId.GenerateNewId(DateTime.Now);
        }
        public Location(string name, string Creator, string parent, string locationProfile)
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
            this.name = name;
            this.description = name;
            this.Creator = Creator;
            this.parent = parent;
            this.profileId = locationProfile;
        }
        public Location(string name, string Creator, Location parent, string locationProfile)
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
            this.name = name;
            this.description = name;
            this.Creator = Creator;
            this.parent = parent.id.ToString();
            this.profileId = locationProfile;
        }

        public Location(string _id, string name, string Creator, Location parent, string locationProfile)
        {
            id = new ObjectId(_id);
            this.name = name;
            this.description = name;
            this.Creator = Creator;
            this.parent = parent.id.ToString();
            this.profileId = locationProfile;
        }
    }
}
