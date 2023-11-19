using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;

namespace duendeMakeApp.Controllers
{
    public class NotificacionesController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;
        //IHttpContextAccessor
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificacionesController(DuendeappContext context, Usuario usuario, IHttpContextAccessor httpContextAccessor)
        {
            _usuario = usuario;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Paquetes
        public async Task<IActionResult> Index()
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            return View();
        }

        // GET: Paquetes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            ViewBag.Usuario = usuario;
            if (id == null || _context.Notificaciones == null)
            {
                return NotFound();
            }

            Notificacion notificacion = await _context.Notificaciones
                .FirstOrDefaultAsync(m => m.NotificacionId == id);
            notificacion.Visto = true;
            _context.SaveChanges();

            if (notificacion == null)
            {
                return NotFound();
            }

            return View(notificacion);
        }

        // POST: Paquetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            if (_context.Notificaciones == null)
            {
                return Problem("Entity set 'DuendeappContext.Paquetes'  is null.");
            }
            var notificacion = await _context.Notificaciones.FindAsync(id);
            if (notificacion != null)
            {
                // Hacer copias de las listas de productos y paquetes
                _context.Notificaciones.Remove(notificacion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public static async Task<bool> Notificar(DuendeappContext context, int idUsuario, string titulo, string mensaje)
        {
            IObservable? usuario = await context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
            {
                return false;
            }
            usuario.Notificar(context, titulo, mensaje);
            return true;
        }
    }
}
