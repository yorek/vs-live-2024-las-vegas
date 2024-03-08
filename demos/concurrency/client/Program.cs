﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using DotNetEnv;

namespace MyProject;
class Program
{
    static async Task Main(string[] args)
    {
        Env.Load();

        int r = 1000;

        if (int.TryParse(args[0], out int n) == false) {
            Console.WriteLine("Please type the number of demo you want to run");
            return;
        }

        if ((args.Length == 2) && (int.TryParse(args[1], out r) == false))
        {
            Console.WriteLine("Row number must be an integer");
            return;
        }        

        var demo = new Demo();
        await demo.Run(n, r);        
    }
}

class Demo {

    readonly string ConnectionString = Environment.GetEnvironmentVariable("MSSQL");

    public async Task Run(int n, int r) {
        int taskCount = 2;

        Console.WriteLine("Adding tasks...");

        var tasks = new List<Task>();    
        var mi = GetType().GetMethod($"RunDemo_{n}", BindingFlags.NonPublic | BindingFlags.Instance);

        Enumerable.Range(1, taskCount).ToList().ForEach(
            i => tasks.Add( 
                (Task)mi.Invoke(this, new object[] { new object[] { i, r }})
            )      
        );
        
        await Task.WhenAll(tasks);

        Console.WriteLine("Done");
    }

    /*
        Demo 1:
        Just connect to the database and then wait
        dotnet run -c Release 1
    */
    private async Task RunDemo_1(params object[] args) {
        int taskId = (int)args[0];
        using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();

        Console.WriteLine(string.Format("[{0}]: Connected.", taskId));

        await Task.Delay(100000);
    }

    /*
        Demo 2:
        Using default transaction level (read committed)
        Start from rowCount=1000 and then change it to 1000000 
        to exahust the TCP transmission buffer

        dotnet run -c Release 2 1000
        dotnet run -c Release 2 1000000
    */
    private async Task RunDemo_2(params object[] args) {
        int taskId = (int)args[0];
        int rowCount = (int)args[1];

        Console.WriteLine(String.Format("[{0}]: Reading {1} rows.", taskId, rowCount));

        var r = new Random();
        await Task.Delay(r.Next(500, 1500));

        using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();

        var cmd = new SqlCommand($"SELECT * FROM dbo.timesheet_big WHERE id BETWEEN 1 AND {rowCount} ORDER BY id", conn);

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine(String.Format("[{0}]: Id:{1}", taskId, reader[0]));
            await Task.Delay(r.Next(500, 1500));
        }
    }

    /*
        Demo 3:
        Move to a higher transaction level (repeatable read)
        Start from rowCount=1000 and then change it to 1000000 
        to exahust the TCP transmission buffer

        dotnet run -c Release 3 1000
        dotnet run -c Release 3 1000000
    */
    private async Task RunDemo_3(params object[] args) {
        int taskId = (int)args[0];
        int rowCount = (int)args[1];

        Console.WriteLine(String.Format("[{0}]: Reading {1} rows.", taskId, rowCount));

        var r = new Random();
        await Task.Delay(r.Next(500, 1500));

        using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();

        var tran = conn.BeginTransaction(IsolationLevel.RepeatableRead);

        var cmd = new SqlCommand($"SELECT * FROM dbo.timesheet_big WHERE id BETWEEN 1 AND {rowCount} ORDER BY id", conn, tran);

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine(String.Format("[{0}]: Id:{1}", taskId, reader[0]));
            await Task.Delay(r.Next(500, 1500));
        }

        tran.Commit();
    }  
}