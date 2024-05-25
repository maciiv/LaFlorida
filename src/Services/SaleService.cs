using LaFlorida.Data;
using LaFlorida.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface ISaleService
    {
        Task<SaveModel<Sale>> CreateSaleAsync(Sale sale);
        Task<SaveModel<Sale>> EditSaleAsync(Sale sale);
        Task<SaveModel<Sale>> DeleteSaleAsync(int id);
        Task<List<Sale>> GetSalesAsync();
        Task<Sale> GetSaleByIdAsync(int id);
        Task<List<Sale>> GetSalesByCycleAsync(int id);
    }

    public class SaleService : ISaleService
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveService<Sale> _saveService;

        public SaleService(IApplicationDbContext context, ISaveService<Sale> saveService)
        {
            _context = context;
            _saveService = saveService;
        }

        public async Task<SaveModel<Sale>> CreateSaleAsync(Sale sale)
        {
            sale.CreateDate = DateTime.Now;
            sale.Total = sale.Quantity * sale.Price;
            sale.Quintals = sale.Quintals == null ? sale.Quantity : sale.Quintals;

            try
            {
                await _context.Sales.AddAsync(sale);
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(sale);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Sale>> EditSaleAsync(Sale sale)
        {
            sale.Total = sale.Quantity * sale.Price;

            _context.SetModified(sale);

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(sale);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Sale>> DeleteSaleAsync(int id)
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(c => c.SaleId == id);
            if (sale == null)
                return _saveService.SaveNotFound();

            try
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
                return _saveService.DeleteSuccess();
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            return await _context.Sales.Include(c => c.Cycle).AsNoTracking().ToListAsync();
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            return await _context.Sales.Include(c => c.Cycle).FirstOrDefaultAsync(c => c.SaleId == id);
        }

        public async Task<List<Sale>> GetSalesByCycleAsync(int id)
        {
            return await _context.Sales.Where(c => c.CycleId == id).AsNoTracking().ToListAsync();
        }
    }
}
