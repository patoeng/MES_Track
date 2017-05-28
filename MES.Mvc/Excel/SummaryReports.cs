using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Web.Hosting;
using System.Web.UI;
using MES.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MES.Mvc.Excel
{
    public static class SummaryReports
    {
        public static string ProductProcessToExcelFile(List<ProductProcess> data )
        {
            data.Reverse();
            var t = DateTime.Now;
            var xlsx = "ProductProcessSummary_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
            var fileName = HostingEnvironment.ApplicationPhysicalPath + "/UploadedFiles/xlsx/" + xlsx;
            var newFile = new FileInfo(fileName);
            if (newFile.Exists)
            {
                newFile.Delete();// ensures we create a new workbook
                newFile = new FileInfo(fileName);
            }
            var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Summary");

            foreach (ProductProcess pp in data)
            {
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, pp.Machine.Name);
                ws.SetValue(1, 2, pp.Product.Reference);
                ws.SetValue(1, 3, pp.Workorder.Number);
                ws.SetValue(1, 4, pp.FullName);
                ws.SetValue(1, 5, pp.DateTime.ToString("F"));
                ws.SetValue(1, 6, pp.Result.ToString());
            }


            //Create header
            ws.InsertRow(1,1);
            ws.SetValue(1, 1, "Machine Name");
            ws.SetValue(1, 2, "Product Reference");
            ws.SetValue(1, 3, "Work Order");
            ws.SetValue(1, 4, "DataMatrix");
            ws.SetValue(1, 5, "Date & Time");
            ws.SetValue(1, 6, "Result");

            using (var rng = ws.Cells[1, 1, 1, 7])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Font.Color.SetColor(Color.White);
                rng.Style.WrapText = false;
                rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                rng.AutoFitColumns();
            }
            package.SaveAs(newFile);
            return xlsx;
        }
        public static string WorkOrderToExcelFile(List<Workorder> data)
        {
            data.Reverse();
            var t = DateTime.Now;
            var xlsx = "WorkOrderSummary_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
            var fileName = HostingEnvironment.ApplicationPhysicalPath + "/UploadedFiles/xlsx/" + xlsx;
            var newFile = new FileInfo(fileName);
            if (newFile.Exists)
            {
                newFile.Delete();// ensures we create a new workbook
                newFile = new FileInfo(fileName);
            }
            var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Summary");

            foreach (Workorder pp in data)
            {
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, pp.Number);
                ws.SetValue(1, 2, pp.Reference.Reference);
                ws.SetValue(1, 3, pp.Quantity);
                ws.SetValue(1, 4, pp.DateTime.ToString("F"));
                ws.SetValue(1, 5, pp.EntryThroughMachine.Name);
            }


            //Create header
            ws.InsertRow(1, 1);
            ws.SetValue(1, 1, "Work Order Number");
            ws.SetValue(1, 2, "Product Reference");
            ws.SetValue(1, 3, "Quantity");
            ws.SetValue(1, 4, "Date & Time");
            ws.SetValue(1, 5, "Entry Machine");

            using (var rng = ws.Cells[1, 1, 1, 5])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Font.Color.SetColor(Color.White);
                rng.Style.WrapText = false;
                rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                rng.AutoFitColumns();
            }
            package.SaveAs(newFile);
            return xlsx;
        }
    }
}