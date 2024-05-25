using LaFlorida.Data;
using LaFlorida.Models;
using LaFlorida.Services;
using Moq;
using Moq.EntityFrameworkCore;

namespace LaFloridaTest.Services;

public class CycleServiceTest
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly CycleService _service;
    private readonly List<Cycle> testData;
    private readonly Cycle newData;
    public CycleServiceTest()
    {
        _context = new Mock<IApplicationDbContext>();
        testData = [
            new() { CycleId = 1, Name = "Cycle 1", IsRent = false, IsComplete = false, CropId = 1 },
            new() { CycleId = 2, Name = "Cycle 2", IsRent = false, IsComplete = true, CropId = 2 },
        ];
        newData = new Cycle()
        {
            CycleId = 3,
            Name = "Cycle 3",
            IsRent = false,
            IsComplete = false,
            CropId = 1
        };
        var cropData = new List<Crop>
        {
            new() { CropId = 1, Name = "Crop 1", Lenght = 3 },
            new() { CropId = 2, Name = "Crop 2", Lenght = 6 },
        };
        var costData = new List<Cost>
        {
            new() { CostId = 1, ApplicationUserId = "a", Cycle = testData[0] },
            new() { CostId = 2, ApplicationUserId = "b", Cycle = testData[1] },
            new() { CostId = 3, ApplicationUserId = "a", Cycle = testData[0] }
        };
        _context.Setup(c => c.Cycles).ReturnsDbSet(testData);
        _context.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();
        _context.Setup(c => c.SetModified(It.IsAny<Cycle>()))
            .Verifiable();
        _context.Setup(c => c.Crops).ReturnsDbSet(cropData);
        _context.Setup(c => c.Costs).ReturnsDbSet(costData);
        var _saveService = new MockSaveService<Cycle>();
        _service = new CycleService(_context.Object, _saveService.MockedService.Object);
    }

    [Fact]
    public async Task CreateAsync()
    {
        var saveResult = await _service.CreateCycleAsync(newData);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task CreateAsyncExists()
    {
        var saveResult = await _service.CreateCycleAsync(testData[0]);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task EditAsync()
    {
        var saveResult = await _service.EditCycleAsync(testData[0]);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SetModified(It.IsAny<Cycle>()), Times.Once());
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var saveResult = await _service.DeleteCycleAsync(1);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsyncNotExists()
    {
        var saveResult = await _service.DeleteCycleAsync(0);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task GetAsync()
    {
        var items = await _service.GetCyclesAsync();
        Assert.Equal(items.Count, testData.Count);
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        var item = await _service.GetCycleByIdAsync(1);
        Assert.Equal(item, testData[0]);
    }

    [Fact]
    public async Task GetByIdAsyncNotExists()
    {
        var item = await _service.GetCycleByIdAsync(0);
        Assert.Null(item);
    }

    [Fact]
    public async Task GetActiveCyclesAsync()
    {
        var item = await _service.GetActiveCyclesAsync();
        Assert.Equal(item[0], testData[0]);
        Assert.Equal(1, item?.Count);
    }

    [Fact]
    public async Task GetCompleteCyclesAsync()
    {
        var item = await _service.GetCompleteCyclesAsync();
        Assert.Equal(item[0], testData[1]);
        Assert.Equal(1, item?.Count);
    }

    [Fact]
    public async Task GetCyclesByUserAsync()
    {
        var item = await _service.GetCyclesByUserAsync("a");
        Assert.Equal(item[0], testData[0]);
        Assert.Equal(1, item?.Count);
    }

    [Fact]
    public async Task CloseCycleAsync()
    {
        var saveResult = await _service.CloseCycleAsync(1);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task CloseCycleAsyncNotExists()
    {
        var saveResult = await _service.CloseCycleAsync(3);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }
}
