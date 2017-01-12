using System;

namespace MicroChallenge5
{
    class UserMessageEditor
    {
        private Message _message;
        private Logger _log;

        public UserMessageEditor(Message message, Logger log)
        {
            _message = message;
            _log = log;
        }

        public bool RunMenu()
        {
            string userChoice;
            bool shouldExit = false;
            Console.WriteLine("Which field would you like to edit?\n1. <to>\n2. <from>\n3. <heading>\n4. <body>\n5. Go back to previous menu\n");
            userChoice = Console.ReadLine().ToLower();
            switch (userChoice)
            {
                case "1":
                case "<to>":
                case "to":
                    // In all 4 cases, will set to exit if edit and parse return no errors
                    shouldExit = !EditText("/note/to");
                    break;
                case "2":
                case "<from>":
                case "from":
                    shouldExit = !EditText("/note/from");
                    break;
                case "3":
                case "<heading>":
                case "heading":
                    shouldExit = !EditText("/note/heading");
                    break;
                case "4":
                case "<body>":
                case "body":
                    shouldExit = !EditText("/note/body");
                    break;
                case "5":
                case "back":
                    shouldExit = true;
                    break;
                default:
                    Console.WriteLine("I'm sorry, I don't understand that input\n");
                    break;
            }
            return shouldExit;
        }

        private bool EditText(string xPath)
        {
            Console.WriteLine("What is the new text that you would like in this field of the .xml message?");
            var newText = Console.ReadLine().ToLower(); 
            var editor = new MessageEditor(_message);
            _message.UnvalidatedFile = editor.EditMessage(xPath, newText);
            var errors = _message.Parse();
            Console.WriteLine("Xml document {0}\n", errors == "" ? "validated" : $"failed validation\n{errors}" + "\nPlease try again");
            if (!(errors == "")) { _log.Log(errors, Levels.WARNING); }
            return !(errors == "");
            // return true if 'errors' string contains anything, false if empty.
        }
    }
}
