using Microsoft.AspNetCore.DataProtection;
using System;

namespace LaFlorida.Helpers
{
    public interface IDataProtectionHelper
    {
        string Protect(string id);
        string Unprotect(string id);
    }
    public class DataProtectionHelper : IDataProtectionHelper
    {
        private readonly IDataProtector _dataProtector;

        public DataProtectionHelper(IDataProtectionProvider provider)
        {
            _dataProtector = provider.CreateProtector("AppDataProtector");
        }

        public string Protect(string id)
        {
            return _dataProtector.Protect(id);
        }

        public string Unprotect(string id)
        {
            try 
            {
                return _dataProtector.Unprotect(id); 
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
