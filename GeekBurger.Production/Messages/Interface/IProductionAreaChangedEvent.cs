using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Production.Messages.Interface
{
	public interface IProductionAreaChangedEvent
	{
		public Task Init();
	}
}
