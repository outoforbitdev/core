using OOD.Core.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OOD.Core.Database
{
    public class NetworkedItem<T>: Item<T>, IEquatable<Item<T>>
        where T : Entity, IEquatable<T>
    {
        private string _entityID;
        private Table<T> _table;
        public new T Value
        {
            get
            {
                if (_useDefaultValue)
                {
                    return _defaultValue;
                }
                if (_value == null)
                {
                    if (_table.TryGetEntity(_entityID, out T entityValue))
                    {
                        _value = entityValue;
                    }
                }
                return _value;
            }
            set
            {
                base.Value = value;
                _entityID = value.ID;
            }
        }
        public NetworkedItem(string tag, Table<T> table, T defaultValue): base(tag, defaultValue)
        {
            _table = table;
        }
        public override void ReadXml(XmlReader reader, bool ignoreItemTag = false)
        {
            try
            {
                _useDefaultValue = !DeserializeProperty(reader, serializer, _tag, out _entityID, true);
            }
            finally
            {

            }
        }

        public override void WriteXml(XmlWriter writer, bool ignoreItemTag = false)
        {
            if (!UsingDefaultValue)
            {
                SerializeProperty(writer, serializer, _tag, _entityID, true);
            }
        }

        public bool Equals(NetworkedItem<T> other)
        {
            return UsingDefaultValue == other.UsingDefaultValue && _entityID.Equals(other._entityID);
        }

        public override bool Equals(object o)
        {
            if (o is NetworkedItem<T>)
            {
                return ((NetworkedItem<T>)o).Equals(this);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return
                _defaultValue.GetHashCode() +
                _entityID.GetHashCode() +
                _tag.GetHashCode();
        }
    }
}
