using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;
using duendeMakeApp.DAO;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Data.SqlClient;
using System.Drawing;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace duendeMakeApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;
        //_httpContextAccessor
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuariosController(DuendeappContext context, Usuario usuario, IHttpContextAccessor httpContextAccessor)
        {
            _usuario = usuario;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var duendeappContext = _context.Usuarios
                .Include(u => u.Tipo)
                .Include(u => u.Carritos)
                .ThenInclude(c => c.Paquetes)
                .ThenInclude(p => p.Productos);

            ViewBag.Ventas = _context.Venta
                .Include(v => v.Carrito)
                .ThenInclude(c => c.Paquetes)
                .ThenInclude(p => p.Productos)
                .ToList();
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            return View(await duendeappContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Tipo)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["TipoId"] = new SelectList(_context.TipoUsuarios, "TipoUsarioId", "TipoUsarioId");
            return View();
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            ViewData["TipoId"] = new SelectList(_context.TipoUsuarios, "TipoUsarioId", "TipoUsarioId", usuario.TipoId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Apellido,Correo,Usuario1,Clave,TipoId")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Usuario? usuarioActual = _context.Usuarios.Find(id);
                    usuario.TipoId = usuarioActual.TipoId;
                    _context.Entry(usuarioActual).State = EntityState.Detached;
                    _context.Entry(usuario).State = EntityState.Modified;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Maquillajes");

            }
            ViewData["TipoId"] = new SelectList(_context.TipoUsuarios, "TipoUsarioId", "TipoUsarioId", usuario.TipoId);

            return RedirectToAction("Index", "Maquillajes");
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Tipo)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'DuendeappContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IniciarSeccion(string correo, string clave)
        {
            if (correo == null || clave == null)
            {
                // Mensaje que indica que no se ingresó la contraseña o el correo
                TempData["Mensaje"] = "No se ha ingresado la contraseña o el correo.";
                return RedirectToAction("Index", "Maquillajes");
            }

            Usuario usuario = _context.Usuarios.FirstOrDefault(item => item.Correo == correo && item.Clave == clave);

            if (usuario == null)
            {
                TempData["Mensaje"] = "El correo o la contraseña son incorrectos.";
                return RedirectToAction("Index", "Maquillajes");
            }

            // Guardar la información del usuario en una cookie
            var usuarioJson = JsonConvert.SerializeObject(usuario);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1), // Define la expiración
                IsEssential = true // Marcar como esencial
            };
            Response.Cookies.Append("UsuarioCookie", usuarioJson, cookieOptions);

            return RedirectToAction("Index", "Maquillajes");
        }


        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string nombre, string apellido, string correo, string usuario1, string clave, string rClave)
        {
            if (clave != rClave)
            {
                TempData["Mensaje"] = "Las contraseñas no coinciden";
                return RedirectToAction("Index", "Maquillajes");
            }
            if (UsuarioExists(correo))
            {
                TempData["Mensaje"] = "El correo ya existe";
                return RedirectToAction("Index", "Maquillajes");
            }
            Usuario usuario = new Usuario();
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Correo = correo;
            usuario.Usuario1 = usuario1;
            usuario.Clave = clave;
            usuario.TipoId = 2;

            //guardar el nuevo usuario en la base de datos
            _context.Add(usuario);
            await _context.SaveChangesAsync();

            // crear un carrito para el usuario
            Carrito carrito = new Carrito();
            carrito.UsuarioId = usuario.UsuarioId;
            carrito.estado = true;
            _context.Add(carrito);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "El usuario se ha creado exitosamente";

            return RedirectToAction("Index", "Maquillajes");
        }

        public string claveRandom()
        {
            //generar una clave aleatoria con letras y numeros
            string clave = "";
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                int numero = random.Next(0, 9);
                int letra = random.Next(65, 90);
                char caracter = (char)letra;
                clave += numero.ToString() + caracter.ToString();
            }
            return clave;
        }

        [HttpPost, ActionName("RegistraAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistraAdmin([Bind("UsuarioId,Nombre,Apellido,Correo,Usuario1,Clave,TipoId")] Usuario usuario)
        {
            if (UsuarioExists(usuario.Correo))
            {
                TempData["Mensaje"] = "El correo ya existe";
                return RedirectToAction("Index", "Maquillajes");
            }

            usuario.Clave = claveRandom();
            usuario.TipoId = 1;

            //enviar correo con la clave
            string mensaje = "Bienvenido " + usuario.Nombre + " " + usuario.Apellido + " al sistema de Duende MakeApp.\n\n Su clave es: " + usuario.Clave;
            string asunto = "Bienvenido al sistema de Duende MakeApp";
            string correo = usuario.Correo;

            EmailSenderDAO emailSenderDAO = EmailSenderDAO.GetInstance();
            await emailSenderDAO.SendEmailAsync(correo, asunto, mensaje);


            //guardar el nuevo usuario en la base de datos
            _context.Add(usuario);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "El usuario se ha creado exitosamente";

            return RedirectToAction("Index", "Maquillajes");
        }

        public async Task<IActionResult> OlvidarClave()
        {
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestablecerContraseña(string correo)
        {
            if(correo == null)
            {
                TempData["Mensaje"] = "No se ha ingresado el correo";
                return RedirectToAction("OlvidarClave", "Usuarios");
            }
            if(!UsuarioExists(correo))
            {
                TempData["Mensaje"] = "El correo no existe";
                return RedirectToAction("OlvidarClave", "Usuarios");
            }
            // Usuario
            Usuario usuario = new Usuario();
            usuario = _context.Usuarios.Where(item => item.Correo == correo).FirstOrDefault();
            // clave 
            string clave = usuario.Clave;
            // enviar correo
            EmailSenderDAO emailSenderDAO = EmailSenderDAO.GetInstance();
            await emailSenderDAO.SendEmailAsync(correo, "Restablecer contraseña", "Su contraseña es: " + clave);

            TempData["Mensaje"] = "Se ha enviado un correo con su contraseña";
            return RedirectToAction("Index", "Maquillajes");
        }


        private bool UsuarioExists(String correo)
        {
            return (_context.Usuarios?.Any(e => e.Correo == correo)).GetValueOrDefault();
        }
        
        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> CerrarSesion()
        {
            // Elimina la cookie de usuario
            Response.Cookies.Delete("UsuarioCookie");

            // Limpia cualquier información relacionada con la sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige al inicio o a la página de inicio de sesión
            return RedirectToAction("Index", "Maquillajes");
        }


        //una funcion estatica para obtener el objeto de un Usuario que se busca por id
        public static Usuario? GetUsuario(int id, DuendeappContext context)
        {
            Usuario? usuario = context.Usuarios.Where(item => item.UsuarioId == id).FirstOrDefault();
            return usuario;
        }

        public static Usuario? GetSessionUser(IHttpContextAccessor httpContextAccessor, DuendeappContext _context)
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext.Request.Cookies.TryGetValue("UsuarioCookie", out string usuarioJson))
            {
                Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);

                // Carga completamente la entidad Usuario desde la base de datos
                Usuario? usuarioCompleto = _context.Usuarios
                    .Include(u => u.Tipo)
                    .Include(u => u.Carritos)
                    .Include(u => u.Notificaciones)
                    .FirstOrDefault(u => u.UsuarioId == usuario.UsuarioId);

                usuarioCompleto.Notificaciones = usuarioCompleto.Notificaciones.OrderByDescending(n => n.FechaEnvio).ToList();

                if (usuarioCompleto != null)
                {
                    return usuarioCompleto;
                }
            }

            return null;
        }

    }
}
