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

        protected override void Clear()
        {
            StringValue = new Item<string>("StringValue", "default string value");
            IntValue = new Item<int>("IntValue", 32);
            BoolValue = new Item<bool>("BoolValue", true);
        }

        public override void ReadXml(XmlReader reader, bool ignoreItemTag = false)
        {
            try { _id = reader.GetAttribute(0); } catch { }
            XmlSerializable.AttemptReadStartTag(reader, _tag, ignoreItemTag);
            DeserializeItemFromXml(reader, StringValue);
            DeserializeItemFromXml(reader, IntValue);
            DeserializeItemFromXml(reader, BoolValue);
            XmlSerializable.AttemptReadEndTag(reader, ignoreItemTag);
        }

        public override void WriteXml(XmlWriter writer, bool ignoreItemTag = false)
        {
            XmlSerializable.AttemptWriteStartTag(writer, _tag, ignoreItemTag);
            writer.WriteAttributeString("id", _id);
            SerializeItemToXml(writer, StringValue);
            SerializeItemToXml(writer, IntValue);
            SerializeItemToXml(writer, BoolValue);
            XmlSerializable.AttemptWriteEndTag(writer, ignoreItemTag);
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
