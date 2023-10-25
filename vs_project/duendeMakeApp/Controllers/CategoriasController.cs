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
    public class CategoriasController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;

        public CategoriasController(DuendeappContext context, Usuario usuario)
        {
            _usuario = usuario;
            _context = context;
        }

        // GET: Categorias
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
            return _context.Categoria != null ? 
                          View(await _context.Categoria.ToListAsync()) :
                          Problem("Entity set 'DuendeappContext.Categoria'  is null.");
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categoria == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
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
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Usuario = _usuario;
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            if (id == null || _context.Categoria == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            //if (id != categoria.CategoriaId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.CategoriaId))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categoria == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categoria == null)
            {
                return Problem("Entity set 'DuendeappContext.Categoria'  is null.");
            }
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria != null)
            {
                _context.Categoria.Remove(categoria);
            }
            
            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
          return (_context.Categoria?.Any(e => e.CategoriaId == id)).GetValueOrDefault();
        }
    }
}
