//Thread thread = new Thread(Work);
//thread.Start();

//Task task = new Task(Work);
//task.Start();

//var t = Task.Run(Work);
//Console.WriteLine($"Result is {t.Result}");

//Console.ReadLine();
//int Work()
//{
//    Console.WriteLine($"I love programming! and is ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}");
//    int result = 100;
//    return result;
//}

int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
int SumSegment(int start, int end)
{
    int segmentSum = 0;
    for (int i = start; i < end; i++)
    {
        Thread.Sleep(100);
        segmentSum += array[i];
    }
    return segmentSum;
}


var startTime = DateTime.Now;
int numOfThreads = 4;
int segmentLength = array.Length / numOfThreads;
Task<int>[] tasks = new Task<int>[numOfThreads];
tasks[0] = Task.Run(() => { return SumSegment(0, segmentLength); });
tasks[1] = Task.Run(() => { return SumSegment(segmentLength, 2 * segmentLength); });
tasks[2] = Task.Run(() => { return SumSegment(2 * segmentLength, 3 * segmentLength); });
tasks[3] = Task.Run(() => { return SumSegment(3 * segmentLength, array.Length); });

//foreach (int i in array)
//{
//    Thread.Sleep(100);
//    sum += i;
//}

//foreach (var thread in threads)
//{
//    thread.Start();
//}

//foreach (var thread in threads)
//{
//    thread.Join();
//}


//Console.WriteLine(sum);
//Console.WriteLine($"The sum is {tasks[0].Result + tasks[1].Result + tasks[2].Result + tasks[3].Result }");
//Console.WriteLine($"The sum is {tasks.Sum(x=>x.Result)}");  //This line is blocking

Task.WhenAll(tasks).ContinueWith(t =>
{
    Console.WriteLine($"The summary is {t.Result.Sum()}");
});

Console.WriteLine("This is the end of the program");
var endTime = DateTime.Now;
var timespan = endTime - startTime;

Console.WriteLine($"Time taken = {timespan.TotalMilliseconds}");
Console.ReadLine();