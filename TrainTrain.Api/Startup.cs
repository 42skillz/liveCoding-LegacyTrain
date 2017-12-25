using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrainTrain.Domain;
using TrainTrain.Infra;

namespace TrainTrain.Api
{
    public class Startup
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Step1: Instantiate the "I want to go out" adapters
            var trainDataServiceAdapter = new TrainDataService(UriTrainDataService);
            var bookingReferenceServiceAdapter =new BookingReferenceService(UriBookingReferenceService);

            IReserveSeats hexagon = new SeatsReservation(trainDataServiceAdapter, bookingReferenceServiceAdapter);

            // Step3: Instantiate the "I want to go in" adapter(s)
            var seatsReservationAdapter = new SeatsReservationAdapter(hexagon);

            services.AddSingleton(seatsReservationAdapter);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
        }
    }
}
