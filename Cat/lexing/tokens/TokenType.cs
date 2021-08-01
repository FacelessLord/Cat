namespace Cat.lexing.tokens
{
    public interface ITokenType
    {
        public string Name { get; }
    }

    public class UnknownTokenType : ITokenType
    {
        public string Name { get; } = "Unknown";
    }
    public class RepetitionVariantTokenType : ITokenType
    {
        public readonly string Terminal;

        public RepetitionVariantTokenType(string name, string terminal)
        {
            Terminal = terminal;
            Name = name;
        }
        public string Name { get; }
    }

    public class SimpleTokenType : ITokenType
    {
        public readonly string Terminal;

        public SimpleTokenType(string name, string terminal)
        {
            Terminal = terminal;
            Name = name;
        }

        public string Name { get; }
    }

    public class KeyWordTokenType : SimpleTokenType
    {
        public KeyWordTokenType(string terminal) : base(terminal, terminal)
        {
        }
    }

    public class StringTokenType : ITokenType
    {
        public string Name { get; } = "string";
    }

    public class NumberTokenType : ITokenType
    {
        public string Name { get; } = "number";
    }

    public class IdTokenType : ITokenType
    {
        public string Name { get; } = "id";
    }
}