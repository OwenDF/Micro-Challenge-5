using System;
using System.IO;

namespace MicroChallenge5
{
    public class Logger
    {
        private string _logFileName;

        public Logger(string fileName)
        {
            _logFileName = $"Logs\\{fileName}";
            Directory.CreateDirectory("Logs");
        }

        public bool Log(string message, Levels Level)
        {
            DateTime currentDateTime = DateTime.Now;
            try
            {
                using (TextWriter logFile = File.AppendText(_logFileName))
                {
                    logFile.Write($"{currentDateTime}  {Level.ToString()}  {message}\n");
                    logFile.Close();
                    return true;
                }
            }
            catch (SystemException) { return false; }
        }

        public bool MoveOldLog()
        {
            DateTime currentDateTime = DateTime.Now;
            if (File.Exists(_logFileName))
            {
                try
                {
                    File.Move(_logFileName, $"{_logFileName}{currentDateTime}");
                    return true;
                }
                catch (IOException ex)
                {
                    throw ex;
                }
            }
            else { return true; }
        }
    }
}
