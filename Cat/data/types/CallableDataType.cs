using System.Collections.Generic;
using System.Linq;
using Cat.data.properties.api;
using Cat.data.types.api;
using Cat.data.types.primitives.@object;

namespace Cat.data.types
{
    public class CallableDataType : ObjectType, ICallableDataType
    {
        public CallableDataType(TypeStorage types, params IDataType[] args) : base(types)
        {
            SourceTypes = args.Take(args.Length - 1).ToArray();
            TargetType = args[^1];

            Name= CreateFunctionName(args);
            FullName = CreateFunctionFullName(args);
        }

        public static string CreateFunctionFullName(params IDataType[] args)
        {
            return $"({string.Join(", ", args.Take(args.Length - 1).Select(t => t.FullName))}) -> {args[^1].FullName}";
        }

        public static string CreateFunctionName(params IDataType[] args)
        {
            return $"({string.Join(", ", args.Take(args.Length - 1).Select(t => t.Name))}) -> {args[^1].Name}";
        }

        public IDataType[] SourceTypes { get; }
        public IDataType TargetType { get; }

        public override bool IsAssignableFrom(IDataType type)
        {
            return type is CallableDataType callable
                   && SourceTypes.Length == callable.SourceTypes.Length
                   && SourceTypes.Select((t, i) => (t, i))
                       .All(p => p.t.IsAssignableFrom(callable.SourceTypes[p.i]))
                   && TargetType.IsAssignableTo(callable.TargetType);
        }
    }
}