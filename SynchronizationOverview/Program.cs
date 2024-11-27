int counter = 0;

object counterLock = new object();


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
        lock (counterLock)
        {
            //assigment is not an atomic operation
            counter = counter + 1;
        }
    }
}

