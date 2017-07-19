using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MES.Mvc.Models
{
    public class ExcelFileModel
    {
        [Required,FileExtensions(Extensions = "xlsx",
                ErrorMessage = "Specify an XLSX file.")]
        public HttpPostedFileBase File { get; set; }
    }
}