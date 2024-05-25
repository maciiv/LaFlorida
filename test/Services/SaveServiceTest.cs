using LaFlorida.Services;

namespace LaFloridaTest.Services;

public class SaveServiceTest
{
    private readonly ISaveService<SaveTestModel> _saveService;

    public SaveServiceTest()
    {
        _saveService = new SaveService<SaveTestModel>();
    }

    [Fact]
    public void SaveExists()
    {
        var save = _saveService.SaveExists();
        Assert.False(save.Success);
        Assert.Equal("Informacion existente", save.Message);
    }

    [Fact]
    public void SaveSuccess()
    {
        var saveModel = new SaveTestModel();
        var save = _saveService.SaveSuccess(saveModel);
        Assert.True(save.Success);
        Assert.Equal("Informacion guardada con exito", save.Message);
        Assert.Equal(saveModel, save.Model);
    }

    [Fact]
    public void SaveFail()
    {
        var saveException = new ArgumentNullException();
        var save = _saveService.SaveFail(saveException);
        Assert.False(save.Success);
        Assert.Equal("No se pudieron guardar los cambios", save.Message);
    }

    [Fact]
    public void SaveNotFound()
    {
        var save = _saveService.SaveNotFound();
        Assert.False(save.Success);
        Assert.Equal("Informacion no encontrada", save.Message);
    }

    [Fact]
    public void DeleteSuccess()
    {
        var save = _saveService.DeleteSuccess();
        Assert.True(save.Success);
        Assert.Equal("Informacion borrada con exito", save.Message);
    }

    [Fact]
    public void InsufficientFunds()
    {
        var balance = 10M;
        var save = _saveService.InsufficientFunds(balance);
        Assert.False(save.Success);
        Assert.Equal("El accionista no tiene suficientes fondos (balance: " + balance + ")", save.Message);
    }

    private class SaveTestModel
    {
        public int Id { get; set; } = 1;
        public string Name { get; set; } = "Name";
    }
}