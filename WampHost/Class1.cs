using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.V2;
using WampSharp.V2.Realm;

namespace MyNamespace
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string serverAddress = "ws://127.0.0.1:8080/ws";

            DefaultWampHost host = new DefaultWampHost(serverAddress);

            host.Open();

            Console.WriteLine("Press enter after a subscriber subscribes to com.myapp.topic1");

            Console.ReadLine();

            IWampHostedRealm realm = host.RealmContainer.GetRealmByName("realm1");

            ISubject<int> subject =
                realm.Services.GetSubject<int>("com.myapp.topic1");

            int counter = 0;

            IObservable<long> timer =
                Observable.Timer(TimeSpan.FromMilliseconds(0),
                                 TimeSpan.FromMilliseconds(1000));

            IDisposable disposable =
                timer.Subscribe(x =>
                {
                    counter++;

                    Console.WriteLine("Publishing to topic 'com.myapp.topic1': " + counter);
                    try
                    {
                        subject.OnNext(counter);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });

            Console.ReadLine();
        }
    }
}