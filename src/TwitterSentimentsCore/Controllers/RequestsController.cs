using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using TwitterSentimentsCore.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TwitterSentimentsCore.Controllers
{
    public class RequestsController : Controller
    {
        public RequestDbContext db;

        public RequestsController(RequestDbContext context)
        {
            db = context;
        }

        // GET: /Requests/Index
        public IActionResult Index()
        {
            return View(db.Requests.ToList());
        }

        // GET: /Requests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Request request)
        {
            // Process, make API calls, add to DB
            if (ModelState.IsValid)
            {
                request.Result = 0.0;
                db.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        //GET: /Requests/Details/5
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return Content(HttpStatusCode.BadRequest.ToString());
            }

            Request request = db.Requests.Single(r => r.Id == id);

            if (request == null)
            {
                return View("Error");
                //return HttpNotFound();
            }
            return View(request);
        }

        //GET: /Requests/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Content(HttpStatusCode.BadRequest.ToString());
            }

            Request request = db.Requests.Single(r => r.Id == id);

            if (request == null)
            {
                return View("Error");
                //return HttpNotFound();
            }
            return View(request);
        }

        //POST: /Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Single(r => r.Id == id);
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
