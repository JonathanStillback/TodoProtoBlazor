using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Reflection;
using Dapper;
using Models;

namespace Implementations
{
    public class SqliteDBProvider<T> : IDBProvider<T>
    {
        public SqliteDBProvider()
        {
            this.CreateTables();
            // this.RecreateTables();
        }
        
        public long Create(T item)
        {
            var datatype = typeof(T);
            var properties = datatype.GetProperties();

            var table = datatype.Name.ToLower();
            var columns = String.Join(",", properties.Where(f=>!f.Name.ToLower().Equals("id")).Select(f=>f.Name));
            var values = String.Join(",", properties.Where(f=>!f.Name.ToLower().Equals("id")).Select(f=>FormatValue(f, f.GetValue(item))));
            var sql = $"insert into {table} ({columns}) values ({values})";
            long createdId = -1;
            try
            {
                using(var conn = Connection())
                {
                    var noOfRows = conn.Execute(sql);
                    if (noOfRows > 0)
                    {
                        createdId = (long)conn.ExecuteScalar("select last_insert_rowid()");
                    }
                }
            }
            catch(Exception ex)
            {
                // Todo: log this
                createdId = -1;
            }
            return createdId;
        }

        // Example:
        // var sql = "SELECT * FROM Invoice WHERE Code = @Code;";
        // var invoices = connection.Query<Invoice>(sql, new {Code = new DbString {Value = "Invoice_1", IsFixedLength = false, Length = 9, IsAnsi = true}}).ToList();
        // string sql = "INSERT INTO Customers (CustomerName) Values (@CustomerName);";
        // var affectedRows = connection.Execute(sql, new {CustomerName = "Mark"});
        public IEnumerable<T> Get(string where = "", object param = null)
        {
            var datatype = typeof(T);
            var table = datatype.Name.ToLower();
            var sql = $"select * from {table} {where}";

            var items = new List<T>();
            try
            {
                using(var conn = Connection())
                {
                    items = conn.Query<T>(sql, param).ToList();
                }
            }
            catch(Exception ex)
            {
                // Todo: log this
                return null;
            }
            return items;
        }
        public T Get(int id)
        {
            var items = Get("where @id = ", new {id = id});
            if (items.Count() > 0)
                return items.First();
            else
                return default(T);
        }
        public IEnumerable<T> GetAll()
        {
            return Get();
        }
        public T Update(T item)
        {
            var datatype = typeof(T);
            var properties = datatype.GetProperties();

            var key = properties.Where(f=>f.Name.ToLower().Equals("id")).Select(f=>f.GetValue(item)).FirstOrDefault();

            var table = datatype.Name.ToLower();
            var setValues = String.Join(",", properties.Where(f=>!f.Name.ToLower().Equals("id")).Select(f=>f.Name + "=" + FormatValue(f, f.GetValue(item))));
            // var values = String.Join(",", properties.Where(f=>!f.Name.ToLower().Equals("id")).Select(f=>FormatValue(f, f.GetValue(item))));

            if (key != null || key.ToString() != string.Empty)
            {
                var sql = $"update {table} set {setValues} where id = {key}";

                try
                {
                    using(var conn = Connection())
                    {
                        var status = conn.Execute(sql);
                    }
                }
                catch(Exception ex)
                {
                    // Todo: log this
                    return default(T);
                }
            }
            else
                return default(T);
            return item;
        }

        public T Delete(T item)
        {
            var datatype = typeof(T);
            var properties = datatype.GetProperties();

            var key = properties.Where(f=>f.Name.ToLower().Equals("id")).Select(f=>f.GetValue(item)).FirstOrDefault();

            var table = datatype.Name.ToLower();
            var setValues = String.Join(",", properties.Where(f=>!f.Name.ToLower().Equals("id")).Select(f=>f.Name + "=" + FormatValue(f, f.GetValue(item))));
            // var values = String.Join(",", properties.Where(f=>!f.Name.ToLower().Equals("id")).Select(f=>FormatValue(f, f.GetValue(item))));

            if (key != null || key.ToString() != string.Empty)
            {
                var sql = $"delete from {table} where id = {key}";

                try
                {
                    using(var conn = Connection())
                    {
                        var status = conn.Execute(sql);
                    }
                }
                catch(Exception ex)
                {
                    // Todo: log this
                    return default(T);
                }
            }
            else
                return default(T);
            return item;
        }

        protected object FormatValue(PropertyInfo p, object item)
        {
            switch (p.PropertyType.Name.ToLower())
            {
                case "string":
                    return $"'{item}'";
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    return item;
                case "bool":
                case "boolean":
                    return item.ToString().ToLower() == "true" ? 1 : 0;
                default:
                return item;
            }
        }

