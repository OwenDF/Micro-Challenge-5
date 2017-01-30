using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace MicroChallenge5
{
    public enum Levels { INFO, WARNING, ERROR }

    class Program
    {
        static private Logger _mainlog;

        static void Main()
        {
            var _logFileName = "Log";
            _mainlog = new Logger(_logFileName);
            _mainlog.MoveOldLog();
            // This function moves the logfile with the name specified to a new file with specified log file name + currDateTime
            // As such this function could be omitted, depending on whether we want one continous log file
            // or one discrete logfile each time we run the program
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
                    catch (XmlException ex)
                    {
                        Console.WriteLine("Error reading file\n" + ex.Message);
                        Console.ReadKey();
                    }
                    catch (XmlSchemaException ex)
                    {
                        Console.WriteLine("Error validating file\n" + ex.Message);
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
            messageToValidate.AddSchema(xsdFileName, "http://www.w3schools.com");
            errors = messageToValidate.Parse();
            Console.WriteLine("Xml document {0}\n", errors == "" ? "validated" : $"failed validation\n{errors}");
            if (!(errors == "")) { _mainlog.Log(errors, Levels.WARNING); }
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
}
