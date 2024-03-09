using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Psl.Chase.Utils
{
    /// <summary>
    /// Contains utility methods for persisting objects as XML documents
    /// </summary>
    public class PSLXmlSerializer
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public PSLXmlSerializer()
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Serializes an object as XML document.
        /// </summary>
        /// <param name="FileName">XML file name</param>
        /// <param name="TypeObj">Type of the object to be persisted</param>
        /// <param name="Obj">Object to be persisted</param>
        public static bool SerializeAsXML(String FileName, Type TypeObj, Object Obj)
        {
            TextWriter writer = null;

            try
            {
                // Create an instance of the XmlSerializer class and specify the type of object to serialize.
                XmlSerializer serializer = new XmlSerializer(TypeObj);
                writer = new StreamWriter(FileName);

                // Serialize the object and close the TextWriter.
                serializer.Serialize(writer, Obj);
                writer.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

            return true;
        }

        /// <summary>
        /// Serializes an object as XML document.
        /// </summary>
        /// <param name="FileName">XML file name</param>
        /// <param name="TypeObj">Type of the object to be persisted</param>
        /// <param name="Obj">Object to be persisted</param>
        public static bool SerializeAsXML(String FileName, Type TypeObj, Object Obj, Boolean bAppend)
        {
            TextWriter writer = null;

            try
            {
                // Create an instance of the XmlSerializer class and specify the type of object to serialize.
                XmlSerializer serializer = new XmlSerializer(TypeObj);
                writer = new StreamWriter(FileName, bAppend);

                // Serialize the object and close the TextWriter.
                serializer.Serialize(writer, Obj);
                writer.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

            return true;
        }

        /// <summary>
        /// Constructs object graph from a given XML document
        /// </summary>
        /// <param name="FileName">XML file name</param>
        /// <param name="TypeObj">Type of the obeject to be constructed</param>
        /// <returns>Constructed object</returns>
        public static Object DeserializeXML(String FileName, Type TypeObj)
        {
            XmlReader reader = null;

            try
            {
                // Create an instance of the XmlSerializer specifying type and namespace.
                XmlSerializer serializer = new XmlSerializer(TypeObj);

                // A FileStream is needed to read the XML document.
                FileStream fs = new FileStream(FileName, FileMode.Open);
                reader = new XmlTextReader(fs);

                Object obj = serializer.Deserialize(reader);

                reader.Close();

                return obj;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Constructs object graph from a given XML document
        /// </summary>
        /// <param name="typeObj">Type of the obeject to be constructed</param>
        /// <param name="xmlReaderAssembly">Assembly of the obeject to be constructed</param>
        /// <param name="xmlResourceString">Resource string of the xml in the manifest</param>
        /// <returns>Constructed object</returns>
        public static Object DeserializeXML(Type typeObj, System.Reflection.Assembly xmlReaderAssembly, string xmlResourceString)
        {
            System.Xml.XmlReader reader = null;

            try
            {
                // Create an instance of the XmlSerializer specifying type and namespace.
                XmlSerializer serializer = new XmlSerializer(typeObj);

                // Get the embedded xml stream from the Resource.
                Stream stream = xmlReaderAssembly.GetManifestResourceStream(xmlResourceString);
                reader = new XmlTextReader(stream);

                Object obj = serializer.Deserialize(reader);

                reader.Close();

                return obj;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        #endregion
    }
}
