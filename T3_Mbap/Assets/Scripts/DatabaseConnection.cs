using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.Data.Common;
using System;
using System.IO;

public class DatabaseConnection
{
    private static DatabaseConnection instance;
    private string connectionString = Path.Combine(Application.streamingAssetsPath, "scenario_database.sqlite");
    private SqliteConnection dbConnection;
    private SqliteCommand command;
    private DatabaseConnection()
    {
        connectionString = $"URI=file:{connectionString.Replace("\\", "/")}";
        dbConnection = new SqliteConnection(connectionString);
    }

    public static DatabaseConnection GetInstance()
    {
        if (instance == null)
        {
            instance = new DatabaseConnection();
        }
        return instance;
    }

    public SqliteConnection GetConnection()
    {
        return dbConnection;
    }

    public void OpenConnection()
    {
        try
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erreur lors de l'ouverture de la connexion : {e.Message}");
            throw;
        }
    }

    public void CloseConnection()
    {
        if (dbConnection.State == ConnectionState.Open)
        {
            dbConnection.Close();
        }
    }

    public void CreateCommand(string query)
    {
        this.command = new SqliteCommand(query, dbConnection);
    }

    public SqliteCommand GetSqliteCommand() 
    {
        return this.command;
    }

    public SqliteDataReader GetSqliteDataReader()
    {
        return this.command.ExecuteReader();
    }
    public int GetSqliteScalar()
    {
        return (int)this.command.ExecuteScalar();
    }

    public void AddParametre(string parametre, object valeur)
    {
       this.command.Parameters.AddWithValue(parametre, valeur);
    }

    public void ExecuteNonQuery()
    {
        this.command.ExecuteNonQuery(); 
    }
}