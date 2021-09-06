using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CatLexing.tokens;

namespace CatLexing.parsers
{
    public class OperatorTokenParser : IParser
    {
        private readonly List<OperatorTokenType> _operators;

        public OperatorTokenParser(IEnumerable<OperatorTokenType> operators)
        {
            _operators = operators.ToList();
        }

        public (Token token, int length)? Parse(in string text)
        {
            var preOperator = _operators.ToList();
            var dOperatorWrongCharacter = new List<OperatorTokenType>();
            var dOperatorTooShort = new List<OperatorTokenType>();

            int i;
            for (i = 0; i < text.Length; i++)
            {
                foreach (var operatorToken in preOperator)
                {
                    if (operatorToken.Terminal.Length <= i)
                    {
                        dOperatorTooShort.Add(operatorToken);
                    }
                    else if (operatorToken.Terminal[i] != text[i])
                    {
                        dOperatorWrongCharacter.Add(operatorToken);
                    }
                }

                if (dOperatorWrongCharacter.Count + dOperatorTooShort.Count == preOperator.Count)
                {
                    break;
                }

                preOperator = preOperator.Except(dOperatorTooShort).Except(dOperatorWrongCharacter).ToList();
                dOperatorTooShort.Clear();
                dOperatorWrongCharacter.Clear();
            }

            if (dOperatorTooShort.Count == 1)
            {
                var tokenType = dOperatorTooShort.Single();
                return (new Token(tokenType, tokenType.Terminal), tokenType.Terminal.Length);
            }

            if (dOperatorTooShort.Count > 1)
                throw new AmbiguousMatchException("Can't decide which token to take from \"" + text[..(Math.Min(10, text.Length))] + "\"");
            
            return null;
        }

        public bool CanStartWith(char character)
        {
            return _operators.Any(t => t.Terminal[0] == character);
        }
    }
}