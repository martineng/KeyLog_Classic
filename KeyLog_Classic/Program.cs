using System;
using System.Data;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace KeyLog_Classic
{
    class MainClass
    {
        public static void Main(string[] args)
        {

        }

        // Print UI
        private static void HomeMenu()
        {
            Console.WriteLine("*========================================*");
            Console.WriteLine("*           KeyLog Classic               *");
            Console.WriteLine("*             Ver. 1.0                   *");
            Console.WriteLine("*========================================*");
            Console.Write("\n");
        }

        // Request Credential for Database Connection
        private static string DatabaseConnectionInfo()
        {
            string server = "", id = "", password = "";
            const string database = "testPassword";

            string connectionInfo = "";
            MySqlConnection connection = new MySqlConnection();

            Console.WriteLine("Please Enter ");
            Console.WriteLine("Server:> ");
            server = Console.ReadLine();

            Console.WriteLine("Id:> ");
            id = Console.ReadLine();

            Console.WriteLine("Password:> ");
            password = Console.ReadLine();

            connectionInfo = "server=" + server + ";" +
                             "User Id=" + id + ";" +
                             "password=" + password + ";" +
                             "database=" + database + ";";

            return connectionInfo;
        } // END DatabaseConnectionInfo();

        // Open Connection with the database
        private static MySqlConnection DatabaseConnection(string inConnectionInfo)
        {
            MySqlConnection connection = new MySqlConnection(inConnectionInfo);
            connection.Open();

            return connection;
        }
    }
}
