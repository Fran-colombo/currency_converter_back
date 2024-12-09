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


        public ICollection<Currency>? GetActiveCurrencies()
        {
            return _context.Currencies.Where(c => c.ConvertionIndex > 0).ToList();
        }

        public Currency? GetCurrencyByCode(string code)
        {
            return _context.Currencies.SingleOrDefault(c => c.Code == code);

        }

        public string CreateCurrency(Currency currency)
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
                _context.Currencies.Remove(currencyToDelete);
                _context.SaveChanges();
            }
        }


        public void UpdateCurrencyIC(string currency, float Ic)
        {

            Currency? currencyToUpdate = _context.Currencies.SingleOrDefault(c => c.Code == currency);
            currencyToUpdate.ConvertionIndex = Ic;
            _context.SaveChanges();
        }
    }

}
