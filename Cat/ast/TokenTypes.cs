using System.Linq;

namespace Cat.ast
{
    public static class TokenTypes
    {
        #region control characters

        public static ITokenType Semicolon = new SingleTokenType("Semicolon",";");
        public static ITokenType Colon = new SingleTokenType("Colon",";");
        public static ITokenType Comma = new SingleTokenType("Comma",",");

        #endregion

        #region operators

        public static ITokenType And = new SingleTokenType("And","&");
        public static ITokenType Or = new SingleTokenType("Or","|");

        #endregion

        #region parentheses

        public static ITokenType LParen = new SingleTokenType("LParen", "(");
        public static ITokenType RParen = new SingleTokenType("RParen", ")");
        public static ITokenType LBracket = new SingleTokenType("LBracket", "[");
        public static ITokenType RBracket = new SingleTokenType("RBracket", "]");
        public static ITokenType LBrace = new SingleTokenType("LBrace", "{");
        public static ITokenType RBrace = new SingleTokenType("RBrace", "}");

        #endregion

        #region keywords

        public static ITokenType Let = new KeyWordTokenType("let");

        #endregion

        #region literals

        public static ITokenType number = new NumberTokenType();
        public static ITokenType @string = new StringTokenType();

        #endregion
    }
}