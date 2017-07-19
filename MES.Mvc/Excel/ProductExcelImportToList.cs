
using System;
using System.Collections.Generic;
using System.IO;
using MES.Models;
using OfficeOpenXml;

namespace MES.Mvc.Excel
{
    public static class ProductExcelImportToList
    {
        public static  List<Product> Parse(string filePath)
        {


            var list  = new List<Product>();
            var newFile = new FileInfo(filePath);
            if (!newFile.Exists)
            {
                return list;
            }

            var package = new ExcelPackage(newFile);
             
            var ws = package.Workbook.Worksheets["Product Reference"];

            try
            {
                var reference = ws.Cells[1 , 1].Value.ToString();
                if (reference != "Reference")
                {
                    return list;
                }            
                var article = ws.Cells[1, 2].Value.ToString();
                if (article != "Article Number")
                {
                    return list;
                }
                var titleSeq = ws.Cells[1, 3].Value.ToString();
                if (titleSeq != "Product Sequence Number")
                {
                    return list;
                }
            }
            catch
            {
                return list;
            }

            var i = 0;
            var walker = 2;

            while (i == 0)
            {
                try
                {
                    var reference = ws.Cells[walker, 1].Value.ToString();
                    if (reference == "")
                    {
                        i++;
                        if (i > 1) return list;
                    }
                    else
                    {
                        i = 0;
                    }
                    var article = ws.Cells[walker, 2].Value.ToString();
                    var sequenceId = ws.Cells[walker, 3].Value.ToString()=="" ? 0: Convert.ToInt32(ws.Cells[walker, 3].Value.ToString());
                    list.Add(new Product {Id=walker-1, ArticleNumber = article, Reference = reference, SequenceId = sequenceId});
                }
                catch
                {
                    return list;
                }
                walker++;
            }
            return list;
        }
    }
}