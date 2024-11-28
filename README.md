# Net Multithreading
.NET Multithreading
## CPU, Thread and Thread Scheduler
- CPU can only process a thread, so a thread is a basic unit that can run inside the CPU.
- CPU cant run the whole application
- One application needs to have atleast one thread
- Without a thread, application wont exist, it cant perform any task
- An application has a main thread
- When application is loaded into memory the main thread is assigned to the CPU and the application can run
- It is the job of thread scheduler that looks at the available application and assigns thread to the CPU 
- Thread is the basic unit that CPU can process
- We can have multiple applications running within a computer
- Thread Scheduler decides which thread should be allocated to CPU to process
- It makes the decision based on several factors, for e.g some applications are more mission critical than other applications, so their thread has higher priority
- If a high priority thread is assigned to CPU but it is not doing anything thread scheduler can remove the thread and assign another thread for time being
- This is called time-slicing. 
- Thread scheduler, the developer cannot influence. It is part of the OS.
- Multi-threading is when one application has multiple threads
- Thread scheduler only sees threads
- It assigns threads to CPU to process based on the thread scheduling algorithms
- In a multi-core CPU, we may be able to process multiple threads in different CPUs 
- Now Thread scheduler can assign one thread per core 
- ![alt text](image.png)

## Basic syntax of using threads
- A thread needs to perform certain task
- So we need to assign a C# delegate(which is the name of the function or the method) that the thread needs to run
```c#
    void WriteThreadId()
{
    for (int i = 0; i < 100; i++)
    {
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(50);
    }
}

WriteThreadId();

Thread thread1 = new Thread(WriteThreadId);
thread1.Start();

Thread thread2 = new Thread(WriteThreadId);
thread2.Start();

Console.ReadLine();
```
- In the above code, the WriteThreadId() method in the main thread is blocking. It has to be completed before thread1 and thread2 can start
- How threadIds are printed in thread1 and thread2 depends on CPU scheduler. There is no particular pattern is this.
- Thread scheduler is working to assign different threads within the application to the CPU.
- We can influence the thread scheduler in different ways:
- We can assign priority to threads like this:
```c#


void WriteThreadId()
{
    for (int i = 0; i < 100; i++)
    {
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        //Thread.Sleep(50);
    }
}



Thread thread1 = new Thread(WriteThreadId);
Thread thread2 = new Thread(WriteThreadId);

thread1.Priority = ThreadPriority.Highest;
thread2.Priority = ThreadPriority.Lowest;
Thread.CurrentThread.Priority = ThreadPriority.Normal;
thread1.Start();
thread2.Start();

WriteThreadId();

Console.ReadLine();

```
- We can also do time-slicing by introducing Thread.Sleep(50)
- In this case, if a task is taking too long to finish, CPU scheduler will kick it out of CPU and put in another thread to process
- In this case assigning priority to threads wont really work. 
- We can assign names to the threads like this
```c#
    thread1.Name = "Thread1";
    thread2.Name = "Thread2";
    Thread.CurrentThread.Name = "MainThread";
```
## Why we do we need to start a new Thread?
- **Solve divide and conquer type of problems**
- If we have a big task, we can divide the task into multiple smaller chunks so multiple people can work on it on parallel
- ![alt text](image-1.png)
- Lets say we have an array with 10 elements from 1- 10, if we do their sum and print out the time taken we can do it like this
```c#
int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
int sum = 0;
var startTime = DateTime.Now;
foreach (int i in array)
{
    Thread.Sleep(100);
    sum += i;
}
var endTime = DateTime.Now;
var timespan = endTime - startTime;
Console.WriteLine($"Time taken = {timespan.TotalMilliseconds}"); //Output of 1100 milliseconds
```
- This will take about 1100 seconds for a single thread to calculate and display the sum of 10 numbers to the user
- Now we can divide this array into multiple segments and have each segment processed by a different thread
```c#
 int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
int SumSegment(int start,int end)
{
    int segmentSum = 0;
    for (int i = start; i < end; i++)
    {
        Thread.Sleep(100);
        segmentSum += array[i];
    }
    return segmentSum;
}

int sum1 = 0,sum2 = 0,sum3 = 0,sum4 = 0;

var startTime = DateTime.Now;
int numOfThreads = 4;
int segmentLength = array.Length / numOfThreads;
Thread[] threads = new Thread[numOfThreads];
threads[0] = new Thread(() => { sum1 = SumSegment(0, segmentLength); });
threads[1] = new Thread(() => { sum2 = SumSegment(segmentLength, 2* segmentLength); });
threads[2] = new Thread(() => { sum3 = SumSegment(2* segmentLength, 3* segmentLength); });
threads[3] = new Thread(() => { sum4 = SumSegment(3* segmentLength, array.Length); });

foreach (var thread in threads)
{
    thread.Start();
}

foreach (var thread in threads)
{
    thread.Join();
}

var endTime = DateTime.Now;
var timespan = endTime - startTime;

Console.WriteLine($"The sum is {sum1 + sum2 + sum3 + sum4}"); 
Console.WriteLine($"Time taken = {timespan.TotalMilliseconds}"); //Outputs 440 milliseconds
Console.ReadLine();
```

