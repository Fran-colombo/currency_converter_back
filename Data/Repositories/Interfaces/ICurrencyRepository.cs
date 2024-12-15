using Common.Enum;
using Common.Models;
using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        ICollection<Currency>? GetAllCurrencies();
        string AddCurrency(Currency currency);
        void DeleteCurrency(string code);
        ICollection<Currency>? GetActiveCurrencies();
        Currency? GetCurrencyByCode(string code);
        void UpdateCurrencyIC(string currency, float Ic);
        void ActivateCurrency(Currency currency);
        //Currency? GetCurrency(CurrencyUpdateRequestDto dto);
    }
}