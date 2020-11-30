using System;

namespace LaFlorida.Services
{
    public interface ISaveService<T>
    {
        SaveModel<T> SaveExists();
        SaveModel<T> SaveSuccess(T model);
        SaveModel<T> SaveFail(Exception e);
        SaveModel<T> SaveNotFound();
        SaveModel<T> DeleteSuccess();
        SaveModel<T> InsufficientFunds();
    }

    public class SaveService<T> : ISaveService<T>
    {
        public SaveModel<T> SaveExists()
        {
            return new SaveModel<T>
            {
                Success = false,
                Message = "Informacion existente",
            };
        }

        public SaveModel<T> SaveSuccess(T model)
        {
            return new SaveModel<T>
            {
                Success = true,
                Message = "Informacion guardada con exito",
                Model = model
            };
        }

        public SaveModel<T> SaveFail(Exception e)
        {
            return new SaveModel<T>
            {
                Success = false,
                Message = "No se pudieron guardar los cambios",
                Exception = e.Message
            };
        }

        public SaveModel<T> SaveNotFound()
        {
            return new SaveModel<T>
            {
                Success = false,
                Message = "Informacion no encontrada",
            };
        }

        public SaveModel<T> DeleteSuccess()
        {
            return new SaveModel<T>
            {
                Success = true,
                Message = "Informacion borrada con exito",
            };
        }

        public SaveModel<T> InsufficientFunds()
        {
            return new SaveModel<T>
            {
                Success = false,
                Message = "El accionista no tiene suficientes fondos"
            };
        }
    }

    public class SaveModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public T Model { get; set; }
    }
}
