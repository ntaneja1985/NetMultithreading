DateTime beginTime = DateTime.Now;

int[] array = Enumerable.Range(0,1000000).ToArray();

long sum = 0;
object lockSum = new object();
//for(int i = 0; i < array.Length; i++)
//{
//    sum += array[i];
//}

//uses different threads
//Parallel.For(0, array.Length, i =>
//{
//    lock (lockSum)
//    {
//        sum += array[i];
//        Console.WriteLine($"Current Task Id: {Task.CurrentId}; Is it a thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
//    }
//});
ParallelLoopResult result;
try
{

    result = Parallel.For(0,
        array.Length,
        () => (long)0,
        (i,state,localStorage) =>
    {
        //lock (lockSum)
        //{
            //if (state.ShouldExitCurrentIteration && state.LowestBreakIteration < i)
            //    return;
                
            
                //if (i == 65)
                //{
                //    //throw new InvalidOperationException("This is on purpose");
                //    //state.Break();
                //    state.Stop();
                //}
                localStorage += array[i];
                //Console.WriteLine($"Current Task Id: {Task.CurrentId}; Is it a thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
                return localStorage;
        //}
    },
    localStorage =>
    {
        //protect the shared resource. only called once per thread not for every iteration
        lock (lockSum)
        {
            sum += localStorage;
            Console.WriteLine($"The task Id is {Task.CurrentId}");
        }
    });
}
catch(Exception ex)
{ Console.WriteLine(ex.ToString()); }


DateTime endTime = DateTime.Now;

Console.WriteLine($"Sum is {sum}");

Console.WriteLine($"Time Spent: {(endTime - beginTime).TotalMilliseconds}");
Console.ReadLine();

////Here i is not the index, it represents the element
//Parallel.ForEach(array, i =>
//{
//    lock (lockSum)
//    {
//        sum += i;
//    }
//});
//Console.WriteLine($"Sum is {sum}");
//Console.ReadLine();

//Takes an input array of delegates 
//Parallel.Invoke(() =>
//{
//    Console.WriteLine("I am one");
//}, () =>
//{
//    Console.WriteLine("I am two");
//});