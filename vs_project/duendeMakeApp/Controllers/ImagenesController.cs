using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;


namespace duendeMakeApp.Controllers
{
    public class ImagenesController : Controller
    {
        private readonly DuendeappContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public ImagenesController(DuendeappContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        // GET: Imagenes
        public async Task<IActionResult> Index()
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            return _context.Imagens.Include(i => i.Tags).ToList() != null ?
                View(await _context.Imagens.ToListAsync()) :
                Problem("Entity set 'DuendeappContext.Imagens'  is null.");
        }

        // GET: Imagenes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Imagens == null)
            {
                return NotFound();
            }

            Imagen imagen = await _context.Imagens
                .Include(i => i.Tags)
                .Include(i => i.Productos)
                .Include(i => i.Venta)
                .Include(i => i.Maquillajes)
                .FirstOrDefaultAsync(m => m.ImagenId == id);
            if (imagen == null)
            {
                return NotFound();
            }

            return View(imagen);
        }

        // GET: Imagenes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Imagenes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Nombre, string Descripcion, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("imageFile", "Please select an image.");
                return View();
            }

            Imagen imagen = new Imagen();
            imagen.Nombre = Nombre;
            imagen.Descripcion = Descripcion;

            ImgurController imgurController = ImgurController.GetInstance(_clientFactory);
            string imgurImageUrl = await imgurController.SubirImagen(imageFile);

            if (!string.IsNullOrEmpty(imgurImageUrl))
            {
                imagen.Url = imgurImageUrl;

                if (ModelState.IsValid)
                {
                    _context.Add(imagen);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(imagen);
        }



        // GET: Imagenes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Imagens == null)
            {
                return NotFound();
            }

            var imagen = await _context.Imagens
                .Include(i => i.Maquillajes)
                .Include(i => i.Productos)
                .Include(i => i.Tags)
                .Include(i => i.Venta)
                .FirstOrDefaultAsync(m => m.ImagenId == id);

            ViewBag.Usuario = UsuariosController.GetSessionUser(_context);
            ViewBag.Productos = _context.Productos.ToList();
            ViewBag.Maquillajes = _context.Maquillajes.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (imagen == null)
            {
                return NotFound();
            }
            return View(imagen);
        }

        // POST: Imagenes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ImagenId, [Bind("ImagenId,Nombre,Descripcion,Url")] Imagen imagen, List<int> TagsIds, List<int> MaquillajesIds, List<int> ProductosIds)
        {
            if (ImagenId != imagen.ImagenId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener la imagen existente desde la base de datos (incluyendo relaciones)
                    var imagenExistente = _context.Imagens
                        .Include(i => i.Tags)
                        .Include(i => i.Maquillajes)
                        .Include(i => i.Productos)
                        .FirstOrDefault(i => i.ImagenId == ImagenId);

                    if (imagenExistente != null)
                    {
                        // Actualiza las propiedades de la imagen
                        imagenExistente.Nombre = imagen.Nombre;
                        imagenExistente.Descripcion = imagen.Descripcion;

                        // Actualiza las relaciones con tags
                        imagenExistente.Tags.Clear();
                        if (TagsIds != null)
                        {
                            foreach (var tagId in TagsIds)
                            {
                                var tag = _context.Tags.Find(tagId);
                                if (tag != null)
                                {
                                    imagenExistente.Tags.Add(tag);
                                }
                            }
                        }

                        // Actualiza las relaciones con maquillajes
                        imagenExistente.Maquillajes.Clear();
                        if (MaquillajesIds != null)
                        {
                            foreach (var maquillajeId in MaquillajesIds)
                            {
                                var maquillaje = _context.Maquillajes.Find(maquillajeId);
                                if (maquillaje != null)
                                {
                                    imagenExistente.Maquillajes.Add(maquillaje);
                                }
                            }
                        }

                        // Actualiza la relación con productos (un producto solo puede tener una imagen, pero la imagen puede estar en varios productos)
                        if (ProductosIds != null && ProductosIds.Count > 0)
                        {
                            foreach (var productoId in ProductosIds)
                            {
                                var producto = _context.Productos.Find(productoId);
                                if (producto != null)
                                {
                                    producto.Imagen = imagenExistente;
                                    producto.ImagenId = imagenExistente.ImagenId;
                                }
                            }
                        }
                        else
                        {
                            // Si no se selecciona un producto, desvincula la imagen de cualquier producto existente
                            var productos = _context.Productos.Where(p => p.ImagenId == imagenExistente.ImagenId);
                            foreach (var producto in productos)
                            {
                                producto.Imagen = null;
                                producto.ImagenId = null;
                            }
                        }

                        // Guarda los cambios en la base de datos
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagenExists(imagen.ImagenId))
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

            return View(imagen);
        }


        // GET: Imagenes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Imagens == null)
            {
                return NotFound();
            }

            var imagen = await _context.Imagens
                .FirstOrDefaultAsync(m => m.ImagenId == id);
            if (imagen == null)
            {
                return NotFound();
            }

            return View(imagen);
        }


        // POST: Imagenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Imagens == null)
            {
                return Problem("Entity set 'DuendeappContext.Imagens'  is null.");
            }
            Console.WriteLine("DeleteConfirmed: ", id);
            var imagen = await _context.Imagens
                .Include(i => i.Tags)
                .Include(i => i.Maquillajes)
                .Include(i => i.Productos)
                .FirstOrDefaultAsync(m => m.ImagenId == id);
            if (imagen != null)
            {
                //se eliminan las relaciones con tags
                imagen.Tags.Clear();
                await _context.SaveChangesAsync();
                //se eliminan las relaciones con maquillajes
                imagen.Maquillajes.Clear();
                await _context.SaveChangesAsync();
                //se eliminan las relaciones con productos
                imagen.Productos.Clear();
                //se elimina la imagen
                _context.Imagens.Remove(imagen);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagenExists(int id)
        {
          return (_context.Imagens?.Any(e => e.ImagenId == id)).GetValueOrDefault();
        }

        public static int buscarImagenXurl (string url, DuendeappContext context)
        {
            if (context.Imagens != null)
            {
                foreach (var imagen in context.Imagens)
                {
                    if (string.Compare(imagen.Url, url) == 0)
                    {
                        return imagen.ImagenId;
                    }
                }
            }
            return 0;
        }
    }
}
