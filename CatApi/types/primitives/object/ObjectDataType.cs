using System.Collections.Generic;
using System.Linq;
using CatApi.bindings;
using CatApi.objects;
using CatApi.objects.primitives;
using CatApi.structures;
using CatApi.types.containers;
using CatApi.types.dataTypes;

namespace CatApi.types.primitives.@object
{
    public class ObjectDataType : DataType
    {
        private const string TypeName = "Object";
        public const string TypeFullName = TypesPaths.System + "." + TypeName;

        public static readonly HashSet<string> BaseMethodNames = new()
        {
            "toString"
        };

        public ObjectDataType() : base(TypeFullName)
        {
        }

        public static HashSet<ObjectMethod> BaseMethods { get; set; }
        public static HashSet<IDataProperty> BaseProperties { get; set; }

        public override HashSet<IDataProperty> Properties => BaseProperties;

        public override void PopulateObject(IDataObject dataObject, params IDataObject[] args)
        {
            dataObject.SetupProperties(BaseMethods.ToDictionary(p => p.Name, p => p as IDataObject));
        }
    }
}