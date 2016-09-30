using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mongo
{
    public interface IMongo
    {
        void Insert(ObjectReal objectReal);
        void Insert(ReferenceObject referenceObject);
        void Insert(Location location);
        void Insert(Category category);

        ObjectReal GetObjectReal(string epc);
        List<ObjectReal> GetObjectRealList(string field, object value);
        List<ObjectReal> GetObjectRealList();
        ReferenceObject GetReferenceObject(string referenceId);
        List<ReferenceObject> GetReferenceObjectList(string field, object value);
        List<ReferenceObject> GetReferenceObjectList();
        Location GetLocation(string locationId);
        List<Location> GetLocationList(string field, object value);
        List<Location> GetLocationList();
        Category GetCategory(string categoryId);
        List<Category> GetCategoryList(string field, object value);
        List<Category> GetCategoryList();

        void ChangeLocation(ObjectReal objectReal, Location location);
        void ChangeSerial(ObjectReal objectReal, string serial);
        void ChangeReference(ObjectReal objectReal, ReferenceObject referenceObject);
        void ChangeTag(string registerId, string epc);
        void ChangeCategory(ReferenceObject referenceObject, Category category);

        void Remove(string epc);
        void Remove(ObjectReal objectReal);
        void Remove(Location location);
        void Remove(ReferenceObject referenceObject);
        void Remove(Category category);
    }
}
