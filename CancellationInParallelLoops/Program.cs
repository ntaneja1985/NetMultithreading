bool cancelThread = false;

using var cts = new CancellationTokenSource();
var token = cts.Token;

var task = Task.Run(Work, token);


Console.WriteLine("To cancel press 'c'");

var input = Console.ReadLine();
if (input.ToLower() == "c")
{
    //cts.CancelAfter(1000);
    cts.Cancel();
}

task.Wait();
Console.WriteLine($"Task Status is {task.Status}");
Console.ReadLine();
void Work()
{
    Console.WriteLine("Started doing work");
    var options = new ParallelOptions { CancellationToken = cts.Token };
    Parallel.For(0, 100000, options, i =>
    {
        Console.WriteLine($"{DateTime.Now}");
        Thread.SpinWait(300000000);
    });
    Console.WriteLine("Work is done");


}