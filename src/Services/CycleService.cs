using LaFlorida.Data;
using LaFlorida.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface ICycleService
    {
        Task<SaveModel<Cycle>> CreateCycleAsync(Cycle cycle);
        Task<SaveModel<Cycle>> EditCycleAsync(Cycle cycle);
        Task<SaveModel<Cycle>> DeleteCycleAsync(int id);
        Task<List<Cycle>> GetCyclesAsync();
        Task<Cycle> GetCycleByIdAsync(int id);
        Task<List<Cycle>> GetActiveCyclesAsync();
        Task<List<Cycle>> GetCompleteCyclesAsync();
        Task<List<Cycle>> GetCyclesByUserAsync(string id);
        Task<SaveModel<Cycle>> CloseCycleAsync(int id);
        Task<SelectList> GetCyclesSelectListAsync();
    }

    public class CycleService : ICycleService
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveService<Cycle> _saveService;

        public CycleService(IApplicationDbContext context, ISaveService<Cycle> saveService)
        {
            _context = context;
            _saveService = saveService;
        }

        public async Task<SaveModel<Cycle>> CreateCycleAsync(Cycle cycle)
        {
            var cycleExists = await _context.Cycles.AnyAsync(c => c.Name == cycle.Name && c.LotId == cycle.LotId && c.CropId == cycle.CropId);
            if (cycleExists)
                return _saveService.SaveExists();

            var crop = await _context.Crops.FirstOrDefaultAsync(c => c.CropId == cycle.CropId);
            if (crop == null)
                return _saveService.SaveNotFound();

            cycle.CreateDate = DateTime.Now;
            cycle.HarvestDate = DateTime.Now.AddMonths(crop.Lenght);
            cycle.IsComplete = false;

            try
            {
                await _context.Cycles.AddAsync(cycle);
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(cycle);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Cycle>> EditCycleAsync(Cycle cycle)
        {
            var crop = await _context.Crops.FirstOrDefaultAsync(c => c.CropId == cycle.CropId);
            if (crop == null)
                return _saveService.SaveNotFound();

            cycle.HarvestDate = cycle.CreateDate.AddMonths(crop.Lenght);

            _context.SetModified(cycle);

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(cycle);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Cycle>> DeleteCycleAsync(int id)
        {
            var cycle = await _context.Cycles.FirstOrDefaultAsync(c => c.CycleId == id);
            if (cycle == null)
                return _saveService.SaveNotFound();

            try
            {
                _context.Cycles.Remove(cycle);
                await _context.SaveChangesAsync();
                return _saveService.DeleteSuccess();
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<List<Cycle>> GetCyclesAsync()
        {
            return await _context.Cycles.Include(c => c.Crop)
                .Include(c => c.Lot).AsNoTracking().ToListAsync();
        }

        public async Task<Cycle> GetCycleByIdAsync(int id)
        {
            return await _context.Cycles.Include(c => c.Crop)
                .Include(c => c.Lot).Include(c => c.Withdraws).FirstOrDefaultAsync(c => c.CycleId == id);
        }

        public async Task<List<Cycle>> GetActiveCyclesAsync()
        {
            return await _context.Cycles.Where(c => !c.IsComplete).Include(c => c.Crop).Include(c => c.Lot).ToListAsync();
        }

        public async Task<List<Cycle>> GetCompleteCyclesAsync()
        {
            return await _context.Cycles.Where(c => c.IsComplete).Include(c => c.Crop).Include(c => c.Lot).ToListAsync();
        }

        public async Task<List<Cycle>> GetCyclesByUserAsync(string id)
        {
            return await _context.Costs.Where(c => c.ApplicationUserId == id)
                .Select(c => c.Cycle)
                .Distinct()
                .ToListAsync();
        }

        public async Task<SaveModel<Cycle>> CloseCycleAsync(int id)
        {
            var cycle = await _context.Cycles.FirstOrDefaultAsync(c => c.CycleId == id);
            if (cycle == null)
                return _saveService.SaveNotFound();

            cycle.IsComplete = true;

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(cycle);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SelectList> GetCyclesSelectListAsync()
        {
            return new SelectList(await GetActiveCyclesAsync(), "CycleId", "Name");
        }
    }
}
