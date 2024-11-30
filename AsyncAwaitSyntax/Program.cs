//class Program
//{
//    static async Task Main(string[] args)
//    {
//        Console.WriteLine("Starting work");
//        await WorkAsync();
//        Console.WriteLine("Press enter to exit");
//        Console.ReadLine();
//    }

//    static async Task WorkAsync()
//    {
//        await Task.Delay(2000);
//        Console.WriteLine("Work is done");
//    }
//}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine($"Main Thread Id is {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine("Starting work");
        var data = await FetchDataAsync();
        Console.WriteLine($"Data is fetch: {data}");
        Console.WriteLine($"Thread Id after await is {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"Press enter to exit");
        Console.ReadLine();
    }

    static async Task<string> FetchDataAsync()
    {
        Console.WriteLine($"Fetch Data Task Thread Id is {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(2000);
        Console.WriteLine($"Fetch Data Task After Delay Thread Id is {Thread.CurrentThread.ManagedThreadId}");
        return "Complex Data";
    }
}