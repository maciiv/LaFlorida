using LaFlorida.Services;
using Moq;

namespace LaFloridaTest;

public class MockSaveService<T>
{
    public Mock<ISaveService<T>> MockedService { get; set; }
    public MockSaveService()
    {
        var saveService = new Mock<ISaveService<T>>();
        saveService.Setup(s => s.SaveSuccess(It.IsAny<T>())).Returns(new SaveModel<T> { Success = true, Message = "Saved" });
        saveService.Setup(c => c.SaveExists()).Returns(new SaveModel<T> { Success = false, Message = "Save Exists" });
        saveService.Setup(c => c.SaveFail(It.IsAny<Exception>())).Returns(new SaveModel<T> { Success = false, Message = "Save Fail" });
        saveService.Setup(c => c.SaveNotFound()).Returns(new SaveModel<T> { Success = false, Message = "Not Found" });
        saveService.Setup(c => c.DeleteSuccess()).Returns(new SaveModel<T> { Success = true, Message = "Deleted" });
        MockedService = saveService;
    }
}
