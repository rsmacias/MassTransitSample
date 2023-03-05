using System.Threading.Tasks;
using MassTransit;
using Example.Contracts;

namespace Example.Publisher {
    public class Program {
        public static async Task Main () {

            var instance = RabbitMqSingleton.GetInstance(x => { 
                x.Host(new Uri("rabbitmq://localhost/workforce"), 
                sbc => { 
                    sbc.Username("foreman"); 
                    sbc.Password("w0rkf0rc3!"); 
                }); 
            });

            var endpoint = await instance.Bus.GetSendEndpoint(new Uri("queue:TC.Workforce.Foreman.LP"));
            await endpoint.Send(new Message { Text = "Ground control to Major Tom" });

           await instance.Bus.StopAsync();
           Console.WriteLine("Message is send");
        }
    }
}
