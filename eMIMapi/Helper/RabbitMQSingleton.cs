using RabbitMQ.Client;
using System;
using System.Configuration;

namespace eMIMapi.Helper
{
    public sealed class RabbitMQSingleton
    {
        private static volatile RabbitMQSingleton instance;
        private static object syncRoot = new object();

        private IConnection _connection;
        private IModel _channel;

        private RabbitMQSingleton() { }

        public static RabbitMQSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new RabbitMQSingleton();
                        }
                    }
                }
                return instance;
            }
        }

        public void InitializeRabbitMQConnection()
        {
            string RMQ_HostName = Convert.ToString(ConfigurationManager.AppSettings["RMQ_HostName"]);
            int RMQ_Port = Convert.ToInt32(ConfigurationManager.AppSettings["RMQ_Port"]);
            string RMQ_UserName = Convert.ToString(ConfigurationManager.AppSettings["RMQ_UserName"]);
            string RMQ_Password = Convert.ToString(ConfigurationManager.AppSettings["RMQ_Password"]);

            var factory = new ConnectionFactory()
            {
                HostName = RMQ_HostName,
                Port = RMQ_Port,
                UserName = RMQ_UserName,
                Password = RMQ_Password,
                VirtualHost = "/"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public IModel GetRabbitMQChannel()
        {
            return _channel;
        }

        public void CloseRabbitMQConnection()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}