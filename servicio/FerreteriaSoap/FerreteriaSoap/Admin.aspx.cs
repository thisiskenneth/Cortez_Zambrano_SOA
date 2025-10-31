using System;
using System.ServiceModel;
using System.Web.UI;
using FerreteriaSoap.Entities;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FerreteriaSoap.Repositories;
using System.Globalization;

namespace FerreteriaSoap
{
    public partial class Admin : Page
    {
        // Declaraciones para evitar requerir archivo .designer
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.TextBox txtCodigo;
        protected global::System.Web.UI.WebControls.TextBox txtNombre;
        protected global::System.Web.UI.WebControls.TextBox txtCategoria;
        protected global::System.Web.UI.WebControls.TextBox txtPrecioCompra;
        protected global::System.Web.UI.WebControls.TextBox txtPrecioVenta;
        protected global::System.Web.UI.WebControls.TextBox txtStock;
        protected global::System.Web.UI.WebControls.TextBox txtStockMin;
        protected global::System.Web.UI.WebControls.TextBox txtProveedor;

        protected global::System.Web.UI.WebControls.TextBox txtBuscarCodigo;
        protected global::System.Web.UI.WebControls.Panel pnlResultado;
        protected global::System.Web.UI.WebControls.Label lblRCodigo;
        protected global::System.Web.UI.WebControls.Label lblRNombre;
        protected global::System.Web.UI.WebControls.Label lblRCategoria;
        protected global::System.Web.UI.WebControls.Label lblRPrecioCompra;
        protected global::System.Web.UI.WebControls.Label lblRPrecioVenta;
        protected global::System.Web.UI.WebControls.Label lblRStock;
        protected global::System.Web.UI.WebControls.Label lblRStockMin;
        protected global::System.Web.UI.WebControls.Label lblRProveedor;
        protected global::System.Web.UI.WebControls.Label lblRFecha;
        protected global::System.Web.UI.WebControls.Label lblWarn;
        protected global::System.Web.UI.WebControls.GridView gvArticulos;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentEncoding = Encoding.UTF8;
            Response.Charset = "utf-8";
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private IService1 CreateClient()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                MaxReceivedMessageSize = 65536
            };
            var svcUri = new Uri(Request.Url, ResolveUrl("~/Service1.svc"));
            var endpoint = new EndpointAddress(svcUri);
            var factory = new ChannelFactory<IService1>(binding, endpoint);
            return factory.CreateChannel();
        }

        private void BindGrid()
        {
            using (var cn = SqlConnectionFactory.Create())
            using (var da = new SqlDataAdapter(@"SELECT Codigo, Nombre, Categoria, PrecioCompra, PrecioVenta, Stock, StockMinimo, Proveedor, FechaCreacion 
FROM dbo.Articulo ORDER BY FechaCreacion DESC", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvArticulos.DataSource = dt;
                gvArticulos.DataBind();
            }
        }

        private void LoadArticuloToForm(string codigo)
        {
            using (var cn = SqlConnectionFactory.Create())
            using (var cmd = new SqlCommand(@"SELECT Codigo, Nombre, Categoria, PrecioCompra, PrecioVenta, Stock, StockMinimo, Proveedor, FechaCreacion
FROM dbo.Articulo WHERE Codigo = @Codigo", cn))
            {
                cmd.Parameters.AddWithValue("@Codigo", codigo);
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        txtCodigo.Text = rd["Codigo"].ToString();
                        txtNombre.Text = rd["Nombre"].ToString();
                        txtCategoria.Text = rd["Categoria"].ToString();
                        txtPrecioCompra.Text = Convert.ToDecimal(rd["PrecioCompra"]).ToString();
                        txtPrecioVenta.Text = Convert.ToDecimal(rd["PrecioVenta"]).ToString();
                        txtStock.Text = Convert.ToInt32(rd["Stock"]).ToString();
                        txtStockMin.Text = Convert.ToInt32(rd["StockMinimo"]).ToString();
                        txtProveedor.Text = rd["Proveedor"].ToString();
                    }
                }
            }
        }

        protected void btnInsertar_Click(object sender, EventArgs e)
        {
            lblMsg.CssClass = string.Empty;
            lblMsg.Text = string.Empty;
            try
            {
                decimal precioCompra = decimal.Parse(txtPrecioCompra.Text);
                decimal precioVenta = decimal.Parse(txtPrecioVenta.Text);
                int stock = int.Parse(txtStock.Text);
                int stockMin = int.Parse(txtStockMin.Text);

                var art = new Articulo
                {
                    Codigo = txtCodigo.Text?.Trim(),
                    Nombre = txtNombre.Text?.Trim(),
                    Categoria = string.IsNullOrWhiteSpace(txtCategoria.Text) ? null : txtCategoria.Text.Trim(),
                    PrecioCompra = precioCompra,
                    PrecioVenta = precioVenta,
                    Stock = stock,
                    StockMinimo = stockMin,
                    Proveedor = string.IsNullOrWhiteSpace(txtProveedor.Text) ? null : txtProveedor.Text.Trim(),
                    FechaCreacion = DateTime.UtcNow
                };

                var client = CreateClient();
                client.InsertArticulo(art);

                lblMsg.CssClass = "ok";
                lblMsg.Text = "Artículo insertado correctamente.";
                BindGrid();
            }
            catch (FaultException<string> fex)
            {
                lblMsg.CssClass = "err";
                lblMsg.Text = "Error de servicio: " + fex.Detail;
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "err";
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            pnlResultado.Visible = false;
            lblWarn.Visible = false;
            lblMsg.CssClass = string.Empty;
            lblMsg.Text = string.Empty;
            try
            {
                var codigo = txtBuscarCodigo.Text?.Trim();
                var client = CreateClient();
                var art = client.GetArticuloPorCodigo(codigo);
                if (art == null)
                {
                    lblWarn.Visible = true;
                    lblWarn.Text = "No se encontró el artículo.";
                    return;
                }
                pnlResultado.Visible = true;
                lblRCodigo.Text = art.Codigo;
                lblRNombre.Text = art.Nombre;
                lblRCategoria.Text = art.Categoria;
                lblRPrecioCompra.Text = art.PrecioCompra.ToString("N2");
                lblRPrecioVenta.Text = art.PrecioVenta.ToString("N2");
                lblRStock.Text = art.Stock.ToString();
                lblRStockMin.Text = art.StockMinimo.ToString();
                lblRProveedor.Text = art.Proveedor;
                lblRFecha.Text = art.FechaCreacion.ToString("yyyy-MM-dd HH:mm");
                // Prefill form for editing
                txtCodigo.Text = art.Codigo;
                txtNombre.Text = art.Nombre;
                txtCategoria.Text = art.Categoria;
                txtPrecioCompra.Text = art.PrecioCompra.ToString();
                txtPrecioVenta.Text = art.PrecioVenta.ToString();
                txtStock.Text = art.Stock.ToString();
                txtStockMin.Text = art.StockMinimo.ToString();
                txtProveedor.Text = art.Proveedor;
                BindGrid();
            }
            catch (FaultException<string> fex)
            {
                lblMsg.CssClass = "err";
                lblMsg.Text = "Error de servicio: " + fex.Detail;
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "err";
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            lblMsg.CssClass = string.Empty;
            lblMsg.Text = string.Empty;
            try
            {
                using (var cn = SqlConnectionFactory.Create())
                using (var cmd = new SqlCommand(@"UPDATE dbo.Articulo
SET Nombre = @Nombre,
    Categoria = @Categoria,
    PrecioCompra = @PrecioCompra,
    PrecioVenta = @PrecioVenta,
    Stock = @Stock,
    StockMinimo = @StockMinimo,
    Proveedor = @Proveedor
WHERE Codigo = @Codigo", cn))
                {
                    cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text?.Trim());
                    cmd.Parameters.AddWithValue("@Nombre", (object)(txtNombre.Text?.Trim()) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Categoria", string.IsNullOrWhiteSpace(txtCategoria.Text) ? (object)DBNull.Value : txtCategoria.Text.Trim());
                    cmd.Parameters.AddWithValue("@PrecioCompra", decimal.Parse(txtPrecioCompra.Text));
                    cmd.Parameters.AddWithValue("@PrecioVenta", decimal.Parse(txtPrecioVenta.Text));
                    cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));
                    cmd.Parameters.AddWithValue("@StockMinimo", int.Parse(txtStockMin.Text));
                    cmd.Parameters.AddWithValue("@Proveedor", string.IsNullOrWhiteSpace(txtProveedor.Text) ? (object)DBNull.Value : txtProveedor.Text.Trim());
                    cn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        lblMsg.CssClass = "warn";
                        lblMsg.Text = "No se encontró el artículo para actualizar.";
                    }
                    else
                    {
                        lblMsg.CssClass = "ok";
                        lblMsg.Text = "Artículo actualizado correctamente.";
                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "err";
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            lblMsg.CssClass = string.Empty;
            lblMsg.Text = string.Empty;
            try
            {
                using (var cn = SqlConnectionFactory.Create())
                using (var cmd = new SqlCommand("DELETE FROM dbo.Articulo WHERE Codigo = @Codigo", cn))
                {
                    cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text?.Trim());
                    cn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        lblMsg.CssClass = "warn";
                        lblMsg.Text = "No se encontró el artículo para eliminar.";
                    }
                    else
                    {
                        lblMsg.CssClass = "ok";
                        lblMsg.Text = "Artículo eliminado correctamente.";
                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "err";
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        protected void gvArticulos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                var codigo = gvArticulos.DataKeys[rowIndex].Value.ToString();

                if (e.CommandName == "EditItem")
                {
                    LoadArticuloToForm(codigo);
                    lblMsg.CssClass = "warn";
                    lblMsg.Text = "Editando artículo " + codigo + ". Modifique campos y presione Actualizar.";
                }
                else if (e.CommandName == "DeleteItem")
                {
                    using (var cn = SqlConnectionFactory.Create())
                    using (var cmd = new SqlCommand("DELETE FROM dbo.Articulo WHERE Codigo = @Codigo", cn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigo);
                        cn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            lblMsg.CssClass = "warn";
                            lblMsg.Text = "No se encontró el artículo para eliminar.";
                        }
                        else
                        {
                            lblMsg.CssClass = "ok";
                            lblMsg.Text = "Artículo eliminado correctamente.";
                            BindGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "err";
                lblMsg.Text = "Error: " + ex.Message;
            }
        }
    }
}
