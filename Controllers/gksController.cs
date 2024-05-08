using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GKHub.Data;
using GKHub.Models;
using Microsoft.AspNetCore.Authorization;

namespace GKHub.Controllers
{
    public class gksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public gksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: gks
        public async Task<IActionResult> Index()
        {
            return View(await _context.gk.ToListAsync());
        }

        // POST: gks show search form
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }



        [Authorize]
        public async Task<IActionResult> JoinNow()
        {
            return View("Create");
        }


        // GET: gks show search result

        public async Task<IActionResult> ShowSearchResult(string SearchPhrase)
        {
            return View("Index", await _context.gk.Where( j => j.question.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: gks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gk = await _context.gk
                .FirstOrDefaultAsync(m => m.id == id);
            if (gk == null)
            {
                return NotFound();
            }

            return View(gk);
        }

        // GET: gks/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: gks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,question,answer")] gk gk)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gk);
        }

        // GET: gks/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gk = await _context.gk.FindAsync(id);
            if (gk == null)
            {
                return NotFound();
            }
            return View(gk);
        }

        // POST: gks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,question,answer")] gk gk)
        {
            if (id != gk.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!gkExists(gk.id))
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
            return View(gk);
        }

        // GET: gks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gk = await _context.gk
                .FirstOrDefaultAsync(m => m.id == id);
            if (gk == null)
            {
                return NotFound();
            }

            return View(gk);
        }

        // POST: gks/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gk = await _context.gk.FindAsync(id);
            if (gk != null)
            {
                _context.gk.Remove(gk);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool gkExists(int id)
        {
            return _context.gk.Any(e => e.id == id);
        }
    }
}
