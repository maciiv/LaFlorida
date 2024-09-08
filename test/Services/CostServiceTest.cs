using LaFlorida.Data;
using LaFlorida.Helpers;
using LaFlorida.Models;
using LaFlorida.Services;
using Moq;
using Moq.EntityFrameworkCore;

namespace LaFloridaTest.Services;

public class CostServiceTest
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly CostService _service;
    private readonly List<Cost> testData;
    private readonly Cost newData;

    public CostServiceTest()
    {
        _context = new Mock<IApplicationDbContext>();
        testData = [
            new() { CostId = 1, ApplicationUserId = "a", CycleId = 1 },
            new() { CostId = 2, ApplicationUserId = "b", CycleId = 2 },
        ];
        newData = new Cost()
        {
            CostId = 3,
            ApplicationUserId = "a",
            CycleId = 1
        };
        _context.Setup(c => c.Costs).ReturnsDbSet(testData);
        _context.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();
        _context.Setup(c => c.SetModified(It.IsAny<Cost>()))
            .Verifiable();
        var _saveService = new MockSaveService<Cost>();
        var _dataProtectionHelper = new Mock<IDataProtectionHelper>();
        _dataProtectionHelper.Setup(c => c.Unprotect(It.IsAny<string>())).Returns("a");
        _service = new CostService(_context.Object, _saveService.MockedService.Object, _dataProtectionHelper.Object);
    }

    [Fact]
    public async Task CreateAsync()
    {
        var saveResult = await _service.CreateCostAsync(newData);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task EditAsync()
    {
        var saveResult = await _service.EditCostAsync(testData[0]);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SetModified(It.IsAny<Cost>()), Times.Once());
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var saveResult = await _service.DeleteCostAsync(1);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsyncNotExists()
    {
        var saveResult = await _service.DeleteCostAsync(0);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task GetAsync()
    {
        var items = await _service.GetCostsAsync();
        Assert.Equal(items.Count, testData.Count);
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        var item = await _service.GetCostByIdAsync(1);
        Assert.Equal(item, testData[0]);
    }

    [Fact]
    public async Task GetByIdAsyncNotExists()
    {
        var item = await _service.GetCostByIdAsync(0);
        Assert.Null(item);
    }

    [Fact]
    public async Task GetCostsByCycleAsync()
    {
        var items = await _service.GetCostsByCycleAsync(1);
        Assert.Equal(items[0], testData[0]);
        Assert.Equal(1, items?.Count);
    }
}
