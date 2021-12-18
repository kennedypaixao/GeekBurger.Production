using GeekBurger.Production.Model.Config;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Production.Extensions
{

    public static class Extensions
    {

        private const string TopicName = "ProductChangedTopic";
        private static IConfiguration _configuration;
        private const string SubscriptionName = "paulista_store";
		private static ServiceBusConfiguration _config;

		public static IServiceBusNamespace GetServiceBusNamespace(this IConfiguration configuration)
        {
			_configuration = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json")
							.Build();

			_config = configuration.GetSection("serviceBus")
                         .Get<ServiceBusConfiguration>();


			var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(_config.ClientId,
							   _config.ClientSecret,
							   _config.TenantId,
                               AzureEnvironment.AzureGlobalCloud);

            var serviceBusManager = ServiceBusManager
                .Authenticate(credentials, _config.SubscriptionId);
            return serviceBusManager.Namespaces
                   .GetByResourceGroup(_config.ResourceGroup,
				   _config.NamespaceName);
        }
		public static async Task ReceiveMessages()
		{
			var subscriptionClient = new Microsoft.Azure.ServiceBus.SubscriptionClient(_config.ConnectionString,
												   TopicName,
												   SubscriptionName);

			//by default a 1=1 rule is added when subscription is created, so we need to remove it
			await subscriptionClient.RemoveRuleAsync("$Default");

			//await subscriptionClient.AddRuleAsync(new RuleDescription
			//{
			//	Filter = new CorrelationFilter { Label = _storeId },
			//	Name = "filter-store"
			//});

			var mo = new MessageHandlerOptions(ExceptionHandler) { AutoComplete = true };

			subscriptionClient.RegisterMessageHandler(MessageHandler, mo);

			Console.ReadLine();
		}

		private static Task MessageHandler(Message message, CancellationToken arg2)
		{
			Console.WriteLine($"message Label: {message.Label}");
			Console.WriteLine($"CorrelationId: {message.CorrelationId}");
			var prodChangesString = Encoding.UTF8.GetString(message.Body);

			Console.WriteLine("Message Received");
			Console.WriteLine(prodChangesString);

			//Thread.Sleep(40000);

			return Task.CompletedTask;
		}


		private static Task ExceptionHandler(ExceptionReceivedEventArgs arg)
		{
			Console.WriteLine($"Handler exception {arg.Exception}.");
			var context = arg.ExceptionReceivedContext;
			Console.WriteLine($"Endpoint: {context.Endpoint}, Path: { context.EntityPath}, Action: { context.Action}");
			return Task.CompletedTask;
		}

		public static async void SendMessageAsync(Message message, string QueuePath)
		{
			var queueClient = new QueueClient(_config.ConnectionString, QueuePath);

			int tries = 0;
			while (true)
			{
				if ((tries > 10))
					break;

				await queueClient.SendAsync(message);
			}

			await queueClient.CloseAsync();
		}


	}
}
