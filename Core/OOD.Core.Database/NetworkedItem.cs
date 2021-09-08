using OOD.Core.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OOD.Core.Database
{
    public class NetworkedItem<T>: XmlSerializable, IEquatable<Item<T>>
        where T : Entity, IEquatable<T>
    {
        private T _defaultValue;
        private string _entityID;
        private T _value;
        private new readonly string _tag;
        private static readonly System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        private Table<T> _table;
        public T Value
        {
            get
            {
                if (_useDefaultValue)
                {
                    return _defaultValue;
                }
                if (_value == null)
                {
                    T entityValue;
                    if (_table.TryGetEntity(_entityID, out entityValue))
                    {
                        _value = entityValue;
                    }
                }
                return _value;
            }
            set
            {
                _value = value;
                _useDefaultValue = false;
                _entityID = value.ID;
            }
        }
        public bool UsingDefaultValue { get { return _useDefaultValue; } }
        private bool _useDefaultValue;
        public NetworkedItem(string tag, Table<T> table, T defaultValue)
        {
            _tag = tag;
            _defaultValue = defaultValue;
            _useDefaultValue = true;
            _table = table;
        }
        public override void ReadXml(XmlReader reader, bool ignoreItemTag = false)
        {
            try
            {
                _useDefaultValue = !DeserializeCLRProperty(reader, _tag, out _entityID);
            }
            finally
            {

            }
        }

        public override void WriteXml(XmlWriter writer, bool ignoreItemTag = false)
        {
            if (!UsingDefaultValue)
            {
                SerializeCLRProperty(writer, _tag, _entityID);
            }
        }

        public bool Equals(Item<T> other)
        {
            return UsingDefaultValue == other.UsingDefaultValue && Value.Equals(other.Value);
        }

        public static bool operator ==(NetworkedItem<T> a, NetworkedItem<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NetworkedItem<T> a, NetworkedItem<T> b)
        {
            return !(a == b);
        }
    }
}
