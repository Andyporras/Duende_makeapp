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
    public class TipoUsuariosController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;

        public TipoUsuariosController(DuendeappContext context, Usuario usuario)
        {
            _usuario = usuario;
            _context = context;
        }

        // GET: TipoUsuarios
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
            return _context.TipoUsuarios != null ? 
                          View(await _context.TipoUsuarios.ToListAsync()) :
                          Problem("Entity set 'DuendeappContext.TipoUsuarios'  is null.");
        }

        // GET: TipoUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoUsuarios == null)
            {
                return NotFound();
            }

            var tipoUsuario = await _context.TipoUsuarios
                .FirstOrDefaultAsync(m => m.TipoUsarioId == id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return View(tipoUsuario);
        }

        // GET: TipoUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoUsarioId,Tipo")] TipoUsuario tipoUsuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoUsuario);
        }

        // GET: TipoUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipoUsuarios == null)
            {
                return NotFound();
            }

            var tipoUsuario = await _context.TipoUsuarios.FindAsync(id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }
            return View(tipoUsuario);
        }

        // POST: TipoUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoUsarioId,Tipo")] TipoUsuario tipoUsuario)
        {
            if (id != tipoUsuario.TipoUsarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoUsuarioExists(tipoUsuario.TipoUsarioId))
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
            return View(tipoUsuario);
        }

        // GET: TipoUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoUsuarios == null)
            {
                return NotFound();
            }

            var tipoUsuario = await _context.TipoUsuarios
                .FirstOrDefaultAsync(m => m.TipoUsarioId == id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return View(tipoUsuario);
        }

        // POST: TipoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoUsuarios == null)
            {
                return Problem("Entity set 'DuendeappContext.TipoUsuarios'  is null.");
            }
            var tipoUsuario = await _context.TipoUsuarios.FindAsync(id);
            if (tipoUsuario != null)
            {
                _context.TipoUsuarios.Remove(tipoUsuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoUsuarioExists(int id)
        {
          return (_context.TipoUsuarios?.Any(e => e.TipoUsarioId == id)).GetValueOrDefault();
        }
    }
}
