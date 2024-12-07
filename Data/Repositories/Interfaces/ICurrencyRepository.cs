using Common.Enum;
using Common.Models;
using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        string CreateCurrency(Currency currency);
        void DeleteCurrency(string code);
        ICollection<Currency>? GetActiveCurrencies();
        Currency? GetCurrencyByCode(string code);
        void UpdateCurrencyIC(string currency, float Ic);
        //Currency? GetCurrency(CurrencyUpdateRequestDto dto);
    }
}