using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterProject.Data;
using TwitterProject.Models;
using TwitterProject.Models.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace TwitterProject.Controllers
{
    public class TweetController : Controller
    {

        private readonly IHttpContextAccessor contxt;

        private readonly MVCDbContext mvcDemoDbContext;

        public TweetController(MVCDbContext mvcDemoDbContext, IHttpContextAccessor contxtA)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
            contxt = contxtA;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tweets = await mvcDemoDbContext.Tweets.ToListAsync();
            return View(tweets);

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTweetViewModel addTweet)
        {
            var tweet = new Tweet()
            {
                Id = Guid.NewGuid(),
                Description = addTweet.Description,
                Upvote = addTweet.Upvote,
                Liked = addTweet.Liked,
                UserName = addTweet.UserName

            };

            await mvcDemoDbContext.Tweets.AddAsync(tweet);

            await mvcDemoDbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> Like()
        {
            var idValue = Request.Form["Id"];

            var likedValue = Request.Form["Liked"];

            var user = Request.Form["Username"];

            var originaluser = contxt.HttpContext.Session.GetString("username");

            if(originaluser != user)
            {
                if (Guid.TryParse(idValue, out Guid tweetId))
                {
                    // Fetch the tweet with the given GUID ID from your database
                    var tweet = await mvcDemoDbContext.Tweets.FindAsync(tweetId);

                    if (tweet != null)
                    {
                        // Increment the upvote count and set Liked to "true"
                        tweet.Upvote++;

                        mvcDemoDbContext.Update(tweet);

                        // Save the changes to the database
                        await mvcDemoDbContext.SaveChangesAsync();

                    }

                }
            }
            else
            {
                TempData["ErrorMessage"] = "You can't like your own tweet!";
            }




            return RedirectToAction("Index");
    
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            // Get the "Id" value from the form
            var idValue = Request.Form["Id"];

            if (Guid.TryParse(idValue, out Guid tweetId))
            {
                // Find the tweet by ID
                var tweet = await mvcDemoDbContext.Tweets.FindAsync(tweetId);

                if (tweet != null)
                {
                    // Remove the tweet from the database
                    mvcDemoDbContext.Tweets.Remove(tweet);
                    await mvcDemoDbContext.SaveChangesAsync();
                }
            }

            // Redirect to a suitable action (e.g., Index or a different page)
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SignupControl(UserLoginModel user)
        {
            // Check if a user with the same username already exists
            var existingUser = await mvcDemoDbContext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (existingUser != null)
            {
                // A user with the same username already exists
                ModelState.AddModelError("UserName", "Username is already taken.");
                return View("Signup");
            }

            if (user.Password != user.ConfirmPassword)
            {
                // Passwords do not match
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View("Signup");
            }

            var userRegister = new User()
            {
                Id = Guid.NewGuid(),
                UserName = user.UserName,
                Password = user.Password
            };

            contxt.HttpContext.Session.SetString("username", user.UserName);

            await mvcDemoDbContext.Users.AddAsync(userRegister);
            await mvcDemoDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginControl(UserLoginModel user)
        {
            // Check if a user with the same username already exists
            var existingUser = await mvcDemoDbContext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);


            if (existingUser != null)
            {
                contxt.HttpContext.Session.SetString("username", user.UserName);

                return RedirectToAction("Index");
            }

            

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Comment()
        {
            var tweetSlctd = contxt.HttpContext.Session.GetString("selectedTweet");

            if (tweetSlctd != null && Guid.TryParse(tweetSlctd, out Guid tweetGuid))
            {
                // Retrieve all comments associated with the selected tweet
                var comments = await mvcDemoDbContext.Comments
                    .Where(comment => comment.TweetId == tweetSlctd)
                    .ToListAsync();

                return View(comments);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> ViewComment()
        {
            var tweetid = Request.Form["TId"];

            contxt.HttpContext.Session.SetString("selectedTweet", tweetid);


            return RedirectToAction("Comment");
        }

        [HttpPost]
        public async Task<IActionResult> DoComment()
        {
            var tweetid = Request.Form["TId"];

            // Get the value of the "Title" input field
            var commentTitle = Request.Form["Title"];

            if (commentTitle == "")
            {
                return RedirectToAction("Comment");
            }

            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                Title = commentTitle,
                UserName = contxt.HttpContext.Session.GetString("username"),
                TweetId = tweetid
            };

            await mvcDemoDbContext.Comments.AddAsync(comment);

            await mvcDemoDbContext.SaveChangesAsync();

            TempData["ConfirmMessage"] = "Comment added!";

            return RedirectToAction("Comment");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("username");

            return RedirectToAction("Signup");
        }



    }





}
