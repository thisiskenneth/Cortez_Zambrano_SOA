using FerreteriaSoap.Entities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FerreteriaSoap.Repositories
{
    public class ArticuloRepository
    {
        public async Task<bool> ExistsByCodigoAsync(string codigo)
        {
            using (var cn = SqlConnectionFactory.Create())
            using (var cmd = new SqlCommand("SELECT 1 FROM dbo.Articulo WHERE Codigo = @Codigo", cn))
            {
                cmd.Parameters.Add(new SqlParameter("@Codigo", SqlDbType.VarChar, 50) { Value = codigo });
                await cn.OpenAsync();
                var res = await cmd.ExecuteScalarAsync();
                return res != null;
            }
        }

        public async Task InsertAsync(Articulo a)
        {
            using (var cn = SqlConnectionFactory.Create())
            using (var cmd = new SqlCommand(@"INSERT INTO dbo.Articulo
(Codigo, Nombre, Categoria, PrecioCompra, PrecioVenta, Stock, StockMinimo, Proveedor, FechaCreacion)
VALUES(@Codigo, @Nombre, @Categoria, @PrecioCompra, @PrecioVenta, @Stock, @StockMinimo, @Proveedor, @FechaCreacion)", cn))
            {
                cmd.Parameters.AddWithValue("@Codigo", a.Codigo);
                cmd.Parameters.AddWithValue("@Nombre", a.Nombre);
                cmd.Parameters.AddWithValue("@Categoria", a.Categoria ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PrecioCompra", a.PrecioCompra);
                cmd.Parameters.AddWithValue("@PrecioVenta", a.PrecioVenta);
                cmd.Parameters.AddWithValue("@Stock", a.Stock);
                cmd.Parameters.AddWithValue("@StockMinimo", a.StockMinimo);
                cmd.Parameters.AddWithValue("@Proveedor", a.Proveedor ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaCreacion", a.FechaCreacion == default ? DateTime.UtcNow : a.FechaCreacion);
                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<Articulo> GetByCodigoAsync(string codigo)
        {
            using (var cn = SqlConnectionFactory.Create())
            using (var cmd = new SqlCommand(@"SELECT Codigo, Nombre, Categoria, PrecioCompra, PrecioVenta, Stock, StockMinimo, Proveedor, FechaCreacion
FROM dbo.Articulo WHERE Codigo = @Codigo", cn))
            {
                cmd.Parameters.Add(new SqlParameter("@Codigo", SqlDbType.VarChar, 50) { Value = codigo });
                await cn.OpenAsync();
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    if (!await rdr.ReadAsync()) return null;
                    return new Articulo
                    {
                        Codigo = rdr.GetString(0),
                        Nombre = rdr.GetString(1),
                        Categoria = rdr.IsDBNull(2) ? null : rdr.GetString(2),
                        PrecioCompra = rdr.GetDecimal(3),
                        PrecioVenta = rdr.GetDecimal(4),
                        Stock = rdr.GetInt32(5),
                        StockMinimo = rdr.GetInt32(6),
                        Proveedor = rdr.IsDBNull(7) ? null : rdr.GetString(7),
                        FechaCreacion = rdr.GetDateTime(8)
                    };
                }
            }
        }
    }
}
