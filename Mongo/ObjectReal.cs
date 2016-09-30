using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mongo
{
    [BsonIgnoreExtraElements]
    public class ObjectReal
    {
        public ObjectId id { get; set; }
        public string CreatedDate { get; set; } = DateTime.Now.ToString();
        public double CreatedTimeStamp { get; set; } = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmss"));
        public string Creator { get; set; }
        public string EPC { get; set; }
        public string LastmodDate { get; set; } = DateTime.Now.ToString();
        public string RH { get; set; }
        public string assetType { get; set; }
        public string comments { get; set; } = "";
        public string date { get; set; }
        public string date_label { get; set; }
        public string department { get; set; }
        public string factura { get; set; }
        public string fechafactura { get; set; }
        public string filefactura { get; set; }
        public string folio { get; set; }
        public string garantia { get; set; }
        public string label { get; set; }
        public string lastmaintenance { get; set; }
        public string location { get; set; } = "";
        public string marca { get; set; } = "";
        public string modelo { get; set; } = "";
        public string name { get; set; } = "";
        public string nextmaintenance { get; set; }
        public string num_ERP { get; set; }
        public string num_pedido { get; set; }
        public string num_reception { get; set; }
        public string num_solicitud { get; set; }
        public string objectReference { get; set; } = "";
        public string object_id { get; set; } = "";
        public string objectfile { get; set; }
        public string observation { get; set; }
        public string perfil { get; set; }
        public string price { get; set; }
        public string proveedor { get; set; }
        public string quantity { get; set; } = "1";
        public string serie { get; set; } = "";
        public string status { get; set; } = "En tu oficnina";
        public bool system_status { get; set; } = true;
        public string userlabel { get; set; }
        public string vale { get; set; }

        public ObjectReal()
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
        }
        public ObjectReal(string _id)
        {
            if (_id != "0")
                id = new ObjectId(_id);
            else
                ObjectId.GenerateNewId(DateTime.Now);
        }
        public ObjectReal(string Creator, string objectReference)
        {
            id = ObjectId.GenerateNewId(DateTime.Now);
            this.Creator = Creator;
            this.objectReference = objectReference;
        }
    }
}
