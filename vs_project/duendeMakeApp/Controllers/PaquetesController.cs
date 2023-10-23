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
    public class PaquetesController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;

        public PaquetesController(DuendeappContext context, Usuario usuario)
        {
            _usuario = usuario;
            _context = context;
        }

        // GET: Paquetes
        public async Task<IActionResult> Index()
        {
            String correo = Usuario.SeccionActual;
            if (correo != "")
            {
                _usuario = _context.Usuarios.Where(u => u.Correo == correo).FirstOrDefault();
            }
            else
            {
                _usuario = null;
            }

            ViewBag.Usuario = _usuario;
            return _context.Paquetes != null ? 
                          View(await _context.Paquetes.ToListAsync()) :
                          Problem("Entity set 'DuendeappContext.Paquetes'  is null.");
        }

        // GET: Paquetes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Paquetes == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes
                .FirstOrDefaultAsync(m => m.PaqueteId == id);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // GET: Paquetes/Create
        public IActionResult Create()
        {
            String correo = Usuario.SeccionActual;
            _usuario = _context.Usuarios.Where(u => u.Correo == correo).FirstOrDefault();
            ViewBag.Usuario = _usuario;
            return View();
        }

        // POST: Paquetes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaqueteId,Nombre,Descripcion,Precio,CantidadDisponible,Estado")] Paquete paquete)
        {
            String correo = Usuario.SeccionActual;
            _usuario = _context.Usuarios.Where(u => u.Correo == correo).FirstOrDefault();
            ViewBag.Usuario = _usuario;
            if (ModelState.IsValid)
            {
                _context.Add(paquete);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paquete);
        }

        // GET: Paquetes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            String correo = Usuario.SeccionActual;
            _usuario = _context.Usuarios.Where(u => u.Correo == correo).FirstOrDefault();
            ViewBag.Usuario = _usuario;
            if (id == null || _context.Paquetes == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete == null)
            {
                return NotFound();
            }
            return View(paquete);
        }

        // POST: Paquetes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaqueteId,Nombre,Descripcion,Precio,CantidadDisponible,Estado")] Paquete paquete)
        {
            if (id != paquete.PaqueteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paquete);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaqueteExists(paquete.PaqueteId))
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
            return View(paquete);
        }

        // GET: Paquetes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            String correo = Usuario.SeccionActual;
            _usuario = _context.Usuarios.Where(u => u.Correo == correo).FirstOrDefault();
            ViewBag.Usuario = _usuario;
            if (id == null || _context.Paquetes == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes
                .FirstOrDefaultAsync(m => m.PaqueteId == id);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // POST: Paquetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            String correo = Usuario.SeccionActual;
            _usuario = _context.Usuarios.Where(u => u.Correo == correo).FirstOrDefault();
            ViewBag.Usuario = _usuario;
            if (_context.Paquetes == null)
            {
                return Problem("Entity set 'DuendeappContext.Paquetes'  is null.");
            }
            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete != null)
            {
                _context.Paquetes.Remove(paquete);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaqueteExists(int id)
        {
          return (_context.Paquetes?.Any(e => e.PaqueteId == id)).GetValueOrDefault();
        }
    }
}
