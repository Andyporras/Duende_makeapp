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
    public class TagsController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;

        public TagsController(DuendeappContext context, Usuario usuario)
        {
            _usuario = usuario;
            _context = context;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {

            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            ViewBag.Tags = _context.Tags.Include(t => t.Imagens).ToList();
            return _context.Tags != null ?
                        View(await _context.Tags.ToListAsync()) :
                        Problem("Entity set 'DuendeappContext.Tags'  is null.");
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.Include(t => t.Imagens)
                .FirstOrDefaultAsync(m => m.TagId == id);
            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Tags/Create
        public IActionResult Create()
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagId,Nombre")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TagId,Nombre")] Tag tag)
        {
            //if (id != tag.TagId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.TagId))
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
            return View(tag);
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .FirstOrDefaultAsync(m => m.TagId == id);
            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tags == null)
            {
                return Problem("Entity set 'DuendeappContext.Tags'  is null.");
            }
            var tag = await _context.Tags.Include(t => t.Imagens).FirstOrDefaultAsync(t => t.TagId == id);
            if (tag != null)
            {
                // quitar las imagenes asociadas al tag pero no eliminarlas de la base de datos
                foreach (Imagen imagen in tag.Imagens)
                {
                    imagen.Tags.Remove(tag);
                }
                _context.Tags.Remove(tag);
                }

            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return (_context.Tags?.Any(e => e.TagId == id)).GetValueOrDefault();
        }
    }
}
