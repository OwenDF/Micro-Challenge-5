using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Xml;

namespace MicroChallenge5
{
    public class Message
    {
        private string _filePathToXSD;
        private int _numberOfSentMessages = 0;
        private static int _numberOfSentMessagesOverall = 0;
        private XDocument _unvalidatedFile, _validatedFile;
        private Logger _messageLog;
        private XmlNamespaceManager _namespaces;

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
                var xmlReader = new XmlTextReader(filename);
                XDocument xmlDoc = XDocument.Load(xmlReader);
                _unvalidatedFile = xmlDoc;
                XmlNameTable nameTable = xmlReader.NameTable;
                _namespaces = new XmlNamespaceManager(nameTable);
            }
            catch
            // may need to rethink the catch block, now that we have added more logic for namespaces to the function.
            {
                var exText = $"Specified file '{filename}'could not be loaded";
                _messageLog.Log(exText, Levels.ERROR);
                throw new IOException(exText);
            }
        }

        public void SetXSD(string filename) { _filePathToXSD = filename; }

        public string Parse() { return Parse(_filePathToXSD); }

        public string Parse(string xsdfile)
        {
            try
            {
                if (_filePathToXSD == null)
                {
                    SetXSD(xsdfile);
                }
                var schemaSet = new XmlSchemaSet();
                schemaSet.Add("http://www.w3schools.com", _filePathToXSD);
                return ParseAgainstSchema(schemaSet);
            }
            catch (XmlSchemaException ex)
            {
                _messageLog.Log($"Error validating file {ex}", Levels.ERROR);
                throw;
            }
            catch (IOException ex)
            {
                _messageLog.Log($"Error loading specified .xsd file {ex}", Levels.ERROR);
                throw;
            }
        }

        private string ParseAgainstSchema(XmlSchemaSet schemas)
        {
            var errors = new StringBuilder("");

            _unvalidatedFile.Validate(schemas, (o, e) =>//I literally have no idea what this is or what it does but it seems to work.
            {
                errors.Append(e.Message);
            });
            if (errors.ToString() == "") _validatedFile = _unvalidatedFile;
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