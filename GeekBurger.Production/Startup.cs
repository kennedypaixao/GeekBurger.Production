using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GeekBurger.Production.Extensions;
using System.IO;
using System.Linq;
using System;
using GeekBurger.Production.Model.Config;
using GeekBurger.Production.Services.Interface;
using GeekBurger.Production.Services;
using GeekBurger.Production.Messages;

namespace GeekBurger.Production
{
	public class Startup
	{

		private const string TopicName = "ProductChangedTopic";
		private static IConfiguration _configuration;
		private const string SubscriptionName = "paulista_store";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			_configuration = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json")
							.Build();


			#region Service Bus
			//var serviceBusNamespace = _configuration.GetServiceBusNamespace();
			//if (!serviceBusNamespace.Topics.List().Any(t => t.Name.Equals(TopicName, StringComparison.InvariantCultureIgnoreCase)))
			//{
			//	serviceBusNamespace.Topics
			//		.Define(TopicName)		
			//		.WithSizeInMB(1024)
			//		.Create();
			//}

			//var topic = serviceBusNamespace.Topics.GetByName(TopicName);

			//if (topic.Subscriptions.List().Any(subscription => subscription.Name.Equals(SubscriptionName, StringComparison.InvariantCultureIgnoreCase)))
			//{
			//	topic.Subscriptions.DeleteByName(SubscriptionName);
			//}

			//topic.Subscriptions.Define(SubscriptionName).Create();
			#endregion

		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekBurger.Production", Version = "v1" });
			});

			var config = Configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
			var communicationService = new CommunicationService(config.ConnectionString);
			services.AddTransient<ICommunicationService>((e) => communicationService);

			new ProductionAreaChangedEvent(communicationService);
			new OrderEvents(communicationService);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekBurger.Production v1"));

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
