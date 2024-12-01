using var cts = new CancellationTokenSource();  


var items = Enumerable.Range(1, 200);

ParallelQuery<int> evenNumbers = null;

try
{
    evenNumbers = items.AsParallel()
        .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
        .WithCancellation(cts.Token)
        .Where(x =>
    {
        Console.WriteLine($"Processing number {x}, Current Thread Id is {Thread.CurrentThread.ManagedThreadId} ");
        //if (x == 5) throw new InvalidOperationException($"This is intentional..{x}");
        //if (x == 19) throw new ArgumentNullException($"This is intentional..{x}");
        return (x % 2 == 0);
    });
}
catch(Exception ex)
{ Console.WriteLine(ex.ToString()); }

//Console.WriteLine($"There are {evenNumbers.Count()} even numbers in the collection");

//foreach (var item in evenNumbers)
//{
//    Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
//}

//Exception is thrown and caught here.
try
{
    evenNumbers.ForAll(item =>
    {
        if(item > 8) cts.Cancel();
        Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
    });
}
catch(OperationCanceledException ex)
{
    Console.WriteLine(ex.Message.ToString());
}
catch(AggregateException ex)
{
    ex.Handle(x =>
    {
        Console.WriteLine(x.Message.ToString());
        return true;
    });
    
}

//Console.ReadLine();