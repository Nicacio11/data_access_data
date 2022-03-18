﻿using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using DataAccessDapper.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True";
            // QueryComAdoNet(connectionString);
            QueryComDapper(connectionString);
        }

        static void QueryComAdoNet(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using( var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "Select id, title from CATEGORY";

                    var reader = command.ExecuteReader();
                    while(reader.Read()){
                        Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
                    }
                }

            }
        }

        static void QueryComDapper(string connectionString)
        {

            using (var connection = new SqlConnection(connectionString))
            {

                //UpdateCategory(connection);
                //CreateManyCategory(connection);
                //ListCategories(connection);
                ExecuteProcedure(connection);
            }
        }
        static void ListCategories(SqlConnection connection)
        {
             var categories = connection.Query<Category>("Select [id], [title] from [CATEGORY]");

                foreach(var category in categories){
                     Console.WriteLine($"{category.Id} - {category.Title}");
                }
        }
        static void CreateCategory(SqlConnection connection)
        {
            var insert = 
            @"INSERT INTO CATEGORY VALUES(@Id, 
            @Title, 
            @Url, 
            @Summary, 
            @Order,  
            @Description, 
            @Featured)";

            var categoryInsert = new Category()
            {
                Id = Guid.NewGuid(),
                Title = "Amazon WS",
                Url = "Amazon",
                Description = "Serviços WS",
                Order = 8,
                Summary = "Aws CLOUD"
            };

            var rows = connection.Execute(insert, categoryInsert);
        }
        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [CATEGORY] SET [Title] = @title where [Id]=@id";
            var rows = connection.Execute(updateQuery, new { title = "FrontEnd 2022", Id = new Guid("b4c5af73-7e02-4ff7-951c-f69ee1729cac")});
            Console.WriteLine($"{rows} registro - atualizado");
        }
        static void CreateManyCategory(SqlConnection connection)
        {
            var insert = 
            @"INSERT INTO CATEGORY VALUES(@Id, 
            @Title, 
            @Url, 
            @Summary, 
            @Order,  
            @Description, 
            @Featured)";

            var categoryInsert =  new List<Category>()
            {
               new Category() 
               {
                    Id = Guid.NewGuid(),
                    Title = "Amazon WS NOVA",
                    Url = "Amazon",
                    Description = "Serviços WS",
                    Order = 8,
                    Summary = "Aws CLOUD"
                },
                new Category() 
               {
                    Id = Guid.NewGuid(),
                    Title = "Amazon WS NOVA 2",
                    Url = "Amazon",
                    Description = "Serviços WS",
                    Order = 8,
                    Summary = "Aws CLOUD"
                }
            };
            

            var rows = connection.Execute(insert, categoryInsert);

            Console.WriteLine($"{rows} registro - atualizado");

        }
    
        static void ExecuteProcedure(SqlConnection connection)
        {
            var sql = "[spDeleteStudent]";
            var @params = new {StudentId = "c28908af-df0d-4457-97e2-0018669e4b9c" };

            connection.Execute(sql, @params, commandType: CommandType.StoredProcedure);
        }
        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[procedure]";

            connection.Query<Category>(procedure, commandType: CommandType.StoredProcedure);
        }
    }

}
