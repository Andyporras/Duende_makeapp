using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace duendeMakeApp.Controllers
{
    public class VentasController : Controller
    {
        private readonly DuendeappContext _context;
        private List<Pedido> productosCarrito = new List<Pedido>();


        private readonly string  concStr  = "server=localhost; database=DUENDEAPP; User=sa; Password=miltonials; TrustServerCertificate=False; Encrypt=False;";

        public VentasController(DuendeappContext context)
        {
            _context = context;
        }



        // GET: Ventas
        public async Task<IActionResult> Index()
        {


            var oLista = new List<Pedido>();

            using (SqlConnection conexion = new SqlConnection(concStr))

            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerVentas", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        oLista.Add(new Pedido()
                        {
                            IdVenta = (int)dr["VentaID"],

                            Cliente = (string)dr["Correo"],

                            Monto = (decimal)dr["monto"]
,
                            FechaPedido = (DateTime?)dr["fechaPedido"],

                            FechaEntrega = (DateTime?)dr["fechaEntrega"],

                            Direccion = (string)dr["direccion"],

                            estado = (int)dr["estado"],

                            imagen = (string)dr["Url"],

                        });

                    }
                }
            }

            return View(oLista);
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Venta == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.Carrito)
                .Include(v => v.ImgComprobanteNavigation)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId");
            ViewData["ImgComprobante"] = new SelectList(_context.Imagens, "ImagenId", "ImagenId");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VentaId,ImgComprobante,CarritoId")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", venta.CarritoId);
            ViewData["ImgComprobante"] = new SelectList(_context.Imagens, "ImagenId", "ImagenId", venta.ImgComprobante);
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Venta == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", venta.CarritoId);
            ViewData["ImgComprobante"] = new SelectList(_context.Imagens, "ImagenId", "ImagenId", venta.ImgComprobante);
            return View(venta);
        }


        public IActionResult aprobar(int id)
        {

            using (SqlConnection conexion = new SqlConnection(concStr))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("aprobarVenta", conexion);
                cmd.Parameters.AddWithValue("@idVenta", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("index", "Ventas");
        }


        public IActionResult denegar(int id)
        {
            using (SqlConnection conexion = new SqlConnection(concStr))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("denegarVenta", conexion);
                cmd.Parameters.AddWithValue("@idVenta", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("index", "Ventas");
        }




        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VentaId,ImgComprobante,CarritoId")] Venta venta)
        {
            if (id != venta.VentaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.VentaId))
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
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "CarritoId", "CarritoId", venta.CarritoId);
            ViewData["ImgComprobante"] = new SelectList(_context.Imagens, "ImagenId", "ImagenId", venta.ImgComprobante);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Venta == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.Carrito)
                .Include(v => v.ImgComprobanteNavigation)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Venta == null)
            {
                return Problem("Entity set 'DuendeappContext.Venta'  is null.");
            }
            var venta = await _context.Venta.FindAsync(id);
            if (venta != null)
            {
                _context.Venta.Remove(venta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
          return (_context.Venta?.Any(e => e.VentaId == id)).GetValueOrDefault();
        }
    }
}
