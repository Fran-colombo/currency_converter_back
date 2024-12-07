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
            var curr = _context.Currencies.SingleOrDefault(c => c.Code == code);
            try
            {
                return curr;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The currency is not able");
            }
        }

        public string CreateCurrency(Currency currency)
        {
            try
            {
                _context.Currencies.Add(currency);
                _context.SaveChanges();
                return currency.Code;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Something while we were trying to create the currency");
            }
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

            var currencyToUpdate = _context.Currencies.SingleOrDefault(c => c.Code == currency);

            if (currencyToUpdate != null)
            {
                currencyToUpdate.ConvertionIndex = Ic;
                _context.SaveChanges();
            }
        }

        //public Currency? GetCurrency(CurrencyUpdateRequestDto dto)
        //{
        //    if (dto == null) return null; 

      
        //    if (dto.Code.HasValue)
        //    {
        //        return _context.Currencies.SingleOrDefault(c => c.Code == dto.Code.Value);
        //    }

        //    if (dto.Legend.HasValue) 
        //    {
        //        return _context.Currencies.SingleOrDefault(c => c.Legend == dto.Legend.Value);
        //    }

        //    return null; 
        //}







    }

}
