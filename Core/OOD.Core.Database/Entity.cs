using OOD.Core.Serializable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace OOD.Core.Database
{
    public abstract class Entity: IXmlSerializable
    {
        public string ID
        {
            get { return _id; }
            set 
            { 
                if (ID == null)
                {
                    _id = value;
                } 
            }
        }
        protected string _id;
        protected string _tag = "Entity";
        internal Database _db;

        protected abstract void Clear();
        public void DeserializeFromXml(TextReader stream)
        {
            Clear();
            XmlReader reader = XmlReader.Create(stream);
            ReadXml(reader, false);
            reader.Close();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            ReadXml(reader, false);
        }

        public abstract void ReadXml(XmlReader reader, bool ignoreItemTag = false);
        protected static void DeserializeItemFromXml<T>(XmlReader reader, Item<T> item)
            where T: IEquatable<T>
        {
            item.ReadXml(reader, false);
        }

        public void SerializeToXml(TextWriter stream)
        {
            XmlWriter writer = XmlWriter.Create(stream);
            WriteXml(writer, false);
            writer.Flush();
            writer.Close();
        }

        public abstract void WriteXml(XmlWriter writer, bool ignoreItemTag = false);

        public void WriteXml(XmlWriter writer)
        {
            WriteXml(writer, false);
        }
        protected static void SerializeItemToXml<T>(XmlWriter writer, Item<T> item)
            where T : IEquatable<T>
        {
            item.WriteXml(writer, false);
        }
    }
}
