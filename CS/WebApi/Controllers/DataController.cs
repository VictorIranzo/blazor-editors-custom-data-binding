using Contracts;
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
        public async Task<ActionResult<LoadResult>> LoadData(DataSourceLoadOptions options)
        {
            await Task.Delay(Random.Shared.Next(150, 300));
            var result = DataSourceLoader.Load(dataService.Get(), options);

            return result;
        }

        [HttpGet("GetCustomers")]
        public async Task<ActionResult<LoadResult>> GetCustomers(DataSourceLoadOptions filter)
        {
            await Task.Delay(Random.Shared.Next(150, 300));

            var data = new List<Customer>()
            {
                new Customer()
                {
                    Address = "507 - 20th Ave. E.Apt. 2A",
                    City = "Seattle",
                    CompanyName = "Alfreds Futterkiste",
                    ContactName = "Maria Anders",
                    ContactTitle = "Sales Representative",
                    Country = "USA",
                    Fax = "(206) 555-4111",
                    Phone = "(206) 555-9857",
                    PostalCode = "98122",
                    Region = "WA",
                    Total = 10,
                },
                new Customer()
                {
                    Address = "Obere Str. 57",
                    City = "Berlin",
                    CompanyName = "Around the Horn",
                    ContactName = "Thomas Hardy",
                    ContactTitle = "Sales Representative",
                    Country = "Germany",
                    Fax = "030-0076545",
                    Phone = "030-0074321",
                    PostalCode = "12209",
                    Total = 5,
                },
            };

            var result = DataSourceLoader.Load(data, filter);

            return result;
        }

        [HttpPost]
        public async Task<WebApiLookup> CreateAsync(WebApiLookup webApiLookup)
        {
            await Task.Delay(Random.Shared.Next(150, 300));
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
