using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIPRUEBAS.Models
{
    public partial class Venta
    {
        
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public decimal? TotalSinIVA { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public virtual Producto? Producto { get; set; }

    }
}