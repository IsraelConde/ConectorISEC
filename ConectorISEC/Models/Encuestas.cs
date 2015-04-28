using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConectorISEC.Models
{
    public class Encuestas
    {
        /// <summary>
        /// Devuelve si existe alguna encuesta por responder del usuario
        /// </summary>
        public bool getEncuestas()
        {
            List<string> encuestas = new List<string>();
            using (OrganizationServiceProxy _serviceProxy = auxiliar.getProxy())
            {
                _serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                OrganizationServiceContext serviceContext = new OrganizationServiceContext(service);

                //Verificamos que no hay ninguna encuesta para responder (si la hay el portal se bloquea)
                encuestas = serviceContext.CreateQuery<Account>().
                    Where(em => em.EMailAddress1 == null/*verificar si el campo de las encuestas están como no contestado*/).
                    Select(em => em.EMailAddress1).ToList();
            }

            if (encuestas.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}