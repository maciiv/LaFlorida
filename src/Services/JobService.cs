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
    public interface IJobService
    {
        Task<SaveModel<Job>> CreateJobAsync(Job job);
        Task<SaveModel<Job>> EditJobAsync(Job job);
        Task<SaveModel<Job>> DeleteJobAsync(int id);
        Task<List<Job>> GetJobsAsync();
        Task<Job> GetJobByIdAsync(int id);
        Task<SelectList> GetJobsSelectListAsync();
    }

    public class JobService : IJobService
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveService<Job> _saveService;

        public JobService(IApplicationDbContext context, ISaveService<Job> saveService)
        {
            _context = context;
            _saveService = saveService;
        }

        public async Task<SaveModel<Job>> CreateJobAsync(Job job)
        {
            var exists = await _context.Jobs.AnyAsync(c => c.Name == job.Name);
            if (exists)
                return _saveService.SaveExists();

            try
            {
                await _context.Jobs.AddAsync(job);
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(job);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Job>> EditJobAsync(Job job)
        {
            _context.SetModified(job);

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(job);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Job>> DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(c => c.JobId == id);
            if (job == null)
                return _saveService.SaveNotFound();

            try
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
                return _saveService.DeleteSuccess();
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<List<Job>> GetJobsAsync()
        {
            return await _context.Jobs.AsNoTracking().ToListAsync();
        }

        public async Task<Job> GetJobByIdAsync(int id)
        {
            return await _context.Jobs.FirstOrDefaultAsync(c => c.JobId == id);
        }

        public async Task<SelectList> GetJobsSelectListAsync()
        {
            return new SelectList((await GetJobsAsync()).OrderBy(c => c.Name), "JobId", "Name");
        }
    }
}
