using System.Collections.Concurrent;

////FIFO
//var queue = new ConcurrentQueue<int>();
//queue.Enqueue(1);
//queue.Enqueue(2);
//queue.Enqueue(3);

//queue.TryDequeue(out var result);
//Console.WriteLine(result);
//Console.ReadLine();

//var stack = new ConcurrentStack<int>();
//stack.Push(0);
//stack.Push(1);
//stack.Push(2);
//stack.Push(3);
//stack.TryPop(out var result);
//Console.WriteLine(result);

using System.Net.Http.Headers;

var requestQueue = new ConcurrentQueue<string?>();
BlockingCollection<string?> collection = new BlockingCollection<string?>(requestQueue,3);

//2. Start the request monitoring thread
Thread monitoringThread = new Thread(MonitorQueue);
monitoringThread.Start();

//1. Enqueue the requests
Console.WriteLine("Server is running. Type 'exit' to stop.");
while (true)
{
    string? input = Console.ReadLine();
    if (input?.ToLower() == "exit")
    {
        collection.CompleteAdding(); 
        break;
    }

    //main thread
    //requestQueue.Enqueue(input);
    collection.Add(input);

    Console.WriteLine($"Enqueued: {input}; queue size is :{collection.Count}");
}

void MonitorQueue()
{
    //while (true)
    //{
        foreach(var request in collection.GetConsumingEnumerable())
        {
            if(collection.IsCompleted) break;
            // Processing thread
            Thread processingThread = new Thread(() => ProcessInput(request));
            processingThread.Start();
            
            Thread.Sleep(2000);
        }
        //if (requestQueue.Count > 0)
        //{
        //    if (requestQueue.TryDequeue(out string? input))
        //    {
        //        // Processing thread
        //        Thread processingThread = new Thread(() => ProcessInput(input));
        //        processingThread.Start();
        //    }
        //}
        
    //}
}


//3. Processing the requests
void ProcessInput(string? input)
{
    // Simulate processing time
    Thread.Sleep(2000);
    Console.WriteLine($"Processed input: {input}");
}

