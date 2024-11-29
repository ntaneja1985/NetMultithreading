
//Thread thread = null;
//try
//{

//    thread = new Thread(() =>
//    {
//       try
//        {
//            throw new InvalidOperationException("An error occured in this worker thread");
//        }
//        catch(Exception ex) {
//            Console.WriteLine(ex.ToString());
//        }
//    });


//}
//catch(Exception ex)
//{
//    Console.WriteLine(ex.ToString());
//}

//thread?.Start();
//thread?.Join();
//Console.ReadLine();

object lockExceptions = new object();
List<Exception> exceptions = new List<Exception> ();
Thread t1 = new Thread(Work);
Thread t2 = new Thread(Work);

t1.Start ();
t2.Start ();

t1.Join ();
t2.Join ();

foreach (Exception ex in exceptions)
{
    Console.WriteLine (ex.ToString ());
}

void Work()
{
    try
    {
        throw new InvalidOperationException($"An error occurred in Thread: {Thread.CurrentThread.ManagedThreadId}");
    }
    catch (Exception ex) {
        lock (lockExceptions)
        {
            exceptions.Add(ex);
        }
    }
}
