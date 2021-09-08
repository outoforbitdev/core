using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace OOD.Core.Collections
{
    public class SerializableHashSet<T>: HashSet<T>, Serializable.IXmlSerializable
    {
        public SerializableHashSet()
        {
        }

        #region XML
        public XmlSchema GetSchema()
        {
            return null;
        }

        #region Deserialize XML
        public void DeserializeFromXml(TextReader stream)
        {
            throw new NotImplementedException();
        }

        public void DeserializeFromXml(string str)
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader, bool ignoreItemTag = false)
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }
        #endregion Deserialize XML
        #region Serialize XML
        public void SerializeToXml(TextWriter stream)
        {
            throw new NotImplementedException();
        }

        public string SerializeToXml()
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer, bool ignoreItemTag = false)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
        #endregion Serialize XML
        #endregion XML
    }
}
