using Cat.data.exceptions;
using Cat.data.properties;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.objects.api
{
    public abstract class ObjectMethod : CallableDataObject, IDataProperty
    {
        protected ObjectMethod(IDataType function, string methodName, PropertyMeta methodMeta, AccessRight accessRights=AccessRight.Rwc) : base(function)
        {
            Name = methodName;
            Meta = methodMeta;
            AccessRights = accessRights;
        }

        public string Name { get; }
        public PropertyMeta Meta { get; }
        public AccessRight AccessRights { get; }
        /**
         * Type where this method declared
         */
        public IDataType DeclaringType { get; private set; }
        
        public IDataObject GetValue(IDataObject owner)
        {
            if ((AccessRights & AccessRight.Read) != 0)
                return owner.GetProperty(this);
            throw new CatIllegalPropertyAccessException(AccessRight.Read, DeclaringType, this);
        }

        public bool HasValue(IDataObject owner)
        {
            return owner.HasProperty(this);
        }

        public void SetValue(IDataObject owner, IDataObject value)
        {
            if ((AccessRights & AccessRight.Write) != 0)
                owner.SetProperty(this, value);
            throw new CatIllegalPropertyAccessException(AccessRight.Write, DeclaringType, this);
        }

        public void CallValue(IDataObject owner, params IDataObject[] args)
        {
            if ((AccessRights & AccessRight.Call) != 0)
                owner.CallProperty(this, args);
            throw new CatIllegalPropertyAccessException(AccessRight.Call, DeclaringType, this);
        }

        public void SetDeclaringType(IDataType type)
        {
            if (DeclaringType != null)
                throw new CatSecurityException("Declaring type can be set only once");
            DeclaringType = type;
        }

        public bool IsReplaceableBy(IDataProperty replacement)
        {
            return Name == replacement.Name && replacement.Type.IsAssignableFrom(Type) &&
                   (AccessRights & replacement.AccessRights) == AccessRights;
        }
    }
}