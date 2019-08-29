﻿using Microsoft.AspNetCore.Mvc;

namespace VersioningOptions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public string Get(ApiVersion apiVersion) 
            => $"Controller = {GetType().Name}\nVersion = {apiVersion}";

        //[HttpGet]
        //public string Get2(ApiVersion apiVersion)
        //    => $"Controller = {GetType().Name}\nVersion = {apiVersion}";
    }
}
