// index.js — versión mejorada con manejo de errores SOAP
const express = require("express");
const cors = require("cors");
const axios = require("axios");

const app = express();
app.use(cors());
app.use(express.json());

// URL del servicio SOAP (ajústala según tu entorno IIS)
const SOAP_URL = "http://10.254.86.117:8080/Service1.svc";

// Helper: construye envelope SOAP 1.1
const soapEnvelope = (body) => `<?xml version="1.0" encoding="utf-8"?>
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    ${body}
  </s:Body>
</s:Envelope>`;

/* ================================
   🔹 ENDPOINT: Insertar Artículo
================================ */
app.post("/api/articulos", async (req, res) => {
  try {
    const a = req.body;
    const body = `
<InsertArticulo xmlns="http://tempuri.org/">
  <articulo xmlns:d4p1="http://schemas.datacontract.org/2004/07/FerreteriaSoap.Entities">
    <d4p1:Codigo>${a.Codigo ?? ""}</d4p1:Codigo>
    <d4p1:Nombre>${a.Nombre ?? ""}</d4p1:Nombre>
    <d4p1:Categoria>${a.Categoria ?? ""}</d4p1:Categoria>
    <d4p1:PrecioCompra>${a.PrecioCompra ?? 0}</d4p1:PrecioCompra>
    <d4p1:PrecioVenta>${a.PrecioVenta ?? 0}</d4p1:PrecioVenta>
    <d4p1:Stock>${a.Stock ?? 0}</d4p1:Stock>
    <d4p1:StockMinimo>${a.StockMinimo ?? 0}</d4p1:StockMinimo>
    <d4p1:Proveedor>${a.Proveedor ?? ""}</d4p1:Proveedor>
    <d4p1:FechaCreacion>${
      a.FechaCreacion ?? new Date().toISOString()
    }</d4p1:FechaCreacion>
  </articulo>
</InsertArticulo>`;

    const xml = soapEnvelope(body);

    const { data } = await axios.post(SOAP_URL, xml, {
      headers: {
        "Content-Type": "text/xml; charset=utf-8",
        SOAPAction: "http://tempuri.org/IService1/InsertArticulo",
      },
      timeout: 20000,
    });

    // ✅ Éxito
    console.log("✅ Artículo insertado correctamente:", a.Codigo);
    res.json({ ok: true, raw: data });
  } catch (e) {
    // Manejo de errores SOAP
    const xml = e.response?.data || "";
    const match =
      xml.match(/<detail[^>]*>(.*?)<\/detail>/) ||
      xml.match(/<faultstring[^>]*>(.*?)<\/faultstring>/);
    const message = match ? match[1] : e.message;

    console.error("❌ Error en el servidor SOAP:", message);
    res.status(400).json({ ok: false, error: message });
  }
});

/* ================================
   🔹 ENDPOINT: Consultar Artículo
================================ */
app.get("/api/articulos/:codigo", async (req, res) => {
  try {
    const codigo = req.params.codigo;
    const body = `
<GetArticuloPorCodigo xmlns="http://tempuri.org/">
  <codigo>${codigo}</codigo>
</GetArticuloPorCodigo>`;

    const xml = soapEnvelope(body);

    const { data } = await axios.post(SOAP_URL, xml, {
      headers: {
        "Content-Type": "text/xml; charset=utf-8",
        SOAPAction: "http://tempuri.org/IService1/GetArticuloPorCodigo",
      },
      timeout: 20000,
    });

    console.log("🔎 Artículo consultado:", codigo);
    res.type("text/xml").send(data);
  } catch (e) {
    const xml = e.response?.data || "";

    // Busca el mensaje dentro de <detail> o <faultstring>
    const match =
      xml.match(/<detail[^>]*>(.*?)<\/detail>/) ||
      xml.match(/<faultstring[^>]*>(.*?)<\/faultstring>/);

    let message = match ? match[1] : e.message;

    // 🔹 Limpia etiquetas XML y espacios innecesarios
    message = message
      .replace(/<[^>]+>/g, "") // elimina <tags>
      .replace(/\r?\n|\r/g, "") // quita saltos de línea
      .trim(); // quita espacios

    console.error("❌ Error en el servidor SOAP:", message);
    res.status(400).json({ ok: false, error: message });
  }

});

// ================================
// 🚀 Servidor
// ================================
app.listen(3000, () => {
  console.log("✅ Proxy SOAP en http://localhost:3000");
});
