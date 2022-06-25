using LaFlorida.Data;
using LaFlorida.Helpers;
using LaFlorida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface IPaymentService
    {
        Task<SaveModel<Payment>> CreatePaymentAsync(Payment payment);
        Task<SaveModel<Payment>> EditPaymentAsync(Payment payment);
        Task<SaveModel<Payment>> DeletePaymentAsync(int id);
        Task<List<Payment>> GetPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<List<Payment>> GetPaymentsByUserAsync(string id);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveService<Payment> _saveService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtectionHelper _dataProtectionHelper;

        public PaymentService(IApplicationDbContext context, ISaveService<Payment> saveService, UserManager<ApplicationUser> userManager, IDataProtectionHelper dataProtectionHelper)
        {
            _context = context;
            _saveService = saveService;
            _userManager = userManager;
            _dataProtectionHelper = dataProtectionHelper;
        }

        public async Task<SaveModel<Payment>> CreatePaymentAsync(Payment payment)
        {
            payment.ApplicationUserId = _dataProtectionHelper.Unprotect(payment.ApplicationUserId);
            var balanceByUser = await GetBalanceByUserAsync(payment);
            if (balanceByUser < payment.Quantity)
                return _saveService.InsufficientFunds(balanceByUser);

            payment.CreateDate = DateTime.Now;

            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(payment);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Payment>> EditPaymentAsync(Payment payment)
        {
            payment.ApplicationUserId = _dataProtectionHelper.Unprotect(payment.ApplicationUserId);
            var balanceByUser = await GetBalanceByUserAsync(payment);
            if (balanceByUser < payment.Quantity)
                return _saveService.InsufficientFunds(balanceByUser);

            _context.Attach(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return _saveService.SaveSuccess(payment);
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<SaveModel<Payment>> DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return _saveService.SaveNotFound();

            try
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
                return _saveService.DeleteSuccess();
            }
            catch (Exception e)
            {
                return _saveService.SaveFail(e);
            }
        }

        public async Task<List<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments.Include(c => c.ApplicationUser).Include(c => c.Cycle)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments.Include(c => c.ApplicationUser).Include(c => c.Cycle)
                .FirstOrDefaultAsync(c => c.PaymentId == id);
        }

        public async Task<List<Payment>> GetPaymentsByUserAsync(string id)
        {
            return await _context.Payments.Where(c => c.ApplicationUserId == _dataProtectionHelper.Unprotect(id))
                .Include(c => c.ApplicationUser).AsNoTracking().ToListAsync();
        }

        private async Task<decimal> GetBalanceByUserAsync(Payment payment)
        {
            var user = await _userManager.FindByIdAsync(payment.ApplicationUserId);
            if (await _userManager.IsInRoleAsync(user, "Machinist"))
            {
                return await GetMachinistBalanceAsync(payment);
            }

            var cycle = await _context.Cycles.FirstOrDefaultAsync(c => c.CycleId == payment.CycleId);
            var costs = await _context.Costs.Where(c => c.CycleId == payment.CycleId).Include(c => c.Job).AsNoTracking().ToListAsync();
            if (cycle.IsRent)
            {
                costs = costs.Where(c => c.Job.IsRent && !c.Job.IsMachinist).ToList();
            }
            var userCosts = costs.Where(c => c.ApplicationUserId == payment.ApplicationUserId).Sum(c => c.Total);
            var payments = await _context.Payments.Where(c => c.CycleId == payment.CycleId && c.ApplicationUserId == payment.ApplicationUserId).ToListAsync();
            var sales = await _context.Sales.Where(c => c.CycleId == payment.CycleId).ToListAsync();
            var salesByUser = userCosts / costs.Sum(c => c.Total) * sales.Sum(c => c.Total);
            return Math.Round((decimal)((cycle.IsRent ? userCosts : salesByUser) - payments.Sum(c => c.Quantity)), 2);      
        }

        private async Task<decimal> GetMachinistBalanceAsync(Payment payment)
        {
            var costs = await _context.Costs.Where(c => c.CycleId == payment.CycleId && c.Job.IsMachinist).ToListAsync();
            var totalCosts = costs.Sum(c => c.Total) * (decimal)0.3;
            var payments = await _context.Payments.Where(c => c.CycleId == payment.CycleId && c.ApplicationUserId == payment.ApplicationUserId).ToListAsync();
            return Math.Round((decimal)(totalCosts - payments.Sum(c => c.Quantity)), 2);
        }
    }
}
