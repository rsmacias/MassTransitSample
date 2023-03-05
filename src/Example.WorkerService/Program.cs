using MassTransit;

namespace Example.WorkerService {

    public class Program {

        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => {

                    services.AddMassTransit(x => {

                        x.AddConsumer<MessageConsumer>();

                        // x.UsingInMemory((context, cfg) => {
                        //     cfg.ConfigureEndpoints(context);
                        // });

                        x.UsingRabbitMq((context, cfg) => {
                            cfg.Host(new Uri("rabbitmq://localhost/workforce"), h => {
                                h.Username("foreman");
                                h.Password("w0rkf0rc3!");
                            });

                            cfg.ReceiveEndpoint("TC.Workforce.Foreman.LP", ep => {
                                ep.ConfigureConsumer<MessageConsumer>(context);
                            });
                        });
                    });

                    //services.AddMassTransitHostedService(true);

                    services.AddHostedService<Worker>();

                });

    }
    
}
// IHost host = Host.CreateDefaultBuilder(args)
//     .ConfigureServices(services =>
//     {
//         services.AddHostedService<Worker>();
//     })
//     .Build();

// host.Run();

 
