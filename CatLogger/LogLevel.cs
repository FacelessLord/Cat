namespace CatLexing
{
    public static class LogLevel
    {
        public static readonly string Debug = nameof(Debug),
            Info = nameof(Info),
            Warn = nameof(Warn),
            Error = nameof(Error),
            Fatal = nameof(Fatal);

        public static readonly string[] Levels = new[] { Debug, Info, Warn, Error, Fatal };
    }
}