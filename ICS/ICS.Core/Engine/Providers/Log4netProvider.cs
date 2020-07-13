using ICS.Core.Engine.IProviders;
using log4net;
using System;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace ICS.Core.Engine.Providers
{
    public class Log4netProvider : ILog4netProvider
    {
        private readonly ILog _log;

        public Log4netProvider()
        {
            _log = LogManager.GetLogger(typeof(Log4netProvider));
        }

        public void Debug<T>(string typeClass, string message, T obj)
        {
            _log.Debug($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}{ToXml(obj)}");
        }

        public void Debug(string typeClass, string message)
        {
            _log.Debug($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}");
        }

        public void Error<T>(string typeClass, string message, T obj)
        {
            _log.Error($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}{ToXml(obj)}");
        }

        public void Error(string typeClass, string message)
        {
            _log.Error($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}");
        }

        public void Fatal<T>(string typeClass, string message, T obj)
        {
            _log.Fatal($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}{ToXml(obj)}");
        }

        public void Fatal(string typeClass, string message)
        {
            _log.Fatal($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}");
        }

        public void Info<T>(string typeClass, string message, T obj)
        {
            _log.Info($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}{ToXml(obj)}");
        }

        public void Info(string typeClass, string message)
        {
            _log.Info($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}");
        }

        public void Warn<T>(string typeClass, string message, T obj)
        {
            _log.Warn($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}{ToXml(obj)}");
        }

        public void Warn(string typeClass, string message)
        {
            _log.Warn($"{typeClass}{Environment.NewLine}{message}{Environment.NewLine}");
        }

        private string ToXml<T>(T obj)
        {
            var dcs = new DataContractSerializer(typeof(T));
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
            {
                dcs.WriteObject(writer, obj);
            }
            return sb.ToString();
        }
    }
}
