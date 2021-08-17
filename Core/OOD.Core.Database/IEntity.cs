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
    public interface IEntity: IXmlSerializable
    {
        public string ID { get; }
        protected string _id { get; set; }
        protected string _tag { get; set; }
        internal Database _db { get; set; }
        protected static void DeserializeItemFromXml<T>(XmlReader reader, Item<T> item)
            where T: IEquatable<T>
        {
            item.ReadXml(reader, false);
        }
        protected static void SerializeItemToXml<T>(XmlWriter writer, Item<T> item)
            where T : IEquatable<T>
        {
            item.WriteXml(writer, false);
        }
    }
}
