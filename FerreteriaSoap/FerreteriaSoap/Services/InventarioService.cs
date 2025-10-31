using System;
using System.Threading.Tasks;
using FerreteriaSoap.Entities;
using FerreteriaSoap.Repositories;

namespace FerreteriaSoap.Services
{
    public class InventarioService
    {
        private readonly ArticuloRepository _repo;

        public InventarioService()
        {
            _repo = new ArticuloRepository();
        }

        public async Task InsertarAsync(Articulo a)
        {
            ValidarArticulo(a);
            if (await _repo.ExistsByCodigoAsync(a.Codigo))
            {
                throw new InvalidOperationException("El código de artículo ya existe.");
            }
            a.FechaCreacion = DateTime.UtcNow;
            await _repo.InsertAsync(a);
        }

        public Task<Articulo> ObtenerPorCodigoAsync(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código es requerido.");
            return _repo.GetByCodigoAsync(codigo.Trim());
        }

        private void ValidarArticulo(Articulo a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (string.IsNullOrWhiteSpace(a.Codigo)) throw new ArgumentException("El código es requerido.");
            if (string.IsNullOrWhiteSpace(a.Nombre)) throw new ArgumentException("El nombre es requerido.");
            if (a.PrecioCompra < 0) throw new ArgumentException("El precio de compra debe ser positivo.");
            if (a.PrecioVenta < 0) throw new ArgumentException("El precio de venta debe ser positivo.");
            if (a.PrecioVenta < a.PrecioCompra) throw new ArgumentException("El precio de venta no puede ser menor que el precio de compra.");
            if (a.Stock < 0) throw new ArgumentException("El stock no puede ser negativo.");
            if (a.StockMinimo < 0) throw new ArgumentException("El stock mínimo no puede ser negativo.");
        }
    }
}
