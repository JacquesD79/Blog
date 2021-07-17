using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using Microsoft.AspNetCore.Http;

namespace Blog.Controllers {
    public class UserProfileController : Controller {
        private readonly DatabaseContext _context;
        

        public UserProfileController(DatabaseContext context) {
            _context = context;
        }

        [HttpGet, ActionName("Login")]
        public ActionResult Login() {
            return View();
        }

        [HttpGet, ActionName("Logout")]
        public ActionResult Logout(UserProfile userProfile) {
            HttpContext.Session.SetString("LoggedIn", "false");
            HttpContext.Session.SetString("LoggedInAs", "nobody");
            userProfile.UserName = string.Empty;
            userProfile.Password = string.Empty;
            userProfile.UserID = 0;
            return RedirectToAction("Index", "Post");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserProfile userProfile) {
                var user = await _context.UserProfile.FirstOrDefaultAsync(x => x.UserName.Trim() == userProfile.UserName.Trim() && x.Password.Trim() == userProfile.Password.Trim());
                
                if (user.UserID == 0) {
                    HttpContext.Session.SetString("LoggedIn", "false");
                    HttpContext.Session.SetString("LoggedInAs", "nobody");
                    userProfile.UserName = string.Empty;
                    userProfile.Password = string.Empty;
                } else {
                    HttpContext.Session.SetString("LoggedIn", "true");
                    HttpContext.Session.SetString("LoggedInAs", user.UserName);
                }
            return RedirectToAction("Index", "Post", new { user = user.UserName });
            //return View("Login", userProfile);
        }

        // GET: UserProfile
        public async Task<IActionResult> Index() {
            return View(await _context.UserProfile.ToListAsync());
        }

        // GET: UserProfile/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (userProfile == null) {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfile/Create
        public IActionResult Create() {
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = string.Empty;
            userProfile.Password = string.Empty;
            return View(userProfile);
        }

        // POST: UserProfile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserName,Password,IsActive")] UserProfile userProfile) {
            if (ModelState.IsValid) {
                if (!_context.UserProfile.Any(x => x.UserName.Trim() == userProfile.UserName.Trim())) {
                    _context.Add(userProfile);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } else {
                    userProfile.UserName = string.Empty;
                    userProfile.Password = string.Empty;
                }
            }
            return View(userProfile);
        }

        // GET: UserProfile/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile == null) {
                return NotFound();
            }
            return View(userProfile);
        }

        // POST: UserProfile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserName,Password,IsActive")] UserProfile userProfile) {
            if (id != userProfile.UserID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!UserProfileExists(userProfile.UserID)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userProfile);
        }

        // GET: UserProfile/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (userProfile == null) {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var userProfile = await _context.UserProfile.FindAsync(id);
            _context.UserProfile.Remove(userProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileExists(int id) {
            return _context.UserProfile.Any(e => e.UserID == id);
        }
    }
}