- In the above code, we have divided the array into multiple segments with segment length equal to array length divided by number of threads = 4
- We calculate the sum of each segment into each of their individual threads and then join their results
- If we see the output here, time taken is 400 milliseconds which is faster than 1100 milliseconds taken earlier
- Therefore, by dividing our problem into multiple threads, we can process it much faster.

## Why threading: Offload long running tasks
- We can offload long running task to a different thread
- ![alt text](image-2.png)
- Lets say we have a form with 2 buttons and each button displays some text inside a label in the form
 ```c#
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        ShowMessage("First Message", 3000);

    }

    

    private void button2_Click(object sender, EventArgs e)
    {
        ShowMessage("Second Message", 5000);

    }

    private void ShowMessage(string message, int delay)
    {
        Thread.Sleep(delay);
        lblMessage.Text = message;
    }
}

```
- In the above case each click is a long running task. It blocks the main thread and makes the UI unresponsive.
- To fix this, we need to run it in its own worker thread so that the main thread is not blocked

```c#
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Thread thread = new Thread(() => ShowMessage("First Message", 3000));
       thread.Start();
    }

    

    private void button2_Click(object sender, EventArgs e)
    {
        Thread thread = new Thread(() => ShowMessage("Second Message", 5000));
        thread.Start();
    }

    private void ShowMessage(string message, int delay)
    {
        Thread.Sleep(delay);
        lblMessage.Text = message;
    }
}

```

## Simulate a web server that handles concurrent requests
- ![alt text](image-3.png)
- Lets say we have a web server that handles multiple requests
- We need to have a Monitor Queue that monitors the incoming requests and assigns them to a Request Processor
- If we do it in a single thread this would be blocking the user inputs till each input is processed
- We need to solve it using Divide and Conquer strategy
```c#

//Using a single thread
Console.WriteLine("Server is running. Type 'exit' to stop.");
while (true)
{
    string? input = Console.ReadLine();
    if (input?.ToLower() == "exit")
    {
        break;
    }
    ProcessInput(input);
}

static void ProcessInput(string? input)
{
    // Simulate processing time
    Thread.Sleep(2000);
    Console.WriteLine($"Processed input: {input}");
}

```
- We need to enqueue the request into the request queue.
- We need to have a monitor queue
- We need to process this request
- To accomplish this we have the following code:
```c#
using System.Net.Http.Headers;

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
            Thread processingThread = new Thread(() => ProcessInput(input));
            processingThread.Start();
        }
        Thread.Sleep(100);
    }
}


//3. Processing the requests
 void ProcessInput(string? input)
{
    // Simulate processing time
    Thread.Sleep(2000);
    Console.WriteLine($"Processed input: {input}");
}
```
- Notice above we have 3 threads, main thread for enqueuing requests, monitoring thread to monitor the queue of requests and processor thread to process the request
- Notice we have using Queue collection here. What if the CPU scheduler schedules the main thread and monitoring thread to work at the same time in different cores of CPU 
- In this case, we can have race conditions so we will have to use Threadsafe collections or Parallel Collections. 

