using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using duendeMakeApp.DAO;
using static System.Net.Mime.MediaTypeNames;

namespace duendeMakeApp.Controllers
{
    public class ProductosController : Controller
    {
        private readonly DuendeappContext _context;
        private List<Producto> productosCarrito = new List<Producto>();
        private static Usuario? _usuario;

        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private static string _context.Database.GetDbConnection().ConnectionString;// = "Data Source=DESKTOP-993UODJ; Initial Catalog=DUENDEAPP; Integrated Security=true; Encrypt=False;";

        public ProductosController(DuendeappContext context, Usuario usuario, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _usuario = usuario;
            _context = context;
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Productos
        public async Task<IActionResult> Index(int? categoriaId, List<int> subcategoriaIds)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            ViewBag.categorias = _context.Categoria;
            ViewBag.subcategorias = _context.Subcategoria;

            IQueryable<Producto> duendeappContext = _context.Productos.Include(p => p.Imagen);

            if (categoriaId.HasValue)
            {
                duendeappContext = duendeappContext.Where(p => p.CategoriaId == categoriaId);
            }

            if (subcategoriaIds != null && subcategoriaIds.Count > 0)
            {
                duendeappContext = duendeappContext.Where(p => p.Subcategoria.Any(s => subcategoriaIds.Contains(s.SubcategoriaId)));
            }


            return View(await duendeappContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "Nombre");
            ViewData["ImagenId"] = new SelectList(_context.Imagens, "ImagenId", "Nombre");

            ViewBag.Subcategoria = _context.Subcategoria.ToList();
                               
            return View("Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Descripcion,Precio,Cantidad,CategoriaId,Estado,ImagenId")] Producto producto, List<int> SubIds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                

                foreach (int subId in SubIds)
                {
                    Subcategoria sub = _context.Subcategoria.Find(subId);
                    if (sub != null)
                    {

                        producto.Subcategoria.Add(sub);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", producto.CategoriaId);
            ViewData["ImagenId"] = new SelectList(_context.Imagens, "ImagenId", "ImagenId", producto.ImagenId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "Nombre");
            ViewData["ImagenId"] = new SelectList(_context.Imagens, "ImagenId", "Nombre");
            ViewBag.Subcategoria = _context.Subcategoria.ToList();
            return View(producto);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Descripcion,Precio,Cantidad,CategoriaId,Estado,ImagenId")] Producto producto)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", producto.CategoriaId);
            ViewData["ImagenId"] = new SelectList(_context.Imagens, "ImagenId", "ImagenId", producto.ImagenId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'DuendeappContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
          return (_context.Productos?.Any(e => e.ProductoId == id)).GetValueOrDefault();
        }




        public IActionResult index3()
        {

            
            _usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context); 
            ViewBag.Usuario = _usuario;

            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);
            var oLista = new List<ProductoCarrito>();

