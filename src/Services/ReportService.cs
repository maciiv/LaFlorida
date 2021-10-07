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
        Task<List<CycleCostByUser>> GetUserCyclesCostsAsync(string applicationUserId);
        Task<CycleStatistics> GetCycleStatisticsAsync(int cycleId);
        Task<CycleCostByUser> GetCycleMachinistCostAsync(int cycleId);
        Task<(List<SummarySummaryStatistics> lotSummary, List<SummarySummaryStatistics> cropSummary)> GetSummaryStatisticsAsync();
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
            var cycle = await _context.Cycles.Include(c => c.Lot).Include(c => c.Crop).FirstOrDefaultAsync(c => c.CycleId == cycleId);
            var costs = await _context.Costs.Where(c => c.CycleId == cycleId).Include(c => c.ApplicationUser).Include(c => c.Job)
                .AsNoTracking().ToListAsync();
            var allCosts = (decimal)costs.Sum(c => c.Total);
            var sales = await _context.Sales.Where(c => c.CycleId == cycleId).AsNoTracking().ToListAsync();
            var allSales = (decimal)sales.Sum(c => c.Total);
            var withdraws = await _context.Withdraws.Where(c => c.CycleId == cycleId).AsNoTracking().ToListAsync();

            return costs.GroupBy(c => c.ApplicationUserId).Select(grp =>
            {
                var userCosts = (decimal)grp.Sum(c => c.Total);
                var sales = cycle.IsRent ? userCosts : allSales * userCosts / allCosts;

                return new CycleCostByUser
                {
                    ApplicationUserId = grp.Key,
                    UserName = $"{grp.Select(c => c.ApplicationUser).FirstOrDefault().FirstName} {grp.Select(c => c.ApplicationUser).FirstOrDefault().LastName}",
                    LotName = cycle.Lot.Name,
                    CycleName = cycle.Name,
                    CycleId = cycle.CycleId,
                    IsCycleComplete = cycle.IsComplete,
                    IsCycleRent = cycle.IsRent,
                    CropName = cycle.Crop.Name,
                    CreateDate = cycle.CreateDate,
                    HarvestDate = cycle.HarvestDate,
                    Costs = Math.Round(userCosts, 2),
                    Percentage = Math.Round(userCosts / allCosts * 100, 2),
                    Sales = Math.Round(sales, 2),
                    Withdraws = withdraws.Where(c => c.ApplicationUserId == grp.Key).Sum(c => c.Quantity),
                    Balance = Math.Round(sales, 2) - withdraws.Where(c => c.ApplicationUserId == grp.Key).Sum(c => c.Quantity),
                    Profit = Math.Round(sales - userCosts, 2),
                    Return = Math.Round((sales - userCosts) * 100 / userCosts, 2)
                };
            }).ToList();
        }

        public async Task<List<CycleCostByUser>> GetUserCyclesCostsAsync(string applicationUserId)
        {
            var costs = await _context.Costs.Where(c => c.ApplicationUserId == applicationUserId)
                .Include(c => c.Cycle).Include(c => c.Cycle.Crop).Include(c => c.Cycle.Lot).Include(c => c.ApplicationUser).Include(c => c.Job)
                .AsNoTracking().ToListAsync();
            var allCosts = await _context.Costs.Where(c => costs.Select(d => d.CycleId).Contains(c.CycleId)).AsNoTracking().ToListAsync();
            var sales = await _context.Sales.Where(c => costs.Select(d => d.CycleId).Contains(c.CycleId)).AsNoTracking().ToListAsync();
            var withdraws = await _context.Withdraws.Where(c => c.ApplicationUserId == applicationUserId).AsNoTracking().ToListAsync();

            return costs.GroupBy(c => c.CycleId).Select(grp =>
            {
                var userCosts = (decimal)grp.Sum(c => c.Total);
                var userSales = grp.FirstOrDefault().Cycle.IsRent ? userCosts : 
                    (decimal)sales.Where(c => c.CycleId == grp.Key).Sum(c => c.Total) * userCosts / (decimal)allCosts.Where(c => c.CycleId == grp.Key).Sum(c => c.Total);
                
                return new CycleCostByUser
                {
                    ApplicationUserId = grp.FirstOrDefault().ApplicationUserId,
                    UserName = $"Lote {grp.FirstOrDefault().Cycle.Lot.Name} - {grp.FirstOrDefault().Cycle.Crop.Name}",
                    LotName = grp.FirstOrDefault().Cycle.Lot.Name,
                    CycleId = grp.Key,
                    CycleName = grp.FirstOrDefault().Cycle.Name,
                    IsCycleComplete = grp.FirstOrDefault().Cycle.IsComplete,
                    IsCycleRent = grp.FirstOrDefault().Cycle.IsRent,
                    CropName = grp.FirstOrDefault().Cycle.Crop.Name,
                    CreateDate = grp.FirstOrDefault().Cycle.CreateDate,
                    HarvestDate = grp.FirstOrDefault().Cycle.HarvestDate,
                    Costs = Math.Round(userCosts, 2),
                    Percentage = Math.Round(userCosts / (decimal)allCosts.Where(c => c.CycleId == grp.Key).Sum(c => c.Total) * 100, 2),
                    Sales = Math.Round(userSales, 2),
                    Withdraws = withdraws.Where(c => c.CycleId == grp.Key).Sum(c => c.Quantity),
                    Balance = Math.Round(userSales, 2) - withdraws.Where(c => c.CycleId == grp.Key).Sum(c => c.Quantity),
                    Profit = Math.Round(userSales - userCosts, 2),
                    Return = userCosts != 0 ? Math.Round((userSales - userCosts) * 100 / userCosts, 2) : 0
                };
            }).Where(c => c.Costs != 0).ToList();
        }

        public async Task<CycleCostByUser> GetCycleMachinistCostAsync(int cycleId)
        {
            var costs = await _context.Costs.Where(c => c.CycleId == cycleId).Include(c => c.Job).AsNoTracking().ToListAsync();
            var withdraws = await _context.Withdraws.Where(c => c.CycleId == cycleId).AsNoTracking().ToListAsync();

            return new CycleCostByUser
            {
                Costs = Math.Round((decimal)(costs.Where(c => c.Job.IsMachinist).Sum(c => c.Total) * (decimal)0.3), 2),
                Withdraws = withdraws.Where(c => !costs.Select(c => c.ApplicationUserId).Contains(c.ApplicationUserId)).Sum(c => c.Quantity),
                Balance = Math.Round((decimal)(costs.Where(c => c.Job.IsMachinist).Sum(c => c.Total) * (decimal)0.3), 2) - withdraws.Where(c => !costs.Select(c => c.ApplicationUserId).Contains(c.ApplicationUserId)).Sum(c => c.Quantity)
            };
        }

        public async Task<CycleStatistics> GetCycleStatisticsAsync(int cycleId)
        {
            var cycle = await _context.Cycles.Include(c => c.Lot).Include(c => c.Costs).Include(c => c.Sales)
                .Include(c => c.Crop).AsNoTracking().FirstOrDefaultAsync(c => c.CycleId == cycleId);

            return GenerateCycleStatistics(cycle);
        }

        public async Task<(List<SummarySummaryStatistics> lotSummary, List<SummarySummaryStatistics> cropSummary)> GetSummaryStatisticsAsync()
        {
            var cycles = await _context.Cycles.Where(c => c.IsComplete)
                .Include(c => c.Lot).Include(c => c.Crop)
                .Include(c => c.Costs).ThenInclude(c => c.Job).Include(c => c.Sales).AsNoTracking().ToListAsync();

            var lotSummary = cycles.GroupBy(c => c.LotId).Select(grp =>
            {
                var totalCosts = (decimal)grp.Select(c => c.Costs.Sum(d => d.Total)).Sum();
                var totalSales = (decimal)grp.Select(c => c.Sales.Sum(d => d.Total)).Sum();

                var summaryStatistics = grp.GroupBy(c => c.CropId).Select(g =>
                {
                    var summaryTotalCosts = (decimal)g.Select(c => c.Costs.Sum(d => d.Total)).Sum();
                    var summaryTotalSales = (decimal)g.Select(c => c.Sales.Sum(d => d.Total)).Sum();

                    return new SummaryStatistics
                    {
                        Name = g.FirstOrDefault().Crop.Name,
                        TotalCosts = summaryTotalCosts,
                        TotalSales = summaryTotalSales,
                        Return = Math.Round((summaryTotalSales - summaryTotalCosts) * 100 / summaryTotalCosts, 2),
                        Profit = Math.Round(summaryTotalSales - summaryTotalCosts, 2),
                        CashFlow = Math.Round((decimal)g.Select(c => c.Costs.Where(c => !c.Job.IsRent && !c.Job.IsMachinist).Sum(d => d.Total)).Sum(), 0)
                    };
                }).ToList();

                return new SummarySummaryStatistics
                {
                    Name = grp.FirstOrDefault().Lot.Name,
                    TotalCosts = totalCosts,
                    TotalSales = totalSales,
                    Return = Math.Round((totalSales - totalCosts) * 100 / totalCosts, 2),
                    Profit = Math.Round(totalSales - totalCosts, 2),
                    SummaryStatistics = summaryStatistics
                };
            }).ToList();

            var cropSummary = cycles.GroupBy(c => c.CropId).Select(grp =>
            {
                var totalCosts = (decimal)grp.Select(c => c.Costs.Sum(d => d.Total)).Sum();
                var totalSales = (decimal)grp.Select(c => c.Sales.Sum(d => d.Total)).Sum();

                var summaryStatistics = grp.GroupBy(c => c.LotId).Select(g =>
                {
                    var summaryTotalCosts = (decimal)g.Select(c => c.Costs.Sum(d => d.Total)).Sum();
                    var summaryTotalSales = (decimal)g.Select(c => c.Sales.Sum(d => d.Total)).Sum();

                    return new SummaryStatistics
                    {
                        Name = g.FirstOrDefault().Lot.Name,
                        TotalCosts = summaryTotalCosts,
                        TotalSales = summaryTotalSales,
                        Return = Math.Round((summaryTotalSales - summaryTotalCosts) * 100 / summaryTotalCosts, 2),
                        Profit = Math.Round(summaryTotalSales - summaryTotalCosts, 2),
                        CashFlow = Math.Round((decimal)g.Select(c => c.Costs.Where(c => !c.Job.IsRent && !c.Job.IsMachinist).Sum(d => d.Total)).Sum(), 0)
                    };
                }).ToList();

                return new SummarySummaryStatistics
                {
                    Name = grp.FirstOrDefault().Crop.Name,
                    TotalCosts = totalCosts,
                    TotalSales = totalSales,
                    Return = Math.Round((totalSales - totalCosts) * 100 / totalCosts, 2),
                    Profit = Math.Round(totalSales - totalCosts, 2),
                    SummaryStatistics = summaryStatistics
                };
            }).ToList();

            return (lotSummary, cropSummary);
        }

        public async Task<List<SummaryStatistics>> GetCashFlowSummaryStatisticsAsync()
        {
            var cycles = await _context.Cycles.Where(c => c.IsComplete)
                .Include(c => c.Lot).Include(c => c.Crop)
                .Include(c => c.Costs).ThenInclude(c => c.Job)
                .Include(c => c.Sales).AsNoTracking().ToListAsync();

            return cycles.GroupBy(c => c.LotId).Select(grp =>
            {
                var totalCosts = (decimal)grp.FirstOrDefault().Costs.Where(c => !c.Job.IsRent && !c.Job.IsMachinist).Sum(c => c.Total);
                var totalSales = (decimal)grp.FirstOrDefault().Sales.Sum(c => c.Total);

                return new SummaryStatistics
                {
                    Name = grp.FirstOrDefault().Lot.Name,
                    TotalCosts = totalCosts,
                    TotalSales = totalSales,
                    Return = Math.Round((totalSales - totalCosts) * 100 / totalCosts, 2),
                    Profit = Math.Round(totalSales - totalCosts, 2)
                };
            }).ToList();
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
                .Include(c => c.Lot).Include(c => c.Costs).ThenInclude(c => c.Job).Include(c => c.Sales).Include(c => c.Crop)
                .AsNoTracking().OrderByDescending(c => c.CreateDate).Take(10).ToListAsync();

            var result = new List<CycleStatistics>();

            cycles.ForEach(c => result.Add(GenerateCycleStatistics(c)));

            return result.OrderBy(c => c.CreateDate).ToList();
        }

        private CycleStatistics GenerateCycleStatistics(Cycle cycle)
        {
            return new CycleStatistics
            {
                Name = cycle.Name,
                LotId = cycle.LotId,
                LotName = cycle.Lot.Name,
                LotSize = cycle.Lot.Size,
                CropId = cycle.CropId,
                CropName = cycle.Crop.Name,
                CropLenght = cycle.Crop.Lenght,
                CreateDate = cycle.CreateDate,
                TotalCosts = cycle.Costs.Any() ? cycle.Costs.Sum(c => c.Total) : 0,
                TotalSales = cycle.Sales.Any() ? cycle.Sales.Sum(c => c.Total) : 0,
                Performace = cycle.Sales.Any() ? Math.Round((decimal)cycle.Sales.Sum(c => c.Quintals) / cycle.Lot.Size, 2) : 0,
                Return = cycle.Sales.Any() && cycle.Costs.Any() ? Math.Round(((decimal)cycle.Sales.Sum(c => c.Total) - (decimal)cycle.Costs.Sum(c => c.Total)) * 100  / (decimal)cycle.Costs.Sum(c => c.Total), 2) : 0,
                Profit = cycle.Sales.Any() && cycle.Costs.Any() ? cycle.Sales.Sum(c => c.Total) - cycle.Costs.Sum(c => c.Total) : 0
            };
        }
    }
}
