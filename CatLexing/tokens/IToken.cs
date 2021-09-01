using System;

namespace Cat.lexing.tokens
{
    public class Token
    {
        public Token(ITokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public ITokenType Type { get; }
        public string Value { get; }

        protected bool Equals(Token other)
        {
            return Equals(Type, other.Type) && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Token) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }

        public override string ToString()
        {
            return $"{Type.Name}({Value})";
        }
    }
}