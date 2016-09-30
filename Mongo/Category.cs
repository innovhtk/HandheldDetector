using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mongo
{
    [BsonIgnoreExtraElements]
    public class Category
    {
        public ObjectId id { get; set; }
        public string CreatedDate { get; set; } = DateTime.Now.ToString();
        public string Creator { get; set; }
        public string LastmodDate { get; set; } = DateTime.Now.ToString();
        public BsonArray customFields { get; set; } = new BsonArray();
        public string depreciacion { get; set; } = "";
        public string ext { get; set; }
        public string name { get; set; }
        public string parentCategory { get; set; } = "null";

        public Category()
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
        }

        public Category(string _id)
        {
            if (_id != "0")
                id = new ObjectId(_id);
            else
                id = ObjectId.GenerateNewId(DateTime.Now);
        }
        public Category(string name, string Creator, string parentCategory)
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
            this.name = name;
            this.Creator = Creator;
            this.parentCategory = parentCategory;
        }
    }
}
