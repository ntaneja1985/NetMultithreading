var tasks = new[]
{
    Task.Run(() =>
    {
        throw new InvalidOperationException("Invalid Operation");
    }),
     Task.Run(() =>
    {
        throw new ArgumentNullException("Argument Null");
    }),
     Task.Run(() =>
    {
        throw new Exception("General Exception");
    })
};

//var t = Task.WhenAll(tasks);
//t.Wait();

Task.WhenAll(tasks).ContinueWith(t =>
{
    if (t.IsFaulted && t.Exception != null)
    {
        foreach (var ex in t.Exception.InnerExceptions)
        {
            Console.WriteLine(ex.Message);
        }
    }
}, TaskContinuationOptions.NotOnFaulted
    );

Console.WriteLine("Press any key to exit");
Console.ReadLine();

