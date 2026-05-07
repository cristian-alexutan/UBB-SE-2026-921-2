using System.Collections.Generic;
using AirportAPI.Domain;

namespace AirportAPI.Repositories.Interfaces
{
    public interface IManagerRepo
    {
        IEnumerable<Manager> GetAll();

        Manager? GetById(int managerId);

        void Add(Manager manager);

        Manager? Delete(int managerId);

        Manager? Update(Manager manager);
    }
}


