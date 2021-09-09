namespace CatImplementations.typings.primitives
{
    // public class AnyType : ObjectType
    // {
    //     private const string TypeName = "Any";
    //     private const string TypeFullName = TypesPaths.System + "." + TypeName;
    //
    //     public AnyType(TypeStorage types) : base(types)
    //     {
    //         Name = TypeName;
    //         FullName = TypeFullName;
    //         Properties = new HashSet<IDataProperty>();
    //     }
    //
    //
    //     public HashSet<IDataProperty> Properties { get; }
    //     public string Name { get; }
    //     public string FullName { get; }
    //
    //     public override bool IsAssignableFrom(IDataType type)
    //     {
    //         return true;
    //     }
    //
    //     public override bool IsAssignableTo(IDataType type)
    //     {
    //         return false;
    //     }
    //
    //     public override bool IsEquivalentTo(IDataType type)
    //     {
    //         return type.Properties.Count == 0;
    //     }
    //
    //     public override IDataProperty GetProperty(string name)
    //     {
    //         return System.Array.Empty<IDataProperty>();
    //     }
    //
    //     public override bool HasProperty(string name)
    //     {
    //         return false;
    //     }
    // }
}