using LaFlorida.Data;
using LaFlorida.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface ILotService
    {
        Task<SaveModel<Lot>> CreateLotAsync(Lot lot);
        Task<SaveModel<Lot>> EditLotAsync(Lot lot);
        Task<SaveModel<Lot>> DeleteLotAsync(int id);
        Task<List<Lot>> GetLotsAsync();
        Task<Lot> GetLotByIdAsync(int id);
        Task<SelectList> GetLotsSelectListAsync();
    }

    public class LotService : ILotService
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveService<Lot> _saveService;

        public LotService(IApplicationDbContext context, ISaveService<Lot> saveService)
        {
            _context = context;
            _saveService = saveService;
        }

        public async Task<SaveModel<Lot>> CreateLotAsync(Lot lot)
        {
            var exists = await _context.Lots.AnyAsync(c => c.Name == lot.Name);
            if (exists)
                return _saveService.SaveExists();

            try
            {
                await _context.Lots.AddAsync(lot);
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(lot);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Lot>> EditLotAsync(Lot lot)
        {
            _context.SetModified(lot);

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(lot);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Lot>> DeleteLotAsync(int id)
        {
            var lot = await _context.Lots.FirstOrDefaultAsync(c => c.LotId == id);
            if (lot == null)
                return _saveService.SaveNotFound();

            try
            {
                _context.Lots.Remove(lot);
                await _context.SaveChangesAsync();
                return _saveService.DeleteSuccess();
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<List<Lot>> GetLotsAsync()
        {
            return await _context.Lots.AsNoTracking().ToListAsync();
        }

        public async Task<Lot> GetLotByIdAsync(int id)
        {
            return await _context.Lots.FirstOrDefaultAsync(c => c.LotId == id);
        }

        public async Task<SelectList> GetLotsSelectListAsync()
        {
            return new SelectList(await GetLotsAsync(), "LotId", "Name");
        }
    }
}
