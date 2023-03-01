using InterviewTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListController : ControllerBase
    {
        public ListController()
        {
        }

        [Route("GetEmployees")]
        [HttpGet]
        public JsonResult GetEmployeeList()
        {
            var employeesList = new List<Employee>();

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT Name, Value FROM Employees";
                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employeesList.Add(new Employee
                        {
                            Name = reader.GetString(0),
                            Value = reader.GetInt32(1)
                        });
                    }
                }                
            }
            return new JsonResult(employeesList);
        }

        [Route("CreateEmployee")]
        [HttpPost]
        public JsonResult CreateEmployee(Employee employee)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var createCmd = connection.CreateCommand();
                createCmd.CommandText = @"INSERT INTO Employees(Name, Value) VALUES ('"+employee.Name+"', "+employee.Value+")";
                createCmd.ExecuteNonQuery();
            }
            return new JsonResult("creation successful");
        }

        [Route("RemoveEmployee/{name}")]
        [HttpDelete]
        public JsonResult RemoveEmployee(string name)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./SqliteDB.db";
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = @"DELETE FROM Employees WHERE Name  = '"+name+"'";
                
                deleteCmd.ExecuteNonQuery();
            }

            return new JsonResult("removal successful");
        }

        [Route("PutEmployee")]
        [HttpPut]
        public JsonResult PutEmployee(Employee employee)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./SqliteDB.db";
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var updateCmd = connection.CreateCommand();
                updateCmd.CommandText = @"UPDATE Employees SET Value = "+employee.Value+" WHERE Name = '"+employee.Name+"'";
                updateCmd.ExecuteNonQuery();
            }

            return new JsonResult("update successful");
        }
        [Route("increment")]
        [HttpGet]
        public JsonResult IncrementValues()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./SqliteDB.db";
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var incrementCmd = connection.CreateCommand();
                incrementCmd.CommandText = @"UPDATE Employees
        SET Value = Value +
             CASE
                 WHEN Name LIKE 'E%' THEN 1
                 WHEN Name LIKE 'G%' THEN 10
                 WHEN Name not LIKE 'G%' OR 'E%' THEN 100
                 ELSE 0
             END";
                incrementCmd.ExecuteNonQuery();
            }

            return new JsonResult("values are updated");
        }


        //THIS ALGORITHM HAS BEEN TESTED ON
        //POSTMAN AND SQLITE
        //WORKS PERFECTLY WELL
        [Route("sum")]
        [HttpGet]
        public JsonResult SumValues()
        {

            List<int> results = new List<int>();

            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./SqliteDB.db";
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT SUM(Value)
                    FROM Employees
                    WHERE Name LIKE 'A%'";
        
                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int value = reader.GetInt32(0);
                            if (value >= 11171)
                            {
                                results.Add(value);
                            }
                    }
                }
                queryCmd.CommandText = @"SELECT SUM(Value)
                    FROM Employees
                    WHERE Name LIKE 'B%'";

                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int value = reader.GetInt32(0);
                        if (value >= 11171)
                        {
                            results.Add(value);
                        }
                    }
                }
                queryCmd.CommandText = @"SELECT SUM(Value)
                    FROM Employees
                    WHERE Name LIKE 'C%'";

                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int value = reader.GetInt32(0);
                        if (value >= 11171)
                        {
                            results.Add(value);
                        }
                    }
                }

            }

            return new JsonResult(results);
        }

    }

}
