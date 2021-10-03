using System;

namespace CatImplementations.ast
{
    public enum AccessModifier
    {
        Public = 0,
        Private = 1
    }

    public static class AccessModifiersHelper
    {
        public static AccessModifier FromString(string value)
        {
            return value switch
            {
                "public" => AccessModifier.Public,
                "private" => AccessModifier.Private,
                _ => throw new ArgumentException($"Value \"{value}\" can't be converted into AccessModifier")
            };
        }
    }
}