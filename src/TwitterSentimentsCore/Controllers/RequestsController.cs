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
        public IActionResult Index2()
        {
            return View(db.Requests.ToList());
        }

        public IActionResult Index(string SortOrder)
        {
            // No sort method specified.
            //if (SortOrder == null) return View(db.Requests.ToList());

            var requests = from r in db.Requests select r;

            switch(SortOrder)
            {
                case "desc":
                    ViewData.Add("sort", "desc");
                    requests = requests.OrderByDescending(r => r.Result);
                    break;
                case "asc":
                    ViewData.Add("sort", "asc");
                    requests = requests.OrderBy(r => r.Result);
                    break; 
            }

            return View(requests.ToList());
        }

        // GET: /Requests/CreateUserRequest
        public IActionResult CreateUserRequest()
        {
            return View();
        }

        // GET: /Requests/CreateTopicRequest
        public IActionResult CreateTopicRequest()
        {
            return View();
        }

        // POST: /Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUserRequest(Request request)
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

                    // Error in the request
                    if(documents == null) return View("Error");

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

        // POST: /Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTopicRequest(Request request)
        {
            // Process, make API calls, add to DB
            if (ModelState.IsValid)
            {
                var wrapper = new CoreTweetWrapper();
                var manager = new RequestManager();

                var score = 0.0;

                // code changes here
                var tweetList = wrapper.GetTweetsByTopic(request.TwitterHandle).Result;

                //var tweetList = wrapper.GetUserTimeline(request.Search, request.Count).Result;
                var responses = manager.MakeRequest(tweetList.ToList()).Result;

                if (responses != null)
                {
                    var json = (JObject)JsonConvert.DeserializeObject(responses);
                    var documents = json.SelectToken("documents");

                    // Error in the request
                    if (documents == null) return View("Error");

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
