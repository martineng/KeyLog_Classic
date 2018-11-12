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
            Boolean toContinue = true;

            HomeMenu();

            while (toContinue)
            {
                toContinue = DecideCotinue(LoginToApp());
            } // END while
        }

        // Print UI
        private static void HomeMenu()
        {
            Console.WriteLine("*========================================*");
            Console.WriteLine("*           KeyLog Classic               *");
            Console.WriteLine("*             Ver. 1.0                   *");
            Console.WriteLine("*========================================*");
        }

        // Request Credential for Database Connection
        private static string DatabaseConnectionInfo()
        {
            string server = "", id = "", password = "";
            const string database = "testPassword";

            string connectionInfo = "";
            MySqlConnection connection = new MySqlConnection();

            Console.Write("\n");
            Console.WriteLine("*====Database Info====*");
            Console.Write("Server:> ");
            server = Console.ReadLine();

            Console.Write("Id:> ");
            id = Console.ReadLine();

            Console.Write("Password:> ");
            password = Console.ReadLine();

            connectionInfo = "server=" + server + ";" +
                             "User Id=" + id + ";" +
                             "password=" + password + ";" +
                             "database=" + database + ";";

            return connectionInfo;
        } // END DatabaseConnectionInfo()

        // Open Connection with the database
        private static MySqlConnection DatabaseConnection(string inConnectionInfo)
        {
            MySqlConnection connection = new MySqlConnection();

            try
            {
                connection = new MySqlConnection(inConnectionInfo);
                connection.Open();
            } // END try
            catch 
            {
                // Return null as connection failed.
                return null;
            } // END catch

            // return the connection when it success
            return connection;
        } // END DatabaseConnnection()

        // Verity Identity before using the App, return Connection Status
        private static Boolean LoginToApp()
        {
            string username = "", password = "";
            string dsUsername = "", dsPassword = "";

            Console.Write("\n");
            Console.WriteLine("*====Login====*");
            Console.Write("username:> ");
            username = Console.ReadLine();

            Console.Write("password:> ");
            password = Console.ReadLine();

            try
            {
                MySqlConnection connection = DatabaseConnection(DatabaseConnectionInfo());

                // SQL Command to obtain information
                MySqlCommand cmdLogin = connection.CreateCommand();
                cmdLogin.CommandText = "SELECT * FROM admin";

                // Data Adapter to run the command
                MySqlDataAdapter loginAdapter = new MySqlDataAdapter(cmdLogin);
                DataSet loginDS = new DataSet();

                loginAdapter.Fill(loginDS);

                dsUsername = loginDS.Tables[0].Rows[0].ItemArray[0].ToString();
                dsPassword = loginDS.Tables[0].Rows[0].ItemArray[1].ToString();

                if (BCrypt.Net.BCrypt.Verify(username, dsUsername) &&
                    BCrypt.Net.BCrypt.Verify(password, dsPassword))
                {
                    Console.WriteLine("\n------IDENTITY CONFIRMED.");
                } // END if
                else
                {
                    Console.WriteLine("\n------UNKNOWN IDENTITY.");
                }

                connection.Close();
            } // END try
            catch 
            {
                // return isConnect = false
                return false;
            } // END catch

            // return isConnect = true
            return true;
        } // END LoginToApp()

        private static Boolean DecideCotinue(Boolean inIsConnect)
        {
            if (inIsConnect == false)
            {
                String toContinueString = "";

                Console.WriteLine("Connection Failed.\nTry Again? (Y\\n)");
                toContinueString = Console.ReadLine();

                // Verify Action
                if (toContinueString == "Y" || toContinueString == "y")
                {
                    // true to try again
                    return true;
                }
                else
                {
                    // false to not try again
                    return false;
                } // END else
            } // END if
            else
            {
                // Escape Connection Loop
                return false;
            } //END else

        } // END DecideContinue()
    }
}
