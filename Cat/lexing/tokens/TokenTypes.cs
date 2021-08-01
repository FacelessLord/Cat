using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cat.lexing.tokens
{
    public static class TokenTypes
    {
        #region control characters

        public static readonly ITokenType Semicolon = new SimpleTokenType(nameof(Semicolon), ";");
        public static readonly ITokenType Colon = new SimpleTokenType(nameof(Colon), ":");
        public static readonly ITokenType Comma = new SimpleTokenType(nameof(Comma), ",");

        #endregion

        #region operators

        public static readonly ITokenType And = new SimpleTokenType(nameof(And), "&");
        public static readonly ITokenType Or = new SimpleTokenType(nameof(Or), "|");
        public static readonly ITokenType Plus = new SimpleTokenType(nameof(Plus), "+");
        public static readonly ITokenType Minus = new SimpleTokenType(nameof(Minus), "-");
        public static readonly ITokenType Star = new SimpleTokenType(nameof(Star), "*");
        public static readonly ITokenType Divide = new SimpleTokenType(nameof(Divide), "/");
        public static readonly ITokenType ExclamationMark = new SimpleTokenType(nameof(ExclamationMark), "!");
        public static readonly ITokenType At = new SimpleTokenType(nameof(At), "@");
        public static readonly ITokenType Hash = new SimpleTokenType(nameof(Hash), "#");
        public static readonly ITokenType Dollar = new SimpleTokenType(nameof(Dollar), "$");
        public static readonly ITokenType Percent = new SimpleTokenType(nameof(Percent), "%");
        public static readonly ITokenType Circumflex = new SimpleTokenType(nameof(Circumflex), "^");
        public static readonly ITokenType Set = new RepetitionVariantTokenType(nameof(Set), "=");
        public static readonly ITokenType Equals = new RepetitionVariantTokenType(nameof(Equals), "==");

        #endregion

        #region parentheses

        public static readonly ITokenType LParen = new SimpleTokenType(nameof(LParen), "(");
        public static readonly ITokenType RParen = new SimpleTokenType(nameof(RParen), ")");
        public static readonly ITokenType LBracket = new SimpleTokenType(nameof(LBracket), "[");
        public static readonly ITokenType RBracket = new SimpleTokenType(nameof(RBracket), "]");
        public static readonly ITokenType LBrace = new SimpleTokenType(nameof(LBrace), "{");
        public static readonly ITokenType RBrace = new SimpleTokenType(nameof(RBrace), "}");

        #endregion

        #region keywords

        public static readonly ITokenType Let = new KeyWordTokenType("let");

        #endregion

        #region literals

        public static readonly ITokenType Number = new NumberTokenType();
        public static readonly ITokenType String = new StringTokenType();
        public static readonly ITokenType Id = new IdTokenType();

        #endregion

        public static readonly ITokenType Unknown = new UnknownTokenType();

        public static Lazy<List<SimpleTokenType>> SingleTokenTypes = new(() => typeof(TokenTypes).GetFields()
            .Where(m => m.Name != "Types")
            .Select(field => field.GetValue(null) as SimpleTokenType)
            .Where(value => value != null)
            .ToList());

        public static Lazy<List<IGrouping<char, RepetitionVariantTokenType>>> RepeatedTokenTypes = new(() =>
            typeof(TokenTypes).GetFields()
                .Where(m => m.Name != "Types")
                .Select(field => field.GetValue(null) as RepetitionVariantTokenType)
                .Where(value => value != null)
                .GroupBy(t => t.Terminal[0])
                .ToList());
    }
}