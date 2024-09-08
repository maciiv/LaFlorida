using LaFlorida.Data;
using LaFlorida.Models;
using LaFlorida.Services;
using Moq;
using Moq.EntityFrameworkCore;

namespace LaFloridaTest.Services;

public class SaleServiceTest
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly SaleService _service;
    private readonly List<Sale> testData;
    private readonly Sale newData;

    public SaleServiceTest()
    {
        _context = new Mock<IApplicationDbContext>();
        testData = [
            new() { SaleId = 1, CycleId = 1 },
            new() { SaleId = 2, CycleId = 2 },
        ];
        newData = new Sale()
        {
            SaleId = 3,
            CycleId = 1
        };
        _context.Setup(c => c.Sales).ReturnsDbSet(testData);
        _context.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();
        _context.Setup(c => c.SetModified(It.IsAny<Sale>()))
            .Verifiable();
        var _saveService = new MockSaveService<Sale>();
        _service = new SaleService(_context.Object, _saveService.MockedService.Object);
    }

    [Fact]
    public async Task CreateAsync()
    {
        var saveResult = await _service.CreateSaleAsync(newData);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task EditAsync()
    {
        var saveResult = await _service.EditSaleAsync(testData[0]);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SetModified(It.IsAny<Sale>()), Times.Once());
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var saveResult = await _service.DeleteSaleAsync(1);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsyncNotExists()
    {
        var saveResult = await _service.DeleteSaleAsync(0);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task GetAsync()
    {
        var items = await _service.GetSalesAsync();
        Assert.Equal(items.Count, testData.Count);
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        var item = await _service.GetSaleByIdAsync(1);
        Assert.Equal(item, testData[0]);
    }

    [Fact]
    public async Task GetByIdAsyncNotExists()
    {
        var item = await _service.GetSaleByIdAsync(0);
        Assert.Null(item);
    }
}
