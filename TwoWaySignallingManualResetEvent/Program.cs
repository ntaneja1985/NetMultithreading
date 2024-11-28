Queue<int> queue = new Queue<int>();
ManualResetEventSlim consumeEvent = new ManualResetEventSlim(false);
ManualResetEventSlim produceEvent = new ManualResetEventSlim(true);

int consumerCounter = 0;
object lockConsumerCount = new object();    

Thread[] consumerThreads = new Thread[3];
for(int i=0; i<consumerThreads.Length;i++)
{
    consumerThreads[i] = new Thread(Consume);
    consumerThreads[i].Name = $"Consumer - {i + 1}";
    consumerThreads[i].Start();
}

while(true)
{
    produceEvent.Wait();
    produceEvent.Reset();
    Console.WriteLine("To produce, enter 'p'");
    var input = Console.ReadLine() ?? "";
    if(input.ToLower() =="p")
    {
        for(int i = 0; i < 10; i++)
        {
            queue.Enqueue(i);
            Console.WriteLine($"Produced: {i}");
        }
        consumeEvent.Set();
    }

}

//Consumer's behaviour
void Consume()
{
    while (true)
    {
        consumeEvent.Wait();

        while (queue.TryDequeue(out int input))
        {
            //work on the items produced
            Thread.Sleep(500);
            Console.WriteLine($"Consumed: {input} from thread: {Thread.CurrentThread.Name}");
        }

        lock (lockConsumerCount)
        {
            consumerCounter++;
            if (consumerCounter == 3)
            {
                consumeEvent.Reset();
                produceEvent.Set();
                consumerCounter = 0;
                Console.WriteLine("***********");
                Console.WriteLine("Please produce more....");
                Console.WriteLine("***********");
            }
        }
       
    }
}
