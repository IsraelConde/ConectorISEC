using System;
using System.Configuration;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Client;

namespace ConectorISEC
{
    public sealed class auxiliar
    {
        #region variables globales
        
        private static string UserName = ConfigurationManager.AppSettings["UserCrm"];
        private static string Password = ConfigurationManager.AppSettings["PssCrm"];
        private static string dominio = ConfigurationManager.AppSettings["Dominio"];
        private static string serverAddress = ConfigurationManager.AppSettings["ServerAddresCrm"];
        private static string DiscoreryUri = ConfigurationManager.AppSettings["DiscoveryUri"];
        private static string OrganizationUri = ConfigurationManager.AppSettings["OrganizationUri"];
        private static string HomeRealmUri = "";

        private ClientCredentials credentials = new ClientCredentials();
        //private OrganizationServiceProxy _serviceProxy;
        private static bool _ocupado;
        private static object sync=new object();
        private static auxiliar _auxiliar;
        
        #endregion

        
        public OrganizationServiceProxy Proxy
        {
            get 
            { 
                return getProxy(); 
            }//this._serviceProxy; }
        }

        private static string getConnectionString()
        {
            string con = string.Empty;
            con = @"Url=" + OrganizationUri + ";"
                + "Username=" + UserName + ";"
                + "DeviceID=MIDEVICEID-aa9f617b2e6d;"
                + "DevicePassword=MiDevicePass@word;"
                + "Password=" + Password;


            return con;
        }

        public static OrganizationServiceProxy getProxy()
        {            
            IServiceManagement<IOrganizationService> serviceManagement =
                ServiceConfigurationFactory.CreateManagement<IOrganizationService>(new Uri(OrganizationUri));
            AuthenticationProviderType endpointType = serviceManagement.AuthenticationType;
            if (endpointType == AuthenticationProviderType.LiveId) 
            {
                var connectionString = getConnectionString();

                var connection = CrmConnection.Parse(connectionString);
                OrganizationService servicio = new OrganizationService(connection);
                IOrganizationService _service = (IOrganizationService)servicio.InnerService;
                OrganizationServiceProxy _proxy = (OrganizationServiceProxy)_service;
                return _proxy;
            }
            else if (endpointType == AuthenticationProviderType.ActiveDirectory) 
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.Windows.ClientCredential = new System.Net.NetworkCredential(UserName, Password, dominio);
                OrganizationServiceProxy _proxy = new OrganizationServiceProxy(new Uri(OrganizationUri), null, credentials, null);
                return _proxy;
            }
            else //esta será la que realmente se utilice en el caso del nuevo Crm online
            {
                AuthenticationCredentials authCredentials = new AuthenticationCredentials();
                authCredentials.ClientCredentials.UserName.UserName = UserName;
                authCredentials.ClientCredentials.UserName.Password = Password;

                OrganizationServiceProxy _proxy = GetProxy<IOrganizationService, OrganizationServiceProxy>(serviceManagement, authCredentials);
                return _proxy;
            }

        }
        private static TProxy GetProxy<TService, TProxy>(
            IServiceManagement<TService> serviceManagement,
            AuthenticationCredentials authCredentials)
            where TService : class
            where TProxy : ServiceProxy<TService>
        {
            Type classType = typeof(TProxy);
            AuthenticationCredentials tokenCredentials =
                serviceManagement.Authenticate(authCredentials);
            return (TProxy)classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse) })
                .Invoke(new object[] { serviceManagement, tokenCredentials.SecurityTokenResponse });
        }        
    }
}
