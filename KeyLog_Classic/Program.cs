using System;
using System.Data;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Text;


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

        // Request Credential for Database Connection, Server, ID and Password
        private static string DatabaseConnectionInfo()
        {
            string server = "", id = "", password = "";
            const string database = "testPassword";

            string connectionInfo = "";
            MySqlConnection connection = new MySqlConnection();

            Console.WriteLine("\n");
            Console.WriteLine("*====Database Info====*");

            Console.Write("Server:> ");
            server = GetHiddenConsoleInput();

            Console.Write("\nID:> ");
            id = GetHiddenConsoleInput();

            Console.Write("\nPassword:> ");
            password = GetHiddenConsoleInput();

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
            username = GetHiddenConsoleInput();

            Console.Write("\npassword:> ");
            password = GetHiddenConsoleInput();

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

                connection.Close();

                if (BCrypt.Net.BCrypt.Verify(username, dsUsername) &&
                    BCrypt.Net.BCrypt.Verify(password, dsPassword))
                {
                    Console.WriteLine("\n\n------IDENTITY CONFIRMED.");
                } // END if
                else
                {
                    Console.WriteLine("\n\n------UNKNOWN IDENTITY.");
                    return false;
                }
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

                Console.WriteLine("\n\nConnection Failed.\nTry Again? (Y\\n)");
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

        // Hide the Character of Console Input and get the input
        private static String GetHiddenConsoleInput()
        {
            StringBuilder input = new StringBuilder();

            while (true)
            {
                var key = Console.ReadKey(true); // Hide Key Press

                // IF Enter, stop the loop
                if (key.Key == ConsoleKey.Enter) 
                {
                    break;
                }
                //IF Backspace and at least 1 char is entered, Delete 1 char
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) 
                {
                    input.Remove(input.Length - 1, 1);
                }
                // IF any Key except Backspace is entered
                else if (key.Key != ConsoleKey.Backspace)
                {
                    input.Append(key.KeyChar);
                }
            } // END while

            return input.ToString();
        } // END GetHiddenConsoleInput()

    }
}
