using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.Data;
using cis2055_NemesysProject.Controllers;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.AspNetCore.Http;
using cis2055_NemesysProject.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace cis2055_NemesysProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly cis2055nemesysContext _context;



        public UsersController(cis2055nemesysContext context)
        {
            _context = context;

        }



        // GET: Users
        public async Task<IActionResult> Index()
        {
            RetreiveSessionUser();

            var user = ViewBag.CurrentUser;

            if(user != null)
            {
                if (user.Role.RoleType == "Admin")
                {
                    var cis2055nemesysContext = _context.Users.Include(u => u.Role);
                    return View(await cis2055nemesysContext.ToListAsync());
                } else
                {
                    return RedirectToAction("Details", new { id = user.UserId});

                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            RetreiveSessionUser();

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                var dbUser = EmailExists(user.Email);

                if(dbUser != null)
                {
                    if(user.Password != null)
                    {
                        user.Password = HashPassword(user.Email, user.Password);

                        if (dbUser.Password == user.Password)
                        {
                            HttpContext.Session.SetObjectAsJson("UserLoggedIn", dbUser);
                            return RedirectToAction(nameof(Index));
                        }

                        else
                        {
                            ViewData["PasswordIncorrect"] = "Password is incorrect, please try again.";
                            return View(user);
                        }
                    }
                   
                    else
                    {
                        ViewData["NoPassword"] = "No Password found, please try again.";
                        return View(user);
                    }
                }
                else
                {
                    ViewData["EmailNotFound"] = "Could not find email address, try again.";
                    return View(user);
                }
            }
            return View(user.Email, user.Password);
        }

        public IActionResult Logout()
        {
            //Removes Session Called
            HttpContext.Session.Remove("UserLoggedIn");

            //Removes All Sessions currently Stored.
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();

            return RedirectToAction(nameof(Index));

        }

        // GET:  Users/Register but without Admin Role
        public async Task<IActionResult> Register()
        {


            var rolesController = new RolesController(_context);

            var abc = await rolesController.NoAdmin();
            ViewData["RoleId"] = new SelectList(abc, "RoleId", "RoleType");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,RoleId,Username,Email,Telephone,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //Checking if Email already exists in the Database and returns a User object
                var userEmail = await _context.Users
               .FirstOrDefaultAsync(m => m.Email == user.Email);

                //Checks if useremail object is empty
                if (userEmail == null)
                { 
                    user.Password = HashPassword(user.Email, user.Password);
                     _context.Add(user);
                    await _context.SaveChangesAsync();

                    user = EmailExists(user.Email);
                    HttpContext.Session.SetObjectAsJson("UserLoggedIn", user);
                    return RedirectToAction(nameof(Index));

                }
                //Checking if Email already exists in the Database
                else
                {
                    ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleType", user.RoleId);
                    ViewData["EmailExists"] = "A user with that Email Already Exists, try again!";
                    return View(user);
                }


            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleType", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            RetreiveSessionUser();

            if (id == null)
            {
                return NotFound();
            }


            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleType", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RoleId,Username,Email,Telephone,Password")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Password = HashPassword(user.Email, user.Password);
                    _context.Update(user);
                    await _context.SaveChangesAsync   ();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleType", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            RetreiveSessionUser();

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Methods that don't have direct interaction with the views
        /// </summary>

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        public void RetreiveSessionUser()
        {
            User currentUser = HttpContext.Session.GetObjectFromJson<User>("UserLoggedIn");
            if(currentUser != null)
            {
                ViewBag.CurrentUser = currentUser;
            }
            
        }

        private User EmailExists(string email)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(e => e.Email == email);
        }

        //Hashes Password and returns it back
        //Reference from microsoft hash passsword put LINKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK
        private string HashPassword(string Email, string Password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(Email);
          
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
