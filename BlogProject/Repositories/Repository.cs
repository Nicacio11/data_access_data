using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper.Contrib.Extensions;
using BlogProject.Models;

namespace BlogProject.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly  SqlConnection _connection;

        public Repository(SqlConnection connection) 
            => _connection = connection;

        public virtual IEnumerable<T> Get() 
            => _connection.GetAll<T>();

        public T Get(int id) 
            => _connection.Get<T>(id);

        public void Create(T obj) 
        {
            _connection.Insert<T>(obj);
        }

        public void Update(T obj) 
        {
            _connection.Update<T>(obj);
        }

        public void Delete(T obj) 
        {
            _connection.Delete<T>(obj);
        }

        public void Delete(int id) 
        {
            var obj = Get(id);
            Delete(obj);
        }
    }
}