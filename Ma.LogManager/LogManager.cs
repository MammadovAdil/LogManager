using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Ma.LogManager
{
    /// <summary>
    /// Main log manager.
    /// </summary>
    public class LogManager
    {
        string LogFolderPath { get; set; }
        EventLog EventLog { get; set; }

        /// <summary>
        /// Initialize log manager.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When logFolderPath is null.
        /// </exception>
        /// <param name="logFolderPath">Folder path to write logs.</param>
        public LogManager(string logFolderPath)
        {
            if (logFolderPath == null)
                throw new ArgumentNullException(nameof(logFolderPath));

            this.LogFolderPath = logFolderPath;
        }

        public LogManager(EventLog eventLog)
        {
            if (eventLog == null)
                throw new ArgumentNullException(nameof(eventLog));

            this.EventLog = eventLog;
        }

        /// <summary>
        /// Create event source and log if it does not exist and initialize event log
        /// </summary>
        /// <param name="sourceName">Name of source</param>
        /// <param name="logName">Name of log</param>
        /// <returns>Initialized EventLog</returns>
        public static EventLog InitializeEventLog(
            string sourceName,
            string logName)
        {
            EventLog EventLogToInitialize;

            EventLogToInitialize = new EventLog();
            EventLogToInitialize.Source = sourceName;
            EventLogToInitialize.Log = logName;

            if (!EventLog.SourceExists(EventLogToInitialize.Source))
                EventLog.CreateEventSource(EventLogToInitialize.Source, EventLogToInitialize.Log);

            return EventLogToInitialize;
        }

        /// <summary>
        /// Write information to log.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When EventLog and logFolder.
        /// </exception>
        /// <param name="info">Information to log.</param>
        public void Log(string info)
        {
            if (EventLog == null
                && LogFolderPath == null)
                throw new InvalidOperationException(
                    "One of EventLog or logFolderLocation must be specified.");

            if (EventLog != null)
                WriteToWindowsLog(info);
            else if (!string.IsNullOrEmpty(LogFolderPath))
                WriteToLogFile(info);
        }

        /// <summary>
        /// Write details of exception to log.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When EventLog and logFolder.
        /// </exception>
        /// <param name="exceptionDetails">Details of exception.</param>
        public void Log(ExceptionDetails exceptionDetails)
        {
            if (EventLog == null
                && LogFolderPath == null)
                throw new InvalidOperationException(
                    "One of EventLog or logFolderLocation must be specified.");

            if (EventLog != null)
                WriteToWindowsLog(exceptionDetails);
            else if (!string.IsNullOrEmpty(LogFolderPath))
                WriteToLogFile(exceptionDetails);
        }

        /// <summary>
        /// Write error to file
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When exceptionDetails is null.
        /// </exception>
        /// <param name="exceptionDetails">Details of exception</param>
        private void WriteToLogFile(ExceptionDetails exceptionDetails)
        {
            if (exceptionDetails == null)
                throw new ArgumentNullException(nameof(exceptionDetails));

            string errorFileFullName = Path.Combine(LogFolderPath,
                string.Format("ErrorLog_{0}.txt",
                DateTime.Now.ToString("ddMMyyyy")));

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append("\n-----------------------------------------------------------------------\n");
            messageBuilder.AppendFormat("Method name:        {0}\n", exceptionDetails.MethodName);
            messageBuilder.AppendFormat("Special ID:         {0}\n", exceptionDetails.SpecialID);
            messageBuilder.AppendFormat("Error message:\n{0}\n", exceptionDetails.ErrorMessage);
            messageBuilder.AppendFormat("Note:               {0}\n", exceptionDetails.Note);
            messageBuilder.AppendFormat("Error date:         {0}\n", exceptionDetails.ErrorDate.ToString("dd.MM.yyyy HH:mm:ss"));
            messageBuilder.AppendFormat("StackTrace:\n{0}\n", exceptionDetails.StackTrace);
            messageBuilder.Append("\n=======================================================================\n");

            using (StreamWriter writer = new StreamWriter(errorFileFullName, true))
            {
                writer.Write(messageBuilder.ToString());
                writer.Flush();
            }
        }

        /// <summary>
        /// Write info to log file
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When info is null or empty.
        /// </exception>
        /// <param name="info">Info to be written to log</param>
        private void WriteToLogFile(string info)
        {
            if (string.IsNullOrEmpty(info))
                throw new ArgumentNullException(nameof(info));

            string infoFileFullName = Path.Combine(LogFolderPath, string.Format("ErrorLog_{0}.txt", DateTime.Now.ToString("ddMMyyyy")));

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append("\n-----------------------------------------------------------------------\n");
            messageBuilder.AppendFormat("Info:         {0}\n", info);
            messageBuilder.AppendFormat("Date:         {0}\n", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            messageBuilder.Append("\n=======================================================================\n");

            using (StreamWriter writer = new StreamWriter(infoFileFullName, true))
            {
                writer.Write(messageBuilder.ToString());
                writer.Flush();
            }
        }

        /// <summary>
        /// Write info to windows event log
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When exceptionDetails is null.
        /// </exception>
        /// <param name="exceptionDetails">Details of exception</param>
        private void WriteToWindowsLog(ExceptionDetails exceptionDetails)
        {
            if (exceptionDetails == null)
                throw new ArgumentNullException(nameof(exceptionDetails));

            if (exceptionDetails != null)
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendFormat("Method name:        {0}\n", exceptionDetails.MethodName);
                strBuilder.AppendFormat("Special ID:         {0}\n", exceptionDetails.SpecialID);
                strBuilder.AppendFormat("Error message:\n{0}\n", exceptionDetails.ErrorMessage);
                strBuilder.AppendFormat("Note:               {0}\n", exceptionDetails.Note);
                strBuilder.AppendFormat("StackTrace:\n{0}\n", exceptionDetails.StackTrace);

                EventLog.WriteEntry(strBuilder.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Write info to log file
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When info is null or empty.
        /// </exception>
        /// <param name="info">Info to be written to log</param>
        private void WriteToWindowsLog(string info)
        {
            if (string.IsNullOrEmpty(info))
                throw new ArgumentNullException(nameof(info));

            if (!string.IsNullOrEmpty(info))
                EventLog.WriteEntry(info);
        }
    }
}