using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IST.Zeiterfassung.Application.DTOs.Report;
using IST.Zeiterfassung.Application.Results;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface IReportService
{
    Task<Result<MonthlyReportDTO>> GetMonthlyReportAsync(Guid userId, int jahr, int monat);
}

