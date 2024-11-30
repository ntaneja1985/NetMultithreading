bool cancelThread = false;

using var cts = new CancellationTokenSource();
var token = cts.Token;
var task = Task.Run(Work, token);


Console.WriteLine("To cancel press 'c'");

var input = Console.ReadLine();
if (input.ToLower() == "c")
{
    cts.CancelAfter(1000);
}

task.Wait();
Console.WriteLine($"Task Status is {task.Status}");
Console.ReadLine();
void Work()
{
    Console.WriteLine("Started doing work");
    for (int i = 0; i < 100000; i++)
    {
        Console.WriteLine($"{DateTime.Now}");
        //if (cancelThread)
        //{
        //    Console.WriteLine($"User requested cancellation at iteration: {i}");
        //    break;
        //}
        if (token.IsCancellationRequested)
        {
            Console.WriteLine($"User requested cancellation at iteration: {i}");
            //break;
            //throw new OperationCanceledException();
            token.ThrowIfCancellationRequested();
        }
        Thread.SpinWait(30000000);
    }
    Console.WriteLine("Work is done");


}