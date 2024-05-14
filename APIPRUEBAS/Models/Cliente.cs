using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIPRUEBAS.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Ventas = new HashSet<Venta>();
        }

        public int IdCliente { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }

        [JsonIgnore]
        public virtual ICollection<Venta> Ventas { get; set; }
    }
}