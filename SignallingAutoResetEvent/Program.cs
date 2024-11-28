using AutoResetEvent autoResetEvent = new AutoResetEvent(false);

////consumer thread
//autoResetEvent.WaitOne();

////producer thread
//autoResetEvent.Set();

string? userInput = null;
Console.WriteLine("Server is running. Type 'go' to proceed");

//Start the worker thread
//Thread workerThread = new Thread(Worker);
//workerThread.Start();
for(int i = 0; i<3;i++)
{
    Thread workThread = new Thread(Worker);
    workThread.Name = $"Worker - {i+1}";
    workThread.Start();
}

//Main Thread receives user input and sends signals

while (true)
{
    userInput = Console.ReadLine() ?? "";

    //Signal the worker thread if the input is "go"
    if(userInput.ToLower() == "go")
    {
        autoResetEvent.Set();
    }
}

void Worker()
{
    while (true)
    {
        Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for the signal");

        autoResetEvent.WaitOne();
        Console.WriteLine($"{Thread.CurrentThread.Name} proceeds");
        Thread.Sleep(2000);
    }
}

