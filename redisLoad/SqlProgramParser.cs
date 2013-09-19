using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace redisLoad
{
    public class SqlProgramParser
    {
        public static SqlProgram Parse(String SqlProgramFileName)
        {
            SqlProgram prog = new SqlProgram();

            if (File.Exists(SqlProgramFileName))
            {
                string[] lines = File.ReadAllLines(SqlProgramFileName);
                List<String> metaLines = new List<string>();
                int line_number = 0;

                // Scan for MEtA Lines 
                foreach (var line in lines)
                {
                    line_number++; // Increment Line Number
                    if (line.StartsWith("//"))
                    {
                        metaLines.Add(line); continue;
                    }

                    if (line.StartsWith("$")) // this token reads another queries file. ex: $C:\data\query.txt
                    {
                        Console.WriteLine("Not implemented yet.");
                    }

                    if (line.StartsWith("#"))
                    {
                        metaLines.Add(line);
                        prog.ConnectionString = line.Remove(0, 1);
                    }
                    else
                    {
                        continue;
                    }
                }
                // Scan for Query Lines 
                String program = File.ReadAllText(SqlProgramFileName);
                foreach (var meta in metaLines)
                {
                    program = program.Replace(meta, "");
                }

                prog.Queries = program.Split(';').ToList();


            }
            else
            {
                Console.WriteLine("String Does Not Exist. {0}", SqlProgramFileName);
            }
            return prog;
        }
    }

    public class SqlProgram
    {
        public String ConnectionString { get; set; }
        public List<String> Queries { get; set; }

        public SqlProgram()
        {
            this.Queries = new List<string>();
        }
    }
}
