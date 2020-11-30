using LaFlorida.Data;
using LaFlorida.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface ICropService
    {
        Task<SaveModel<Crop>> CreateCropAsync(Crop crop);
        Task<SaveModel<Crop>> EditCropAsync(Crop crop);
        Task<SaveModel<Crop>> DeleteCropAsync(int id);
        Task<List<Crop>> GetCropsAsync();
        Task<Crop> GetCropByIdAsync(int id);
        Task<SelectList> GetCropsSelectListAsync();
    }

    public class CropService : ICropService
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveService<Crop> _saveService;

        public CropService(IApplicationDbContext context, ISaveService<Crop> saveService)
        {
            _context = context;
            _saveService = saveService;
        }

        public async Task<SaveModel<Crop>> CreateCropAsync(Crop crop)
        {
            var cropExists = await _context.Crops.AnyAsync(c => c.Name == crop.Name);
            if (cropExists)
                return _saveService.SaveExists();

            try
            {
                await _context.Crops.AddAsync(crop);
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(crop);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Crop>> EditCropAsync(Crop crop)
        {
            _context.Attach(crop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(crop);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Crop>> DeleteCropAsync(int id)
        {
            var crop = await _context.Crops.FindAsync(id);
            if (crop == null)
                return _saveService.SaveNotFound();

            try
            {
                _context.Crops.Remove(crop);
                await _context.SaveChangesAsync();
                return _saveService.DeleteSuccess();
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<List<Crop>> GetCropsAsync()
        {
            return await _context.Crops.AsNoTracking().ToListAsync();
        }

        public async Task<Crop> GetCropByIdAsync(int id)
        {
            return await _context.Crops.FirstOrDefaultAsync(c => c.CropId == id);
        }

        public async Task<SelectList> GetCropsSelectListAsync()
        {
            return new SelectList(await GetCropsAsync(), "CropId", "Name");
        }
    }
}
