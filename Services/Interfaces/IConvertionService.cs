using Common.Models;
using Data.Entities;

namespace Services.Interfaces
{
    public interface IConvertionService
    {
        IEnumerable<ConvertionToShowDto> GetUserConvertions(int id);
        bool canConvert(int userId);
        string? MakeConvertion(int userId, MakeConvertionDto conv);
    }
}