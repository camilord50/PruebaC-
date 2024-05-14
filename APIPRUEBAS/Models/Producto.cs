using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIPRUEBAS.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Ventas = new HashSet<Venta>();
        }

        public int IdProducto { get; set; }
        public string? CodigoBarra { get; set; }
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public int? IdCategoria { get; set; }
        public decimal? Precio { get; set; }

        public virtual Categoria? oCategoria { get; set; }

        [JsonIgnore]
        public virtual ICollection<Venta> Ventas { get; set; }
    }
}
