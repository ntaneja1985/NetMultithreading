using System.Text.Json;


OutputFirstPokemon();
Console.WriteLine("This is the end of the program");
Console.ReadLine();

async void OutputFirstPokemon()
{
    using var client = new HttpClient();
    var taskGetPokemonList = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");
    //var result = taskGetPokemonList.Result; //blocking call

    var result = await taskGetPokemonList;

    var doc = JsonDocument.Parse(result);
    JsonElement root = doc.RootElement;
    JsonElement results = root.GetProperty("results");
    JsonElement pokemon = results[0];

    Console.WriteLine($"First pokemon name is :{pokemon.GetProperty("name")}");
    Console.WriteLine($"First pokemon url is :{pokemon.GetProperty("url")}");
}


