using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace redisLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Program.....");
            
            Console.WriteLine("Parsing Queries File....");
            
            var program = SqlProgramParser.Parse("queries.txt");
            
            Console.WriteLine("Program Parsed...Executing...");

            foreach (var query in program.Queries)
            {
                if (GetDataFromSql(program.ConnectionString, query) != 1)
                {
                    Console.WriteLine("Error...{0}", query);
                }
            }

            Console.WriteLine("Program Complete");
        }

        static int GetDataFromSql(String ConnectionString, String Query)
        {
            int error = 0;

            try
            {
                Int32 redis_port = 6379;
                

                // Connect to SQL and Execute the Query 
                var results = (new DataAccess(ConnectionString)).ExecuteSql(Query);
                if(results.Tables.Count > 0 && results.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine("Retreived DataSet ( Tables: {0}, Rows: {1} )",results.Tables.Count, results.Tables[0].Rows.Count);
                    using (var redis = new RedisClient("127.0.0.1",6379))
                    {
                        string query_key = CreateCleanQueryKey(Query);
                        var client = redis.As<String>();
                        client.SetEntry(query_key, JsonConvert.SerializeObject(results));
                        Console.WriteLine("{0} saved to redis", query_key);
                    }
                }
                else
                {
                    Console.WriteLine("Retreived DataSet (NO DATA))");
                }
                
                error = 1;
            }
            catch(Exception x)
            {
                error = -1;
                Console.WriteLine(x.Message);
            }

            return error;
        }

        static string CreateCleanQueryKey(String query)
        {
            string clean = "";

            foreach (var c in query.ToLower())
            {
                if (char.IsLetterOrDigit(c))
                {
                    clean = clean + c;
                }
            }

            return clean;
        }
    }
}
