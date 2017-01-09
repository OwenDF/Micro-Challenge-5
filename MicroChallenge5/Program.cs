using System;
using System.IO;

namespace MicroChallenge5
{
    public enum Levels { INFO, WARNING, ERROR }

    class Program
    {
        static private Logger _mainlog;

        static void Main()
        {
            var _logFileName = "Log.txt";
            _mainlog = new Logger(_logFileName);
            _mainlog.Log("Program started.", Levels.INFO);
            Console.WriteLine("Welcome to Owen's program...\n");
            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = RunProgram();
            }
        }

        static bool RunProgram()
        {
            string userChoice;
            Message xmlMessage;

            var shouldExit = false;
            Console.WriteLine("\nPlease select one of the following options:\n\n1. Load and validate new XML file\n2. Exit\n");
            userChoice = (Console.ReadLine());
            userChoice = userChoice.ToLower();
            switch (userChoice)
            {
                case "exit":
                case "e":
                case "2":
                    shouldExit = true;
                    break;
                case "1":
                case "xml":
                case "load":
                    try
                    {
                        xmlMessage = CreateMessage();
                        if (!ValidateMessage(xmlMessage))
                        {
                            ManipulateMessage(xmlMessage);
                        }
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("\n" + ex.Message);
                        Console.ReadKey();
                    }
                    break;
                default:
                    Console.WriteLine("I'm sorry, I don't understand that input\n");
                    break;
            }
            return shouldExit;
        }

        static Message CreateMessage()
        {
            string xmlFileName;
            Message xmlMessage;

            Console.WriteLine("Please enter the name of the xml file (including the .xml extension)");
            xmlFileName = Console.ReadLine();
            xmlMessage = new Message(_mainlog, xmlFileName);
            return xmlMessage;
        }

        static bool ValidateMessage(Message messageToValidate)
        {
            string xsdFileName, errors;

            Console.WriteLine("Please enter the name of the xsd file to validate against (including the .xsd extension");
            xsdFileName = Console.ReadLine();
            errors = messageToValidate.Parse(xsdFileName);
            Console.WriteLine("Xml document {0}\n", errors == "" ? "validated" : $"failed validation\n{errors}");
            return !(errors == "");
            // return true if 'errors' string contains anything, false if empty.
        }

        static void ManipulateMessage(Message valMessage)
        {
            bool shouldExit = false;
            var manipulator = new MessageManipulator(valMessage, _mainlog);
            while (!shouldExit)
            {
                shouldExit = manipulator.RunMenu();
            }
        }
    }

    public class Logger
    {
        private string _logFileName;

        public Logger(string fileName)
        {
            _logFileName = fileName;
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
    }
}
