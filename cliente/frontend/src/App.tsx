// App.tsx
import ConsultarArticulo from "./components/ConsultarArticulo";
import InsertarArticulo from "./components/InsertarArticulo";

export default function App() {
  return (
    <div
      style={{
        fontFamily: "Segoe UI, Roboto, sans-serif",
        padding: "2rem",
        background: "#f4f6f8",
        minHeight: "100vh",
      }}
    >
      <h1
        style={{
          textAlign: "center",
          color: "#1f2937",
          marginBottom: "2rem",
        }}
      >
        🛠️ Sistema de Inventario — Ferretería
      </h1>
      <div
        style={{
          display: "grid",
          gap: "2rem",
          maxWidth: 900,
          margin: "auto",
          gridTemplateColumns: "1fr 1fr",
        }}
      >
        <InsertarArticulo />
        <ConsultarArticulo />
      </div>
    </div>
  );
}
