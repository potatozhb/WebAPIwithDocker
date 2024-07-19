using Microsoft.AspNetCore.Mvc;
using WebDockerAPI.Models;

namespace WebDockerAPI.Interface
{
    public interface ICountryRepo
    {
        Country? GetCountry(int id);
        ICollection<Country> GetCountries();
        bool AddCountry(Country data);
        bool UpdateCountry(Country data);
        bool DeleteCountry(Country data);
        bool IsExist(int cid);
    }
}
