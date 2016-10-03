using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using Mongo;

namespace HandheldDetector_wf
{
    public class SDF
    {
        public string Path { get; set; }
        public bool Exists { get; set; }
        private string connectionString { get; set; }
        public Location Region { get; set; }

        private string query = "select * from htk_Catalogo_Activos_Etiquetado";

        public SDF(string path)
        {
            Exists = File.Exists(path);
            connectionString = "Data Source = " + path + "; Persist Security Info = False;";
            Path = path;
        }

        public DataTable Read()
        {
            DataTable t = new DataTable();
            using (SqlCeConnection c = new SqlCeConnection(connectionString))
            {
                c.Open();
                using (SqlCeDataAdapter a = new SqlCeDataAdapter(query, c))
                {
                    a.Fill(t);
                }
            }
            return t;
        }
        public delegate void UpdateProgressHandler(int percentage);
        public event UpdateProgressHandler UpdateProgress;
        protected virtual void OnProgress(int percentage)
        {
            if (UpdateProgress != null)
                UpdateProgress(percentage);
        }
        public void Update(DataTable data, string db, string user, string pwd, string host)
        {
            Mongo.Mongo mongo = new Mongo.Mongo(db, user, pwd, host);
            bool regionChecked = false;
            Region = mongo.GetFirstRegion();
            string Creator = mongo.GetAdminUser();

            int total = data.Rows.Count+9;
            int faltan = data.Rows.Count;
            foreach (DataRow row in data.Rows)
            {
                faltan--;
                if(!regionChecked)
                {
                    var changeRegion = mongo.GetRegionOfSub(row["UB_ID_SUBUBICACION"].ToString());

                    if (changeRegion != null)
                        Region = changeRegion;
                    regionChecked = true;
                }
                string assetType = mongo.GetAssetType(row["AF_ID_ARTICULO"].ToString());
                ObjectReal asset = CastAsset(row, Creator, assetType);
                if (mongo.ExistsEPC(row["AF_EPC_COMPLETO"].ToString()))
                {
                    AddToUpdateList(mongo, row, Creator);
                }
                else
                {
                    if(mongo.ExistsRegister(row["ID_REGISTRO"].ToString()))
                    {
                        mongo.ChangeTag(row["ID_REGISTRO"].ToString(), row["AF_EPC_COMPLETO"].ToString(), true);
                        AddToUpdateList(mongo, row, Creator);
                    }
                    else
                    {
                        mongo.Insert(asset);
                        InsertLocations(mongo, row, Creator);
                        InsertReference(mongo, row, Creator);
                    }
                }
                int progress = (total - faltan) * 100 / total;
                OnProgress(progress);
            }

            mongo.UpdateBulkLocations();
            OnProgress(93);
            mongo.UpdateBulkReferenceObjects();
            OnProgress(96);
            mongo.UpdateBulkObjectsReal();
            OnProgress(100);

        }

        private ReferenceObject InsertReference(Mongo.Mongo mongo, DataRow row, string creator)
        {
            string id = row["AF_ID_ARTICULO"].ToString();
            ReferenceObject reference = mongo.GetReferenceObject(id);
            if (reference == null)
            {
                Category noCategory = new Category("Sin Categoría", creator, "null");
                noCategory = mongo.Insert(noCategory);
                reference = new ReferenceObject(
                        id,
                        row["AF_DESC_ARTICULO"].ToString(),
                        row["AF_MARCA"].ToString(),
                        row["AF_MODELO"].ToString(),
                        creator,
                        noCategory
                    );
                mongo.Insert(reference);
            }
            return reference;
        }

        private Location InsertLocations(Mongo.Mongo mongo, DataRow row, string Creator)
        {
            Location conjunto = mongo.GetLocationByName(row["AF_NOMBRE_CONJUNTO"].ToString());
            Location ubicacion = mongo.GetLocation(row["UB_ID_UBICACION"].ToString());
            Location sububicacion = mongo.GetLocation(row["UB_ID_SUBUBICACION"].ToString());
            //Buscar Conjunto
            if (conjunto == null)
            {
                conjunto = new Location(row["AF_NOMBRE_CONJUNTO"].ToString(), Creator, Region, mongo.ConjuntoProfile);
                mongo.Insert(conjunto);
            }
            //Buscar Ubicación
            if (ubicacion == null)
            {
                ubicacion = new Location(row["UB_ID_UBICACION"].ToString(),row["AF_UBICACION"].ToString(), Creator, conjunto, mongo.UbicacionProfile);
                mongo.Insert(ubicacion);
            }
            //BuscarSubUbicacion
            if (sububicacion == null)
            {
                sububicacion = new Location(row["UB_ID_SUBUBICACION"].ToString(),row["AF_SUBUBICACION"].ToString(), Creator, ubicacion, mongo.SubUbicacionProfile);
                mongo.Insert(sububicacion);
            }
            return sububicacion;
        }

        private void AddToUpdateList(Mongo.Mongo mongo, DataRow row, string creator)
        {
            ObjectReal asset = CastAsset(row, creator);
            Location location = InsertLocations(mongo, row, creator);
            ReferenceObject reference = InsertReference(mongo, row, creator);
            mongo.ChangeSerial(asset, row["AF_NUM_SERIE"].ToString(), true);
            mongo.ChangeLocation(asset, location, true);
            mongo.ChangeReference(asset, reference, true);
        }

        private ObjectReal CastAsset(DataRow row, string creator)
        {
            
            ObjectReal asset = new ObjectReal(row["ID_REGISTRO"].ToString())
            {
                EPC = row["AF_EPC_COMPLETO"].ToString(),
                location = row["UB_ID_SUBUBICACION"].ToString(),
                marca = row["AF_MARCA"].ToString(),
                modelo = row["AF_MODELO"].ToString(),
                name = row["AF_DESC_ARTICULO"].ToString(),
                objectReference = row["AF_ID_ARTICULO"].ToString(),
                quantity = row["AF_CANTIDAD"].ToString(),
                serie = row["AF_NUM_SERIE"].ToString(),
                Creator = creator
            };
            return asset;
        }
        private ObjectReal CastAsset(DataRow row, string creator, string assettype)
        {

            ObjectReal asset = new ObjectReal(row["ID_REGISTRO"].ToString())
            {
                EPC = row["AF_EPC_COMPLETO"].ToString(),
                location = row["UB_ID_SUBUBICACION"].ToString(),
                marca = row["AF_MARCA"].ToString(),
                modelo = row["AF_MODELO"].ToString(),
                name = row["AF_DESC_ARTICULO"].ToString(),
                objectReference = row["AF_ID_ARTICULO"].ToString(),
                quantity = row["AF_CANTIDAD"].ToString(),
                serie = row["AF_NUM_SERIE"].ToString(),
                Creator = creator,
                assetType = assettype
            };
            return asset;
        }

    }
}
