using System.Collections.Generic;

namespace Cat.data.types.primitives
{
    public static class Primitives
    {
        public const string Any = TypesPaths.System + "." + "Any",
            Never = TypesPaths.System + "." + "Never",
            Object = TypesPaths.System + "." + "Object",
            String = TypesPaths.System + "." + "String",
            Number = TypesPaths.System + "." + "Number",
            Bool = TypesPaths.System + "." + "Bool";

        public static HashSet<string> All = new HashSet<string>() { Any, Never, Object, String, Number, Bool };
    }
}