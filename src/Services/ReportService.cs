using LaFlorida.Data;
using LaFlorida.Models;
using LaFlorida.ServicesModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface IReportService
    {
        Task<List<CycleCostByUser>> GetCycleCostByUsersAsync(int cycleId);
        Task<CycleStatistics> GetCycleStatisticsAsync(int cycleId);
        Task<CycleCostByUser> GetCycleMachinistCost(int cycleId);
        Task<List<CycleStatistics>> GetLotStatisticsAsync(int lotId);
        Task<List<CycleStatistics>> GetCropStatisticsAsync(int cropId);

    }
    public class ReportService : IReportService
    {
        private readonly IApplicationDbContext _context;

        public ReportService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CycleCostByUser>> GetCycleCostByUsersAsync(int cycleId)
        {
            var cycle = await _context.Cycles.Include(c => c.Lot).FirstOrDefaultAsync(c => c.CycleId == cycleId);
            var costs = await _context.Costs.Where(c => c.CycleId == cycleId).Include(c => c.ApplicationUser).AsNoTracking().ToListAsync();
            var sales = await _context.Sales.Where(c => c.CycleId == cycleId).AsNoTracking().ToListAsync();
            var withdraws = await _context.Withdraws.Where(c => c.CycleId == cycleId).AsNoTracking().ToListAsync();

            return costs.GroupBy(c => c.ApplicationUserId).Select(grp => new CycleCostByUser 
                {
                    ApplicationUserId = grp.Key,
                    UserName = $"{grp.Select(c => c.ApplicationUser).FirstOrDefault().FirstName} {grp.Select(c => c.ApplicationUser).FirstOrDefault().LastName}",
                    LotName = cycle.Lot.Name,
                    CycleName = cycle.Name,
                    Costs = Math.Round((decimal)grp.Sum(c => c.Total), 2),
                    Percentage = Math.Round((decimal)grp.Sum(c => c.Total) / (decimal)costs.Sum(c => c.Total) * 100, 2),
                    Sales = Math.Round((decimal)sales.Sum(c => c.Total) * (decimal)grp.Sum(c => c.Total) / (decimal)costs.Sum(c => c.Total), 2),
                    Withdraws = withdraws.Where(c => c.ApplicationUserId == grp.Key).Sum(c => c.Quantity),
                    Balance = Math.Round((decimal)sales.Sum(c => c.Total) * (decimal)grp.Sum(c => c.Total) / (decimal)costs.Sum(c => c.Total), 2) - TotalWithdrawsByUser(withdraws, grp.Key)
            }).ToList();
        }

        public async Task<CycleCostByUser> GetCycleMachinistCost(int cycleId)
        {
            var costs = await _context.Costs.Where(c => c.CycleId == cycleId).Include(c => c.Job).AsNoTracking().ToListAsync();
            var withdraws = await _context.Withdraws.Where(c => c.CycleId == cycleId).AsNoTracking().ToListAsync();

            return new CycleCostByUser
            {
                Costs = Math.Round((decimal)(costs.Where(c => c.JobId == 1).Sum(c => c.Total) * (decimal)0.3), 2),
                Withdraws = withdraws.Where(c => !costs.Select(c => c.ApplicationUserId).Contains(c.ApplicationUserId)).Sum(c => c.Quantity),
                Balance = Math.Round((decimal)(costs.Where(c => c.JobId == 1).Sum(c => c.Total) * (decimal)0.3), 2) - withdraws.Where(c => !costs.Select(c => c.ApplicationUserId).Contains(c.ApplicationUserId)).Sum(c => c.Quantity)
            };
        }

        public async Task<CycleStatistics> GetCycleStatisticsAsync(int cycleId)
        {
            var cycle = await _context.Cycles.Include(c => c.Lot).Include(c => c.Costs).Include(c => c.Sales)
                .Include(c => c.Crop).AsNoTracking().FirstOrDefaultAsync(c => c.CycleId == cycleId);

            return GenerateCycleStatistics(cycle);
        }

        public async Task<List<CycleStatistics>> GetLotStatisticsAsync(int lotId)
        {
            var cycles = await _context.Cycles.Where(c => c.LotId == lotId && c.IsComplete)
                .Include(c => c.Lot).Include(c => c.Costs).Include(c => c.Sales).Include(c => c.Crop)
                .AsNoTracking().OrderByDescending(c => c.CreateDate).Take(10).ToListAsync();

            var result = new List<CycleStatistics>();

            cycles.ForEach(c => result.Add(GenerateCycleStatistics(c)));

            return result.OrderBy(c => c.CreateDate).ToList();
        }

        public async Task<List<CycleStatistics>> GetCropStatisticsAsync(int cropId)
        {
            var cycles = await _context.Cycles.Where(c => c.CropId == cropId && c.IsComplete)
                .Include(c => c.Lot).Include(c => c.Costs).Include(c => c.Sales).Include(c => c.Crop)
                .AsNoTracking().OrderByDescending(c => c.CreateDate).Take(10).ToListAsync();

            var result = new List<CycleStatistics>();

            cycles.ForEach(c => result.Add(GenerateCycleStatistics(c)));

            return result.OrderBy(c => c.CreateDate).ToList();
        }

        private CycleStatistics GenerateCycleStatistics(Cycle cycle)
        {
            return new CycleStatistics
            {
                CycleName = cycle.Name,
                LotName = cycle.Lot.Name,
                LotSize = cycle.Lot.Size,
                CropName = cycle.Crop.Name,
                CropLenght = cycle.Crop.Lenght,
                CreateDate = cycle.CreateDate,
                TotalCosts = cycle.Costs.Any() ? cycle.Costs.Sum(c => c.Total) : 0,
                TotalSales = cycle.Sales.Any() ? cycle.Sales.Sum(c => c.Total) : 0,
                Performace = cycle.Sales.Any() ? Math.Round((decimal)cycle.Sales.Sum(c => c.Quantity) / cycle.Lot.Size, 2) : 0,
                Return = cycle.Sales.Any() && cycle.Costs.Any() ? Math.Round(((decimal)cycle.Sales.Sum(c => c.Total) - (decimal)cycle.Costs.Sum(c => c.Total)) * 100  / (decimal)cycle.Costs.Sum(c => c.Total), 2) : 0,
                Profit = cycle.Sales.Any() && cycle.Costs.Any() ? cycle.Sales.Sum(c => c.Total) - cycle.Costs.Sum(c => c.Total) : 0,
                ProfitByLenght = cycle.Sales.Any() && cycle.Costs.Any() ? Math.Round(((decimal)cycle.Sales.Sum(c => c.Total) - (decimal)cycle.Costs.Sum(c => c.Total)) / cycle.Crop.Lenght, 2) : 0,
                ProfitBySize = cycle.Sales.Any() && cycle.Costs.Any() ? Math.Round(((decimal)cycle.Sales.Sum(c => c.Total) - (decimal)cycle.Costs.Sum(c => c.Total)) / cycle.Lot.Size, 2) : 0,
            };
        }

        private decimal? TotalWithdrawsByUser(List<Withdraw> withdraws, string applicationUserId)
        {
            return withdraws.Where(c => c.ApplicationUserId == applicationUserId).Sum(c => c.Quantity);
        }
    }
}
