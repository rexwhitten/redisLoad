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

namespace redisLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Sync .... ");

            var program = SqlProgramParser.Parse("queries.txt");




            Console.WriteLine("");
        }



        static int GetDataFromSql(String ConnectionStringName, String Query)
        {
            int error = 0;

            try
            {
                Int32 redis_port = 6379;

                error = 1;
            }
            catch(Exception x)
            {
                error = -1;
                Console.WriteLine(x.Message);
            }

            return error;
        }

    }
}
