import { useState } from "react";

export default function InsertarArticulo() {
  const [articulo, setArticulo] = useState({
    Codigo: "",
    Nombre: "",
    Categoria: "",
    PrecioCompra: "",
    PrecioVenta: "",
    Stock: "",
    StockMinimo: "",
    Proveedor: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setArticulo({ ...articulo, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await fetch("http://localhost:3000/api/articulos", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(articulo),
      });

      const data = await res.json();
      if (!data.ok) throw new Error(data.error || "Error desconocido");

      alert("✅ Artículo insertado correctamente");
      setArticulo({
        Codigo: "",
        Nombre: "",
        Categoria: "",
        PrecioCompra: "",
        PrecioVenta: "",
        Stock: "",
        StockMinimo: "",
        Proveedor: "",
      });
    } catch (err: any) {
      alert("❌ Error al insertar: " + err.message);
    }
  };

  return (
    <div
      style={{
        background: "white",
        padding: "1.5rem",
        borderRadius: "12px",
        boxShadow: "0 2px 6px rgba(0,0,0,0.1)",
      }}
    >
      <h2 style={{ color: "#1e40af", marginBottom: "1rem" }}>
        🧱 Registrar Artículo
      </h2>
      <form onSubmit={handleSubmit}>
        {[
          "Codigo",
          "Nombre",
          "Categoria",
          "Proveedor",
          "PrecioCompra",
          "PrecioVenta",
          "Stock",
          "StockMinimo",
        ].map((campo) => (
          <div key={campo} style={{ marginBottom: "0.8rem" }}>
            <label
              style={{
                display: "block",
                fontWeight: 600,
                marginBottom: "4px",
                color: "#374151",
              }}
            >
              {campo}:
            </label>
            <input
              name={campo}
              type={
                [
                  "PrecioCompra",
                  "PrecioVenta",
                  "Stock",
                  "StockMinimo",
                ].includes(campo)
                  ? "number"
                  : "text"
              }
              value={(articulo as any)[campo]}
              onChange={handleChange}
              required={campo !== "Categoria" && campo !== "Proveedor"}
              style={{
                width: "100%",
                padding: "8px",
                borderRadius: "8px",
                border: "1px solid #d1d5db",
                outlineColor: "#2563eb",
              }}
            />
          </div>
        ))}
        <button
          type="submit"
          style={{
            width: "100%",
            background: "#2563eb",
            color: "white",
            padding: "10px",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer",
            fontWeight: 600,
            marginTop: "0.5rem",
          }}
        >
          Guardar Artículo
        </button>
      </form>
    </div>
  );
}
