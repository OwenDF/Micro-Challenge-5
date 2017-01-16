using System;
using System.IO;
using System.Globalization;

namespace MicroChallenge5
{
    public class Logger
    {
        private string _logFileName;

        public Logger(string fileName)
        {
            _logFileName = $"Logs/{fileName}";
            Directory.CreateDirectory("Logs");
        }

        public bool Log(string message, Levels Level)
        {
            DateTime currentDateTime = DateTime.Now;
            try
            {
                using (TextWriter logFile = File.AppendText($"{_logFileName}.log"))
                {
                    logFile.Write($"{currentDateTime.ToString("o", CultureInfo.InvariantCulture)}  {Level.ToString()}  {message}\n");
                    logFile.Close();
                    return true;
                }
            }
            catch (SystemException) { return false; }
        }

        public bool MoveOldLog()
        {
            DateTime currentDateTime = DateTime.Now;
            if (File.Exists($"{_logFileName}.log"))
            {
                try
                {
                    File.Move($"{_logFileName}.log", $"{_logFileName}{currentDateTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) }.log");
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
