using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;

namespace ConectorISEC.Models
{
    public class FacturasController : ApiController
    {
        // GET: Facturas
        public List<Account> Get()
        {
            return Models.Facturas.getFacturas();
        }
    }
}