
//var task = Task.Run(() => {
//    int sum = 0;
//    for (int i = 0; i < 100; i++)
//    {
//        Task.Delay(100);//analogous to Thread.Sleep
//        sum += i;
//    }
//    return sum;
//});

////Block the program to wait for the tasks to finish
////task.Wait();//will wait for the task to finish. Analogous to thread.Join() 
////Task.WaitAll(task1, task2, task3); // we can pass a task array also. Will wait for all these specified tasks to finish

////var result = task.Result;//This line is blocking. Wait block the current thread till the task is finished

//Console.WriteLine($"The result is {task.Result}");
//Console.ReadLine();

using System.Text.Json;

using var client = new HttpClient();
var taskListJson = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");
var taskGetFirstUrl =  taskListJson.ContinueWith(r => {
    var result = r.Result;
    var doc = JsonDocument.Parse(result);
    JsonElement root = doc.RootElement;
    JsonElement results = root.GetProperty("results");
    JsonElement pokemon = results[0];

    //Console.WriteLine($"First pokemon name is :{pokemon.GetProperty("name")}");
    // Console.WriteLine($"First pokemon url is :{pokemon.GetProperty("url")}");
    return pokemon.GetProperty("url").ToString();
});
var taskGetDetailsJson = taskGetFirstUrl.ContinueWith(r =>
{
    var result = r.Result;
    return client.GetStringAsync(result);
}).Unwrap();

taskGetDetailsJson.ContinueWith(r =>
{
    var result = r.Result;
    var doc = JsonDocument.Parse(result);
    JsonElement root = doc.RootElement;
    Console.WriteLine($"Name: {root.GetProperty("name").ToString()}");
    Console.WriteLine($"Weight: {root.GetProperty("weight").ToString()}");
    Console.WriteLine($"Height: {root.GetProperty("height").ToString()}");

});

Console.WriteLine("Waiting for data to come inside");

Console.ReadLine();


