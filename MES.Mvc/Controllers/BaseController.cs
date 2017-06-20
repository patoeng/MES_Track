using System.Web.Mvc;
using MES.Data;
using MES.Data.Migrations;



namespace MES.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            this.Db = new MesData();
            
        }

   
        protected IMesData Db { get; private set; }
    }
}