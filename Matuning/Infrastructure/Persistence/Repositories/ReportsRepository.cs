using Matuning.Domain.Interfaces;
using Matuning.Domain.Models.Reports;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Matuning.Infrastructure
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportsRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddReportAsync(Report report)
        {
            _context.Reports.Add(report);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _context.Reports.FindAsync(reportId);
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<bool> UpdateReportAsync(Report report)
        {
            _context.Reports.Update(report);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null)
                return false;

            _context.Reports.Remove(report);
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}