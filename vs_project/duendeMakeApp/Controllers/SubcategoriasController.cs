﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;

namespace duendeMakeApp.Controllers
{
    public class SubcategoriasController : Controller
    {
        private readonly DuendeappContext _context;
        private static Usuario? _usuario;
        //_httpContextAccessor
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubcategoriasController(DuendeappContext context, Usuario usuario, IHttpContextAccessor httpContextAccessor)
        {
            _usuario = usuario;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Subcategorias
        public async Task<IActionResult> Index()
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            return _context.Subcategoria != null ? 
                          View(await _context.Subcategoria.ToListAsync()) :
                          Problem("Entity set 'DuendeappContext.Subcategoria'  is null.");
        }

        // GET: Subcategorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subcategoria == null)
            {
                return NotFound();
            }

            var subcategoria = await _context.Subcategoria
                .FirstOrDefaultAsync(m => m.SubcategoriaId == id);
            if (subcategoria == null)
            {
                return NotFound();
            }

            return View(subcategoria);
        }

        // GET: Subcategorias/Create
        public IActionResult Create()
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            return View();
        }

        // POST: Subcategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubcategoriaId,Nombre")] Subcategoria subcategoria)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            if (ModelState.IsValid)
            {
                _context.Add(subcategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subcategoria);
        }

        // GET: Subcategorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            if (id == null || _context.Subcategoria == null)
            {
                return NotFound();
            }

            var subcategoria = await _context.Subcategoria.FindAsync(id);
            if (subcategoria == null)
            {
                return NotFound();
            }
            return View(subcategoria);
        }

        // POST: Subcategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubcategoriaId,Nombre")] Subcategoria subcategoria)
        {

            //if (id != subcategoria.SubcategoriaId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subcategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubcategoriaExists(subcategoria.SubcategoriaId))
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
            return View(subcategoria);
        }

        // GET: Subcategorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            if (id == null || _context.Subcategoria == null)
            {
                return NotFound();
            }

            var subcategoria = await _context.Subcategoria
                .FirstOrDefaultAsync(m => m.SubcategoriaId == id);
            if (subcategoria == null)
            {
                return NotFound();
            }

            return View(subcategoria);
        }

        // POST: Subcategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subcategoria == null)
            {
                return Problem("Entity set 'DuendeappContext.Subcategoria'  is null.");
            }
            var subcategoria = await _context.Subcategoria.FindAsync(id);
            if (subcategoria != null)
            {
                _context.Subcategoria.Remove(subcategoria);
            }
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubcategoriaExists(int id)
        {
          return (_context.Subcategoria?.Any(e => e.SubcategoriaId == id)).GetValueOrDefault();
        }
    }
}
