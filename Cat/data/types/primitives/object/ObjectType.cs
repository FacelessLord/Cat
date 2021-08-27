using System.Collections.Generic;
using System.Linq;
using Cat.data.exceptions;
using Cat.data.objects;
using Cat.data.objects.api;
using Cat.data.objects.primitives;
using Cat.data.properties.api;

namespace Cat.data.types.primitives.@object
{
    public class ObjectType : DataType
    {
        private const string TypeName = "Object";
        private const string TypeFullName = TypesPaths.System + TypeName;

        private static List<ObjectMethod> BaseMethodsFactory(TypeStorage types) =>
            new List<ObjectMethod> { new ToStringMethod(types) };

        private List<ObjectMethod> _baseMethods;

        public HashSet<IDataProperty> BaseProperties(TypeStorage types)
        {
            _baseMethods = BaseMethodsFactory(types);
            return _baseMethods
                .Cast<IDataProperty>()
                .ToHashSet();
        }

        public ObjectType(TypeStorage types) : base(TypeName, TypeFullName)
        {
            Properties = BaseProperties(types);
        }

        public override void PopulateObject(IDataObject dataObject)
        {
            dataObject.SetupProperties(_baseMethods.ToDictionary(p => p.Name, p => p as IDataObject));
        }

        public virtual IDataObject NewInstance(params object[] args)
        {
            return new DataObject(this);
        }

        public override IDataObject CreateInstance(params object[] args)
        {
            var obj = NewInstance(args);
            PopulateObject(obj);
            return obj;
        }
    }
}