using Azure.Messaging.ServiceBus;
using GeekBurger.Production.Model.Config;
using GeekBurger.Production.Services.Interface;
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

namespace GeekBurger.Production.Services
{
	public class CommunicationService : ICommunicationService
	{
		private readonly string _connectionString;
		private ServiceBusClient client;
		private ServiceBusSender sender;

		public CommunicationService(string connection)
		{
			_connectionString = connection;
		}

		public Microsoft.Azure.ServiceBus.SubscriptionClient ReceiveMessages(string topicName)
		{
			var subscriptionName = "production";
			return new Microsoft.Azure.ServiceBus.SubscriptionClient(_connectionString, topicName, subscriptionName);
		}
		public async Task SendMessageAsync(string message, string topicName)
		{
			client = new ServiceBusClient(_connectionString);
			sender = client.CreateSender(topicName);

			using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

			if (!messageBatch.TryAddMessage(new ServiceBusMessage(message)))
			{
				throw new Exception($"The message is too large to fit in the batch.");
			}

			try
			{
				await sender.SendMessagesAsync(messageBatch);
			}
			finally
			{
				await sender.DisposeAsync();
				await client.DisposeAsync();
			}
		}
	}
}
