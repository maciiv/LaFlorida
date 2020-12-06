using LaFlorida.Data;
using LaFlorida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface IWithdrawService
    {
        Task<SaveModel<Withdraw>> CreateWithdrawAsync(Withdraw withdraw);
        Task<SaveModel<Withdraw>> EditWithdrawAsync(Withdraw withdraw);
        Task<SaveModel<Withdraw>> DeleteWithdrawAsync(int id);
        Task<List<Withdraw>> GetWithdrawsAsync();
        Task<Withdraw> GetWithdrawByIdAsync(int id);
        Task<List<Withdraw>> GetWithdrawsByUserAsync(string id);
    }
    public class WithdrawService : IWithdrawService
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveService<Withdraw> _saveService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WithdrawService(IApplicationDbContext context, ISaveService<Withdraw> saveService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _saveService = saveService;
            _userManager = userManager;
        }

        public async Task<SaveModel<Withdraw>> CreateWithdrawAsync(Withdraw withdraw)
        {
            var user = await _userManager.FindByIdAsync(withdraw.ApplicationUserId);
            decimal balanceByUser;
            if (await _userManager.IsInRoleAsync(user, "Machinist"))
            {
                balanceByUser = await GetMachinistBalanceAsync(withdraw);
            }
            else
            {
                balanceByUser = await GetBalanceByUserAsync(withdraw);
            }

            if (balanceByUser <= withdraw.Quantity)
                return _saveService.InsufficientFunds();

            withdraw.CreateDate = DateTime.Now;

            try
            {
                await _context.Withdraws.AddAsync(withdraw);
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(withdraw);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Withdraw>> EditWithdrawAsync(Withdraw withdraw)
        {
            var balanceByUser = await GetBalanceByUserAsync(withdraw);
            if (balanceByUser <= withdraw.Quantity)
                return _saveService.InsufficientFunds();

            _context.Attach(withdraw).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(withdraw);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Withdraw>> DeleteWithdrawAsync(int id)
        {
            var withdraw = await _context.Withdraws.FindAsync(id);
            if (withdraw == null)
                return _saveService.SaveNotFound();

            try
            {
                _context.Withdraws.Remove(withdraw);
                await _context.SaveChangesAsync();
                return _saveService.DeleteSuccess();
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<List<Withdraw>> GetWithdrawsAsync()
        {
            return await _context.Withdraws.Include(c => c.ApplicationUser).Include(c => c.Cycle)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Withdraw> GetWithdrawByIdAsync(int id)
        {
            return await _context.Withdraws.Include(c => c.ApplicationUser).Include(c => c.Cycle)
                .FirstOrDefaultAsync(c => c.WithdrawId == id);
        }

        public async Task<List<Withdraw>> GetWithdrawsByUserAsync(string id)
        {
            return await _context.Withdraws.Where(c => c.ApplicationUserId == id)
                .Include(c => c.ApplicationUser).AsNoTracking().ToListAsync();
        }

        private async Task<decimal> GetBalanceByUserAsync(Withdraw withdraw)
        {
            var cycle = await _context.Cycles.FirstOrDefaultAsync(c => c.CycleId == withdraw.CycleId);
            var costs = await _context.Costs.Where(c => c.CycleId == withdraw.CycleId).ToListAsync();
            var totalCosts = costs.Sum(c => c.Total);
            var userCosts = costs.Where(c => c.ApplicationUserId == withdraw.ApplicationUserId).Sum(c => c.Total);
            var withdraws = await _context.Withdraws.Where(c => c.CycleId == withdraw.CycleId && c.ApplicationUserId == withdraw.ApplicationUserId).ToListAsync();
            var sales = await _context.Sales.Where(c => c.CycleId == withdraw.CycleId).ToListAsync();
            var salesByUser = userCosts / totalCosts * sales.Sum(c => c.Total);
            return Math.Round((decimal)((cycle.IsRent ? totalCosts : salesByUser) - withdraws.Sum(c => c.Quantity)), 2);      
        }

        private async Task<decimal> GetMachinistBalanceAsync(Withdraw withdraw)
        {
            var costs = await _context.Costs.Where(c => c.CycleId == withdraw.CycleId && c.JobId == 1).ToListAsync();
            var totalCosts = costs.Sum(c => c.Total) * (decimal)0.3;
            var withdraws = await _context.Withdraws.Where(c => c.CycleId == withdraw.CycleId && c.ApplicationUserId == withdraw.ApplicationUserId).ToListAsync();
            return Math.Round((decimal)(totalCosts - withdraws.Sum(c => c.Quantity)), 2);
        }
    }
}
