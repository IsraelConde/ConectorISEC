using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConectorISEC.Models
{
    public class Facturas
    {
        /// <summary>
        /// Devuelve si existe alguna factura bloqueada del usuario
        /// </summary>
        public static List<Account> getFacturas()
        {
            List<Account>cuentas = new List<Account>();
            using (OrganizationServiceProxy _serviceProxy = auxiliar.getProxy())
            {
                _serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                OrganizationServiceContext serviceContext = new OrganizationServiceContext(service);

                //Verificamos que no hay ninguna factura bloqueada (si la hay el portal se cierra)
                cuentas = serviceContext.CreateQuery<Account>().
                    Where(em => em.EMailAddress1 == "someone_i@example.com").ToList();

            }

            return cuentas;
        }
    }
}