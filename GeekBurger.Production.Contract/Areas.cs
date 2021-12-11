using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Production.Contract
{
	public class Areas
	{
		public int ProductionId { get; set; }
		public IEnumerable<string> Restrictions { get; set; }
		public bool On { get; set; }
	}
}
