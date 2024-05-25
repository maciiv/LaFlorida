using LaFlorida.Data;
using LaFlorida.Models;
using LaFlorida.Services;
using Moq;
using Moq.EntityFrameworkCore;

namespace LaFloridaTest.Services;

public class LotServiceTest
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly LotService _service;
    private readonly List<Lot> testData;
    private readonly Lot newData;
    public LotServiceTest()
    {
        _context = new Mock<IApplicationDbContext>();
        testData = [
            new() { LotId = 1, Name = "Lot 1", Size = 5 },
            new() { LotId = 2, Name = "Lot 2", Size = 10 },
        ];
        newData = new Lot()
        {
            LotId = 3,
            Name = "Lot 3",
            Size = 15,
        };
        _context.Setup(c => c.Lots).ReturnsDbSet(testData);
        _context.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();
        _context.Setup(c => c.SetModified(It.IsAny<Lot>()))
            .Verifiable();
        var _saveService = new MockSaveService<Lot>();
        _service = new LotService(_context.Object, _saveService.MockedService.Object);
    }

    [Fact]
    public async Task CreateAsync()
    {
        var saveResult = await _service.CreateLotAsync(newData);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task CreateAsyncExists()
    {
        var saveResult = await _service.CreateLotAsync(testData[0]);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task EditAsync()
    {
        var saveResult = await _service.EditLotAsync(testData[0]);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SetModified(It.IsAny<Lot>()), Times.Once());
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var saveResult = await _service.DeleteLotAsync(1);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsyncNotExists()
    {
        var saveResult = await _service.DeleteLotAsync(0);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task GetAsync()
    {
        var items = await _service.GetLotsAsync();
        Assert.Equal(items.Count, testData.Count);
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        var item = await _service.GetLotByIdAsync(1);
        Assert.Equal(item, testData[0]);
    }

    [Fact]
    public async Task GetByIdAsyncNotExists()
    {
        var item = await _service.GetLotByIdAsync(0);
        Assert.Null(item);
    }
}