## Thread Synchronization
- What if threads need to share some resources ?
- ![alt text](image-4.png)
- We need to synchronize the threads to process them in parallel
- We need thread synchronization
- Lets say we have a counter variable and 2 threads are trying to increment it at the same time like this
```c#
int counter = 0;

Thread thread1 = new Thread(IncrementCounter);

//thread1.Join();
Thread thread2 = new Thread(IncrementCounter);
//Execute the threads in parallel at the same time
thread1.Start();
thread2.Start();

//Wait for both the threads to complete
thread1.Join();
thread2.Join();

Console.WriteLine($"Final Counter Value = {counter}");
Console.ReadLine();
void IncrementCounter()
{
    for (int i = 0; i < 100000; i++)
    {
        //assigment is not an atomic operation
        var temp = counter;
        counter = temp + 1;
    }
}

```
- In the above case we will have inconsistent output, the value of counter can be 200000 or 179541 etc.
- Here 2 threads are trying to share the same resource
- This kind of problems are called race conditions and inconsistent behaviors. How the Thread scheduler schedules the threads to run, it depends on it.
- In above case, final result changes several times every time we run it. Here threads are interfering with each other since they are sharing one resource(counter)
- Therefore, we need Thread synchronization

## Critical Section
- A critical section in multi-threading is like a VIP area in a club where only one person can enter at a time. It refers to a segment of code that accesses shared resources, such as variables, data structures, or hardware devices, which must not be concurrently accessed by multiple threads. 
- This ensures data integrity and avoids race conditions.
- **Exclusive Access**: Only one thread can enter the critical section at a time.
- **Mutual Exclusion**: Mechanisms like locks, semaphores, or mutexes are used to enforce exclusive access.
- By carefully managing access to critical sections, we ensure the integrity and consistency of shared resources in a multi-threaded environment.
- In the above code, following is the critical section as this accesses the shared resource: counter:
```c#
 var temp = counter;
 counter = temp + 1;
```
- If the code inside the critical section of code, is a single indivisible step, then there is no problem of thread synchronization.
- However if it is not atomic and there are multiple steps, then we need thread synchronization
- We need a mechanism to make code inside critical section as indivisible.

## Synchronization technique : Exclusive lock
```c#
lock()
{
    //critical section of code
}

```
- Body of lock can only be accessed by only one thread
- Makes operations within body of lock as atomic.
```c#
object counterLock = new object();

void IncrementCounter()
{
    for (int i = 0; i < 100000; i++)
    {
        lock (counterLock)
        {
            //assigment is not an atomic operation
            counter = counter + 1;
        }
    }
}

```

