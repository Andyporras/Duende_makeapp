using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;

namespace duendeMakeApp.Controllers
{
    public class MaquillajesController : Controller
    {
        private readonly DuendeappContext _context;

        public MaquillajesController(DuendeappContext context)
        {
            _context = context;
        }

        // GET: Maquillajes
        public async Task<IActionResult> Index()
        {
              return _context.Maquillajes != null ? 
                          View(await _context.Maquillajes.ToListAsync()) :
                          Problem("Entity set 'DuendeappContext.Maquillajes'  is null.");
        }

        // GET: Maquillajes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Maquillajes == null)
            {
                return NotFound();
            }

            var maquillaje = await _context.Maquillajes
                .FirstOrDefaultAsync(m => m.MaquillajeId == id);
            if (maquillaje == null)
            {
                return NotFound();
            }

            return View(maquillaje);
        }

        // GET: Maquillajes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Maquillajes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaquillajeId,Nombre,Descripcion,Estado")] Maquillaje maquillaje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maquillaje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(maquillaje);
        }

        // GET: Maquillajes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Maquillajes == null)
            {
                return NotFound();
            }

            var maquillaje = await _context.Maquillajes.FindAsync(id);
            if (maquillaje == null)
            {
                return NotFound();
            }
            return View(maquillaje);
        }

        // POST: Maquillajes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaquillajeId,Nombre,Descripcion,Estado")] Maquillaje maquillaje)
        {
            if (id != maquillaje.MaquillajeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maquillaje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaquillajeExists(maquillaje.MaquillajeId))
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
            return View(maquillaje);
        }

        // GET: Maquillajes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Maquillajes == null)
            {
                return NotFound();
            }

            var maquillaje = await _context.Maquillajes
                .FirstOrDefaultAsync(m => m.MaquillajeId == id);
            if (maquillaje == null)
            {
                return NotFound();
            }

            return View(maquillaje);
        }

        // POST: Maquillajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Maquillajes == null)
            {
                return Problem("Entity set 'DuendeappContext.Maquillajes'  is null.");
            }
            var maquillaje = await _context.Maquillajes.FindAsync(id);
            if (maquillaje != null)
            {
                _context.Maquillajes.Remove(maquillaje);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaquillajeExists(int id)
        {
          return (_context.Maquillajes?.Any(e => e.MaquillajeId == id)).GetValueOrDefault();
        }
    }
}
