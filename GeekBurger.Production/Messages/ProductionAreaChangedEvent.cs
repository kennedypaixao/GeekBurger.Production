using GeekBurger.Production.Contract;
using GeekBurger.Production.Services.Interface;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Production.Messages
{
	public class ProductionAreaChangedEvent
	{
		private IEnumerable<Areas> AreasList;
		private ICommunicationService _commService;

		public ProductionAreaChangedEvent(ICommunicationService commService)
		{
			_commService = commService;
			AreasList = Areas.Init();

			Init();
		}

		public Task Init()
		{
			string messageBody = JsonConvert.SerializeObject(AreasList);
			return _commService.SendMessageAsync(messageBody, "ProductionAreaChanged");
		}

	}
}
