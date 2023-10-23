using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;

namespace duendeMakeApp.Controllers
{
    public class CatalogosController : Controller
    {
        private readonly DuendeappContext _context;

        public CatalogosController(DuendeappContext context)
        {
            _context = context;
        }

        // GET: Catalogos
        public async Task<IActionResult> Index()
        {
              return _context.Catalogos != null ? 
                          View(await _context.Catalogos.ToListAsync()) :
                          Problem("Entity set 'DuendeappContext.Catalogos'  is null.");
        }

        // GET: Catalogos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Catalogos == null)
            {
                return NotFound();
            }

            var catalogo = await _context.Catalogos
                .FirstOrDefaultAsync(m => m.CatalogoId == id);
            if (catalogo == null)
            {
                return NotFound();
            }

            return View(catalogo);
        }

        // GET: Catalogos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogoId,Nombre,Descripcion,Estado")] Catalogo catalogo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catalogo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catalogo);
        }

        // GET: Catalogos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Catalogos == null)
            {
                return NotFound();
            }

            var catalogo = await _context.Catalogos.FindAsync(id);
            if (catalogo == null)
            {
                return NotFound();
            }
            return View(catalogo);
        }

        // POST: Catalogos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatalogoId,Nombre,Descripcion,Estado")] Catalogo catalogo)
        {
            if (id != catalogo.CatalogoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catalogo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogoExists(catalogo.CatalogoId))
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
            return View(catalogo);
        }

        // GET: Catalogos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Catalogos == null)
            {
                return NotFound();
            }

            var catalogo = await _context.Catalogos
                .FirstOrDefaultAsync(m => m.CatalogoId == id);
            if (catalogo == null)
            {
                return NotFound();
            }

            return View(catalogo);
        }

        // POST: Catalogos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Catalogos == null)
            {
                return Problem("Entity set 'DuendeappContext.Catalogos'  is null.");
            }
            var catalogo = await _context.Catalogos.FindAsync(id);
            if (catalogo != null)
            {
                _context.Catalogos.Remove(catalogo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogoExists(int id)
        {
          return (_context.Catalogos?.Any(e => e.CatalogoId == id)).GetValueOrDefault();
        }
    }
}
