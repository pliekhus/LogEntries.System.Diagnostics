using LogentriesCore.Net;
using System;
using System.Configuration;
using System.Diagnostics;

namespace Diagnostics.Listeners
{
    public class LogEntriesListener : TraceListener
    {
        private AsyncLogger _logEntries;

        public LogEntriesListener()
        {
            _logEntries = new AsyncLogger();

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.Token"])) { Token = ConfigurationManager.AppSettings["LogEntries.Token"]; }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.AccountKey"])) { AccountKey = ConfigurationManager.AppSettings["LogEntries.AccountKey"]; }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.Location"])) { Location = ConfigurationManager.AppSettings["LogEntries.Location"]; }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.ImmediateFlush"])) { ImmediateFlush = bool.Parse(ConfigurationManager.AppSettings["LogEntries.ImmediateFlush"]); }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.Debug"])) { Debug = bool.Parse(ConfigurationManager.AppSettings["LogEntries.Debug"]); }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.UseHttpPut"])) { UseHttpPut = bool.Parse(ConfigurationManager.AppSettings["LogEntries.UseHttpPut"]); }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.UseSsl"])) { UseSsl = bool.Parse(ConfigurationManager.AppSettings["LogEntries.UseSsl"]); }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.IsUsingDataHub"])) { IsUsingDataHub = bool.Parse(ConfigurationManager.AppSettings["LogEntries.IsUsingDataHub"]); }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.DataHubAddr"])) { DataHubAddr = ConfigurationManager.AppSettings["LogEntries.DataHubAddr"]; }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.DataHubPort"])) { DataHubPort = int.Parse(ConfigurationManager.AppSettings["LogEntries.DataHubPort"]); }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.LogHostname"])) { LogHostname = bool.Parse(ConfigurationManager.AppSettings["LogEntries.LogHostname"]); }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.HostName"])) { HostName = ConfigurationManager.AppSettings["LogEntries.HostName"]; }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.LogID"])) { LogID = ConfigurationManager.AppSettings["LogEntries.LogID"]; }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogEntries.LogPattern"])) { LogPattern = ConfigurationManager.AppSettings["LogEntries.LogPattern"]; }
        }

        private string _logPattern = "%e% %u% %t% %m%";
        public string LogPattern { get { return _logPattern; } set { _logPattern = value; } }

        public string Token
        {
            get { return _logEntries.getToken(); }
            set { _logEntries.setToken(value); }
        }

        public String AccountKey
        {
            get { return _logEntries.getAccountKey(); }
            set { _logEntries.setAccountKey(value); }
        }

        public String Location
        {
            get { return _logEntries.getLocation(); }
            set { _logEntries.setLocation(value); }
        }

        public bool ImmediateFlush
        {
            get { return _logEntries.getImmediateFlush(); }
            set { _logEntries.setImmediateFlush(value); }
        }

        public bool Debug
        {
            get { return _logEntries.getDebug(); }
            set { _logEntries.setDebug(value); }
        }

        public bool UseHttpPut
        {
            get { return _logEntries.getUseHttpPut(); }
            set { _logEntries.setUseHttpPut(value); }
        }

        public bool UseSsl
        {
            get { return _logEntries.getUseSsl(); }
            set { _logEntries.setUseSsl(value); }
        }

        public bool IsUsingDataHub
        {
            get { return _logEntries.getIsUsingDataHab(); }
            set { _logEntries.setIsUsingDataHub(value); }
        }

        public String DataHubAddr
        {
            get { return _logEntries.getDataHubAddr(); }
            set { _logEntries.setDataHubAddr(value); }
        }

        public int DataHubPort
        {
            get { return _logEntries.getDataHubPort(); }
            set { _logEntries.setDataHubPort(value); }
        }

        public bool LogHostname
        {
            get { return _logEntries.getUseHostName(); }
            set { _logEntries.setUseHostName(value); }
        }

        public String HostName
        {
            get { return _logEntries.getHostName(); }
            set { _logEntries.setHostName(value); }
        }

        public String LogID
        {
            get { return _logEntries.getLogID(); }
            set { _logEntries.setLogID(value); }
        }


        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            string formattedMessage = LogPattern.Replace("%e%", Environment.MachineName);
            formattedMessage = formattedMessage.Replace("%u%", Environment.UserName);
            formattedMessage = formattedMessage.Replace("%t%", string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
            formattedMessage = formattedMessage.Replace("%m%", message);
            _logEntries.AddLine(formattedMessage);
        }

        public override void Write(object o)
        {
            base.Write(o);
        }

        public override void Write(object o, string category)
        {
            base.Write(o, category);
        }

        public override void Write(string message, string category)
        {
            base.Write(message, category);
        }

        public override void WriteLine(object o)
        {
            base.WriteLine(o);
        }

        public override void WriteLine(object o, string category)
        {
            base.WriteLine(o, category);
        }

        public override void WriteLine(string message, string category)
        {
            base.WriteLine(message, category);
        }

        public static void ShutdownLogging()
        {
            int waits = 3;
            while (!AsyncLogger.AreAllQueuesEmpty(TimeSpan.FromSeconds(30)) && waits > 0)
            {
                waits--;
            }

        }
    }
}
