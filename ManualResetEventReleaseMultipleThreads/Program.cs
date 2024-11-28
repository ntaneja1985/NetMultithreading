using ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

//turn on the signal
//manualResetEvent.Set();

//turn off the signal
//manualResetEvent.Reset();
Console.WriteLine("Press enter to release all threads...");

for (int i = 0; i < 3; i++)
{
    Thread thread = new Thread(Work);
    thread.Name = $"Thread {i}";
    thread.Start();
}


Console.ReadLine();
manualResetEvent.Set();
Thread.Sleep(5000);
Console.WriteLine("Press enter to reset all threads");
Console.ReadLine();
manualResetEvent.Reset();
Console.ReadKey();
void Work()
{
    Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for the signal");
    manualResetEvent.Wait();
    Thread.Sleep(1000);
    Console.WriteLine($"{Thread.CurrentThread.Name} has been released");
}



