using System;
using WampSharp.V2;
using WampSharp.V2.Rpc;

namespace MyNamespace
{
    public interface IArgumentsService
    {
        [WampProcedure("com.arguments.ping")]
        void Ping();

        [WampProcedure("com.arguments.add2")]
        int Add2(int a, int b);

        [WampProcedure("com.arguments.stars")]
        string Stars(string nick = "somebody", int stars = 0);

        [WampProcedure("com.arguments.orders")]
        string[] Orders(string product, int limit = 5);
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            DefaultWampChannelFactory factory =
                new DefaultWampChannelFactory();

            const string serverAddress = "ws://127.0.0.1:8080/ws";

            IWampChannel channel =
                factory.CreateJsonChannel(serverAddress, "realm1");

            channel.Open().Wait(5000);

            IArgumentsService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IArgumentsService>();

            proxy.Ping();
            Console.WriteLine("Pinged!");

            int result = proxy.Add2(2, 3);
            Console.WriteLine("Add2: {0}", result);

            var starred = proxy.Stars();
            Console.WriteLine("Starred 1: {0}", starred);

            starred = proxy.Stars(nick: "Homer");
            Console.WriteLine("Starred 2: {0}", starred);

            starred = proxy.Stars(stars: 5);
            Console.WriteLine("Starred 3: {0}", starred);

            starred = proxy.Stars(nick: "Homer", stars: 5);
            Console.WriteLine("Starred 4: {0}", starred);

            string[] orders = proxy.Orders("coffee");
            Console.WriteLine("Orders 1: {0}", string.Join(", ", orders));

            orders = proxy.Orders("coffee", limit: 10);
            Console.WriteLine("Orders 2: {0}", string.Join(", ", orders));

            Console.ReadLine();
        }
    }
}