//e-commerce users and orders
//1. managing users ( user -> order)
//2. managing orders (order -> user)

// Thread 1 wants to lock user first and then lock order
//Thread 2 wants to lock order first and then lock user

object userLock = new object();
object orderLock = new object();

Thread thread = new Thread(ManageOrder);
thread.Name = "Order Thread";
thread.Start();

ManageUser();
Thread.CurrentThread.Name = "Main User Thread";

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


