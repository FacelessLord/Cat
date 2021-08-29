﻿using System.Collections.Generic;
using System.Linq;
using Cat.data.objects.api;
using Cat.data.objects.primitives;

namespace Cat.data.types.primitives.@object
{
    public class ObjectType : DataType
    {
        private const string TypeName = "Object";
        private const string TypeFullName = TypesPaths.System + "." + TypeName;

        public static HashSet<ObjectMethod> BaseMethods = new HashSet<ObjectMethod>();

        public ObjectType(TypeStorage types) : base(TypeName, TypeFullName)
        {
        }

        public override void PopulateObject(IDataObject dataObject)
        {
            dataObject.SetupProperties(BaseMethods.ToDictionary(p => p.Name, p => p as IDataObject));
        }

        public override IDataObject NewInstance(params object[] args)
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