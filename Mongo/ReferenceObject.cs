using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mongo
{
    [BsonIgnoreExtraElements]
    public class ReferenceObject
    {
        public ObjectId id { get; set; }
        public string CreatedDate { get; set; } = DateTime.Now.ToString();
        public double CreatedTimeStamp { get; set; } = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmss"));
        public string Creator { get; set; }
        public string LastmodDate { get; set; } = DateTime.Now.ToString();
        public string department { get; set; } = "null";
        public string ext { get; set; }
        public string location_id { get; set; } = "null";
        public string marca { get; set; } = "";
        public string modelo { get; set; } = "";
        public string name { get; set; } = "";
        public string object_id { get; set; }
        public string parentCategory { get; set; }
        public string perfil { get; set; }
        public string precio { get; set; } = "";
        public BsonDocument profileFields { get; set; } = new BsonDocument { { "proveedor", "null" } };
        public string proveedor { get; set; } = "null";
        public bool system_status { get; set; } = true;

        public ReferenceObject()
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
        }

        public ReferenceObject(string _id)
        {
            if (_id != "0")
                id = new ObjectId(_id);
            else
                id = ObjectId.GenerateNewId(DateTime.Now);
        }

        public ReferenceObject(string name, string marca, string modelo, string Creator, Category parentCategory)
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
            this.Creator = Creator;
            this.parentCategory = parentCategory.id.ToString();
        }
        public ReferenceObject(string _id, string name, string marca, string modelo, string Creator, Category parentCategory)
        {
            id = new ObjectId(_id);
            this.name = name;
            this.marca = marca;
            this.modelo = modelo;
            this.Creator = Creator;
            this.parentCategory = parentCategory.id.ToString();
        }
    }
}
