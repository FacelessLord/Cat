using System.Collections.Generic;

namespace CatApi.types.primitives
{
    public static class PrimitivesNames
    {
        public const string Any = TypesPaths.System + "." + "Any",
            Never = TypesPaths.System + "." + "Never",
            Object = TypesPaths.System + "." + "Object",
            String = TypesPaths.System + "." + "String",
            Number = TypesPaths.System + "." + "Number",
            Bool = TypesPaths.System + "." + "Bool",
            Class = TypesPaths.System + "." + "Class";

        public static HashSet<string> All = new() { Any, Never, Object, String, Number, Bool, Class };
    }
    public static class Primitives
    {
        public static readonly TypeBox Any = new(PrimitivesNames.Any),
            Never = new(PrimitivesNames.Never),
            Object = new(PrimitivesNames.Object),
            String = new(PrimitivesNames.String),
            Number = new(PrimitivesNames.Number),
            Bool = new(PrimitivesNames.Bool),
            Class = new(PrimitivesNames.Class);

        public static HashSet<TypeBox> All = new() { Any, Never, Object, String, Number, Bool, Class };
    }
}