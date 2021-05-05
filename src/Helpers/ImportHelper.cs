using LaFlorida.Extensions;
using LaFlorida.Models;
using LaFlorida.PageModels;
using LaFlorida.Services;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LaFlorida.Helpers
{
    public interface IImportHelper
    {
        Task<List<Cost>> ValidateCostsFileAsync(IFormFile file, int cycleId);
        _ImportPartialPageModel PartialModel(int Id, List<Cost> costs= null, List<Sale> sales = null);
    }
    public class ImportHelper : IImportHelper
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICycleService _cycleService;
        private readonly IJobService _jobService;

        public ImportHelper(IApplicationUserService applicationUserService, ICycleService cycleService, IJobService jobService)
        {
            _applicationUserService = applicationUserService;
            _cycleService = cycleService;
            _jobService = jobService;
        }

        public _ImportPartialPageModel PartialModel(int Id, List<Cost> costs = null, List<Sale> sales = null)
        {
            return new _ImportPartialPageModel
            {
                Costs = costs,
                Sales = sales
            };
        }

        public async Task<List<Cost>> ValidateCostsFileAsync(IFormFile file, int cycleId)
        {
            var result = new List<Cost>();
            var fileStream = new MemoryStream();
            await file.CopyToAsync(fileStream);
            fileStream.Position = 0;
            ISheet sheet;

            if (file.Name.EndsWith(".xls"))
            {
                var hssfwb = new HSSFWorkbook(fileStream);
                sheet = hssfwb.GetSheetAt(0);
            }
            else
            {
                var hssfwb = new XSSFWorkbook(fileStream);
                sheet = hssfwb.GetSheetAt(0);
            }

            for (var i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var newCost = new Cost();

                try
                {
                    newCost.Quantity = row.GetCell(2).ToString().GetDecimal();
                    newCost.Price = row.GetCell(3).ToString().GetDecimal();
                    newCost.Total = newCost.Quantity * newCost.Price;
                    newCost.Details = row.GetCell(4).ToString();
                    newCost.CreateDate = DateTime.Now;
                    newCost.ApplicationUserId = row.GetCell(5).StringCellValue;
                    newCost.CycleId = cycleId;
                    newCost.JobId = row.GetCell(6).NumericCellValue.ToString().GetInt();

                    newCost.Cycle = await _cycleService.GetCycleByIdAsync(newCost.CycleId);
                    newCost.Job = await _jobService.GetJobByIdAsync(newCost.JobId);
                    newCost.ApplicationUser = await _applicationUserService.GetApplicationUserByIdAsync(newCost.ApplicationUserId);
                }
                catch (Exception e)
                {

                    newCost.Quantity = -1;
                    newCost.Price = -1;
                    newCost.Total = -1;
                    newCost.Details = e.Message;
                    newCost.CreateDate = DateTime.Now;
                    newCost.ApplicationUserId = "";
                    newCost.CycleId = -1;
                    newCost.JobId = -1;
                }

                result.Add(newCost);
            }

            return result;
        }
    }
}
