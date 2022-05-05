using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper.Contrib.Extensions;
using BlogProject.Models;

namespace BlogProject.Repositories
{
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(SqlConnection connection) : base(connection) 
        {
        }
    }
}