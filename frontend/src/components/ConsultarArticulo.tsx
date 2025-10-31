import { useState } from "react";
import { XMLParser } from "fast-xml-parser";

export default function ConsultarArticulo() {
  const [codigo, setCodigo] = useState("");
  const [articulo, setArticulo] = useState<any>(null);

  const handleBuscar = async () => {
    try {
      const res = await fetch(`http://localhost:3000/api/articulos/${codigo}`);
      if (!res.ok) throw new Error("Error en la consulta");
      const xml = await res.text();

      const parser = new XMLParser({ ignoreAttributes: false });
      const json = parser.parse(xml);

      const data =
        json["s:Envelope"]["s:Body"]["GetArticuloPorCodigoResponse"][
          "GetArticuloPorCodigoResult"
        ];

      setArticulo({
        codigo: data["a:Codigo"],
        nombre: data["a:Nombre"],
        categoria: data["a:Categoria"],
        precioCompra: data["a:PrecioCompra"],
        precioVenta: data["a:PrecioVenta"],
        stock: data["a:Stock"],
        stockMinimo: data["a:StockMinimo"],
        proveedor: data["a:Proveedor"],
        fechaCreacion: data["a:FechaCreacion"],
      });
    } catch {
      alert("❌ Error al consultar el artículo");
    }
  };

  return (
    <div
      style={{
        background: "#1e293b",
        color: "#f1f5f9",
        padding: "1.5rem",
        borderRadius: "12px",
        boxShadow: "0 2px 6px rgba(0,0,0,0.2)",
      }}
    >
      <h2 style={{ color: "#38bdf8", marginBottom: "1rem" }}>
        🔍 Consultar Artículo
      </h2>
      <div style={{ display: "flex", gap: 10, marginBottom: "1rem" }}>
        <input
          value={codigo}
          onChange={(e) => setCodigo(e.target.value)}
          placeholder="Código del artículo"
          style={{
            flex: 1,
            padding: "8px",
            borderRadius: "8px",
            border: "1px solid #64748b",
            background: "#0f172a",
            color: "white",
          }}
        />
        <button
          onClick={handleBuscar}
          style={{
            background: "#38bdf8",
            color: "#0f172a",
            border: "none",
            padding: "8px 12px",
            borderRadius: "8px",
            cursor: "pointer",
            fontWeight: 600,
          }}
        >
          Buscar
        </button>
      </div>

      {articulo && (
        <div
          style={{
            background: "#0f172a",
            padding: "1rem",
            borderRadius: "10px",
          }}
        >
          {Object.entries(articulo).map(([k, v]) => (
            <p key={k}>
              <b style={{ color: "#38bdf8" }}>
                {k.charAt(0).toUpperCase() + k.slice(1)}:
              </b>{" "}
              {String(v) || "—"}
            </p>
          ))}
        </div>
      )}
    </div>
  );
}
