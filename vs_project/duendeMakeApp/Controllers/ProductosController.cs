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

namespace duendeMakeApp.Controllers
{
    public class ProductosController : Controller
    {
        private readonly DuendeappContext _context;
        private List<Producto> productosCarrito = new List<Producto>();
        private static Usuario? _usuario;

        private static string conecction = "Data Source=DESKTOP-993UODJ; Initial Catalog=DUENDEAPP; Integrated Security=true; Encrypt=False;";

        public ProductosController(DuendeappContext context, Usuario usuario)
        {
            _usuario = usuario;
            _context = context;
        }

        // GET: Productos
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
            var duendeappContext = _context.Productos.Include(p => p.Categoria).Include(p => p.Imagen);
            return View(await duendeappContext.ToListAsync());
        }

        // GET: Productos/Details/5
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

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId");
            ViewData["ImagenId"] = new SelectList(_context.Imagens, "ImagenId", "ImagenId");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Descripcion,Precio,Cantidad,CategoriaId,Estado,ImagenId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", producto.CategoriaId);
            ViewData["ImagenId"] = new SelectList(_context.Imagens, "ImagenId", "ImagenId", producto.ImagenId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Descripcion,Precio,Cantidad,CategoriaId,Estado,ImagenId")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

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
            _usuario = UsuariosController.GetSessionUser(_context);
            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);
            var oLista = new List<Producto>();
            using (SqlConnection conexion = new SqlConnection(conecction))

                

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
                        oLista.Add(new Producto()
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

                        });

                    }
                }
            }

            return View(oLista);
        }


        public IActionResult agregarAlCarrito(int id)
        {
            _usuario = UsuariosController.GetSessionUser(_context);

            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            try
            {
                using (SqlConnection conexion = new SqlConnection(conecction))
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
            _usuario = UsuariosController.GetSessionUser(_context);

            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            try
            {
                using (SqlConnection conexion = new SqlConnection(conecction))
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
            _usuario = UsuariosController.GetSessionUser(_context);


            using (SqlConnection connection = new SqlConnection(conecction))
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
            _usuario = UsuariosController.GetSessionUser(_context);

            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            try
            {
                using (SqlConnection conexion = new SqlConnection(conecction))
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(string codPostal, string provincia, string dir, string imageFile)
        {

            //Usuario usuario = new Usuario();
            //usuario = _context.Usuarios.Where(item => item.Correo == correo && item.Clave == clave).FirstOrDefault();
            //if (usuario == null)
            //{
            //    TempData["Mensaje"] = "El correo o la contraseña son incorrectos.";
            //    return RedirectToAction("Index", "Maquillajes");
            //}
            //// sacamos el tipo de usuario
            //int tipo = usuario.TipoId.GetValueOrDefault();
            //TipoUsuario tipoUsuario = _context.TipoUsuarios.Where(item => item.TipoUsarioId == tipo).FirstOrDefault();
            //// guardamos el tipo de usuario en la sesion

            ////TempData["Usuario"] = usuario;
            //// cambiar el valor de inicio de seccion en Usuario
            //Usuario.SeccionActual = correo;
            ////TempData["Mensaje"] = "Bienvenido " + usuario.Nombre + " " + usuario.Apellido + " al sistema de Duende MakeApp";
            Console.WriteLine(codPostal);
            Console.WriteLine(provincia);
            Console.WriteLine(imageFile);
            Console.WriteLine(dir);
            return RedirectToAction("Index", "Productos");
        }




        public IActionResult RestarProducto(int id)
        {
            _usuario = UsuariosController.GetSessionUser(_context);
            int carrito = ObtenerCarritoPorUsuarioID(_usuario.UsuarioId);

            try
            {
                using (SqlConnection conexion = new SqlConnection(conecction))
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
