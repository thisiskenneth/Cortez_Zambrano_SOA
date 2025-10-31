using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using FerreteriaSoap.Entities;
using FerreteriaSoap.Services;

namespace FerreteriaSoap
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IService1
    {
        private readonly InventarioService inventario = new InventarioService();

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void InsertArticulo(Articulo articulo)
        {
            try
            {
                inventario.InsertarAsync(articulo).GetAwaiter().GetResult();
            }
            catch (ArgumentException ex)
            {
                throw new FaultException<string>(ex.Message, new FaultReason("Validación de datos"));
            }
            catch (InvalidOperationException ex)
            {
                throw new FaultException<string>(ex.Message, new FaultReason("Negocio"));
            }
            catch (Exception ex)
            {
                throw new FaultException<string>("Error interno del servidor", new FaultReason(ex.Message));
            }
        }

        public Articulo GetArticuloPorCodigo(string codigo)
        {
            try
            {
                var art = inventario.ObtenerPorCodigoAsync(codigo).GetAwaiter().GetResult();
                if (art == null)
                    throw new FaultException<string>("Artículo no encontrado", new FaultReason("No existe"));
                return art;
            }
            catch (ArgumentException ex)
            {
                throw new FaultException<string>(ex.Message, new FaultReason("Validación de datos"));
            }
            catch (Exception ex)
            {
                throw new FaultException<string>("Error interno del servidor", new FaultReason(ex.Message));
            }
        }
    }
}
