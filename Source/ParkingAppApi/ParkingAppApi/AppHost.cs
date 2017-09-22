using Funq;
using ServiceStack;
using ParkingAppApi.ServiceInterface;
using ServiceStack.Api.Swagger;
using ParkingAppApi.Domain.Interfaces;
using ParkingAppApi.Domain.Repositories;
using System.Configuration;
using ServiceStack.Logging;
using AutoMapper;
using ServiceStack.Logging.Log4Net;
using ParkingAppApi.Domain.Models;
using ParkingAppApi.ServiceModel.Employee;
using ParkingAppApi.ServiceModel.Absence;
using ServiceStack.Text;

namespace ParkingAppApi
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyAspNet
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        
        public AppHost()
            : base("ParkingAppApi", typeof(ServiceInterface.EmployeeService).Assembly) {  }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            JsConfig.DateHandler = DateHandler.ISO8601;
            //added the swagger plugin
            Plugins.Add(new SwaggerFeature());

			// Config Dependancy Injection
			container.Register<IEmployeeRepository>((c) => new EmployeeRepository(ConfigurationManager.ConnectionStrings["MongoDatabaseConnection"].ConnectionString));
			container.RegisterAutoWiredAs<Domain.Services.EmployeeService, IEmployeeService>();
            container.RegisterAutoWiredAs<Domain.Gateways.AbsenceServiceGateway, IAbsenceServiceGateway>();

            //Configure logger
            LogManager.LogFactory = new Log4NetFactory(configureLog4Net: true);

			Plugins.Add(new CorsFeature(allowedOrigins: "*",
			allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
			allowedHeaders: "Content-Type",
			allowCredentials: false));

			// Mapper Configuration 
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<EmployeeView, EmployeeViewDto>();
				cfg.CreateMap<Absence, AbsenceDto>();
				cfg.CreateMap<ParkingBay, ParkingBayDto>();
				cfg.CreateMap<Employee, EmployeeDto>();

				cfg.CreateMap<AbsenceView, AbsenceViewDto>();

				cfg.CreateMap<Absence, LogAbsenceDto>().ReverseMap();

				cfg.CreateMap<EmployeeView, EmployeeViewDto>().ReverseMap();
				cfg.CreateMap<Absence, AbsenceDto>().ReverseMap();
				cfg.CreateMap<ParkingBay, ParkingBayDto>().ReverseMap();
				cfg.CreateMap<AbsenceView, AbsenceViewDto>().ReverseMap();

			});

		}
    }
}