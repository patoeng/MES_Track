using System.Collections.Generic;
using System.Web.Mvc;
using MES.Models;

namespace MES.Mvc.Models
{
    public class ProductViewModels
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> AllSequences { get; set; }
    }
}