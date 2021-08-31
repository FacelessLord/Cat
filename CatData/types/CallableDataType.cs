using System.Collections.Generic;
using System.Linq;
using Cat.data.types.api;
using Cat.data.types.primitives.@object;

namespace Cat.data.types
{
    public class CallableDataType : ObjectType, ICallableDataType
    {
        public CallableDataType(params IDataType[] args)
        {
            SourceTypes = args.Take(args.Length - 1).ToArray();
            TargetType = args[^1];

            ConstructorTypes = new List<(IDataType, Variancy)>(SourceTypes.Select(t => (t, Variancy.Contravariant)))
                { (TargetType, Variancy.Covariant) };

            Name = CreateFunctionName(args);
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
    }
}