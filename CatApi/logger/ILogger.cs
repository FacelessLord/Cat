namespace CatApi.logger
{
    public interface ILogger
    {
        public void Log(string level, string source, string message);
    }
}