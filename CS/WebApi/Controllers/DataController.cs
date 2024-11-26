﻿using Contracts;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : Controller
    {
        private readonly DataService dataService;

        public DataController(DataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet("LoadData")]
        public ActionResult<LoadResult> LoadData(DataSourceLoadOptions options)
        {
            var result = DataSourceLoader.Load(dataService.Get(), options);

            return result;
        }

        [HttpPost]
        public WebApiLookup Create(WebApiLookup webApiLookup)
        {
            dataService.Create(webApiLookup);
            return webApiLookup;
        }

        [HttpDelete("{tag}")]
        public void Delete(string tag)
        {
            dataService.Delete(tag);
        }

        [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
        public class DataSourceLoadOptions : DataSourceLoadOptionsBase
        {
        }

        public class DataSourceLoadOptionsBinder : IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                var loadOptions = new DataSourceLoadOptions();
                DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
                bindingContext.Result = ModelBindingResult.Success(loadOptions);
                return Task.CompletedTask;
            }
        }
    }
}
