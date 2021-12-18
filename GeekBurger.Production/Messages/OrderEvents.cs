using GeekBurger.Production.Services.Interface;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Production.Messages
{
	public class OrderEvents
	{
		private ICommunicationService _commService;

		public OrderEvents(ICommunicationService commService)
		{
			_commService = commService;
			InitNewOrder();
			InitNewOrder();
		}

		private void InitNewOrder()
		{
			Microsoft.Azure.ServiceBus.SubscriptionClient sub = _commService.ReceiveMessages("NewOrder");
			var mo = new MessageHandlerOptions(_exceptionHandler) { AutoComplete = true };
			sub.RegisterMessageHandler(_newOrderMessageHandler, mo);
		}

		private void InitOrderChanged()
		{
			Microsoft.Azure.ServiceBus.SubscriptionClient sub = _commService.ReceiveMessages("OrderChanged");
			var mo = new MessageHandlerOptions(_exceptionHandler) { AutoComplete = true };
			sub.RegisterMessageHandler(_orderChangedMessageHandler, mo);
		}

		private Task _newOrderMessageHandler(Message message, CancellationToken arg2)
		{
			var prodChangesString = Encoding.UTF8.GetString(message.Body);
			return Task.CompletedTask;
		}

		private Task _orderChangedMessageHandler(Message message, CancellationToken arg2)
		{
			var prodChangesString = Encoding.UTF8.GetString(message.Body);
			return Task.CompletedTask;
		}

		private Task _exceptionHandler(ExceptionReceivedEventArgs arg)
		{
			Console.WriteLine($"Handler exception {arg.Exception}.");
			var context = arg.ExceptionReceivedContext;
			Console.WriteLine($"Endpoint: {context.Endpoint}, Path: { context.EntityPath}, Action: { context.Action}");
			return Task.CompletedTask;
		}
	}
}
