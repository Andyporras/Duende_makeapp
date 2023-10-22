using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;

namespace duendeMakeApp.Controllers
{
    public class MaquillajesController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;

        public MaquillajesController(DuendeappContext context, Usuario usuario)
        {
            _usuario = usuario;
            _context = context;
        }

        // GET: Maquillajes
        public async Task<IActionResult> Index(Usuario usuario)
        {
            if (usuario != null)
            {
                _usuario = usuario;
            }

            ViewBag.usuario = _usuario;
            ViewBag.tags = _context.Tags;
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
            Console.WriteLine("id: " + id);
            Console.WriteLine("MaquillajeId: " + maquillaje.MaquillajeId);
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

        [HttpGet]
        public async Task<IActionResult> Filtrar(string selectedTag)
        {
            ViewBag.usuario = _usuario;
            ViewBag.tags = _context.Tags;
            //obtener maquillajes filtrados por tag
            Console.WriteLine("selectedTag: " + selectedTag);
            if (selectedTag != null || selectedTag == "Todas")
            {
                ViewBag.selectedTag = selectedTag;
                // hay que obtener las imagenes de los maquillajes y si al menos una tiene el tag seleccionado, se muestra
                var maquillajes = await _context.Maquillajes.ToListAsync();
                var maquillajesFiltrados = new List<Maquillaje>();
                foreach (var maquillaje in maquillajes)
                {
                    var imagenes = maquillaje.Imagens;
                    foreach (var imagen in imagenes)
                    {
                        var tags = await _context.Tags.ToListAsync();
                        foreach (var tag in tags)
                        {
                            if (tag.Nombre == selectedTag)
                            {
                                maquillajesFiltrados.Add(maquillaje);
                                break;
                            }
                        }
                    }
                }

                //retorna la vista index pero con el modelo de maquillajes filtrados
                return View("Index", maquillajesFiltrados);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
