using OOD.Core.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD.Core.Database
{
    public class LocalTable<T> : Table<T> where T : Entity, new()
    {
        private SerializableDictionary<string, T> _entities;
        public LocalTable(Database db) : base(db)
        {
            _entities = new SerializableDictionary<string, T>();
        }
        public override T this[string entityId] { 
            get
            {
                T result;
                TryGetEntity(entityId, out result);
                return result;
            }
            set
            {
                AddOrUpdateEntity(value);
                value._db = _db;
            }
        }

        public override void AddOrUpdateEntity(T entity)
        {
            _entities[entity.ID] = entity;
        }

        public override bool Load(string path)
        {
            try
            {
                _entities.DeserializeFromXml(path);
                _path = path;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool Save()
        {
            return SaveAs(_path);
        }

        public override bool SaveAs(string path)
        {
            try
            {
                _entities.SerializeToXml(path);
                _path = path;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override void SaveAs(TextWriter stream)
        {
            _entities.SerializeToXml(stream);
        }

        public override bool TryAddEntity(T entity)
        {
            if (_entities.TryAdd(entity.ID, entity))
            {
                entity._db = _db;
                return true;
            }
            return false;
        }

        public override bool TryGetEntity(string entityID, out T entity)
        {
            return _entities.TryGetValue(entityID, out entity);
        }

        public override bool TryUpdateEntity(T entity)
        {
            if (_entities.ContainsKey(entity.ID))
            {
                _entities[entity.ID] = entity;
                entity._db = _db;
                return true;
            }
            return false;
        }
    }
}
