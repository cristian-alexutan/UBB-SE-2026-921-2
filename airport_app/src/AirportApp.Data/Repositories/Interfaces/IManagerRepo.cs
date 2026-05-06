using System.Collections.Generic;
using AirportApp.Data.Domain;

namespace AirportApp.Data.Repositories.Interfaces
{
    public interface IManagerRepo
    {
        IEnumerable<Manager> GetAll();

        Manager GetById(int managerId);

        void Add(Manager manager);

        Manager? Delete(int managerId);

        Manager? Update(Manager manager);
    }
}
