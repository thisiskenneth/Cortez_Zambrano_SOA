export interface Articulo {
  Codigo: string;
  Nombre: string;
  Descripcion?: string;
  CategoriaID: string;
  ProveedorID?: string;
  PrecioCompra: string;
  PrecioVenta: string;
  StockActual: string;
  StockMinimo: string;
  Activo: string;
}

const API_URL = "http://localhost:3000/api/articulos"; // tu proxy Node

/* ================================
   INSERTAR ARTÍCULO
================================ */
export async function insertarArticulo(articulo: Articulo) {
  console.log("📦 Enviando artículo:", articulo);

  const resp = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(articulo),
  });

  const data = await resp.json();

  if (!data.ok) {
    console.error("❌ Error del servidor:", data.error);
    throw new Error(data.error || "Error al insertar artículo");
  }

  return data;
}
