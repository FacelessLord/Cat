using CatApi.objects;
using CatApi.structures;
using CatApi.types;
using CatImplementations.exceptions;
using CatImplementations.structures.properties;

namespace CatImplementations.structures.variables
{
    public class Variable : IVariable
    {
        public int AccessRights { get; }
        public IDataObject Value { get; private set; }
        public string Name { get; }

        public Variable(string name, AccessRight accessRights)
        {
            Name = name;
            AccessRights = (int) accessRights;
        }

        public void Set(IDataObject value)
        {
            if ((AccessRights & (int) AccessRight.Write) > 0)
            {
                Value = value;
            }

            throw new CatIllegalPropertyAccessException(AccessRight.Write, Name);
        }

        public IDataObject Get()
        {
            if ((AccessRights & (int) AccessRight.Write) > 0)
            {
                return Value;
            }

            throw new CatIllegalPropertyAccessException(AccessRight.Write, Name);
        }

        public IDataObject Call(IScope scope, IDataObject[] arguments)
        {
            if ((AccessRights & (int) AccessRight.Call) > 0)
            {
                return Value;
            }

            throw new CatIllegalPropertyAccessException(AccessRight.Call, Name);
        }

        public IDataType Type { get; private set; }

        public void AssignType(IDataType type)
        {
            Type = type;
        }
    }
}