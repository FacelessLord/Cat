using System;
using System.Collections.Generic;
using System.Linq;

namespace CatLexing.tokens
{
    public static class TokenTypes
    {
        #region control characters

        public static readonly ITokenType Semicolon = new SimpleTokenType(nameof(Semicolon), ";");
        public static readonly ITokenType Colon = new SimpleTokenType(nameof(Colon), ":");
        public static readonly ITokenType Comma = new SimpleTokenType(nameof(Comma), ",");
        public static readonly ITokenType Dot = new SimpleTokenType(nameof(Dot), ".");

        #endregion

        #region operators

        public static readonly ITokenType PipelineAnd = new OperatorTokenType(nameof(PipelineAnd), "&");
        public static readonly ITokenType PipelineOr = new OperatorTokenType(nameof(PipelineOr), "|");
        public static readonly ITokenType And = new OperatorTokenType(nameof(And), "and");
        public static readonly ITokenType Or = new OperatorTokenType(nameof(Or), "or");
        public static readonly ITokenType Plus = new OperatorTokenType(nameof(Plus), "+");
        public static readonly ITokenType PlusEquals = new OperatorTokenType(nameof(PlusEquals), "+=");
        public static readonly ITokenType Minus = new OperatorTokenType(nameof(Minus), "-");
        public static readonly ITokenType MinusEquals = new OperatorTokenType(nameof(MinusEquals), "-=");
        public static readonly ITokenType Star = new OperatorTokenType(nameof(Star), "*");
        public static readonly ITokenType StarEquals = new OperatorTokenType(nameof(StarEquals), "*=");
        public static readonly ITokenType Divide = new OperatorTokenType(nameof(Divide), "/");
        public static readonly ITokenType DivideEquals = new OperatorTokenType(nameof(DivideEquals), "/=");
        public static readonly ITokenType ExclamationMark = new OperatorTokenType(nameof(ExclamationMark), "!");
        public static readonly ITokenType At = new OperatorTokenType(nameof(At), "@");
        public static readonly ITokenType Hash = new OperatorTokenType(nameof(Hash), "#");
        public static readonly ITokenType Dollar = new OperatorTokenType(nameof(Dollar), "$");
        public static readonly ITokenType Percent = new OperatorTokenType(nameof(Percent), "%");
        public static readonly ITokenType PercentEquals = new OperatorTokenType(nameof(PercentEquals), "%=");
        public static readonly ITokenType Circumflex = new OperatorTokenType(nameof(Circumflex), "^");
        public static readonly ITokenType Power = new OperatorTokenType(nameof(Power), "**");
        public static readonly ITokenType CircumflexEquals = new OperatorTokenType(nameof(CircumflexEquals), "^=");
        public static readonly ITokenType Set = new OperatorTokenType(nameof(Set), "=");
        public static readonly ITokenType Equals = new OperatorTokenType(nameof(Equals), "==");
        public static readonly ITokenType NotEquals = new OperatorTokenType(nameof(NotEquals), "!=");
        public static readonly ITokenType Greater = new OperatorTokenType(nameof(Greater), ">");
        public static readonly ITokenType GreaterOrEqual = new OperatorTokenType(nameof(GreaterOrEqual), ">=");
        public static readonly ITokenType Less = new OperatorTokenType(nameof(Less), "<");
        public static readonly ITokenType LessOrEqual = new OperatorTokenType(nameof(LessOrEqual), "<=");
        public static readonly ITokenType QuestionMark = new OperatorTokenType(nameof(QuestionMark), "?");
        public static readonly ITokenType QuestionMarkEquals = new OperatorTokenType(nameof(QuestionMarkEquals), "?=");
        public static readonly ITokenType TwoQuestionMark = new OperatorTokenType(nameof(TwoQuestionMark), "??");
        public static readonly ITokenType Tilda = new OperatorTokenType(nameof(TwoQuestionMark), "~");

        //todo move every SimpleTokenType to OperatorTokenType
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
        public static readonly ITokenType As = new KeyWordTokenType("as");
        public static readonly ITokenType Is = new KeyWordTokenType("is");

        #endregion

        #region literals

        public static readonly ITokenType Number = new NumberTokenType();
        public static readonly ITokenType String = new StringTokenType();
        public static readonly ITokenType Id = new IdTokenType();
        public static readonly ITokenType Bool = new BoolTokenType();
        
        #endregion

        public static readonly ITokenType Unknown = new UnknownTokenType();

        public static Lazy<List<SimpleTokenType>> SingleTokenTypes = new(() => typeof(TokenTypes).GetFields()
            .Select(field => field.GetValue(null) as SimpleTokenType)
            .Where(value => value != null)
            .ToList());

        public static Lazy<List<OperatorTokenType>> OperatorTokenTypes = new(() =>
            typeof(TokenTypes).GetFields()
                .Select(field => field.GetValue(null) as OperatorTokenType)
                .Where(value => value != null)
                .ToList());

    }
}