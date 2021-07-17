using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Blog.Controllers {
    public class PostController : Controller {
        private readonly DatabaseContext _context;
        private string baseUrl = "https://sq1-api-test.herokuapp.com/";

        public PostController(DatabaseContext context) {
            _context = context;
        }



        [HttpGet, ActionName("Fetch")]
        public async Task<ActionResult> FetchExternalPosts() {
            List<Post> posts = new List<Post>();
            List<Post> updatedPosts = new List<Post>();
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("posts");

                if (Res.IsSuccessStatusCode) {
                    var postResponse = Res.Content.ReadAsStringAsync().Result;
                    posts = JsonConvert.DeserializeObject<Post>(postResponse).data.ToList();
                }
                posts = posts.Select(p => { p.User = "admin"; return p; }).ToList();
                var storedPosts = _context.Post.ToList();
                foreach (Post post in posts) {
                    var postExists = storedPosts.Any(p => p.Description == post.Description);
                    if (!postExists) {
                        _context.Add(post);
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Index(string user) {
            if (user != null && HttpContext.Session.GetString("LoggedIn") == "true") {
                return View(await _context.Post.Where(x => x.User == user.Trim()).ToListAsync());
            }

            if (HttpContext.Session.GetString("LoggedIn") == "true") {
                return View(await _context.Post.Where(x => x.User == HttpContext.Session.GetString("LoggedInAs").Trim()).ToListAsync());
            } else {
                return View();
            }
        }

        // GET: Post
        public async Task<IActionResult> Index() {
            if (HttpContext.Session.GetString("LoggedIn") == "true") {
                return View(await _context.Post.Where(x => x.User == HttpContext.Session.GetString("LoggedInAs").Trim()).ToListAsync());
            }
            return View(await _context.Post.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.PostID == id);
            if (post == null) {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create() {
            
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostID,Title,Description,PublicationDate,User")] Post post) {
            if (ModelState.IsValid) {
                Post updatedPost = new Post()
                {
                    User = post.User,
                    Description = post.Description,
                    Title = post.Title,
                    PublicationDate = DateTime.Now
                };
                _context.Add(updatedPost);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("LoggedIn", "true");
                HttpContext.Session.SetString("LoggedInAs", updatedPost.User.Trim());
                return RedirectToAction("Index", "Post", new { user = updatedPost.User });
            }
            return View(post);
        }


        private bool PostExists(int id) {
            return _context.Post.Any(e => e.PostID == id);
        }
    }
}
