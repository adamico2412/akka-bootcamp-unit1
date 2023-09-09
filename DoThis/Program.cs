using System;
using Akka.Actor;
using Akka.Actor.Dsl;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            // nothing here where our actors used to be!
            var tailCoordinatorProps = Props.Create<TailCoordinatorActor>();
            var tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");
            
            var consoleWriterProps = Props.Create(() => new ConsoleWriterActor());
            var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps,
                "consoleWriterActor");
            
            var validationActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
            var validationActor = MyActorSystem.ActorOf(validationActorProps,
                "validationActor");
            
            var consoleReaderProps = Props.Create<ConsoleReaderActor>();
            var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps,
                "consoleReaderActor");
            
            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
