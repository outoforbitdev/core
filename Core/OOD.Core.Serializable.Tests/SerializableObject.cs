using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OOD.Core.Serializable.Tests
{
    public class SerializableObject : XmlSerializable, IEquatable<SerializableObject>
    {
        private const string _defaultTag = "SerializableObject";
        protected string _tag
        {
            get { return _defaultTag; }
        }
        public int Number;
        private const string _defaultNumberTag = "Number";
        protected string _numberTag
        {
            get { return _defaultNumberTag; }
        }
        private bool _boolean;
        private const string _defaultBooleanTag = "Boolean";
        protected string _booleanTag
        {
            get { return _defaultBooleanTag; }
        }

        public SerializableObject()
        {
            Number = 0;
            _boolean = true;
        }
        public SerializableObject(int number, bool boolean)
        {
            Number = number;
            _boolean = boolean;
        }

        public override void ReadXml(XmlReader reader, bool ignoreItemTag)
        {
            SerializableObject.AttemptReadStartTag(reader, _tag, ignoreItemTag);
            DeserializeIntProperty(reader, _numberTag, out Number);
            DeserializeBoolProperty(reader, _booleanTag, out _boolean);
            SerializableObject.AttemptReadEndTag(reader, ignoreItemTag);
        }

        public override void WriteXml(XmlWriter writer, bool ignoreItemTag)
        {
            SerializableObject.AttemptWriteStartTag(writer, _tag, ignoreItemTag);
            SerializeIntProperty(writer, _numberTag, Number);
            SerializeBoolProperty(writer, _booleanTag, _boolean);
            SerializableObject.AttemptWriteEndTag(writer, ignoreItemTag);
        }

        public bool Equals(SerializableObject other)
        {
            return
                Number == other.Number &&
                _boolean == other._boolean;
        }
    }
}
