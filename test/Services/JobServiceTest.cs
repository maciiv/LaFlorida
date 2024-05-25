using LaFlorida.Data;
using LaFlorida.Models;
using LaFlorida.Services;
using Moq;
using Moq.EntityFrameworkCore;

namespace LaFloridaTest.Services;

public class JobServiceTest
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly JobService _service;
    private readonly List<Job> testData;
    private readonly Job newData;
    public JobServiceTest()
    {
        _context = new Mock<IApplicationDbContext>();
        testData = [
            new() { JobId = 1, Name = "Job 1", IsRent = false, IsMachinist = false },
            new() { JobId = 2, Name = "Jon 2", IsRent = false, IsMachinist = false },
        ];
        newData = new Job()
        {
            JobId = 3,
            Name = "Job 3",
            IsRent = false,
            IsMachinist = false
        };
        _context.Setup(c => c.Jobs).ReturnsDbSet(testData);
        _context.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();
        _context.Setup(c => c.SetModified(It.IsAny<Job>()))
            .Verifiable();
        var _saveService = new MockSaveService<Job>();
        _service = new JobService(_context.Object, _saveService.MockedService.Object);
    }

    [Fact]
    public async Task CreateAsync()
    {
        var saveResult = await _service.CreateJobAsync(newData);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task CreateAsyncExists()
    {
        var saveResult = await _service.CreateJobAsync(testData[0]);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task EditAsync()
    {
        var saveResult = await _service.EditJobAsync(testData[0]);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SetModified(It.IsAny<Job>()), Times.Once());
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var saveResult = await _service.DeleteJobAsync(1);
        Assert.True(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsyncNotExists()
    {
        var saveResult = await _service.DeleteJobAsync(0);
        Assert.False(saveResult.Success);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task GetAsync()
    {
        var items = await _service.GetJobsAsync();
        Assert.Equal(items.Count, testData.Count);
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        var item = await _service.GetJobByIdAsync(1);
        Assert.Equal(item, testData[0]);
    }

    [Fact]
    public async Task GetByIdAsyncNotExists()
    {
        var item = await _service.GetJobByIdAsync(0);
        Assert.Null(item);
    }
}
