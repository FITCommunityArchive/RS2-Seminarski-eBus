﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eBus.Model.Requests;
using eBus.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eBus.WebApi.Controllers
{

    [AllowAnonymous]
    public class KompanijaController : BaseCRUDController<Model.Kompanija, KompanijaSearchRequest, KompanijaUpsertRequest, KompanijaUpsertRequest>
    {
        public KompanijaController(ICRUDService<Model.Kompanija, KompanijaSearchRequest, KompanijaUpsertRequest, KompanijaUpsertRequest> service) : base(service)
        {
        }
    }
}