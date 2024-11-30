using System.Text.Json;

using var client = new HttpClient();
Console.WriteLine($"Main Thread Id is {Thread.CurrentThread.ManagedThreadId}");
var pokemonListJson = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");
Console.WriteLine($"Thread Id After Pokemon List is {Thread.CurrentThread.ManagedThreadId}");
//Get the first pokemon's url

var doc = JsonDocument.Parse(pokemonListJson);
JsonElement root = doc.RootElement;
JsonElement results = root.GetProperty("results");
JsonElement pokemon = results[0];
var url = pokemon.GetProperty("url").ToString();

//Get first pokemon details
Console.WriteLine($"Thread Id Before Pokemon Details is {Thread.CurrentThread.ManagedThreadId}");
var firstPokemonDetailsJson = await client.GetStringAsync(url);
Console.WriteLine($"Thread Id After Pokemon Details is {Thread.CurrentThread.ManagedThreadId}");

//Get the weight and height
var doc2 = JsonDocument.Parse(firstPokemonDetailsJson);
JsonElement root2 = doc2.RootElement;
Console.WriteLine($"Name: {root2.GetProperty("name").ToString()}");
Console.WriteLine($"Weight: {root2.GetProperty("weight").ToString()}");
Console.WriteLine($"Height: {root2.GetProperty("height").ToString()}");

Console.WriteLine($"Thread Id before finishing is {Thread.CurrentThread.ManagedThreadId}");
Console.WriteLine("Waiting for data to come inside");

Console.ReadLine();