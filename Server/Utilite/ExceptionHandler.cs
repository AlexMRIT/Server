using System;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

namespace Server.Utilite
{
    public static class ExceptionHandler
    {
        public static void Execute(Exception exception, string methodName)
        {
            Console.WriteLine($"Exception message: {exception.Message}\n Method name: {methodName}\n Error stack: {exception.StackTrace}.");
        }

        public static void ExecuteMySQLException(MySqlException exception, string methodName)
        {
            Console.WriteLine($"Method: {methodName}. Message: '{exception.Message}' (Error Number: '{exception.Number}')");
        }

        public static void ExecuteSocketException(SocketException exception, string methodName)
        {
            Console.WriteLine($"Method: {methodName}. Message: '{exception.Message}' Socket Error '{exception.ErrorCode}' (Error Number: '{exception.NativeErrorCode}')");
        }
    }
}