using Matuning.Domain.Models.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matuning.Domain.Interfaces
{
    public interface IReportsRepository
    {
        Task<bool> AddReportAsync(Report report);
        Task<Report> GetReportByIdAsync(int reportId);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<bool> UpdateReportAsync(Report report);
        Task<bool> DeleteReportAsync(int reportId);
    }
}