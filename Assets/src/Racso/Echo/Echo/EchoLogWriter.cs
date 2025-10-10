namespace Racso.Echo
{
    public interface EchoLogWriter
    {
        public void WriteLog(LogLevel level, string system, string message);
    }
}