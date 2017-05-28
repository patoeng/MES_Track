using System.Web.Mvc;
using MES.Data;
using MES.Data.Migrations;

namespace MES.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            this.db = new MesData();
        }

        protected IMesData db { get; private set; }
    }
}