﻿

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