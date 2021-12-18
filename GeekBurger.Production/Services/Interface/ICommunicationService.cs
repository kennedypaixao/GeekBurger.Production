using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Production.Services.Interface
{
	public interface ICommunicationService
	{
		public Microsoft.Azure.ServiceBus.SubscriptionClient ReceiveMessages(string topicName);
		public Task SendMessageAsync(string message, string topicName);
	}
}
