﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Earth
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");   
            
            //--- Setup

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            

            channel.QueueDeclare(queue: "a",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.QueueDeclare(queue: "b",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            channel.QueueDeclare(queue: "c",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

            channel.ExchangeDeclare("emergency", "topic", true, false, null);

            channel.QueueBind("a", "emergency", "red.*");
            channel.QueueBind("b", "emergency", "*.report.*");
            channel.QueueBind("c", "emergency", "*.critical");

            //--- Chat

            var message = " ";

            while (message!=string.Empty)
            {
                Console.WriteLine("Message : ");
                message = Console.ReadLine();
     
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("emergency",
                     routingKey: "red.order",
                     basicProperties: null,
                     body: body);

                Console.WriteLine(" [Earth] Sent {0}", message);
            }
           


        }
    }
}
