using System;
using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Xml;

namespace MicroChallenge5
{
    public class Message
    {
        private int _numberOfSentMessages = 0;
        private static int _numberOfSentMessagesOverall = 0;
        private XDocument _unvalidatedFile, _validatedFile;
        private Logger _messageLog;
        private XmlNamespaceManager _namespaces;
        private XmlSchemaSet _schemas = new XmlSchemaSet();

        public XDocument UnvalidatedFile
        {
            set
            {
                _unvalidatedFile = value;
            }
        }

        public XmlNamespaceManager Namespaces
        {
            get
            {
                return _namespaces;
            }
        }

        public Message(string logName)
        {
            var log = new Logger(logName);
            _messageLog = log;
        }

        public Message(Logger log) { _messageLog = log; }//I couldn't decide.

        public Message(Logger log, string xmlFileName)
        {
            // In this constructor, the xml file is considered to be an essential part of the object and is therefore included here.
            // This is the function that will be used by our console application.
            _messageLog = log;
            Load(xmlFileName);
        }

        public void Load(string filename)
        {
            // The specification for the task asked for a standalone 'Load' function, so this has been left as public.
            try
            {
                using (var xmlReader = new XmlTextReader(filename))
                {
                    XDocument xmlDoc = XDocument.Load(xmlReader);
                    _unvalidatedFile = xmlDoc;
                    XmlNameTable nameTable = xmlReader.NameTable;
                    _namespaces = new XmlNamespaceManager(nameTable);
                }
            }
            catch (XmlException ex)
            {
                _messageLog.Log($"Error reading XML {ex}", Levels.ERROR);
                throw;
            }
            catch (ArgumentException ex) when (ex.Message == "The URL cannot be empty.\r\nParameter name: url")
            // For catching 0 input
            {
                _messageLog.Log($"Blank input {ex}", Levels.ERROR);
                throw new IOException("Error: no input");
            }
            catch (FileNotFoundException ex)
            // This exception occurs in the XmlTextReader constructor.
            {
                _messageLog.Log($"Error loading specified .xml file {ex}", Levels.ERROR);
                throw;
            }
            //catch
            //// may need to rethink the catch block, now that we have added more logic for namespaces to the function.
            //{
            //    var exText = $"Specified file '{filename}' could not be loaded.";
            //    _messageLog.Log(exText, Levels.ERROR);
            //    throw new IOException(exText);
            //}
        }

        public void AddSchema(string xsdFile, string xmlNamespace)
        {
            // Strictly speaking, this wasn't required, but seems sensible to allow for multiple schema files
            if (_schemas.Contains(xmlNamespace))
            {
                _messageLog.Log("Specified schema already added", Levels.INFO);
                return;
            }
            try
            {
                using (var xmlSchema = new XmlTextReader(xsdFile))
                {
                    _schemas.Add(xmlNamespace, xmlSchema);
                }
            }
            catch (FileNotFoundException ex)
            // Creation of XmlTextReader
            {
                _messageLog.Log($"Error loading specified file {ex}", Levels.ERROR);
                throw;
            }
            catch (ArgumentException ex) when (ex.Message == "The URL cannot be empty.\r\nParameter name: url")
            // For catching 0 input
            {
                _messageLog.Log($"Blank input {ex}", Levels.ERROR);
                throw new IOException("Error: no input");
            }
            catch (XmlException ex)
            // Creation of XmlTextReader
            {
                _messageLog.Log($"Error loading XSD file {ex}", Levels.ERROR);
                throw;
            }
        }

        public void ClearSchemas()
        {
            // Not sure if this would ever be needed...
            _schemas = new XmlSchemaSet();
        }

        public string Parse()
        {
            var errors = new StringBuilder("");

            _unvalidatedFile.Validate(_schemas, (o, e) =>//I literally have no idea what this is or what it does but it seems to work.
            {
                errors.Append(e.Message + "\n");
            });
            if (errors.ToString() == "") _validatedFile = _unvalidatedFile;
            else _messageLog.Log(errors.ToString(), Levels.WARNING);
            return errors.ToString();
            // Return an empty string if no errors.
        }

        public void AddDefaultNamespace(string xmlNamespace)
        {
            // This is a method for adding a namespace, really would like to improve this
            _namespaces.AddNamespace("e", xmlNamespace);
        }

        public string View() { return _validatedFile.ToString(); }

        public int GetNumberOfSentMessages() { return _numberOfSentMessages; }

        public XDocument GetValidatedFile() { return _validatedFile; }

        public void SetValidatedFile(XDocument NewFile) { _validatedFile = NewFile; }

        public void IncrementSentMessages()
        {
            _numberOfSentMessages++;
            _numberOfSentMessagesOverall++;
        }
    }
}