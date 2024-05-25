using LaFlorida.Services;
using LaFlorida.Data;
using Moq;
using Moq.EntityFrameworkCore;
using LaFlorida.Models;

namespace LaFloridaTest.Services;

public class CropServiceTest
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly CropService _service;
    private readonly List<Crop> testData;
    private readonly Crop newData;
    public CropServiceTest()
    {
        _context = new Mock<IApplicationDbContext>();
        testData = [
            new() { CropId = 1, Name = "Crop 1", Lenght = 3 },
            new() { CropId = 2, Name = "Crop 2", Lenght = 6 },
        ];
        newData = new Crop()
        {
            CropId = 3,
            Name = "Crop 3",
            Lenght = 9
        };
        _context.Setup(c => c.Crops).ReturnsDbSet(testData);
        _context.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();
        _context.Setup(c => c.SetModified(It.IsAny<Crop>()))
            .Verifiable();
        var _saveService = new MockSaveService<Crop>();
        _service = new CropService(_context.Object, _saveService.MockedService.Object);
    }

    [Fact]
    public async Task CreateAsync()
    {
        var saveResult = await _service.CreateCropAsync(newData);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task CreateAsyncExists()
    {
        var saveResult = await _service.CreateCropAsync(testData[0]);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task EditAsync()
    {
        var saveResult = await _service.EditCropAsync(testData[0]);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SetModified(It.IsAny<Crop>()), Times.Once());
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var saveResult = await _service.DeleteCropAsync(1);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsyncNotExists()
    {
        var saveResult = await _service.DeleteCropAsync(0);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task GetAsync()
    {
        var items = await _service.GetCropsAsync();
        Assert.Equal(items.Count, testData.Count);
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        var item = await _service.GetCropByIdAsync(1);
        Assert.Equal(item, testData[0]);
    }

    [Fact]
    public async Task GetByIdAsyncNotExists()
    {
        var item = await _service.GetCropByIdAsync(0);
        Assert.Null(item);
    }
}