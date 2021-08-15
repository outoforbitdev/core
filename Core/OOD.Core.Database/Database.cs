using OOD.Core.Collections;
using System;
using System.Collections.Generic;

namespace OOD.Core.Database
{
    public class Database
    {
        private Dictionary<string, Table<Entity>> Tables;

        public Database()
        {
            Tables = new Dictionary<string, Table<Entity>>();
        }

        public void AddTable(string tableName, Table<Entity> table)
        {
            Tables.TryAdd(tableName, table);
        }
        public bool TryGetTable(string tableName, out Table<Entity> table)
        {
            return Tables.TryGetValue(tableName, out table);
        }

        public bool TryAddEntity(string tableName, Entity entity)
        {
            Table<Entity> table;
            if (TryGetTable(tableName, out table))
            {
                return table.TryAddEntity(entity);
            }
            return false;
        }
        public bool TryAddOrUpdateEntity(string tableName, Entity entity)
        {
            Table<Entity> table;
            if (TryGetTable(tableName, out table))
            {
                table.AddOrUpdateEntity(entity);
                return true;
            }
            return false;
        }
        public bool TryGetEntity<T>(string tableName, string entityID, out Entity entity)
        {
            Table<Entity> table;
            if (TryGetTable(tableName, out table))
            {
                return table.TryGetEntity(entityID, out entity);
            }
            entity = null;
            return false;
        }
    }
}
