using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterProject.Data;
using TwitterProject.Models;
using TwitterProject.Models.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace TwitterProject.Controllers
{
    public class UserController : Controller

    {

        private readonly IHttpContextAccessor contxt;

        private readonly MVCDbContext mvcDemoDbContext;

        public UserController(MVCDbContext mvcDemoDbContext, IHttpContextAccessor contxtA)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
            contxt = contxtA;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userClick)
        {
            if(userClick != null)
            {

                var tweetsp = await mvcDemoDbContext.Tweets
                    .Where(tweet => tweet.UserName == userClick)
                    .ToListAsync();

                return View(tweetsp);
            }

            var originaluser = contxt.HttpContext.Session.GetString("username");


            var tweets = await mvcDemoDbContext.Tweets
                .Where(tweet => tweet.UserName == originaluser)
                .ToListAsync();

            return View(tweets);
        }

        [HttpPost]
        public IActionResult GoToView(string userClick)
        {
            // Redirect to the Index action of the UserController and pass the value as a query parameter
            return RedirectToAction("Index", "User", new { userClick });
        }

    }
}
