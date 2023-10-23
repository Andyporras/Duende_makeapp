using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace duendeMakeApp.Controllers
{
    public class MaquillajesController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;
        private readonly IHttpClientFactory _clientFactory;
            
        public MaquillajesController(DuendeappContext context, Usuario usuario, IHttpClientFactory clientFactory)
        {
            _usuario = usuario;
            _usuario.UsuarioId = 0;
            _usuario.TipoId = 2;
            _context = context;
            _clientFactory = clientFactory;
        }

        // GET: Maquillajes
        public async Task<IActionResult> Index(int usuarioId)
        {
            ViewBag.usuario = UsuariosController.GetSessionUser(_context);
            ViewBag.tags = _context.Tags;
            var maquillajes = _context.Maquillajes.Include(m => m.Imagens).ThenInclude(i => i.Tags).ToList();
            return maquillajes != null ? 
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

        // POST: Maquillajes/Create
        [HttpPost, ActionName("CreateIndex")]
        public IActionResult Create(int usuarioId)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.usuario = usuario;
            ViewBag.Tags = _context.Tags.ToList();
            return View("Create");
        }


        // POST: Maquillajes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre, Descripcion, Estado")] Maquillaje maquillaje, List<int> TagIds, List<IFormFile> Imagens)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maquillaje);
                // Guarda las imágenes
                if (Imagens != null && Imagens.Any())
                {
                    // guardar imagen ImagensController tiene la funcion private async Create(string Nombre, string Descripcion, IFormFile imageFile)
                    foreach (var imageFile in Imagens)
                    {
                        ImgurController imgurController = ImgurController.GetInstance(_clientFactory);
                        string imgurImageUrl = await imgurController.SubirImagen(imageFile);
                        if (imgurImageUrl != null)
                        {
                            Imagen imagen = new Imagen();
                            imagen.Url = imgurImageUrl;
                            imagen.Nombre = maquillaje.Nombre;
                            imagen.Descripcion = maquillaje.Descripcion;
                            _context.Imagens.Add(imagen);
                            await _context.SaveChangesAsync();

                            int idImg = ImagenesController.buscarImagenXurl(imgurImageUrl, _context);
                            if (idImg != 0)
                            {
                                maquillaje.Imagens.Add(_context.Imagens.Find(idImg));
                            }

                            foreach (int tagId in TagIds)
                            {
                                Tag tag = _context.Tags.Find(tagId);
                                if (tag != null)
                                {
                                    imagen.Tags.Add(tag);
                                }
                            }
                        }
                    }
                }

                // Guarda el maquillaje en la base de datos
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores de validación, vuelve a la vista de creación
            ViewBag.Tags = _context.Tags.ToList();
            return View(maquillaje);
        }


        // GET: Maquillajes/Edit/5
        public async Task<IActionResult> Edit(int? id, int usuarioId)
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
            Usuario usuario = UsuariosController.GetUsuario(usuarioId, _context);
            ViewBag.usuario = usuario;
            ViewBag.Tags = _context.Tags.ToList();
            return View(maquillaje);
        }



        // POST: Maquillajes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idUsuario, [Bind("MaquillajeId,Nombre,Descripcion,Estado")] Maquillaje maquillaje, List<int> TagIds, List<IFormFile> Imagens)
        {
            Console.WriteLine("id: " + idUsuario);
            Console.WriteLine("MaquillajeId: " + maquillaje.MaquillajeId);
            Usuario usuario = UsuariosController.GetUsuario(idUsuario, _context);
            if (ModelState.IsValid)
            {
                Maquillaje maquillajeRegistrado = _context.Maquillajes.Find(maquillaje.MaquillajeId);
                if (maquillajeRegistrado == null)
                {
                    return NotFound();
                }

                maquillajeRegistrado.Nombre = maquillaje.Nombre;
                maquillajeRegistrado.Descripcion = maquillaje.Descripcion;
                //maquillajeRegistrado.Estado = maquillaje.Estado;

                // Guarda las imágenes
                // guardar imagen ImagensController tiene la funcion private async Create(string Nombre, string Descripcion, IFormFile imageFile)
                foreach (var imageFile in Imagens)
                {
                    ImgurController imgurController = ImgurController.GetInstance(_clientFactory);
                    string imgurImageUrl = await imgurController.SubirImagen(imageFile);
                    if (imgurImageUrl != null)
                    {
                        Imagen imagen = new Imagen();
                        imagen.Url = imgurImageUrl;
                        imagen.Nombre = maquillaje.Nombre;
                        imagen.Descripcion = maquillaje.Descripcion;
                        _context.Imagens.Add(imagen);
                        await _context.SaveChangesAsync();

                        //int idImg = ImagenesController.buscarImagenXurl(imgurImageUrl, _context);
                        int idImg = _context.Imagens.FirstOrDefault(i => i.Url == imgurImageUrl).ImagenId;

                        if (idImg != 0)
                        {
                            maquillaje.Imagens.Add(_context.Imagens.Find(idImg));
                        }

                        foreach (int tagId in TagIds)
                        {
                            Tag tag = _context.Tags.Find(tagId);
                            if (tag != null)
                            {
                                imagen.Tags.Add(tag);
                            }
                        }

                        maquillajeRegistrado.Imagens.Add(imagen);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), usuario);
            //return View(maquillaje);
        }

        // GET: Maquillajes/Delete/5
        public async Task<IActionResult> Delete(int? id, int usuarioId)
        {
            if (id == null || _context.Maquillajes == null)
            {
                return NotFound();
            }

            Maquillaje maquillaje = _context.Maquillajes.Include(m => m.Imagens).ThenInclude(i => i.Tags).FirstOrDefault(m => m.MaquillajeId == id);
            if (maquillaje == null)
            {
                return NotFound();
            }
            Usuario usuario = UsuariosController.GetUsuario(usuarioId, _context);
            ViewBag.usuario = usuario;
            ViewBag.Tags = _context.Tags.ToList();

            return View(maquillaje);
        }

        // POST: Maquillajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int usuarioId)
        {
            Usuario usuario = UsuariosController.GetUsuario(usuarioId, _context);
            ViewBag.usuario = usuario;
            ViewBag.Tags = _context.Tags.ToList();
            Console.WriteLine("id: " + id);
            Console.WriteLine("idUsuario: " + usuarioId);
            if (_context.Maquillajes == null)
            {
                return Problem("Entity set 'DuendeappContext.Maquillajes'  is null.");
            }

            Maquillaje maquillaje = _context.Maquillajes.Include(m => m.Imagens).FirstOrDefault(m => m.MaquillajeId == id);

            if (maquillaje != null)
            {
                // Hacer una copia de la lista de imágenes
                List<Imagen> imagenesACopiar = maquillaje.Imagens.ToList();

                // Iterar sobre la copia de la lista de imágenes y eliminarlas del maquillaje
                foreach (Imagen imagen in imagenesACopiar)
                {
                    maquillaje.Imagens.Remove(imagen);
                }

                _context.Maquillajes.Remove(maquillaje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), usuario);
        }

        private bool MaquillajeExists(int id)
        {
          return (_context.Maquillajes?.Any(e => e.MaquillajeId == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> Filtrar(int usuarioId,  string selectedTag)
        {
            Usuario usuario = UsuariosController.GetUsuario(usuarioId, _context);
            if (usuario == null)
            {
                ViewBag.usuario = new Usuario();
            }
            else
            {
                ViewBag.usuario = usuario;
            }
            ViewBag.tags = _context.Tags;
            //obtener maquillajes filtrados por tag
            Console.WriteLine("selectedTag: " + selectedTag);
            if (selectedTag != null || selectedTag == "Todas")
            {
                ViewBag.selectedTag = selectedTag;
                var maquillajes = _context.Maquillajes.Include(m => m.Imagens).ThenInclude(i => i.Tags).ToList();
                var maquillajesFiltrados = maquillajes.Where(m => m.Imagens.Any(i => i.Tags.Any(t => t.Nombre == selectedTag))).ToList();
                
                return View("Index", maquillajesFiltrados);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
