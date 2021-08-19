using OOD.Core.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OOD.Core.Database.Tests
{
    public class EntityObject: OOD.Core.Database.Entity, IEquatable<EntityObject>
    {
        public Item<string> StringValue;
        public Item<int> IntValue;
        public Item<bool> BoolValue;
        private new string _tag = "EntityObject";

        public EntityObject()
        {
            Clear();
        }

        public EntityObject(string id)
        {
            _id = id;
            Clear();
        }
        public EntityObject(string id, string stringObject, int intObject, bool boolObject): this(id)
        {
            StringValue.Value = stringObject;
            IntValue.Value = intObject;
            BoolValue.Value = boolObject;
        }

        protected void Clear()
        {
            StringValue = new Item<string>("StringValue", "default string value");
            IntValue = new Item<int>("IntValue", 32);
            BoolValue = new Item<bool>("BoolValue", true);
        }

        public override void ReadXml(XmlReader reader, bool includeStartEndElement = true)
        {
            Clear();
            try { _id = reader.GetAttribute(0); } catch { }
            if (XmlSerializable.AttemptReadStartElement(reader, _tag, includeStartEndElement))
            {
                StringValue.ReadXml(reader);
                IntValue.ReadXml(reader);
                BoolValue.ReadXml(reader);
                XmlSerializable.AttemptReadEndElement(reader, includeStartEndElement);
            }
        }

        public override void WriteXml(XmlWriter writer, bool includeStartEndElement = true)
        {
            XmlSerializable.AttemptWriteStartElement(writer, _tag, includeStartEndElement);
            writer.WriteAttributeString("id", _id);
            StringValue.WriteXml(writer);
            IntValue.WriteXml(writer);
            BoolValue.WriteXml(writer);
            XmlSerializable.AttemptWriteEndElement(writer, includeStartEndElement);
        }

        public bool Equals(EntityObject other)
        {
            bool ids = _id == other._id;
            bool strings = StringValue == other.StringValue;
            bool ints = IntValue == other.IntValue;
            bool bools = BoolValue == other.BoolValue;
            return ids && strings && ints && bools;
        }
    }
}
