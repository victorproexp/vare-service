using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using MongoDB.Driver;
using vareAPI.Controllers;
using vareAPI.Models;
using vareAPI.Services;
using Microsoft.Extensions.Options;

namespace vare_service_test;

public class Tests
{
    private ILogger<VareController>? _logger;
    private IOptions<VareDatabaseSettings>? _settings;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<VareController>>().Object;

        _settings = Options.Create(new VareDatabaseSettings()
        {
            DatabaseName = "VareDB",
            VareCollectionName = "Varer"
        });
    }

    [Test]
    public async Task AddVare()
    {
        // Arrange
        var requestTime = new DateTime(2022, 5, 14, 11, 8, 52);
        var vareDTO = CreateVare(requestTime);
        var mockRepo = new Mock<IVareService>();
        mockRepo.Setup(svc => svc.CreateAsync(vareDTO)).Returns(Task.CompletedTask);
        var controller = new VareController(_logger!, mockRepo.Object);

        // Act
        var result = await controller.Post(vareDTO);

        // Assert
        Assert.IsInstanceOf(typeof(ActionResult), result);
    }

    private Vare CreateVare(DateTime requestTime)
    {
        var vare = new Vare()
        {
            Title = "Stol",
            Description = "God stand",
            AuktionStart = requestTime,
        };
        return vare;
    }

    private static List<Vare> SampleEntities()
    {
        return new List<Vare>
        {
            new Vare
            {
                ProductId = "vare",
                Title = "Stol"
            },
            new Vare
            {
                ProductId = "vare1",
                Title = "Bord"
            }
        };
    }
}
