using Microsoft.AspNetCore.Mvc;
using TwitterSentimentsCore.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TwitterSentimentsCore.Controllers
{
    public class RequestsController : Controller
    {
        // GET: /Requests/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Requests/Create
        public IActionResult Create()
        {
            return null;
        }

        // POST: /Requests/Create
        [HttpPost]
        public IActionResult Create([Bind(include:"ID,TwitterHandle,Count,Result")] Request request)
        {
            return null;
        }

        //GET: /Requests/Details/5
        public IActionResult Details(int? id)
        {
            return null;
        }

        //GET: /Requests/Delete/5
        public IActionResult Delete(int? id)
        {
            return null;
        }

        //POST: /Requests/Delete/5
        public IActionResult Delete(int id)
        {
            return null;
        }

    }
}
