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
    public class EnviosController : Controller
    {
        private readonly DuendeappContext _context;

        public EnviosController(DuendeappContext context)
        {
            _context = context;
        }

        // GET: Envios
        public async Task<IActionResult> Index()
        {
            var duendeappContext = _context.Envios.Include(e => e.Carrito).Include(e => e.Direccion).Include(e => e.Estado);
            return View(await duendeappContext.ToListAsync());
        }

        // GET: Envios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Envios == null)
            {
                return NotFound();
            }

            var envio = await _context.Envios
                .Include(e => e.Carrito)
                .Include(e => e.Direccion)
                .Include(e => e.Estado)
                .FirstOrDefaultAsync(m => m.EnvioId == id);
            if (envio == null)
            {
                return NotFound();
            }

            return View(envio);
        }

        // GET: Envios/Create
        public IActionResult Create()
        {
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId");
            ViewData["DireccionId"] = new SelectList(_context.Direccions, "DireccionId", "DireccionId");
            ViewData["EstadoId"] = new SelectList(_context.EstadoEnvios, "EstadoId", "EstadoId");
            return View();
        }

        // POST: Envios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnvioId,FechaPedido,FechaEntrega,EstadoId,CarritoId,DireccionId")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(envio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", envio.CarritoId);
            ViewData["DireccionId"] = new SelectList(_context.Direccions, "DireccionId", "DireccionId", envio.DireccionId);
            ViewData["EstadoId"] = new SelectList(_context.EstadoEnvios, "EstadoId", "EstadoId", envio.EstadoId);
            return View(envio);
        }

        // GET: Envios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Envios == null)
            {
                return NotFound();
            }

            var envio = await _context.Envios.FindAsync(id);
            if (envio == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", envio.CarritoId);
            ViewData["DireccionId"] = new SelectList(_context.Direccions, "DireccionId", "DireccionId", envio.DireccionId);
            ViewData["EstadoId"] = new SelectList(_context.EstadoEnvios, "EstadoId", "EstadoId", envio.EstadoId);
            return View(envio);
        }

        // POST: Envios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnvioId,FechaPedido,FechaEntrega,EstadoId,CarritoId,DireccionId")] Envio envio)
        {
            if (id != envio.EnvioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(envio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnvioExists(envio.EnvioId))
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
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", envio.CarritoId);
            ViewData["DireccionId"] = new SelectList(_context.Direccions, "DireccionId", "DireccionId", envio.DireccionId);
            ViewData["EstadoId"] = new SelectList(_context.EstadoEnvios, "EstadoId", "EstadoId", envio.EstadoId);
            return View(envio);
        }

        // GET: Envios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Envios == null)
            {
                return NotFound();
            }

            var envio = await _context.Envios
                .Include(e => e.Carrito)
                .Include(e => e.Direccion)
                .Include(e => e.Estado)
                .FirstOrDefaultAsync(m => m.EnvioId == id);
            if (envio == null)
            {
                return NotFound();
            }

            return View(envio);
        }

        // POST: Envios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Envios == null)
            {
                return Problem("Entity set 'DuendeappContext.Envios'  is null.");
            }
            var envio = await _context.Envios.FindAsync(id);
            if (envio != null)
            {
                _context.Envios.Remove(envio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnvioExists(int id)
        {
          return (_context.Envios?.Any(e => e.EnvioId == id)).GetValueOrDefault();
        }
    }
}
