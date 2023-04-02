using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Tokenizer.RFC4180;
using webapi.CsvMapping;
using webapi.Models;
using webapi.Models.CsvModels;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{
    private readonly ILogger<LocationController> _logger;

    public LocationController(ILogger<LocationController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetNeswLocations")]
    public async Task<List<Location>> Get()
    {
        await PrepareFile();
        var locations = GetLocations();
        var locationList = new List<Location>()
        {
            locations.MinBy(l => l.Dd_N),
            locations.MaxBy(l => l.Dd_N),
            locations.MinBy(l => l.Dd_E),
            locations.MaxBy(l => l.Dd_E)
        };

        return locationList;
    }

    [HttpGet("SearchLocations")]
    public List<Location> Search(string searchValue)
    {
        return GetLocations().FindAll(l => l.Std.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).OrderBy(t => t.Std.Length).ToList();
    }

    private List<Location> GetLocations()
    {
        var options = new Options('#', '\\', ';');
        var tokenizer = new RFC4180Tokenizer(options);

        CsvParserOptions csvParserOptions = new CsvParserOptions(true, tokenizer);
        var csvParser = new CsvParser<VietuCentroidi>(csvParserOptions, new CsvVietuCentroidiMapping());

        return csvParser.ReadFromFile(Constants.DataFilePath, Encoding.UTF8).Where(l => l.IsValid).Select(l => new Location()
        {
            Code = int.Parse(l.Result.Kods),
            Type_Cd = int.Parse(l.Result.Tips_Cd),
            Title = l.Result.Nosaukums,
            Vkur_Cd = int.Parse(l.Result.Vkur_Cd),
            Vkur_Type = int.Parse(l.Result.Vkur_Tips),
            Std = l.Result.Std,
            Coord_X = double.Parse(l.Result.Koord_X, CultureInfo.InvariantCulture),
            Coord_Y = double.Parse(l.Result.Koord_Y, CultureInfo.InvariantCulture),
            Dd_N = double.Parse(l.Result.Dd_N, CultureInfo.InvariantCulture),
            Dd_E = double.Parse(l.Result.Dd_E, CultureInfo.InvariantCulture)
        }).ToList();
    }

    private async Task<bool> PrepareFile()
    {
        if (!System.IO.File.Exists(Constants.DataFilePath))
        {
            using var response = await new HttpClient().GetAsync(Constants.LocationsUrl);
            using var streamToReadFrom = await response.Content.ReadAsStreamAsync();
            using var zip = new ZipArchive(streamToReadFrom);
            zip.ExtractToDirectory(Constants.DefaultDirectory);
        }
        return true;
    }
}
