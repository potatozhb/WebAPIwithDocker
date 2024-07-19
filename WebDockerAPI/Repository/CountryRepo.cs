using Microsoft.AspNetCore.Mvc;
using WebDockerAPI.Data;
using WebDockerAPI.Interface;
using WebDockerAPI.Models;

namespace WebDockerAPI.Repository
{
    public class CountryRepo : ICountryRepo
    {
        private readonly DataContext dataContext;

        public CountryRepo(DataContext context)
        {
            this.dataContext = context;
        }

        public bool AddCountry(Country data)
        {
            this.dataContext.Countries.Add(data);
            return this.Save();
        }

        public bool DeleteCountry(Country data)
        {
            this.dataContext.Countries.Remove(data);
            return this.Save();
        }

        public ICollection<Country> GetCountries()
        {
            return this.dataContext.Countries.OrderBy(a => a.Id).ToList();
        }

        public Country? GetCountry(int id)
        {
            return this.dataContext.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public bool IsExist(int cid)
        {
            return this.dataContext.Countries.Select(c => c.Id == cid).Any();
        }

        public bool UpdateCountry(Country data)
        {
            this.dataContext.Countries.Update(data);
            return this.Save();
        }

        private bool Save()
        {
            return this.dataContext.SaveChanges() > 0;
        }
    }
}
