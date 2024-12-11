using Common.Models;
using Data.Entities;

namespace Services.Interfaces
{
    public interface IConvertionService
    {
        IEnumerable<ConvertionToShowDto> GetUserConvertions(int id);
        float? MakeConvertion(int userId, MakeConvertionDto conv);
    }
}