- In .NET 9(C# 13) instead of using an object type for counterLock use System.Threading.Lock 
```c#
System.Threading.Lock counterLock = new System.Threading.Lock();

```
## Assignment: Airplane Booking System
- web server for users to buy tickets
- ![alt text](image-5.png)
- use letter b to book and letter c to cancel ticket
```c#


Queue<string?> requestQueue = new Queue<string?>();
int availableTickets = 10;

object ticketsLock = new object();

//2. Start the request monitoring thread
Thread monitoringThread = new Thread(MonitorQueue);
monitoringThread.Start();

//1. Enqueue the requests
Console.WriteLine("Server is running. \r\n Type 'b' to book a ticket. Type 'c' to cancel. \r\n Type 'exit' to stop.");
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
            Thread processingThread = new Thread(() => ProcessBooking(input));
            processingThread.Start();
        }
        Thread.Sleep(100);
    }
}


//3. Processing the requests
void ProcessBooking(string? input)
{
    // Simulate processing time
    Thread.Sleep(2000);
    //Console.WriteLine($"Processed input: {input}");
    lock (ticketsLock)
    {
        if (input == "b")
        {

            if (availableTickets > 0)
            {
                availableTickets--;
                Console.WriteLine();
                Console.WriteLine($"Your seat is booked. {availableTickets} seats are still available");
            }
            else
            {
                Console.WriteLine("No tickets are available");
            }


        }
        else if (input == "c")
        {
            if (availableTickets < 10)
            {
                availableTickets++;
                Console.WriteLine();
                Console.WriteLine($"Your seat is cancelled. {availableTickets} seats are available");

            }
            else
            {
                Console.WriteLine("Cannot cancel booking. 10 or more tickets are already available");
            }
        }

    }   
}
```

## Monitor : Thread Synchronization technique
- Monitor monitors the critical section
- If one thread enters the critical section, monitor blocks other threads
- Monitor generates an exclusive lock
- Monitor gives more control compared to lock
- Monitor.Enter generates an exclusive lock
- Monitor.Exit is called at end of critical section to release the lock
- we use try {} finally{} mechanism
```c#
Monitor.Enter(lockObject)
try {
    //Your code here
}
finally {
    Monitor.Exit(lockObject)
}
```
- We can express our increment counter code as following:
```c#
void IncrementCounter()
{
    for (int i = 0; i < 100000; i++)
    {
        Monitor.Enter(counterLock);
        try
        {
            //assigment is not an atomic operation
             counter = counter + 1;
        }
        finally
        {
            Monitor.Exit(counterLock);
        }
    }
}

```
- Monitor has another feature called Waiting Timeout time
- Lets say there are multiple threads in operation and one thread is waiting to acquire an exclusive lock on the critical section
- In such a scenario, we may want to show a message to the user that the system is busy, Please wait. We can do it like this
- Here we use Monitor.TryEnter(ticketsLock, 2000). This means thread will try to acquire a lock for 2 seconds, if it is not able to get the lock, then it will give the error message to wait to the user
```c#
 void ProcessBooking(string? input)
{
    
    //Console.WriteLine($"Processed input: {input}");
    if(Monitor.TryEnter(ticketsLock, 2000))
    {
        try
        {
            // Simulate processing time
            Thread.Sleep(3000);
            if (input == "b")
            {

                if (availableTickets > 0)
                {
                    availableTickets--;
                    Console.WriteLine();
                    Console.WriteLine($"Your seat is booked. {availableTickets} seats are still available");
                }
                else
                {
                    Console.WriteLine("No tickets are available");
                }


            }
            else if (input == "c")
            {
                if (availableTickets < 10)
                {
                    availableTickets++;
                    Console.WriteLine();
                    Console.WriteLine($"Your seat is cancelled. {availableTickets} seats are available");

                }
                else
                {
                    Console.WriteLine("Cannot cancel booking. 10 or more tickets are already available");
                }
            }
        }

        finally
        {
            Monitor.Exit(ticketsLock);
        }
    }
    else
    {
        Console.WriteLine("The system is busy. Please wait");
    }


}

```
- Both lock and Monitor acquire an exclusive lock on the critical section

## Mutex: Thread Synchronization Mechanism
- Similar to lock or Monitor 
- Creates a lock around critical section
- Syntax is similar to monitor
```c#
using(var mutex = new Mutex())
{
    //ownership of mutex and critical section
    //Used to acquire mutex
    mutex.WaitOne();
    try{
        //critical section
    }
    finally
    {
        mutex.ReleaseMutex();
    }
}
```
- If we already have lock and monitor why we need mutex?
- **Mutex can not only be used within the process but it can be used across processes also**
- ![alt text](image-6.png)
- Each app can have different processes and each process can have different threads inside it
- ![alt text](image-7.png)
- We usually have multiple processes in each application to improve its performance
- **For e.g in a browser, each tab could be in a different process**
- From an architecture point of view, this is a better separation of concerns.
- From performance point of view, we could utilize multiple cores in the CPU 
- When multiple processes access the same resource, it can cause race conditions. Here we cant use monitor or lock anymore coz they are limited to the same process. So we need mutex.
- The above code for mutex is for within the same process.
- To use mutex across processes use this syntax:
```c#
//Give the mutex a name to be used across processes
using(var mutex = new Mutex(false, "GlobalFileMutex))
{
    //ownership of mutex and critical section
    //Used to acquire mutex
    mutex.WaitOne();
    try{
        //critical section
    }
    finally
    {
        mutex.ReleaseMutex();
    }
}

```
- We use using statement so that we can dispose the mutex in the end as it is a resource in the OS.
- ![alt text](image-8.png)
- If we go to bin/debug of our project and run the exe for console app twice, it will start 2 separate processes.
- Here the counter.txt file is the shared resource. Since Mutex helps us to keep a lock across processes, therefore value of counter inside the file would be 20000
```c#
string filePath = "counter.txt";

using (var mutex = new Mutex(false, "GlobalFileMutext"))
{
    for (int i = 0; i < 10000; i++)
    {
        mutex.WaitOne();
        try
        {
            int counter = ReadCounter(filePath);
            counter++;
            WriteCounter(filePath, counter);
        }

        finally
        {
            mutex.ReleaseMutex();
        }
    }
}


Console.WriteLine("Process finished");
Console.ReadLine();

int ReadCounter(string filePath)
{
    using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
    using (var reader = new StreamReader(stream))
    {
        string content = reader.ReadToEnd();
        return string.IsNullOrEmpty(content) ? 0 : int.Parse(content);
    }
}

void WriteCounter(string filePath, int counter)
{
    using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
    using (var writer = new StreamWriter(stream))
    {
        writer.Write(counter);
    }
}

```
- As we give mutex a name, it becomes global across all the processes.
- Within the process, use lock or monitor to protect critical section and across processes use Mutex.
- Remember mutexes are an operating system wide resource, it actually takes more resource to create a mutex than to create a lock or monitor


## Reader and Writer Lock: Another Thread Synchronization Technique
- lock, monitor, mutex are exclusive locks.
- the above locks affect performance of the application.
- ![alt text](image-10.png)
- Suppose we have multiple Readers and Writer threads trying to access a shared resource
- We want that when we acquire a reader lock, then we can share the lock only across the reader threads but block all writer threads.
- So it is shared lock for reader threads but exclusive lock against the writer threads.
- The reader and writer lock has to work in such a way that allows multiple readers to read the resource simultaneously.
- When one of the reader threads, for example, acquires the lock, other reader threads should be able to acquire the lock at the same time so that they can work simultaneously.
- However, when any of the reader threads is holding onto the lock, the writer cannot actually do anything
- When writer thread is holding a lock, that lock is an absolute exclusive lock.
- In DB we use such locks a lot.
- In Sql Server we have shared locks and exclusive locks
- When SELECT statement is executed, a shared lock is applied to certain area of the table. We can lock rows or pages.
- UPDATE OR INSERT OR DELETE statements generate exclusive lock.
- ![alt text](image-11.png)
- Another example is web-server 
- ![alt text](image-13.png)
- We need similar locks on the shared cache for reading and writing.
- Lets say we have multiple users on the webserver being catered to by different threads.
- All of these threads are trying to read from shared cache.
```c#
 public class GlobalConfigurationCache
{
    private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    //Dictionary is not thread safe
    //Solve this problem using Concurrent Data Structures or ConcurrentDictionary
    private Dictionary<int, string> _cache = new Dictionary<int, string>();
    public void Add(int key, string value)
    {
        bool lockAcquired = false;
        try
        {
            //Exclusive lock, everyone else is blocked
            _lock.EnterWriteLock();
            lockAcquired = true;
            //Not an atomic operation, broken into multiple parts while being executed.
            _cache[key] = value;
        }
        finally
        {
            if (lockAcquired)
            {
                _lock.ExitWriteLock();
            }
        }
    }
    public string? Get(int key)
    {
        bool lockAcquired = false;
        try
        {
            //Allow different reader threads to access cache simultaneously
            _lock.EnterReadLock(); 
            lockAcquired = true;
            //not an atomic operation
            return _cache.TryGetValue(key, out var value) ? value : null;
        }
        finally 
        {
            if (lockAcquired)
            {
                _lock.ExitReadLock();
            }
        }
    }
}

```

## Semaphore : Thread synchronization technique, less often used for critical sections.
- Can be used to protect critical sections
- But it is used mostly to limit the number of concurrent threads or processes.
- ![alt text](image-15.png)
- Remember the web server example where we had a monitor thread that monitors the queues and each time a request comes it used to create and start a new processing thread.
- Now if we have millions of such requests at any given time, we could start a million threads and it would place lot of stress on the server 
- To avoid this most web-servers have connection pools to limit the number of the concurrent connections
- For e.g in Azure Sql Server, the number of concurrent threads allowed is 100 by default.
- Semaphores can limit the number of concurrent threads.
```c#
using (SemaphoreSlim semaphore = new SemaphoreSlim(initialCount:3, maxCount: 3))
{
    semaphore.Wait();
    try {
        //use semaphore to limit access to this section
        //your code here
    }
    finally {
        semaphore.Release();
    }
}

```
- Semaphore Slim is lighter than Semaphore.
- **semaphore.Wait() and semaphore.Release() dont have to exist in the same thread**
- Compared to reader writer lock, lock, Monitor and Mutex which all have thread affinity(i.e they can be used within the same thread), semaphore doesnot have thread affinity.
- Since it doesn't have thread affinity, it can used from different threads.
- Semaphore slows down processing but protects application from overloading.
- Semaphore can be across processes also.
- Please note Semaphore can be used across processes but Semaphore Slim can be used only within a process.
```c#
 

Queue<string?> requestQueue = new Queue<string?>();
using SemaphoreSlim semaphore = new SemaphoreSlim(initialCount:3, maxCount: 3);

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
            semaphore.Wait();
            // Processing thread
            Thread processingThread = new Thread(() => ProcessInput(input));
            processingThread.Start();
        }
        Thread.Sleep(100);
    }
}


//3. Processing the requests
void ProcessInput(string? input)
{
    try
    {
        // Simulate processing time
        Thread.Sleep(2000);
        Console.WriteLine($"Processed input: {input}");
    }
    finally
    {
        var prevCount = semaphore.Release();
        Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} release the semaphore. Previous count is {prevCount}");
    }
}
```
- We should use Concurrent Collections rather than simple Queue data structure.


## AutoReset Event: Used for signalling between threads
- Producer and Consumer scenario
- Signalling mechanism between Producer and Consumer
- Binary Signal : ON or OFF 
- ![alt text](image-16.png)
- An AutoResetEvent is a synchronization primitive used in multithreading environments, such as in C# programming. 
- It allows threads to wait for an event to occur and then automatically reset itself after releasing a single waiting thread
- Here's a simple analogy: Imagine a store with only one checkout lane. Only one customer (thread) can be served at a time, and after that customer is done, the lane resets and the next customer can proceed
- In technical terms, an AutoResetEvent can be in either a signaled or non-signaled state
- When a thread calls the WaitOne() method on an AutoResetEvent, it will block until the event is signaled
- When another thread calls the Set() method, it signals the event, releasing one waiting thread and resetting the event back to the non-signaled state
```c#
using System;
using System.Threading;

class Program
{
    private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
    
    static void Main()
    {
        new Thread(Worker).Start();
        
        Console.WriteLine("Main thread is doing some work...");
        Thread.Sleep(2000); // Simulate work
        
        Console.WriteLine("Main thread signals the worker thread.");
        autoResetEvent.Set(); // Signal the event
        
        Console.ReadLine();
    }
    
    private static void Worker()
    {
        Console.WriteLine("Worker thread is waiting for a signal...");
        autoResetEvent.WaitOne(); // Wait for a signal
        
        Console.WriteLine("Worker thread received a signal and is continuing its work.");
    }
}

```
- **Explanation of the Code**
- Main Thread:
    Performs some initial work.
    Signals the worker thread using autoResetEvent.Set().
- Worker Thread:
    Starts and waits for a signal using autoResetEvent.WaitOne().
    Once the signal is received, it continues its work.
- **One Time Signal**: The signal from Set() only allows one waiting thread to proceed, and then the event automatically resets to the non-signaled state.
- **Synchronization**: This mechanism ensures that the threads are properly synchronized, and the worker thread only proceeds when the main thread is ready.
  
```c#
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



```
- AutoResetEvent is for interaction between threads, not for protecting the critical section.

## Manual Reset Event: Similar to Auto Reset Event except that reset is not automatic
 - A ManualResetEvent is another synchronization primitive used in multithreading environments, much like the AutoResetEvent. 
 - However, there is a key difference: ManualResetEvent does not automatically reset itself after releasing threads. 
 - Instead, it remains in the signaled state until you manually reset it.
```c#
using System;
using System.Threading;

class Program
{
    private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
    
    static void Main()
    {
        new Thread(Worker).Start();
        new Thread(Worker).Start();
        
        Console.WriteLine("Main thread is doing some work...");
        Thread.Sleep(2000); // Simulate work
        
        Console.WriteLine("Main thread signals the worker threads.");
        manualResetEvent.Set(); // Signal the event
        
        Thread.Sleep(1000); // Give threads time to process the signal
        
        Console.WriteLine("Main thread resets the event.");
        manualResetEvent.Reset(); // Reset the event

        Console.ReadLine();
    }
    
    private static void Worker()
    {
        Console.WriteLine("Worker thread is waiting for a signal...");
        manualResetEvent.WaitOne(); // Wait for a signal
        
        Console.WriteLine("Worker thread received a signal and is continuing its work.");
    }
}


```
### Explanation of the Code
**Main Thread**:
- Starts two worker threads.
- Signals the worker threads using manualResetEvent.Set().
- Resets the event after a delay using manualResetEvent.Reset().

**Worker Threads:**
- Start and wait for a signal using manualResetEvent.WaitOne().
- Once the signal is received, they proceed with their work.

**Key Points:**
- Persistent Signal: The signal from Set() allows all waiting threads to proceed, and the event remains in the signaled state until you manually reset it.
- Manual Control: You have to explicitly call Reset() to block threads again.


## Assignment: Two way signalling in Producer-Consumer scenario
- Let us say we want a producer that add 10 integers one by one to a queue
- Then we want 3 threads that will consume the queue and dequeue the items from it.
- Once they are done dequeuing, they should send a signal to the producer to produce more.
- In this case, we will one manual reset event to send a signal from the producer. 
- Producer can run a loop and add 10 integers to the queue and then send a Set() signal to the worker threads
- The worker threads can consume the queue and then they also can use a separate manual reset event to send a signal to the Producer to produce some more.
```c#
Queue<int> queue = new Queue<int>();
ManualResetEventSlim consumeEvent = new ManualResetEventSlim(false);
ManualResetEventSlim produceEvent = new ManualResetEventSlim(true);

int consumerCounter = 0;
object lockConsumerCount = new object();    

Thread[] consumerThreads = new Thread[3];
for(int i=0; i<consumerThreads.Length;i++)
{
    consumerThreads[i] = new Thread(Consume);
    consumerThreads[i].Name = $"Consumer - {i + 1}";
    consumerThreads[i].Start();
}

while(true)
{
    produceEvent.Wait();
    produceEvent.Reset();
    Console.WriteLine("To produce, enter 'p'");
    var input = Console.ReadLine() ?? "";
    if(input.ToLower() =="p")
    {
        for(int i = 0; i < 10; i++)
        {
            queue.Enqueue(i);
            Console.WriteLine($"Produced: {i}");
        }
        consumeEvent.Set();
    }

}

//Consumer's behaviour
void Consume()
{
    while (true)
    {
        consumeEvent.Wait();

        while (queue.TryDequeue(out int input))
        {
            //work on the items produced
            Thread.Sleep(500);
            Console.WriteLine($"Consumed: {input} from thread: {Thread.CurrentThread.Name}");
        }

        lock (lockConsumerCount)
        {
            consumerCounter++;
            if (consumerCounter == 3)
            {
                consumeEvent.Reset();
                produceEvent.Set();
                consumerCounter = 0;
                Console.WriteLine("***********");
                Console.WriteLine("Please produce more....");
                Console.WriteLine("***********");
            }
        }
       
    }
}

```
- Note that we have to use a counter to keep track of consumer threads and only one of them should be allowed inside the critical section
- For this we use a lock for thread synchronization.

## Thread Affinity
- Thread affinity refers to the binding of a software thread to a specific hardware core or set of cores, ensuring that the thread runs only on those cores. 
- This is also known as CPU affinity or processor affinity.
- **Performance Optimization**: By binding a thread to a specific CPU core, you can optimize performance, particularly in real-time systems or when dealing with threads that frequently access the same data. 
- This can reduce cache misses and improve data locality.
- **Consistent Performance**: Thread affinity can help achieve consistent performance since the thread won't be moved to another core by the scheduler, which could potentially lead to a performance hit due to cache invalidation.
- **Multi-Core Systems**: In multi-core systems, it allows developers to assign specific tasks to specific cores, making it easier to manage the workload distribution and optimize the system's overall performance.
- Often we run into problems where in a multi-threaded environment, resources or variables created in one thread are accessed by another thread.
- This is particular observed in FrontEnd UI based applications like Winforms or Blazor.
- Suppose in a winforms application we have a label called lblMessage.
- Now on click of a button or 2 buttons we create 1 or more threads that try to access that label and try and update it.
- This can cause the following error:
- ![alt text](image-17.png)
- This is because lblMessage is created in the UI thread (which is the main thread) and now 2 other threads created on button click events are trying to access it
- To set the affinity we can use Invoke() method like this:
  ```c#
    if (lblMessage.InvokeRequired)
    {
        lblMessage.Invoke(() =>
        {
            lblMessage.Text = message;
        });
    }
    else
    {
        lblMessage.Text = message;
    }

  ```

## Thread Safety
- A function or data structure or class is considered thread safe when it can be used concurrently by multiple threads without causing race conditions or unexpected behaviors or data corruptions
- So this means that within the data structure or class proper locking mechanisms have been used.


## Nested Locks and Deadlocks
- Deadlocks happens within threads that have nested locks.
- Lets say we have 2 threads that are waiting for each other: Thread 1 waits for Thread 2 and vice versa. This is a deadlock situation
- Here is an example of deadlock
```c#
 //e-commerce users and orders
//1. managing users ( user -> order)
//2. managing orders (order -> user)

// Thread 1 wants to lock user first and then lock order
//Thread 2 wants to lock order first and then lock user

object userLock = new object();
object orderLock = new object();

Thread thread = new Thread(ManageOrder);
thread.Start();

ManageUser();

thread.Join();
Console.WriteLine("Program finished");
Console.ReadLine();

void ManageUser()
{
    lock(userLock)
    {
        Console.WriteLine("User Management acquired the user lock. ");
        Thread.Sleep(2000);

        lock(orderLock)
        {
            Console.WriteLine("User Management acquired the order lock. ");
        }
    }
}

void ManageOrder()
{
    lock (orderLock)
    {
        Console.WriteLine("Order Management acquired the order lock. ");
        Thread.Sleep(1000);

        lock (userLock)
        {
            Console.WriteLine("Order Management acquired the user lock. ");
        }
    }
}




```

### Strategies to Avoid Deadlocks:
- Avoid nested locks
- Lock Ordering: Ensure that all threads acquire locks in a consistent order.
- Timeouts: Use timeouts when trying to acquire locks, so threads don't wait indefinitely.
- Deadlock Detection: Implement algorithms to detect and resolve deadlocks, such as releasing and reacquiring locks.
- Lock Hierarchies: Design systems with a hierarchy of locks, where lower-level locks are always acquired before higher-level locks. 
