using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IConvertionsRepository
    {
        void AddConversion(Convertions convertion);
        IEnumerable<Convertions> GetUserConvertions(int id);
        int GetConvertionsByMonths(int id);
    }
}