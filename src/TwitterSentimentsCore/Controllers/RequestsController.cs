using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using CoreTweet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwitterSentimentsCore.Models;
using TwitterSentimentsCore.Twitter;

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
            var thing = from req 
                        in db.Requests
                        where req.Count > 10
                        select new
                        {
                            req.Id,
                            req.TwitterHandle,
                            req.Result
                        };

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
                var wrapper = new CoreTweetWrapper();
                var manager = new RequestManager();

                var score = 0.0;

                var tweetList = wrapper.GetUserTimeline(request.TwitterHandle, request.Count).Result;
                var responses = manager.MakeRequest(tweetList.ToList()).Result;

                if (responses != null)
                {
                    var json = (JObject)JsonConvert.DeserializeObject(responses);
                    var documents = json.SelectToken("documents");

                    // Access each document and sum the score token
                    for (var i = 0; i < documents.Count(); i++)
                    {
                        var val = documents[i].SelectToken("score");
                        score += Convert.ToDouble(val.ToString());
                    }
                }

                request.Result = score / tweetList.Count;

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
