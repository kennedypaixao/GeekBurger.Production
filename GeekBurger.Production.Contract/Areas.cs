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

		public static IEnumerable<Areas> Init()
		{
			IList<Areas> areasList = new List<Areas>();
			areasList.Add(new Areas { ProductionId = 1111, Restrictions = new List<string> { "soy", "dairy", "gluten", "sugar" }, On = true });
			areasList.Add(new Areas { ProductionId = 1112, Restrictions = new List<string> { "soy" }, On = true });
			areasList.Add(new Areas { ProductionId = 1113, Restrictions = new List<string> { "gluten" }, On = true });
			areasList.Add(new Areas { ProductionId = 1114, Restrictions = new List<string> { }, On = true });
			return areasList;
		}
	}
}
