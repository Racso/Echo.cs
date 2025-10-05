namespace Racso.Echo
{
    public interface LogWriter
    {
        public void WriteLog(LogLevel level, string system, string message);
    }
}