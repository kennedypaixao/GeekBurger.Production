using GeekBurger.Production.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Production.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductionController : Controller
	{
		private IList<Areas> AreasList = new List<Areas>();

		public ProductionController()
		{
			AreasList.Add(new Areas { ProductionId = 1111, Restrictions = new List<string> { "soy", "dairy", "gluten", "sugar" }, On = true });
			AreasList.Add(new Areas { ProductionId = 1112, Restrictions = new List<string> { "soy" }, On = true });
			AreasList.Add(new Areas { ProductionId = 1113, Restrictions = new List<string> { "gluten" }, On = true });
			AreasList.Add(new Areas { ProductionId = 1114, Restrictions = new List<string> { }, On = true });
		}

		[HttpGet]
		public IActionResult GetAreas()
		{
			if (AreasList.Count() == 0)
				return NotFound();
			
			return Ok(AreasList);
		}
	}
}