            Console.WriteLine(carrito);
            using (SqlConnection conexion = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
    

            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerProductosEnCarrito", conexion);
                cmd.Parameters.AddWithValue("@CarritoID", carrito);
                cmd.Parameters.AddWithValue("@UsuarioID", _usuario.UsuarioId);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        oLista.Add(new ProductoCarrito()
                        {
                            ProductoId = (int)dr["ProductoID"]
,
                            Nombre = dr["Nombre"].ToString(),

                            Descripcion = dr["Descripcion"].ToString(),

                            Precio = (decimal)dr["Precio"],

                            Cantidad = (int?)dr["Cantidad"],

                            CategoriaId = (int)dr["CategoriaID"],
                            Estado = true,
                            ImagenId = (int)dr["ImagenID"],

                            url = (string)dr["Url"],
                        });

                    }
                }
            }

            return View(oLista);
        }


        public IActionResult agregarAlCarrito(int id)
        {
            _usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            ViewBag.Usuario = _usuario;

            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            try
            {
                using (SqlConnection conexion = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("AgregarProductoCarrito", conexion);


                    cmd.Parameters.AddWithValue("@idProducto", id);
                    cmd.Parameters.AddWithValue("@idCarrito", carrito);
                    cmd.Parameters.AddWithValue("@idCliente", _usuario.UsuarioId);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception e)
            {

                string error = e.Message;
                TempData["mensaje"] = "El producto ya se encuentra en el carrito.";
                return RedirectToAction("Index", "Productos");

            }
            TempData["mensaje"] = "Producto agregado al carrito exitosamente.";
            return RedirectToAction("index", "Productos");
        }



        public IActionResult eliminarDelCarrrito(int id)
        {
            _usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            ViewBag.Usuario = _usuario;

            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            try
            {
                using (SqlConnection conexion = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("eliminarDelCarrito", conexion);


                    cmd.Parameters.AddWithValue("@IDProducto", id);
                    cmd.Parameters.AddWithValue("@IDCarrito", carrito);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception e)
            {

                string error = e.Message;
                TempData["mensaje"] = "Ha sucedido un error inesperado";
                return RedirectToAction("index3", "Productos");

            }
            TempData["mensaje"] = "Producto eliminado del carrito exitosamente.";
            return RedirectToAction("index3", "Productos");
        }

        public int ObtenerCarritoPorUsuarioID(int usuarioID)
        {
            int carritoID = 0;
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);


            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT dbo.ObtenerCarritoPorUsuarioID(@UsuarioID)", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@UsuarioID", SqlDbType.Int) { Value = usuarioID });

                    carritoID = (int)command.ExecuteScalar();
                }
            }

            return carritoID;
        }
    


    public IActionResult SumarProducto(int id)
        {
            _usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            ViewBag.Usuario = _usuario;

            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            try
            {
                using (SqlConnection conexion = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("SumarCantidadProductos", conexion);


                    cmd.Parameters.AddWithValue("@IDProducto", id);
                    cmd.Parameters.AddWithValue("@IDCarrito", carrito);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception e)
            {

                string error = e.Message;
                TempData["mensaje"] = "Ha sucedido un error inesperado";
                return RedirectToAction("index3", "Productos");

            }
            return RedirectToAction("index3", "Productos");
        }



        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(string codPostal, int provincia, string dir, IFormFile imageFile)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            Imagen imagen = new Imagen();
            imagen.Nombre = "comprobante";
            imagen.Descripcion = dir;

            int idComprobante = imagen.ImagenId;
            ImgurController imgurController = ImgurController.GetInstance(_clientFactory);
            
            string imgurImageUrl = await imgurController.SubirImagen(imageFile);
            if (!string.IsNullOrEmpty(imgurImageUrl))
            {
                imagen.Url = imgurImageUrl;

                if (ModelState.IsValid)
                {
                    _context.Add(imagen);
                    await _context.SaveChangesAsync();
                }
            }

            try {
                using (SqlConnection conexion = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("concretarVenta", conexion);
                    int codigo;
                    if (int.TryParse(codPostal, out codigo))
                    {
                        Console.WriteLine(codigo);
                    }
                    else
                    {
                        TempData["mensaje"] = "Ha ingresado un valor no numérico";
                        return RedirectToAction("index3", "Productos");
                    }

                    cmd.Parameters.AddWithValue("@usuario", _usuario.UsuarioId);
                    cmd.Parameters.AddWithValue("@carrito", carrito);
                    cmd.Parameters.AddWithValue("@codPostal", codigo);
                    cmd.Parameters.AddWithValue("@direccion", dir);
                    cmd.Parameters.AddWithValue("@provincia", provincia);
                    cmd.Parameters.AddWithValue("@imagenID", imagen.ImagenId);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

                string error = e.Message;
                TempData["mensaje"] = "Ha sucedido un error inesperado";
                return RedirectToAction("index3", "Productos");

            }
            //envio de factura
            EmailSenderDAO emailSenderDAO = EmailSenderDAO.GetInstance();


            string mensaje = $"------------------------- FACTURA -------------------------\n" +
                $"Código Postal: {codPostal}\n" +
                $"Provincia: {ObtenerProvincia(provincia)}\n" +
                $"Dirección: {dir}\n" +
                $"Comprobante:{imagen.Url}\n" +
                $"Fecha: {DateTime.Now.ToShortDateString()}\n" +
                $"------------------------- DETALLE -------------------------\n" +
                ObtenerDetalle(carrito) +
                $"------------------------- ¡Gracias por su compra! -------------------------";


            await emailSenderDAO.SendEmailAsync(_usuario.Correo, "Factura - Duende MakeApp", mensaje);
            TempData["mensaje"] = "Su compra se ha realizado exitosamente";
            return RedirectToAction("Index", "Productos");
        }

        public string ObtenerProvincia(int p)
        {
            string provincia = string.Empty;
            switch (p)
            {
                case 1:
                    provincia = "San José";
                    break;
                case 2:
                    provincia = "Alajuela";
                    break;
                case 3:
                    provincia = "Cartago";
                    break;
                case 4:
                    provincia = "Heredia";
                    break;
                case 5:
                    provincia = "Guanacaste";
                    break;
                case 6:
                    provincia = "Puntarenas";
                    break;
                case 7:
                    provincia = "Limón";
                    break;
                default:
                    provincia = "Provincia Desconocida";
                    break;
            }
            return provincia;
        }

        public string ObtenerDetalle(int carrito)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            string detalle = "";
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT dbo.ObtenerDetalle(@carritoID)", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@carritoID", SqlDbType.Int) { Value = carrito });
                    detalle = (string)command.ExecuteScalar();
                }
            }

            return detalle;
        }

        public IActionResult RestarProducto(int id)
        {
            _usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            ViewBag.Usuario = _usuario;
            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            try
            {
                using (SqlConnection conexion = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("RestarCantidadProductos", conexion);


                    cmd.Parameters.AddWithValue("@IDProducto", id);
                    cmd.Parameters.AddWithValue("@IDCarrito", carrito);
                    cmd.Parameters.AddWithValue("@cantFinal", 1);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception e)
            {

                string error = e.Message;
                TempData["mensaje"] = "Ha sucedido un error inesperado";
                return RedirectToAction("index3", "Productos");

            }
            return RedirectToAction("index3", "Productos");
        }
    }
}
