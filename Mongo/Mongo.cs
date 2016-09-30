using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using System.Threading.Tasks;

namespace Mongo
{
    public class Mongo
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public IMongoCollection<Category> Categories;
        public IMongoCollection<ReferenceObject> ReferenceObjects;
        public IMongoCollection<ObjectReal> ObjectsReal;
        public IMongoCollection<Location> Locations;

        public List<WriteModel<Category>> BulkCategories = new List<WriteModel<Category>>();
        public List<WriteModel<ReferenceObject>> BulkReferenceObjects = new List<WriteModel<ReferenceObject>>();
        public List<WriteModel<ObjectReal>> BulkObjectsReal = new List<WriteModel<ObjectReal>>();
        public List<WriteModel<Location>> BulkLocations = new List<WriteModel<Location>>();
        public BulkWriteOptions writeOptions = new BulkWriteOptions() { IsOrdered = false };

        public string RegionProfile { get; set; } = "";
        public string ConjuntoProfile { get; set; } = "";
        public string UbicacionProfile { get; set; } = "";
        public string SubUbicacionProfile { get; set; } = "";

        private int LocationsNumber = 0;
        private int ObjectsNumber = 0;

        public Mongo(string db, string user, string pwd, string host)
        {
            var connectionString = String.Format(
           @"mongodb://{0}:{1}@{2}/{3}", user, pwd, host, db);

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(db);

            Categories = _database.GetCollection<Category>("Categories");
            ReferenceObjects = _database.GetCollection<ReferenceObject>("ReferenceObjects");
            ObjectsReal = _database.GetCollection<ObjectReal>("ObjectReal");
            Locations = _database.GetCollection<Location>("Locations");

            GetLocationsProfiles();
            LocationsNumber = GetlocationsCount();
        }

        public string GetAssetType(string referenceId)
        {
            var result = ReferenceObjects.Find(x => x.id == new ObjectId(referenceId)).ToList();
            if (result.Count < 1) return "";
            return result[0].parentCategory;
        }

        public int GetlocationsCount()
        {
            var count = Locations.CountAsync(new BsonDocument());
            return Convert.ToInt32(count.Result);
        }

        private void GetLocationsProfiles()
        {
            var collection = _database.GetCollection<BsonDocument>("LocationProfiles");
            var filter = Builders<BsonDocument>.Filter.Eq("name", "Region");
            var result = collection.Find(filter).ToList();
            if (result.Count > 0)
                RegionProfile = result[0].GetElement("_id").Value.AsObjectId.ToString();

            filter = Builders<BsonDocument>.Filter.Eq("name", "Ubicacion");
            result = collection.Find(filter).ToList();
            if (result.Count > 0)
                UbicacionProfile = result[0].GetElement("_id").Value.AsObjectId.ToString();

            filter = Builders<BsonDocument>.Filter.Eq("name", "Conjunto");
            result = collection.Find(filter).ToList();
            if (result.Count > 0)
                ConjuntoProfile = result[0].GetElement("_id").Value.AsObjectId.ToString();

            filter = Builders<BsonDocument>.Filter.Eq("name", "Sub-Ubicaciones");
            result = collection.Find(filter).ToList();
            if (result.Count > 0)
                SubUbicacionProfile = result[0].GetElement("_id").Value.AsObjectId.ToString();
        }

