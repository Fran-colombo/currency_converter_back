using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IConvertionsRepository
    {
        void AddConversion(Convertions convertion);
        IEnumerable<Convertions> GetUserConvertions(int id);
        int GetConvertionsByMonths(int id);
        IEnumerable<Convertions>? getConvertionsForMonth(int userId, int month);
        IEnumerable<Convertions>? getConvertionsForMonth(string username, int month);
    }
}