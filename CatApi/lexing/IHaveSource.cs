namespace CatApi.lexing
{
    public interface IHaveSource
    {
        public string SourceFileName { get; }
        public int SourceLine { get; }
        public int SourceStartingCharacter { get; }
        public int SourceEndingCharacter { get; }

        public void FillSource(string sourceFileName, int sourceLine, int sourceStartingCharacter,
            int sourceEndingCharacter);
    }
}