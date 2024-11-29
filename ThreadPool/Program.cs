//ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxIOThreads);
//Console.WriteLine($"Max worker threads: {maxWorkerThreads} and Max IO threads: {maxIOThreads}");
//ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out var availableIOThreads);
//Console.WriteLine($"Max worker threads: {availableWorkerThreads} and Max IO threads: {availableIOThreads}");
//Console.WriteLine($"Max active threads = {maxWorkerThreads- availableWorkerThreads}");
////ThreadPool.SetMinThreads(availableIOThreads);
////ThreadPool.SetMaxThreads(maxWorkerThreads);
//ThreadPool.QueueUserWorkItem(a, b);
//Console.ReadLine();


Queue<string?> requestQueue = new Queue<string?>();

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
        break;
    }
    //main thread
    requestQueue.Enqueue(input);
}

void MonitorQueue()
{
    while (true)
    {
        if (requestQueue.Count > 0)
        {
            string? input = requestQueue.Dequeue();
            // Processing thread
            //Thread processingThread = new Thread(() => ProcessInput(input));
            //processingThread.Start();
            ThreadPool.QueueUserWorkItem(ProcessInput, input);
        }
        Thread.Sleep(100);
    }
}


//3. Processing the requests
void ProcessInput(object? input)
{
    // Simulate processing time
    Thread.Sleep(2000);
    Console.WriteLine($"Processed input: {input} Is ThreadPool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
}
