using Common.Enum;
using Common.Models;
using Data.Entities;

namespace Services.Interfaces
{
    public interface ICurrencyService
    {
        void AddCurrency(CurrencyForDto CreationDTO);
        void DeleteCurrency(string code);
        IEnumerable<CurrencyForDto>? GetAllCurrencies();
        CurrencyForDto? GetCurrencyByCode(string code);
        //float? MakeConvertion(int userId ,MakeConvertionDto conv);
        void UpdateCurrencyByCode(string code, float Ic);
    }
}