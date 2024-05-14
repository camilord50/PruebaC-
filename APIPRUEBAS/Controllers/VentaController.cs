using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRUEBAS.Models;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using System.Collections.Generic;
using System;

namespace APIPRUEBAS.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly DBAPIContext _dbcontext;

        public VentaController(DBAPIContext dbContext)
        {
            _dbcontext = dbContext;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Venta> lista = new List<Venta>();
            try
            {
                lista = _dbcontext.Ventas.Include(v => v.Cliente).Include(v => v.Producto).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener/{idVenta:int}")]
        public IActionResult Obtener(int idVenta)
        {
            Venta venta = _dbcontext.Ventas.Include(v => v.Cliente).Include(v => v.Producto).FirstOrDefault(v => v.IdVenta == idVenta);
            if (venta == null)
            {
                return BadRequest("Venta no encontrada");
            }
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = venta });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = venta });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Venta venta)
        {
            try
            {
                var producto = _dbcontext.Productos.FirstOrDefault(p => p.IdProducto == venta.IdProducto);
                if (producto == null)
                {
                    return BadRequest("Producto no encontrado");
                }

                venta.Total = (decimal)(producto.Precio * venta.Cantidad);

                decimal IVA = 0.19m;
                venta.TotalSinIVA = venta.Total / (1 + IVA);

                _dbcontext.Ventas.Add(venta);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Venta venta)
        {
            Venta oVenta = _dbcontext.Ventas.Find(venta.IdVenta);
            if (oVenta == null)
            {
                return BadRequest("Venta no encontrada");
            }
            try
            {
                if (venta.Fecha.Year >= DateTime.Now.Year)
                {
                    oVenta.Fecha = venta.Fecha;
                }

                oVenta.IdCliente = venta.IdCliente == 0 ? oVenta.IdCliente : venta.IdCliente;
                oVenta.IdProducto = venta.IdProducto == 0 ? oVenta.IdProducto : venta.IdProducto;
                oVenta.Cantidad = venta.Cantidad == 0 ? oVenta.Cantidad : venta.Cantidad;


        var producto = _dbcontext.Productos.FirstOrDefault(p => p.IdProducto == venta.IdProducto);
                if (producto == null)
                {
                    return BadRequest("Producto no encontrado");
                }

                venta.Total = (decimal)(producto.Precio * venta.Cantidad);
            
                oVenta.Total = venta.Total;
                oVenta.TotalSinIVA = venta.Total / 1.19m;

                _dbcontext.Ventas.Update(oVenta);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idVenta:int}")]
        public IActionResult Eliminar(int idVenta)
        {
            Venta oVenta = _dbcontext.Ventas.Find(idVenta);
            if (oVenta == null)
            {
                return BadRequest("Venta no encontrada");
            }
            try
            {
                _dbcontext.Ventas.Remove(oVenta);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("{idCliente}/Ventas")]
        public IActionResult VentasCliente(int idCliente)
        {
            try
            {
                var ventasCliente = _dbcontext.Ventas
                    .Include(v => v.Producto)
                    .ThenInclude(p => p.oCategoria)
                    .Include(v => v.Cliente)
                    .Where(v => v.IdCliente == idCliente)
                    .ToList();

                if (ventasCliente.Count == 0)
                {
                    return NotFound("No se encontraron ventas para el cliente especificado.");
                }

                return Ok(ventasCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

    }
}
