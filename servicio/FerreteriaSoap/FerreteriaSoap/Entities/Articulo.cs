using System;
using System.Runtime.Serialization;

namespace FerreteriaSoap.Entities
{
    [DataContract]
    public class Articulo
    {
        [DataMember(Order = 1)]
        public string Codigo { get; set; }

        [DataMember(Order = 2)]
        public string Nombre { get; set; }

        [DataMember(Order = 3)]
        public string Categoria { get; set; }

        [DataMember(Order = 4)]
        public decimal PrecioCompra { get; set; }

        [DataMember(Order = 5)]
        public decimal PrecioVenta { get; set; }

        [DataMember(Order = 6)]
        public int Stock { get; set; }

        [DataMember(Order = 7)]
        public int StockMinimo { get; set; }

        [DataMember(Order = 8)]
        public string Proveedor { get; set; }

        [DataMember(Order = 9)]
        public DateTime FechaCreacion { get; set; }
    }
}