        protected string MapDatatype(PropertyInfo p)
        {
            switch (p.PropertyType.Name.ToLower())
            {
                case "string":
                    return "text";
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    return "integer";
                case "bool":
                case "boolean":
                    return "boolean";
                default:
                return "text";
            }
        }

        protected string MapDatatypeValue(PropertyInfo p)
        {
            switch (p.PropertyType.Name.ToLower())
            {
                case "string":
                    return "''";
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    return "0";
                case "bool":
                case "boolean":
                    return "0";
                default:
                return "''";
            }
        }

        protected IDbConnection Connection()
        {
            // var connStr = "Data Source=db/mydb.db;Version=3;New=True;Cache Size=3000;UTF8Encoding=True;Journal Mode=Off;Synchronous=Off;";
            var dbPath = Directory.GetCurrentDirectory() + "..\\db";
            var connStr = "Data Source=" + dbPath + "\\db.sqlite;";
            try
            {
                if (!Directory.Exists(dbPath))
                        Directory.CreateDirectory(dbPath);
                var conn = new SqliteConnection(connStr);
                conn.Open();
                return conn;
            }
            catch(Exception ex)
            {
                // Todo: log this
            }
            return null;
        }

        protected void CreateTables()
        {
            // var sql = "create table if not exists child (id integer primary key);";
            // sql += "alter table child add column firstname text;";
            // sql += "alter table child add column lastname text;";

            var datatype = typeof(T);
            var properties = datatype.GetProperties();

            var table = datatype.Name.ToLower();
            var columns = properties.Where(f=>!f.Name.ToLower().Equals("id"));

            var sql = $"create table if not exists {table} (id integer primary key);";

            try
            {
                using( var conn = Connection())
                {
                    conn.Execute(sql);
                }
            }
            catch (Exception ex)
            {
                // Todo: log this
            }

            var existingColumns = new List<dynamic>();
            sql = $"PRAGMA table_info('{table}')";
            try
            {
                using( var conn = Connection())
                {
                    existingColumns = conn.Query(sql).AsList();
                }
            }
            catch (Exception ex)
            {
                // Todo: log this
            }

            sql = string.Empty;
            foreach (var col in columns)
            {
                var dbDataType = MapDatatype(col);
                var defaultValue = MapDatatypeValue(col);
                if (!existingColumns.Any( f => f.name == col.Name.ToLower()))
                {
                    sql = $"alter table {table} add column {col.Name.ToLower()} {dbDataType} not null default {defaultValue};";
                    try
                    {
                        using( var conn = Connection())
                        {
                            if (sql != string.Empty)
                                conn.Execute(sql);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Todo: log this
                    }
                }
            }
        }

        protected void RecreateTables()
        {
            // var sql = "create table if not exists child (id integer primary key);";
            // sql += "alter table child add column firstname text;";
            // sql += "alter table child add column lastname text;";

            var sqlStatements = new List<string>();

            var datatype = typeof(T);
            var properties = datatype.GetProperties();

            var table = datatype.Name.ToLower();
            var columns = properties.Where(f=>!f.Name.ToLower().Equals("id"));

            // sqlStatements.Add($"drop table _{table};");
            // sqlStatements.Add($"create table _{table} as select * from {table};");
            // sqlStatements.Add($"drop table {table};");
            // sqlStatements.Add($"create table if not exists {table} (id integer primary key);");

            sqlStatements.Add($"drop table _{table};");
            sqlStatements.Add($"alter table {table} rename to _{table};");
            sqlStatements.Add($"create table if not exists {table} (id integer primary key);");

            foreach (var col in columns)
            {
                var dbDataType = MapDatatype(col);
                var defaultValue = MapDatatypeValue(col);
                sqlStatements.Add($"update _{table} set {col.Name.ToLower()} = {defaultValue} where {col.Name.ToLower()} is null;");
                sqlStatements.Add($"alter table {table} add column {col.Name.ToLower()} {dbDataType} not null default {defaultValue};");
            }

            // sqlStatements.Add($"insert into _{table} select * from {table};");
            // sqlStatements.Add($"drop table {table};");
            // sqlStatements.Add($"create table if not exists {table} (id integer primary key);");
            sqlStatements.Add($"insert into {table} select * from _{table};");
            sqlStatements.Add($"drop table _{table};");

            foreach (var s in sqlStatements)
            {
                try
                {
                    using( var conn = Connection())
                    {
                        conn.Execute(s);
                    }
                }
                catch (Exception ex)
                {
                    // Todo: log this
                }
            }

        }
    }
}