        public void ChangeCategory(ReferenceObject referenceObject, Category category, bool bulk = false)
        {
            var filter = Builders<ReferenceObject>.Filter.Eq(reference => reference.id, referenceObject.id);
            var update = Builders<ReferenceObject>.Update
                .Set(r => r.parentCategory, category.id.ToString())
                .Set(r2 => r2.LastmodDate, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            if(!bulk)
            {
                var result = ReferenceObjects.UpdateOneAsync(filter, update);
                ReferenceObjects = _database.GetCollection<ReferenceObject>("ReferenceObjects");
            }
            else
            {
                BulkReferenceObjects.Add(new UpdateOneModel<ReferenceObject>(filter, update));
            }
        }

        public void ChangeLocation(ObjectReal objectReal, Location location, bool bulk = false)
        {
            var filter = Builders<ObjectReal>.Filter.Eq(or => or.EPC, objectReal.EPC);
            var update = Builders<ObjectReal>.Update
                .Set(r => r.location, location.id.ToString())
                .Set(r2 => r2.LastmodDate, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            if (!bulk)
            {
                var result = ObjectsReal.UpdateOneAsync(filter, update);
                ObjectsReal = _database.GetCollection<ObjectReal>("ObjectReal");
            }
            else
            {
                BulkObjectsReal.Add(new UpdateOneModel<ObjectReal>(filter, update));
            }
        }

        public void ChangeReference(ObjectReal objectReal, ReferenceObject referenceObject, bool bulk = false)
        {
            var filter = Builders<ObjectReal>.Filter.Eq(or => or.EPC, objectReal.EPC);
            var objects = ObjectsReal.Find(filter).ToList();
            if (objects.Count < 1)
                return;
            if (objects[0].objectReference == referenceObject.id.ToString())
                return;
            var update = Builders<ObjectReal>.Update
                .Set(r => r.objectReference, referenceObject.id.ToString())
                .Set(r => r.name, referenceObject.name)
                .Set(r => r.marca, referenceObject.marca)
                .Set(r => r.price, referenceObject.precio)
                .Set(r2 => r2.LastmodDate, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            if (!bulk)
            { 
                var result = ObjectsReal.UpdateOneAsync(filter, update);
                ObjectsReal = _database.GetCollection<ObjectReal>("ObjectReal");
            }
            else
            {
                BulkObjectsReal.Add(new UpdateOneModel<ObjectReal>(filter, update));
            }
        }

        public void ChangeSerial(ObjectReal objectReal, string serial, bool bulk = false)
        {
            var filter = Builders<ObjectReal>.Filter.Eq(or => or.EPC, objectReal.EPC);

            var test = ObjectsReal.Find(filter).ToList();
            if (test.Count < 1)
            {
                filter = Builders<ObjectReal>.Filter.Eq(or => or.id, objectReal.id);
                test = ObjectsReal.Find(filter).ToList();
                if (test.Count < 1) return;
            }
            if (test[0].serie == serial)
                return;

            var update = Builders<ObjectReal>.Update
                .Set(r => r.serie, serial)
                .Set(r2 => r2.LastmodDate, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            if (!bulk)
            {
                var result = ObjectsReal.UpdateOneAsync(filter, update);
                ObjectsReal = _database.GetCollection<ObjectReal>("ObjectReal");
            }
            else
            {
                BulkObjectsReal.Add(new UpdateOneModel<ObjectReal>(filter, update));
            }
        }

        public void ChangeTag(string registerId, string epc, bool bulk = false)
        {
            var filter = Builders<ObjectReal>.Filter.Eq(or => or.id, new ObjectId(registerId));
            var update = Builders<ObjectReal>.Update
                .Set(r => r.EPC, epc)
                .Set(r2 => r2.LastmodDate, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            if (!bulk)
            {
                var result = ObjectsReal.UpdateOneAsync(filter, update);
                ObjectsReal = _database.GetCollection<ObjectReal>("ObjectReal");
            }
            else
            {
                BulkObjectsReal.Add(new UpdateOneModel<ObjectReal>(filter, update));
            }
        }

        public Category GetCategory(string categoryId)
        {
            var result = Categories.Find(or => or.id == new ObjectId(categoryId)).ToList();
            if (result.Count > 0)
                return result[0];
            else
                return null;
        }

        public List<Category> GetCategoryList()
        {
            return Categories.Find(_ => true).ToList();
        }

        public List<Category> GetCategoryList(string field, object value)
        {
            System.Reflection.PropertyInfo prop = typeof(Category).GetProperty(field);
            return Categories.Find(x => prop.GetValue(x, null) == value).ToList();
        }

        public Location GetLocation(string locationId)
        {
            var result = Locations.Find(x => x.id == new ObjectId(locationId) && x.parent != "none").ToList();
            if (result.Count > 0)
                return result[0];
            else
                return null;
        }
        public Location GetLocationByName(string name)
        {
            var result = Locations.Find(x => x.name == name && x.parent != "none").ToList();
            if (result.Count > 0)
                return result[0];
            else
                return null;
        }

        public Location GetRegion(string locationId)
        {
            var location = GetLocation(locationId);
            if (location == null)
                return null;
            List<Location> result = new List<Location>();
            do
            {
                result = Locations.Find(x => x.id == new ObjectId(location.parent)).ToList();
                if (result.Count > 0)
                    location = result[0];
            }
            while (result.Count > 0);
            return location;
        }
        public Location GetFirstRegion()
        {
            var result = Locations.Find(x => x.parent == "null").ToList();
            if (result.Count > 0)
                return result[0];
            else
                return null;
        }
        public Location GetRegionOfSub(string locationId)
        {
            var sub = GetLocation(locationId);
            if (sub == null)
            {
                return null;
            }
            var ub = GetLocation(sub.parent);
            if (ub == null)
            {
                return null;
            }
            var conj = GetLocation(ub.parent);
            if(conj == null)
            {
                return null;
            }
            var reg = GetLocation(conj.parent);
            if(reg == null)
            {
                return null;
            }

            return reg;
        }

        public List<Location> GetLocationList()
        {
            return Locations.Find(_ => true).ToList();
        }

        public List<Location> GetLocationList(string field, object value)
        {
            System.Reflection.PropertyInfo prop = typeof(Category).GetProperty(field);
            return Locations.Find(x => prop.GetValue(x, null) == value).ToList();
        }

        public ObjectReal GetObjectReal(string epc)
        {
            var result = ObjectsReal.Find(x => x.EPC == epc).ToList();
            if (result.Count > 0)
                return result[0];
            else
                return null;
        }

        public List<ObjectReal> GetObjectRealList()
        {
            return ObjectsReal.Find(_ => true).ToList();
        }

        public List<ObjectReal> GetObjectRealList(string field, object value)
        {
            System.Reflection.PropertyInfo prop = typeof(Category).GetProperty(field);
            return ObjectsReal.Find(x => prop.GetValue(x, null) == value).ToList();
        }

        public ReferenceObject GetReferenceObject(string referenceId)
        {
            var result = ReferenceObjects.Find(x => x.id == new ObjectId(referenceId)).ToList();
            if (result.Count > 0)
                return result[0];
            else
                return null;
        }

        public List<ReferenceObject> GetReferenceObjectList()
        {
            return ReferenceObjects.Find(_ => true).ToList();
        }

        public List<ReferenceObject> GetReferenceObjectList(string field, object value)
        {
            System.Reflection.PropertyInfo prop = typeof(Category).GetProperty(field);
            return ReferenceObjects.Find(x => prop.GetValue(x, null) == value).ToList();
        }

        public void Insert(Category category)
        {
            var result = Categories.Find(x => x.id == category.id).ToList();
            if (result.Count > 0) return;
            Categories.InsertOne(category);
        }

        public void Insert(List<Category> categories)
        {
            Categories.InsertMany(categories);
        }

        public void Insert(ReferenceObject referenceObject)
        {
            string objectId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var result = ReferenceObjects.Find(_ => true).SortByDescending(x => x.object_id).Limit(1).ToList();
            if (result.Count > 0)
                objectId = (Convert.ToInt32(result[0].object_id) + 1).ToString();
            referenceObject.object_id = objectId;
            ReferenceObjects.InsertOne(referenceObject);
        }

        public void Insert(List<ReferenceObject> referenceObjects)
        {
            string objectId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var result = ReferenceObjects.Find(_ => true).SortByDescending(x => x.object_id).Limit(1).ToList();
            if (result.Count > 0)
                objectId = (Convert.ToInt32(result[0].object_id) + 1).ToString();
            foreach (var r in referenceObjects)
            {
                r.object_id = objectId;
                objectId = (Convert.ToInt32(objectId) + 1).ToString();
            }

            ReferenceObjects.InsertMany(referenceObjects);
        }

        public void Insert(Location location)
        {
            if (location.profileId == ConjuntoProfile)
            {
                InsertConjunto(location);
            }
            else if(location.profileId == UbicacionProfile)
            {
                InsertUbicacion(location);
            }
            else if(location.profileId == SubUbicacionProfile)
            {
                InsertSubUbicacion(location);
            }
        }
        public void InsertSubUbicacion(Location location)
        {
            var id = location.id;
            var parent = location.parent;
            location.number = (++LocationsNumber).ToString();
            Locations.InsertOne(location);
            Location location2 = location;
            location2.number = (++LocationsNumber).ToString();
            location2.parent = "none";
            location2.id = ObjectId.GenerateNewId(DateTime.Now);
            Locations.InsertOne(location2);
            location.id = id;
            location.parent = parent;
        }
        public void InsertUbicacion(Location location)
        {
            var id = location.id;
            var parent = location.parent;
            location.number = (++LocationsNumber).ToString();
            Locations.InsertOne(location);
            Location location2 = location;
            location2.number = (++LocationsNumber).ToString();
            location2.parent = "none";
            location2.id = ObjectId.GenerateNewId(DateTime.Now);
            Locations.InsertOne(location2);
            location.id = id;
            location.parent = parent;
        }
        public void InsertConjunto(Location location)
        {
            location.number = (++LocationsNumber).ToString();
            Locations.InsertOne(location);
        }
        public void Insert(List<Location> locations)
        {
            Locations.InsertMany(locations);
        }

        public void Insert(ObjectReal objectReal)
        {
            if(ObjectsNumber == 0)
            {
                var result = ReferenceObjects.Find(_ => true).SortByDescending(x => x.object_id).Limit(1).ToList();
                if (result.Count > 0)
                    ObjectsNumber = Convert.ToInt32(result[0].object_id) + 1;
            }
            
            objectReal.object_id = ObjectsNumber.ToString();
            ObjectsReal.InsertOne(objectReal);
            ObjectsNumber++;
        }

        public void Insert(List<ObjectReal> objectsReal)
        {
            if (ObjectsNumber == 0)
            {
                var result = ReferenceObjects.Find(_ => true).SortByDescending(x => x.object_id).Limit(1).ToList();
                if (result.Count > 0)
                    ObjectsNumber = Convert.ToInt32(result[0].object_id) + 1;
            }

            foreach (var r in objectsReal)
            {
                r.object_id = ObjectsNumber.ToString();
                ObjectsNumber++;
            }
            ObjectsReal.InsertMany(objectsReal);
        }

        public string GetAdminUser()
        {
            var users = _database.GetCollection<BsonDocument>("Users");
            var filter = Builders<BsonDocument>.Filter.Eq("user", "admin");
            var result = users.Find<BsonDocument>(filter).ToList();
            if (result.Count > 0)
            {
                var id = result[0].GetElement("_id").Value.AsObjectId.ToString();
                return id;
            }

            var document = new BsonDocument
            {
                { "user" , "admin" },
                { "pwd" , "1000:aRHo0W2cKToN+VB0e+Ijh2wtmLJA/bwe:I8FC1fVjm428oihzAxSvAOJlbi4WZRBO" },
                { "imgext" , "jpg" },
                { "email" , "sistemas@htk-id.com" },
                { "name" , "Admin" },
                { "lastname" , "Admin" },
                { "profileId" , "543573676e57601a204bec00" },
                { "boss" , "52e95ab907719e0d40637d96" },
                { "userKey" , "971001091051103297100109105110|97100109105110" },
                { "userLocations" , new BsonArray
                    {
                        new BsonDocument
                        {
                            { "id" , "undefined"},
                            {"name" , "Home"}
                        }
                    }
                },
                { "areaSelect" , "Finanzas" },
                { "departmentSelect" , "13031"},
                { "descripcionpuesto" , "ADMINISTRADOR DE SISTEMA"},
                { "alias" , "admin"},
                { "permissionsHTK" , new BsonDocument {
                    {"users" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"profiles" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"custom_fields" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"objects" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"objectsreference" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"demand" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"hardware" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"location" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"processes" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"circuits" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"movement" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"rules" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"inventory" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"lists" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"messages" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"tickets" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"designs" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"edgeware" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"reports" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"selectreports" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"conexionext" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"support" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } },
                    {"help" , new BsonDocument { { "grant" , new BsonArray{"c","u","d","r","a" } } } }
                }
                },
                { "creatorId" , ""},
                { "profileFields" , new BsonDocument {
                    { "z" , "on" },
                    { "_HTKFieldsueldo" , "" }
                }
                },
                { "CreatedDate" , DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") },
                { "LastmodDate" , DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") },
                { "CreatedTimeStamp" , Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmss"))}
            };


            var collection = _database.GetCollection<BsonDocument>("Users");
            //await collection.InsertOneAsync(document);
            collection.InsertOne(document);

            result = users.Find<BsonDocument>(filter).ToList();
            if (result.Count > 0)
            {
                var id = result[0].GetElement("_id").Value.AsObjectId.ToString();
                return id;
            }
            return "";
        }

        public void Remove(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(or => or.id, category.id);
            var result = Categories.DeleteOneAsync(filter);
        }

        public void Remove(List<Category> categories)
        {
            if (categories.Count == 0) return;
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq(or => or.id, categories[0].id);
            for (int i = 1; i < categories.Count; i++)
            {
                filter |= Builders<Category>.Filter.Eq(or => or.id, categories[i].id);
            }
            var result = Categories.DeleteMany(filter);
        }

        public void Remove(Location location)
        {
            var filter = Builders<Location>.Filter.Eq(or => or.id, location.id);
            var result = Locations.DeleteOneAsync(filter);
        }

        public void Remove(List<Location> locations)
        {
            if (locations.Count == 0) return;
            FilterDefinition<Location> filter = Builders<Location>.Filter.Eq(or => or.id, locations[0].id);
            for (int i = 1; i < locations.Count; i++)
            {
                filter |= Builders<Location>.Filter.Eq(or => or.id, locations[i].id);
            }
            var result = Locations.DeleteMany(filter);
        }

        public void Remove(ReferenceObject referenceObject)
        {
            var filter = Builders<ReferenceObject>.Filter.Eq(or => or.id, referenceObject.id);
            var result = ReferenceObjects.DeleteOneAsync(filter);
        }

        public void Remove(List<ReferenceObject> referenceObjects)
        {
            if (referenceObjects.Count == 0) return;
            FilterDefinition<ReferenceObject> filter = Builders<ReferenceObject>.Filter.Eq(or => or.id, referenceObjects[0].id);
            for (int i = 1; i < referenceObjects.Count; i++)
            {
                filter |= Builders<ReferenceObject>.Filter.Eq(or => or.id, referenceObjects[i].id);
            }
            var result = ReferenceObjects.DeleteMany(filter);
        }

        public void Remove(ObjectReal objectReal)
        {
            var filter = Builders<ObjectReal>.Filter.Eq(or => or.id, objectReal.id);
            var result = ObjectsReal.DeleteOneAsync(filter);
        }

        public void Remove(List<ObjectReal> objectsReal)
        {
            if (objectsReal.Count == 0) return;
            FilterDefinition<ObjectReal> filter = Builders<ObjectReal>.Filter.Eq(or => or.id, objectsReal[0].id);
            for (int i = 1; i < objectsReal.Count; i++)
            {
                filter |= Builders<ObjectReal>.Filter.Eq(or => or.id, objectsReal[i].id);
            }
            var result = ObjectsReal.DeleteMany(filter);
        }

        public void Remove(string epc)
        {
            var filter = Builders<ObjectReal>.Filter.Eq(or => or.EPC, epc);
            var result = ObjectsReal.DeleteOneAsync(filter);
        }

        public void Remove(List<string> epcs)
        {
            if (epcs.Count == 0) return;
            FilterDefinition<ObjectReal> filter = Builders<ObjectReal>.Filter.Eq(or => or.EPC, epcs[0]);
            for (int i = 1; i < epcs.Count; i++)
            {
                filter |= Builders<ObjectReal>.Filter.Eq(or => or.EPC, epcs[i]);
            }
            var result = ObjectsReal.DeleteMany(filter);
        }

        public void UpdateBulkLocations()
        {
            if (BulkLocations.Count < 1) return;
            BulkWriteResult<Location> result = Locations.BulkWriteAsync(BulkLocations, writeOptions).Result;
            BulkLocations.Clear();
            Locations = _database.GetCollection<Location>("Locations");
        }
        public void UpdateBulkReferenceObjects()
        {
            if (BulkReferenceObjects.Count < 1) return;
            BulkWriteResult<ReferenceObject> result = ReferenceObjects.BulkWriteAsync(BulkReferenceObjects, writeOptions).Result;
            BulkReferenceObjects.Clear();
            ReferenceObjects = _database.GetCollection<ReferenceObject>("ReferenceObjects");
        }
        public void UpdateBulkObjectsReal()
        {
            if (BulkObjectsReal.Count < 1) return;
            BulkWriteResult<ObjectReal> result = ObjectsReal.BulkWriteAsync(BulkObjectsReal, writeOptions).Result;
            BulkObjectsReal.Clear();
            ObjectsReal = _database.GetCollection<ObjectReal>("ObjectReal");
        }

        public bool ExistsEPC(string epc)
        {
            return ObjectsReal.Find(x => x.EPC == epc).ToList().Count > 0;
        }

        public bool ExistsRegister(string id)
        {
            if (id == "0") return false;
            return ObjectsReal.Find(x => x.id == new ObjectId(id)).ToList().Count > 0;
        }

        public bool ExistsLocation(string id)
        {
            return Locations.Find(x => x.id == new ObjectId(id)).ToList().Count > 0;
        }

        public bool ExistsReference(string id)
        {
            return ReferenceObjects.Find(x => x.id == new ObjectId(id)).ToList().Count > 0;
        }

    }
}
