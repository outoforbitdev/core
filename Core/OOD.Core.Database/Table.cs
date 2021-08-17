using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD.Core.Database
{
    public abstract class Table<T> where T:Entity
    {
        protected string _path;
        public abstract bool Contains(string entityId);
        public abstract bool TryAddEntity(T entity);
        public abstract bool TryUpdateEntity(T entity);
        public abstract void AddOrUpdateEntity(T entity);
        public abstract bool TryGetEntity(string entityID, out T entity);
        public abstract T this[string entityId] { get; set; }
        public abstract bool Load(string path);
        public abstract bool Save();
        public abstract bool SaveAs(string path);
        public abstract void SaveAs(TextWriter stream);
    }
}
