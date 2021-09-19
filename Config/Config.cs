using System;

namespace SocialNetWork.Config
{
    public class Config
    {
        public static string UserId { get; private set; }
        public static string Password { get; private set; }
        public static string Host { get; private set; } 
        public static string Port { get; private set; } 
        public static string Database { get; private set; }

        public static string SetConfig()
        {
            Config.UserId = "postgres";
            Config.Password = "1234";
            Config.Host = "localhost";
            Config.Port = "5432";
            Config.Database = "postgres-0";

            return $"Server={Host};Port={Port};User Id={UserId};Password={Password};Database={Database};";
        }
        
    }
    
    
}