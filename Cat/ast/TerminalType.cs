using System;
using System.Collections.Generic;
using System.Linq;

namespace Cat.ast
{
    public interface ITokenType
    {
        public bool IsOfType(string token);

        public string Name { get; }
    }

    public class SingleTokenType : ITokenType
    {
        private readonly string _terminal;

        public SingleTokenType(string name, string terminal)
        {
            _terminal = terminal;
            Name = name;
        }

        public bool IsOfType(string token)
        {
            return _terminal.Equals(token);
        }

        public string Name { get; }
    }

    public class KeyWordTokenType : SingleTokenType
    {
        public KeyWordTokenType(string terminal) : base(terminal, terminal)
        {
        }
    }


    public class ListTokenType : ITokenType
    {
        private readonly HashSet<string> _terminals;

        public ListTokenType(string name, IEnumerable<string> terminals)
        {
            Name = name;
            _terminals = terminals.ToHashSet();
        }

        public bool IsOfType(string token)
        {
            return _terminals.Contains(token);
        }

        public string Name { get; }
    }

    public class StringTokenType : ITokenType
    {
        public bool IsOfType(string token)
        {
            if (!(token.StartsWith("\"") && token.EndsWith("\"")) &&
                !(token.StartsWith("\'") && token.EndsWith("\'"))) return false;
            var escaped = false;

            for (int i = 1; i < token.Length - 1; i++)
            {
                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (token[i] == '\"' || token[i] == '\'')
                    return false;

                escaped = token[i] == '\\';
            }

            return !escaped;
        }

        public string Name { get; } = "string";
    }

    public class NumberTokenType : ITokenType
    {
        public bool IsOfType(string token)
        {
            var startsWithDigitOrMinus = token[0] == '-' || char.IsDigit(token[0]);
            var containsNoMoreThanOneDot = token.Split('.').Length <= 2;
            var everyOtherCharacterIsDigit = token
                .Replace("-", "")
                .Replace(".", "")
                .All(char.IsDigit);

            return startsWithDigitOrMinus && containsNoMoreThanOneDot && everyOtherCharacterIsDigit;
        }

        public string Name { get; } = "number";
    }

    public class IdTokenType : ITokenType
    {
        public bool IsOfType(string token)
        {
            var startsWithLetter = char.IsLetter(token[0]);
            var containsNoSpaces = !token.Contains(" ");

            return startsWithLetter && containsNoSpaces;
        }

        public string Name { get; } = "id";
    }
}