using GeekBurger.Production.Contract;
using GeekBurger.Production.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Production.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductionController : Controller
	{
		private IEnumerable<Areas> AreasList;
		private ICommunicationService _commService;

		public ProductionController(ICommunicationService commService)
		{
			_commService = commService;
			AreasList = Areas.Init();
		}

		[HttpGet("/areas")]
		public IActionResult GetAreas()
		{
			if (AreasList.Count() == 0)
				return NotFound();
			
			return Ok(AreasList);
		}
	}
}
