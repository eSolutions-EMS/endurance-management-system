using Core.ConventionalServices;
using OfficeOpenXml;
using System;
using System.IO;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace EMS.Judge.Application.Common.Services;

public abstract class ExcelServiceBase : ITransientService, IDisposable
{
    protected ExcelServiceBase()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    protected ExcelPackage Excel { get; private set; }

    protected void Initialize(FileInfo file)
    {
        this.Excel = new ExcelPackage(file);
    }

    public void Dispose()
    {
        this.Excel?.Dispose();
    }
}
