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

namespace duendeMakeApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly DuendeappContext _context;

        public UsuariosController(DuendeappContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var duendeappContext = _context.Usuarios.Include(u => u.Tipo);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoId"] = new SelectList(_context.TipoUsuarios, "TipoUsarioId", "TipoUsarioId", usuario.TipoId);
            return View(usuario);
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
                // mensaje que indique que no ingreso la contraseña o correo 
                TempData["Mensaje"] = "No se ha ingresado la contraseña o el correo.";
                return RedirectToAction("Index", "Maquillajes"); // Retorna la vista actual ; // Retorna la vista actual
            }
            Usuario usuario = new Usuario();
            usuario = _context.Usuarios.Where(item => item.Correo == correo && item.Clave == clave).FirstOrDefault();
            if (usuario == null)
            {
                TempData["Mensaje"] = "El correo o la contraseña son incorrectos.";
                return RedirectToAction("Index", "Maquillajes");
            }
            // sacamos el tipo de usuario
            int tipo = usuario.TipoId.GetValueOrDefault();
            TipoUsuario tipoUsuario = _context.TipoUsuarios.Where(item => item.TipoUsarioId == tipo).FirstOrDefault();
            // guardamos el tipo de usuario en la sesion

            TempData["Usuario"] = tipoUsuario.Tipo;
            return RedirectToAction("Index", "Maquillajes");
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,Apellido,Correo,Usuario1,Clave,TipoId")] Usuario usuario)
        public async Task<IActionResult> Create(string nombre, string apellido, string correo, string usuario1, string clave, string rClave)
        {
            if (clave != rClave)
            {
                return Problem("Las contraseñas no coinciden");
            }
            if (UsuarioExists(correo))
            {
                return Problem("El correo ya existe");
            }
            Usuario usuario = new Usuario();
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Correo = correo;
            usuario.Usuario1 = usuario1;
            usuario.Clave = clave;
            usuario.TipoId = 1;
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
    }
}
