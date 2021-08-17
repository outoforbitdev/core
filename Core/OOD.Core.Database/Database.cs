using OOD.Core.Collections;
using System;
using System.Collections.Generic;

namespace OOD.Core.Database
{
    //public class Database
    //{
    //    private Dictionary<string, Table<IEntity>> Tables;

    //    public Database()
    //    {
    //        Tables = new Dictionary<string, Table<IEntity>>();
    //    }

    //    public void AddTable(string tableName, Table<IEntity> table)
    //    {
    //        Tables.TryAdd(tableName, table);
    //    }
    //    public bool TryGetTable(string tableName, out Table<IEntity> table)
    //    {
    //        return Tables.TryGetValue(tableName, out table);
    //    }

    //    public bool TryAddEntity(string tableName, IEntity entity)
    //    {
    //        Table<IEntity> table;
    //        if (TryGetTable(tableName, out table))
    //        {
    //            return table.TryAddEntity(entity);
    //        }
    //        return false;
    //    }
    //    public bool TryAddOrUpdateEntity(string tableName, IEntity entity)
    //    {
    //        Table<IEntity> table;
    //        if (TryGetTable(tableName, out table))
    //        {
    //            table.AddOrUpdateEntity(entity);
    //            return true;
    //        }
    //        return false;
    //    }
    //    public bool TryGetEntity<T>(string tableName, string entityID, out T entity)
    //        where T: IEntity
    //    {
    //        Table<IEntity> table;
    //        if (TryGetTable(tableName, out table))
    //        {
    //            if (table.Contains(entityID)) {
    //                entity = (T)table[entityID];
    //                return true;
    //            }
    //        }
    //        entity = default(T);
    //        return false;
    //    }
    //}
}
