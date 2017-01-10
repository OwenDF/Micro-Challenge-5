using System;
using System.Xml.Linq;
using System.IO;

namespace MicroChallenge5
{
    class MessageManipulator
    {
        private Message _message;
        private Logger _log;
        private MessageEmailer _emailer = null;

        public MessageManipulator(Message message, Logger log)
        {
            _message = message;
            _log = log;
        }

        public bool RunMenu()
        {
            string userChoice;
            bool shouldExit = false;
            Console.WriteLine("What would you like to do with the .xml file?\nPlease select from:\n\n1. View xml file\n2. Edit the file\n3. Save a copy of the file");
            Console.WriteLine("4. Email the file\n5. Edit email server\n6. View number of times this message has been emailed\n7. Go back to the previous menu\n\n");
            userChoice = Console.ReadLine().ToLower();
            switch (userChoice)
            {
                case "1":
                case "view":
                    Console.WriteLine("\n" + _message.View() + "\n\n");
                    Console.ReadKey();
                    break;
                case "2":
                case "edit":
                    try
                    {
                        UserEdit();
                        // Decided that we should have the option to save as a new file seperated, edit function contains the save function, but this way we can save an unedited file as well if we choose.
                    }
                    catch
                    {
                        _log.Log("Error processing file.", Levels.ERROR);
                        Console.WriteLine("Error processing file. See log for details");
                    }
                    break;
                case "3":
                case "save":
                    try
                    {
                        UserSave();
                    }
                    catch
                    {
                        _log.Log("Error processing file.", Levels.ERROR);
                        Console.WriteLine("Error processing file. See log for details");
                    }
                    break;
                case "4":
                case "email":
                    // Send an email.
                    CreateEmailer();
                    try
                    {
                        UserSend();
                    }
                    catch
                    {
                        _log.Log("Error sending file.", Levels.ERROR);
                        Console.WriteLine("Error sending file. See log for details");
                    }
                    break;
                case "5":
                    // Set the SMTP server.
                    CreateEmailer();
                    UserSetIPAddrSMTPServ();
                    break;
                case "6":
                    Console.WriteLine($"So far {_message.NumberOfSentMessages} messages have been sent\n");
                    Console.ReadKey();
                    break;
                case "7":
                case "back":
                    shouldExit = true;
                    break;
                default:
                    Console.WriteLine("I'm sorry, I don't understand that input\n");
                    break;
            }
            return shouldExit;
        }

        private void UserSave()
        {
            UserSave(_message.GetValidatedFile());
        }

        private void UserSave(XDocument saveFile)
        {
            Console.WriteLine("Please select a name to save the file as\n");
            string saveAs = Console.ReadLine();
            bool fileExists = File.Exists(saveAs);
            if (fileExists)
            {
                string aborted = "Save Aborted/n";
                Console.WriteLine("File already exists. Continue saving and overwrite?\ny/n\n");
                string userChoice = (Console.ReadLine());
                userChoice = userChoice.ToLower();
                switch (userChoice)
                {
                    case "y":
                    case "yes":
                        MessageEditor.Save(saveFile, saveAs, _message);
                        Console.WriteLine("File saved\n\n");
                        Console.ReadKey();
                        break;
                    case "n":
                    case "no":
                        Console.WriteLine(aborted);
                        break;
                    default:
                        Console.WriteLine("I'm sorry, I don't understand that input\n" + aborted);
                        break;
                }
            }
            else
            {
                MessageEditor.Save(saveFile, saveAs, _message);
                Console.WriteLine("File saved\n\n");
            }
        }

        private void UserEdit()
        {
            XDocument editedXml = MessageEditor.Edit(_message);
            UserSave(editedXml);
        }

        private void CreateEmailer()
        {
            if (_emailer == null)
            {
                var emailer = new MessageEmailer(_message);
                _emailer = emailer;
            }
        }

        private void UserSend()
        {
            Console.WriteLine("Please enter the email address to send from\n");
            string senderAddr = Console.ReadLine();
            Console.WriteLine("Please enter the email address to send to\n");
            string receiverAddr = Console.ReadLine();
            _emailer.Send(senderAddr, receiverAddr);
            Console.WriteLine("Email sent\n");
            Console.ReadKey();
        }

        private void UserSetIPAddrSMTPServ()
        {
            Console.WriteLine($"Current email server is {_emailer.GetIPAddrSMTPServ()}\nEdit? y/n\n");
            string userChoice = (Console.ReadLine());
            userChoice = userChoice.ToLower();
            if (userChoice == "y" || userChoice == "yes")
            {
                Console.WriteLine("Enter new email server\n");
                _emailer.SetIPAddrSMTPServ(Console.ReadLine());
                Console.WriteLine("Edit successful\n");
                Console.ReadKey();
            }
        }
    }
}
