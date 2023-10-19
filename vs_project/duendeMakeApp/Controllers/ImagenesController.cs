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
              return _context.Imagens != null ? 
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

            var imagen = await _context.Imagens
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

            var imagen = await _context.Imagens.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ImagenId,Nombre,Descripcion,Url")] Imagen imagen)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagen);
                    await _context.SaveChangesAsync();
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
            var imagen = await _context.Imagens.FindAsync(id);
            if (imagen != null)
            {
                _context.Imagens.Remove(imagen);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagenExists(int id)
        {
          return (_context.Imagens?.Any(e => e.ImagenId == id)).GetValueOrDefault();
        }
    }
}
