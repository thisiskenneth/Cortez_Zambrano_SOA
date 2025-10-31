<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="FerreteriaSoap.Admin" ResponseEncoding="utf-8" Culture="es-ES" UICulture="es" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Administraci&oacute;n Inventario - Ferreter&iacute;a</title>
    <meta charset="utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        fieldset { margin-bottom: 20px; padding: 15px; }
        label { display:inline-block; width:150px; }
        input[type=text], input[type=number] { width: 250px; }
        .row { margin:6px 0; }
        .ok { color: #0a7d00; }
        .err { color: #b30000; }
        .warn { color: #a67300; }
        .grid { border-collapse: collapse; }
        .grid td { border: 1px solid #ddd; padding: 6px 10px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Inventario - Admin</h1>

        <asp:Label ID="lblMsg" runat="server" />

        <fieldset>
            <legend>Insertar art&iacute;culo</legend>
            <div class="row"><label>C&oacute;digo</label><asp:TextBox ID="txtCodigo" runat="server" /></div>
            <div class="row"><label>Nombre</label><asp:TextBox ID="txtNombre" runat="server" /></div>
            <div class="row"><label>Categor&iacute;a</label><asp:TextBox ID="txtCategoria" runat="server" /></div>
            <div class="row"><label>Precio compra</label><asp:TextBox ID="txtPrecioCompra" runat="server" /></div>
            <div class="row"><label>Precio venta</label><asp:TextBox ID="txtPrecioVenta" runat="server" /></div>
            <div class="row"><label>Stock</label><asp:TextBox ID="txtStock" runat="server" /></div>
            <div class="row"><label>Stock m&iacute;nimo</label><asp:TextBox ID="txtStockMin" runat="server" /></div>
            <div class="row"><label>Proveedor</label><asp:TextBox ID="txtProveedor" runat="server" /></div>
            <div class="row">
                <asp:Button ID="btnInsertar" runat="server" Text="Insertar" OnClick="btnInsertar_Click" />
                <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" Visible="false" />
            </div>
        </fieldset>

        <fieldset>
            <legend>Consultar art&iacute;culo por c&oacute;digo</legend>
            <div class="row"><label>C&oacute;digo</label><asp:TextBox ID="txtBuscarCodigo" runat="server" /></div>
            <div class="row"><asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" /></div>
            <asp:Panel ID="pnlResultado" runat="server" Visible="false">
                <h3>Resultado</h3>
                <table class="grid">
                    <tr><td>C&oacute;digo</td><td><asp:Label ID="lblRCodigo" runat="server" /></td></tr>
                    <tr><td>Nombre</td><td><asp:Label ID="lblRNombre" runat="server" /></td></tr>
                    <tr><td>Categor&iacute;a</td><td><asp:Label ID="lblRCategoria" runat="server" /></td></tr>
                    <tr><td>Precio compra</td><td><asp:Label ID="lblRPrecioCompra" runat="server" /></td></tr>
                    <tr><td>Precio venta</td><td><asp:Label ID="lblRPrecioVenta" runat="server" /></td></tr>
                    <tr><td>Stock</td><td><asp:Label ID="lblRStock" runat="server" /></td></tr>
                    <tr><td>Stock m&iacute;nimo</td><td><asp:Label ID="lblRStockMin" runat="server" /></td></tr>
                    <tr><td>Proveedor</td><td><asp:Label ID="lblRProveedor" runat="server" /></td></tr>
                    <tr><td>Fecha creaci&oacute;n</td><td><asp:Label ID="lblRFecha" runat="server" /></td></tr>
                </table>
                <asp:Label ID="lblWarn" CssClass="warn" runat="server" Visible="false" />
            </asp:Panel>
        </fieldset>
        <fieldset>
            <legend>Art&iacute;culos guardados</legend>
            <asp:GridView ID="gvArticulos" runat="server" CssClass="grid" AutoGenerateColumns="true"
                DataKeyNames="Codigo" OnRowCommand="gvArticulos_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:Button ID="btnEditarRow" runat="server" Text="Editar"
                                CommandName="EditItem" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                            <asp:Button ID="btnEliminarRow" runat="server" Text="Eliminar"
                                CommandName="DeleteItem" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                OnClientClick="return confirm('¿Eliminar este artículo?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
    </form>
</body>
</html>
