using LaFlorida.Data;
using LaFlorida.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface ICostService
    {
        Task<SaveModel<Cost>> CreateCostAsync(Cost cost);
        Task<SaveModel<Cost>> EditCostAsync(Cost cost);
        Task<SaveModel<Cost>> DeleteCostAsync(int id);
        Task<List<Cost>> GetCostsAsync();
        Task<Cost> GetCostByIdAsync(int id);
        Task<List<Cost>> GetCostsByCycleAsync(int id);
        Task<List<Cost>> GetCostsByUserAsync(string id);
    }

    public class CostService : ICostService
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveService<Cost> _saveService;

        public CostService(IApplicationDbContext context, ISaveService<Cost> saveService)
        {
            _context = context;
            _saveService = saveService;
        }

        public async Task<SaveModel<Cost>> CreateCostAsync(Cost cost)
        {
            cost.CreateDate = DateTime.Now;
            cost.Total = cost.Quantity * cost.Price;

            try
            {
                await _context.Costs.AddAsync(cost);
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(cost);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Cost>> EditCostAsync(Cost cost)
        {
            cost.Total = cost.Quantity * cost.Price;

            _context.Attach(cost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(cost);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Cost>> DeleteCostAsync(int id)
        {
            var cost = await _context.Costs.FindAsync(id);
            if (cost == null)
                return _saveService.SaveNotFound();

            try
            {
                _context.Costs.Remove(cost);
                await _context.SaveChangesAsync();
                return _saveService.DeleteSuccess();
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<List<Cost>> GetCostsAsync()
        {
            return await _context.Costs.Include(c => c.Cycle).Include(c => c.Job).Include(c => c.ApplicationUser)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Cost> GetCostByIdAsync(int id)
        {
            return await _context.Costs.Include(c => c.Cycle).Include(c => c.Job).Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.CostId == id);
        }

        public async Task<List<Cost>> GetCostsByCycleAsync(int id)
        {
            return await _context.Costs.Where(c => c.CycleId == id)
                .Include(c => c.Cycle).Include(c => c.Job).Include(c => c.ApplicationUser)
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<Cost>> GetCostsByUserAsync(string id)
        {
            return await _context.Costs.Where(c => c.ApplicationUserId == id)
                .Include(c => c.Cycle).Include(c => c.Job).Include(c => c.ApplicationUser)
                .AsNoTracking().ToListAsync();
        }
    }
}
