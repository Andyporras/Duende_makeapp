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
    public class EstadoEnviosController : Controller
    {
        private readonly DuendeappContext _context;

        public EstadoEnviosController(DuendeappContext context)
        {
            _context = context;
        }

        // GET: EstadoEnvios
        public async Task<IActionResult> Index()
        {
              return _context.EstadoEnvios != null ? 
                          View(await _context.EstadoEnvios.ToListAsync()) :
                          Problem("Entity set 'DuendeappContext.EstadoEnvios'  is null.");
        }

        // GET: EstadoEnvios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EstadoEnvios == null)
            {
                return NotFound();
            }

            var estadoEnvio = await _context.EstadoEnvios
                .FirstOrDefaultAsync(m => m.EstadoId == id);
            if (estadoEnvio == null)
            {
                return NotFound();
            }

            return View(estadoEnvio);
        }

        // GET: EstadoEnvios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoEnvios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstadoId,Estado")] EstadoEnvio estadoEnvio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoEnvio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoEnvio);
        }

        // GET: EstadoEnvios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EstadoEnvios == null)
            {
                return NotFound();
            }

            var estadoEnvio = await _context.EstadoEnvios.FindAsync(id);
            if (estadoEnvio == null)
            {
                return NotFound();
            }
            return View(estadoEnvio);
        }

        // POST: EstadoEnvios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EstadoId,Estado")] EstadoEnvio estadoEnvio)
        {
            if (id != estadoEnvio.EstadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoEnvio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoEnvioExists(estadoEnvio.EstadoId))
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
            return View(estadoEnvio);
        }

        // GET: EstadoEnvios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EstadoEnvios == null)
            {
                return NotFound();
            }

            var estadoEnvio = await _context.EstadoEnvios
                .FirstOrDefaultAsync(m => m.EstadoId == id);
            if (estadoEnvio == null)
            {
                return NotFound();
            }

            return View(estadoEnvio);
        }

        // POST: EstadoEnvios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EstadoEnvios == null)
            {
                return Problem("Entity set 'DuendeappContext.EstadoEnvios'  is null.");
            }
            var estadoEnvio = await _context.EstadoEnvios.FindAsync(id);
            if (estadoEnvio != null)
            {
                _context.EstadoEnvios.Remove(estadoEnvio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoEnvioExists(int id)
        {
          return (_context.EstadoEnvios?.Any(e => e.EstadoId == id)).GetValueOrDefault();
        }
    }
}
