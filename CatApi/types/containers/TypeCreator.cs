using System;
using System.Linq;
using CatApi.modules;
using CatApi.structures;
using CatApi.structures.properties;
using CatApi.types.dataTypes;

namespace CatApi.types.containers
{
    public class TypeCreator
    {
        private TypeStorage _typeStorage;

        public TypeCreator(TypeStorage typeStorage)
        {
            _typeStorage = typeStorage;
        }

        public IDataType CreateType(IMember member)
        {
            if (member is not ClassMember clazz)
            {
                var actualValue = member == null ? "null" : member.GetType().FullName;
                throw new ArgumentException(
                    $"Member to create type from should be ClassMember, not {actualValue}");
            }

            var fields = clazz.Members.Where(m => m is FieldMember).Cast<FieldMember>();
            var properties = fields.Select(CreateProperty);

            var methods = clazz.Members.Where(m => m is FunctionMember).Cast<FunctionMember>();
            properties = properties.Concat(methods.Select(CreateProperty));

            return new DefinedType(member.Name, properties.ToHashSet());
        }

        public IDataProperty CreateProperty(FieldMember field)
        {
            return new DataProperty(field.Type, field.Name);
        }

        public IDataProperty CreateProperty(FunctionMember field)
        {
            var functionTypeName = _typeStorage
                .Function(field.Args.Select(p => p.argType)
                    .Concat(new[] { field.ReturnType })
                    .ToArray());
            return new DataProperty(functionTypeName, field.Name);
        }
    }
}