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
            var cis2055nemesysContext = _context.Users.Include(u => u.Role);
            return View(await cis2055nemesysContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
                    user.Password = HashPassword(user);
                     _context.Add(user);
                    await _context.SaveChangesAsync();
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
                    user.Password = HashPassword(user);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


        //Hashes Password and returns it back
        private string HashPassword(User user)
        {
            byte[] salt = Encoding.ASCII.GetBytes(user.Email);
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(salt);
            //}


            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
