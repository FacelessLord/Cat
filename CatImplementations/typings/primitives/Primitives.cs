using System.Collections.Generic;

namespace CatImplementations.typings.primitives
{
    public static class Primitives
    {
        public const string Any = TypesPaths.System + "." + "Any",
            Never = TypesPaths.System + "." + "Never",
            Object = TypesPaths.System + "." + "Object",
            String = TypesPaths.System + "." + "String",
            Number = TypesPaths.System + "." + "Number",
            Bool = TypesPaths.System + "." + "Bool",
            Class = TypesPaths.System + "." + "Class";

        public static HashSet<string> All = new HashSet<string>() { Any, Never, Object, String, Number, Bool, Class };
    }
}