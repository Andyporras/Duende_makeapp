using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;

namespace duendeMakeApp.Controllers
{
    public class CatalogosController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CatalogosController(DuendeappContext context, Usuario usuario, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _usuario = usuario;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Catalogos
        public async Task<IActionResult> Index()
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
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
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
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
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            return View(catalogo);
        }

        // GET: Catalogos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            if (id == null || _context.Catalogos == null)
            {
                return NotFound();
            }

            var catalogo = await _context.Catalogos
                .Include(c => c.Productos)
                .Include(c => c.Paquetes)
                .FirstOrDefaultAsync(c => c.CatalogoId == id);
            
            ViewBag.productos = _context.Productos;
            ViewBag.paquetes = _context.Paquetes;

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
        public async Task<IActionResult> Edit(int CatalogoId, [Bind("CatalogoId,Nombre,Descripcion,Estado")] Catalogo catalogo, List<int> ProductosIds, List<int> PaquetesIds)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            if (CatalogoId != catalogo.CatalogoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Catalogo? catalagoRegistrado = await _context.Catalogos
                        .Include(c => c.Productos)
                        .Include(c => c.Paquetes)
                        .FirstOrDefaultAsync(c => c.CatalogoId == CatalogoId);
                    
                    catalagoRegistrado.Nombre = catalogo.Nombre;
                    catalagoRegistrado.Descripcion = catalogo.Descripcion;
                    catalagoRegistrado.Estado = catalogo.Estado;

                    if (catalagoRegistrado != null)
                    {
                        // Hacer copias de las listas de productos y paquetes
                        List<Producto> productosACopiar = catalagoRegistrado.Productos.ToList();
                        List<Paquete> paquetesACopiar = catalagoRegistrado.Paquetes.ToList();

                        // Iterar sobre las copias de las listas y eliminar los elementos de las copias
                        foreach (Producto producto in productosACopiar)
                        {
                            catalagoRegistrado.Productos.Remove(producto);
                        }

                        foreach (Paquete paquete in paquetesACopiar)
                        {
                            catalagoRegistrado.Paquetes.Remove(paquete);
                        }
                    }
                    await _context.SaveChangesAsync();

                    //asociar los productos y paquetes
                    foreach (int id in ProductosIds)
                    {
                        Producto? producto = _context.Productos.Find(id);
                        if (producto != null)
                        {
                            catalagoRegistrado.Productos.Add(producto);
                        }
                    }
                    
                    foreach (int id in PaquetesIds)
                    {
                        Paquete? paquete = _context.Paquetes.Find(id);
                        if (paquete != null)
                        {
                            catalagoRegistrado.Paquetes.Add(paquete);
                        }
                    }
                    //_context.Entry(catalogo).State = EntityState.Modified;
                    //_context.Entry(catalogo).Collection(c => c.Productos).IsModified = true;
                    //_context.Entry(catalogo).Collection(c => c.Paquetes).IsModified = true;
                    _context.Update(catalagoRegistrado);
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
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);

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
            var catalogo = await _context.Catalogos
                .Include(c => c.Productos)
                .Include(c => c.Paquetes)
                .FirstOrDefaultAsync(c => c.CatalogoId == id);

            if (catalogo != null)
            {
                // Hacer copias de las listas de productos y paquetes
                List<Producto> productosACopiar = catalogo.Productos.ToList();
                List<Paquete> paquetesACopiar = catalogo.Paquetes.ToList();

                // Iterar sobre las copias de las listas y eliminar los elementos de las copias
                foreach (Producto producto in productosACopiar)
                {
                    catalogo.Productos.Remove(producto);
                }

                foreach (Paquete paquete in paquetesACopiar)
                {
                    catalogo.Paquetes.Remove(paquete);
                }
            }

            await _context.SaveChangesAsync();



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
