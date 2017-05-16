using System;

namespace Cmas.Infrastructure.Configuration
{
    public class CmasConfiguration
    {
        public static string secretKey = @"P546C8TF278CH5931069B622E695D4F4";

        public Databases Databases { get; set; }

        public CORS CORS { get; set; }

        public Database GetDbSettings(string serviceName)
        {
            serviceName = serviceName.ToLower();

            Database result = null;

            switch (serviceName)
            {
                case "users":
                    result = Databases.Users;
                    break;
                case "call-off-orders":
                    result = Databases.CallOffOrders;
                    break;
                case "contracts":
                    result = Databases.Contracts;
                    break;
                case "requests":
                    result = Databases.Requests;
                    break;
                case "time-sheets":
                    result = Databases.TimeSheets;
                    break;
                default:
                    throw new Exception($"unknown service: {serviceName}");
            }

            return result.WithDefaults(Databases.Schema, Databases.Host, Databases.Login, Databases.Password);
        }

        public Smtp Smtp { get; set; }

        public string CmasUrl { get; set; }
    }

    public class Database
    {
       

        public string Schema { get; set; }
        public string Host { get; set; }
        public string Login { get; set; }

        private string cryptedPassword;

        public string Password
        {
            get { return ConfigurationHelper.DecryptString(cryptedPassword, CmasConfiguration.secretKey); }
            set { cryptedPassword = value; }
        }

        public string Name { get; set; }

        public string ConnectionString
        {
            get { return $"{Schema}://{Login}:{Password}@{Host}"; }
        }
    }

    public class Databases
    {
        public string Schema { get; set; }
        public string Host { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Database Users { get; set; }
        public Database CallOffOrders { get; set; }
        public Database Contracts { get; set; }
        public Database Requests { get; set; }
        public Database TimeSheets { get; set; }
    }

    public class CORS
    {
        public string Origin { get; set; }
        public string Methods { get; set; }
        public string Headers { get; set; }
    }

    public class Smtp{
        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }

        private string cryptedPassword;

        public string Password
        {
            get { return ConfigurationHelper.DecryptString(cryptedPassword, CmasConfiguration.secretKey); }
            set { cryptedPassword = value; }
        }
    }
}