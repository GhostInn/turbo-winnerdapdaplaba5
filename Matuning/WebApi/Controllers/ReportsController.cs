using Matuning.Domain.Interfaces;
using Matuning.Domain.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matuning.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsController(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }

        // Create report
        [HttpPost]
        public async Task<IActionResult> AddReportAsync([FromBody] Report report)
        {
            if (report == null)
            {
                return BadRequest("Report cannot be null.");
            }

            // Дополнительная валидация модели
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool result = await _reportsRepository.AddReportAsync(report);
                if (result)
                {
                    // Возвращаем статус 201 Created с ссылкой на созданный ресурс
                    return CreatedAtAction(nameof(GetReportByIdAsync), new { reportId = report.ReportId }, report);
                }
                else
                {
                    // Если по каким-то причинам добавление не удалось
                    return StatusCode(500, "An error occurred while creating the report.");
                }
            }
            catch (ArgumentException ex)
            {
                // Возвращаем ошибку валидации
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Логирование ошибки (можно добавить логгер через DI)
                // _logger.LogError(ex, "Error occurred while adding a report.");

                // Возвращаем статус 500 Internal Server Error
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        // Get report by ID
        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetReportByIdAsync(int reportId)
        {
            if (reportId <= 0)
            {
                return BadRequest("ReportId must be a positive integer.");
            }

            try
            {
                var report = await _reportsRepository.GetReportByIdAsync(reportId);

                if (report == null)
                {
                    return NotFound($"Report with ID {reportId} was not found.");
                }

                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                // _logger.LogError(ex, "Error occurred while retrieving the report.");

                return StatusCode(500, "An internal server error occurred.");
            }
        }

        // Get all reports
        [HttpGet]
        public async Task<IActionResult> GetAllReportsAsync()
        {
            try
            {
                var reports = await _reportsRepository.GetAllReportsAsync();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                // _logger.LogError(ex, "Error occurred while retrieving all reports.");

                return StatusCode(500, "An internal server error occurred.");
            }
        }

        // Update report
        [HttpPut("{reportId}")]
        public async Task<IActionResult> UpdateReportAsync(int reportId, [FromBody] Report report)
        {
            if (report == null)
            {
                return BadRequest("Report cannot be null.");
            }

            if (reportId != report.ReportId)
            {
                return BadRequest("Report ID in the URL does not match the Report ID in the body.");
            }

            // Дополнительная валидация модели
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool result = await _reportsRepository.UpdateReportAsync(report);

                if (result)
                {
                    return NoContent(); // 204 No Content означает успешное обновление без возвращаемого содержимого
                }
                else
                {
                    return NotFound($"Report with ID {reportId} was not found.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                // _logger.LogError(ex, "Error occurred while updating the report.");

                return StatusCode(500, "An internal server error occurred.");
            }
        }

        // Delete report
        [HttpDelete("{reportId}")]
        public async Task<IActionResult> DeleteReportAsync(int reportId)
        {
            if (reportId <= 0)
            {
                return BadRequest("ReportId must be a positive integer.");
            }

            try
            {
                bool result = await _reportsRepository.DeleteReportAsync(reportId);

                if (result)
                {
                    return NoContent(); // 204 No Content означает успешное удаление
                }
                else
                {
                    return NotFound($"Report with ID {reportId} was not found.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                // _logger.LogError(ex, "Error occurred while deleting the report.");

                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}