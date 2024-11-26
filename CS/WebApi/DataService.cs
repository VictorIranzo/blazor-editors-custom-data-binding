using Contracts;

namespace WebApi
{
    public class DataService
    {
        private static int idCount = 0;
        private List<WebApiLookup> dataList = new List<WebApiLookup>()
        {
            new WebApiLookup(){ Id = ++idCount, Text = "A" },
            new WebApiLookup(){ Id = ++idCount, Text = "Z" },
        };

        public WebApiLookup Create(WebApiLookup webApiLookup)
        {
            webApiLookup.Id = ++idCount;
            dataList.Add(webApiLookup);

            return webApiLookup;
        }

        public void Delete(string tag)
        {
            dataList.RemoveAll(x => x.Text == tag);
        }

        public IQueryable<WebApiLookup> Get()
        {
            return dataList.AsQueryable().OrderBy(x => x.Text);
        }
    }
}
