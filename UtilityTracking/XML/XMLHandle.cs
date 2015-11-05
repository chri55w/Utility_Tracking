using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;

namespace UtilityTracking.XML {

    public class FileChangedEventArgs : EventArgs
    {
        public string oldFileName;
        public string newFileName;

        public FileChangedEventArgs(string oldName, string newName) {
            oldFileName = oldName;
            newFileName = newName;
        }
    }
    
    public delegate void FileChanged(FileChangedEventArgs e);

    class XMLHandle {

        public static event FileChanged FileChange;

        static XDocument Loaded_XDoc = null;

        static XmlSchemaSet loaded_Schema = new XmlSchemaSet();

        private static string m_FileName;
        private static int m_RecordFilesCount = 0;

        public static void initialise() {
            if (!Directory.Exists(Properties.Settings.Default.RecordFileLocation)) {
                throw new Exception("Queue folder (" + Properties.Settings.Default.RecordFileLocation + ") does not exist");
            }
            m_RecordFilesCount = Directory.GetFiles(Properties.Settings.Default.RecordFileLocation, "*.xml").Count();
        }

        public static void loadXMLDoc(string fileName) {
            Loaded_XDoc = XDocument.Load(fileName);
            m_FileName = fileName;
        }

        public static void CreateXMLDoc(string fileName)
        {
            Loaded_XDoc = new XDocument(new XElement("Records"));
            Loaded_XDoc.Save(fileName);
        }

        public static void loadSchema(string fileName) {
            try {
                loaded_Schema.Add(null, fileName);
            } catch (Exception load_error) {
                throw new Exception("Failed to Load Schema (" + load_error + ")");
            }
        }

        public static bool validateXML() {
            if (loaded_Schema.Count < 1) {
                throw new Exception("Error: No schema loaded into XML_Handle");
            } else if (Loaded_XDoc == null) {
                throw new Exception("Error: No XML Doc loaded into XML_Handle");
            } else {
                Loaded_XDoc.Validate(loaded_Schema, (o, validate_error) =>
                {
                    //Throw as exception for better error handling outside event handler.
                    throw new Exception(validate_error.Message);
                });

                return true;
            }
        }

        public static IEnumerable<XElement> getElementsOfType(string elementName) {
            return Loaded_XDoc.Descendants(elementName);

        }

        private static void CheckFileSizeLimits() {
            if (Loaded_XDoc.Element("Records").Elements("Record").Count() >= Properties.Settings.Default.MaxRecordsPerDoc) {
                string saveLoc = SaveLocationWithDate();
                if (FileChange != null)
                {
                    FileChange(new FileChangedEventArgs(Properties.Settings.Default.RecordFileLocation + Properties.Settings.Default.XMLRecordName + ".xml", saveLoc));
                }
                Loaded_XDoc.Save(saveLoc);
                CreateXMLDoc(SaveLocation());
            }
        }
        
        public static void addXElement(string name, string parent, XElement[] children, XAttribute[] attributes)
        {
            XElement newXElement = new XElement(name);
            foreach (XElement xe in children) {
                newXElement.Add(xe);
            }
            foreach (XAttribute xa in attributes) {
                newXElement.Add(xa);
            }
            if (parent != null) {
                Loaded_XDoc.Element(parent).Add(newXElement);
            }
            Loaded_XDoc.Save(SaveLocation());
            CheckFileSizeLimits();
        }
        private static string SaveLocation()
        {
            return Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + ".xml";
        }
        private static string SaveLocationWithDate()
        {
            return Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + DateTime.Now.ToString("dd-MMM-yy (HH-mm-ss)") + ".xml";
        }

        public static void deleteFromFile(string filename, XElement element) {
            if (m_FileName == filename) {
                deleteElement(Loaded_XDoc, element);
                Loaded_XDoc.Save(filename);
            }
            else
            {
                XDocument DocAccess = new XDocument();
                DocAccess = XDocument.Load(filename);
                deleteElement(DocAccess, element);
                DocAccess.Save(filename);
            }
        }
        private static void deleteElement(XDocument doc, XElement element)
        {
            doc.Descendants(element.Name).Where(e => (DateTime)e.Attribute("DateTime") == (DateTime)element.Attribute("DateTime")).Remove();
        }
    }
}
