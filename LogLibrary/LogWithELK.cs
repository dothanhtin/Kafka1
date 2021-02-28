using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LogLibrary
{
    public class LogWithELK : ILogLibraryInterface
    {
        private static readonly Logger _logger;
        static LogWithELK()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }
        public async Task WriteAsync(string message, string keySearch)
        {
            var logModel = new LoggingClass
            {
                message = message,
                keySearch = keySearch,
                ip = getIpClient()
            };
            Action actLog = () =>
            {
                _logger.Info($"Info|{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} {JsonConvert.SerializeObject(logModel)}");
                Task.Delay(100);
            };
            var writeLogTask = new Task(actLog);
            writeLogTask.Start();
            await writeLogTask;
        }

        private static string getIpClient()
        {
            string ipAddress = string.Empty;
            try
            {
                if (Dns.GetHostAddresses(Dns.GetHostName()).Length > 0)
                    ipAddress = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
                else
                    ipAddress = "x.x.x.x";
            }
            catch (Exception ex)
            {
                _logger.Info($"getIpClient: {ex}");
            }
            return ipAddress;
        }
    }
    public class LoggingClass
    {
        public string message { get; set; }
        public string ip { get; set; }
        public string keySearch { get; set; }
        public string appname { get; set; }
        public string type { get; set; }
        public string document_type { get; set; }
        public LoggingClass()
        {
            this.appname = "Kafka1";
            this.type = "tintest-logs";
            this.document_type = "tintest-logs";
        }
    }
}
