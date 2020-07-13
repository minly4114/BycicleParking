namespace ICS.Core.Engine.IProviders
{
    public interface ILog4netProvider
    {
        void Info<T>(string typeClass, string message, T obj);
        void Info(string typeClass, string message);
        void Debug<T>(string typeClass, string message, T obj);
        void Debug(string typeClass, string message);
        void Error<T>(string typeClass, string message, T obj);
        void Error(string typeClass, string message);
        void Fatal<T>(string typeClass, string message, T obj);
        void Fatal(string typeClass, string message);
        void Warn<T>(string typeClass, string message, T obj);
        void Warn(string typeClass, string message);
    }
}
