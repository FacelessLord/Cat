namespace CatLexing.tokens
{
    public interface ITokenType
    {
        public string Name { get; }
    }

    public class UnknownTokenType : ITokenType
    {
        public string Name { get; } = "Unknown";
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

    public class OperatorTokenType : ITokenType
    {
        public readonly string Terminal;

        public OperatorTokenType(string name, string terminal)
        {
            Terminal = terminal;
            Name = name;
        }

        public string Name { get; }
    }

    public class KeyWordTokenType : ITokenType
    {
        public string Name { get; }
        public readonly string Terminal;

        public KeyWordTokenType(string terminal)
        {
            Terminal = terminal;
            Name = terminal;
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

    public class BoolTokenType : ITokenType
    {
        public string Name { get; } = "bool";
    }

    public class IdTokenType : ITokenType
    {
        public string Name { get; } = "id";
    }
}