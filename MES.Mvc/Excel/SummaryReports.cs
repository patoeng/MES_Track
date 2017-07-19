using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Web.Hosting;
using System.Web.UI;
using MES.Models;
using MES.Mvc.Models;
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
        public static string WorkOrderToExcelFile(List<WorkOrderDetailsModels> data, bool isAdmin)
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

            foreach (WorkOrderDetailsModels pp in data)
            {
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, pp.Number);
                ws.SetValue(1, 2, pp.Reference);
                ws.SetValue(1, 3, pp.Quantity);
                ws.SetValue(1, 4, pp.DateTime.ToString("F"));
                ws.SetValue(1, 5, pp.FrontLine);
                ws.SetValue(1, 6, pp.UseProductSequence);
                ws.SetValue(1, 7, pp.Machine);
                ws.SetValue(1, 8, pp.GeneratedQty);
                ws.SetValue(1, 9, pp.ProcessQty);
                if (isAdmin)
                {
                    ws.SetValue(1, 10, pp.PassQty);
                    ws.SetValue(1, 11, pp.FailQty);
                    ws.SetValue(1, 12, pp.DismantleQty);
                    ws.SetValue(1, 13, pp.PosDismantlePassQty);
                    ws.SetValue(1, 14, pp.PosDismantleFailQty);
                }
                else
                {
                    ws.SetValue(1, 10, pp.PosDismantlePassQty);
                    ws.SetValue(1, 11, pp.PosDismantleFailQty);
                }
            }

            //Create header
            ws.InsertRow(1, 1);
            ws.SetValue(1, 1, "Work Order Number");
            ws.SetValue(1, 2, "Product Reference");
            ws.SetValue(1, 3, "Target Quantity");
            ws.SetValue(1, 4, "Date & Time");
            ws.SetValue(1, 5, "Front Line");
            ws.SetValue(1, 6, "Sequence Name");
            ws.SetValue(1, 7, "Machine");
            ws.SetValue(1, 8, "Generated Qty");
            ws.SetValue(1, 9, "Processed Qty");

            ws.SetValue(1, 10, "Pass Qty");
            ws.SetValue(1, 11, "Fail Qty");
            if (isAdmin)
            {
                ws.SetValue(1, 12, "Dismantled Qty");
                ws.SetValue(1, 13, "Pos Dismantle Pass Qty");
                ws.SetValue(1, 14, "Pos Dismantle Fail Qty");
            }

            var j = isAdmin ? 14 : 11;

            using (var rng = ws.Cells[1, 1, 1, j])
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

        public static string ExportProduct(List<Product> products, List<ProductSequence> sequences, List<ProductSequenceItem>  sequenceItems)
        {
            sequences.Reverse();
            products.Reverse();
            sequenceItems.Reverse();

            var t = DateTime.Now;
            var xlsx = "TraceabilityReference_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
            var fileName = HostingEnvironment.ApplicationPhysicalPath + "/UploadedFiles/xlsx/" + xlsx;
            var newFile = new FileInfo(fileName);
            if (newFile.Exists)
            {
                newFile.Delete();// ensures we create a new workbook
                newFile = new FileInfo(fileName);
            }
            var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Product Reference");

            foreach (Product pp in products)
            {
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, pp.Reference);
                ws.SetValue(1, 2, pp.ArticleNumber);
                ws.SetValue(1, 3, pp.SequenceId);
            }

            //Create header
            ws.InsertRow(1, 1);
            ws.SetValue(1, 1, "Reference");
            ws.SetValue(1, 2, "Article Number");
            ws.SetValue(1, 3, "Product Sequence Number");

            using (var rng = ws.Cells[1, 1, 1, 3])
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

            var wsSequence = package.Workbook.Worksheets.Add("Sequences");

            foreach (ProductSequence pp in sequences)
            {
                wsSequence.InsertRow(1, 1);
                wsSequence.SetValue(1, 1, pp.Id);
                wsSequence.SetValue(1, 2, pp.Name);
            }
            //Create header
            wsSequence.InsertRow(1, 1);
            wsSequence.SetValue(1, 1, "Sequence Id");
            wsSequence.SetValue(1, 2, "Sequence Name");
            using (var rng = wsSequence.Cells[1, 1, 1, 2])
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


            var wsSequenceItem = package.Workbook.Worksheets.Add("Sequence Items");
            foreach (ProductSequence pps in sequences)
            {
                foreach (ProductSequenceItem pp in sequenceItems)
                {
                    if (pps.Id == pp.ProductSequenceId)
                    {
                        wsSequenceItem.InsertRow(1, 1);
                        wsSequenceItem.SetValue(1, 1, pp.Level);
                        wsSequenceItem.SetValue(1, 2, pp.MachineFamily.Name);
                    }
                }
                //Create header
                wsSequenceItem.InsertRow(1, 1);
                wsSequenceItem.SetValue(1, 1, "Sequence Item Level");
                wsSequenceItem.SetValue(1, 2, "Machine Family Name");


                using (var rng = wsSequenceItem.Cells[1, 1, 1, 2])
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

                wsSequenceItem.InsertRow(1, 1);
                wsSequenceItem.SetValue(1, 1, pps.Name + "("+pps.Id+")");
                using (var rng = wsSequenceItem.Cells[1, 1, 1, 2])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.Black);
                    rng.Style.WrapText = false;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.Green);
                    rng.AutoFitColumns();
                }

                wsSequenceItem.InsertRow(1, 1);
            }

            package.SaveAs(newFile);
            return xlsx;
        }

       
    }
}