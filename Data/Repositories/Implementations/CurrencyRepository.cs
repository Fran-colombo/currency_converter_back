using Common.Enum;
using Common.Models;
using Data;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Implementations.CurrencyRepository;

namespace Services.Implementations
{
    public class CurrencyRepository : ICurrencyRepository
    {

        private readonly CurrencyContext _context;
        public CurrencyRepository(CurrencyContext context)
        {
            _context = context;
        }

        public ICollection<Currency>? GetAllCurrencies()
        {
            return _context.Currencies.Where(c => c.ConvertionIndex > 0).ToList();
        }
        public ICollection<Currency>? GetActiveCurrencies()
        {
            return _context.Currencies.Where(c => c.ConvertionIndex > 0 && c.isActive == true).ToList();
        }

        public Currency? GetCurrencyByCode(string code)
        {
            return _context.Currencies.SingleOrDefault(c => c.Code == code);

        }

        public string AddCurrency(Currency currency)
        {

                _context.Currencies.Add(currency);
                _context.SaveChanges();
                return currency.Code;
        }
        public void DeleteCurrency(string code)
        {
            Currency? currencyToDelete = _context.Currencies.SingleOrDefault(c => c.Code == code);
            if (currencyToDelete != null)
            {
                currencyToDelete.isActive = false;
                _context.SaveChanges();
            }
        }


        public void UpdateCurrencyIC(string currency, float Ic)
        {

            Currency? currencyToUpdate = _context.Currencies.SingleOrDefault(c => c.Code == currency);
            currencyToUpdate.ConvertionIndex = Ic;
            _context.SaveChanges();
        }
        public void ActivateCurrency(Currency currency)
        {

            Currency? currencyToUpdate = _context.Currencies.SingleOrDefault(c => c.Code == currency.Code);
            currencyToUpdate.Legend = currency.Legend;
            currencyToUpdate.Symbol = currency.Symbol;
            currencyToUpdate.ConvertionIndex = currency.ConvertionIndex;
            currencyToUpdate.isActive = true;
            _context.SaveChanges();
        }
    }

}
