using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class ConvertionsRepository : IConvertionsRepository
    {
        private readonly CurrencyContext _context;
        public ConvertionsRepository(CurrencyContext context)
        {
            _context = context;
        }

        public IEnumerable<Convertions>? GetUserConvertions(int id)
        {
            return _context.Convertions.Where(c => c.User.Id == id).Include(u => u.User).
                Include(c => c.FromCurrency).Include(c => c.ToCurrency).OrderByDescending(c => c.Date);
        }
        public void AddConversion(Convertions convertion)
        {
            if (convertion.Date == DateTime.MinValue)
            {
                throw new InvalidOperationException("La fecha de la conversión no se ha asignado correctamente.");
            }
            else
            {
                _context.Convertions.Add(convertion);
                _context.SaveChanges();
            }
        }
        //public int GetConvertionsByMonths(int id)
        //{
        //    int currentMonth = DateTime.Now.Month;
        //    int currentYear = DateTime.Now.Year;

        //    return GetUserConvertions(id)
        //        .Where(c => c.Date.Month == currentMonth && c.Date.Year == currentYear)
        //        .Count();
        //}
        public int GetConvertionsByMonths(int userId)
        {
            int currentMonth = DateTime.UtcNow.Month;
            int currentYear = DateTime.UtcNow.Year;

            return _context.Convertions.Where(c => c.User.Id == userId)
                .Where(c=> c.Date.Month == currentMonth && c.Date.Year == currentYear)
                .Count();    
        }
        public IEnumerable<Convertions>? getConvertionsForMonth(string username, int month, int year)
        {

            return _context.Convertions
                .Include(c => c.User)
                .Include(c => c.FromCurrency)
                .Include(c => c.ToCurrency)
                .Where(c => c.User.Username== username && c.Date.Month == month && c.Date.Year == year)
                .ToList();
        }

        public IEnumerable<Convertions>? getConvertionsForMonth(int userId, int month, int year)
        {
            //int currentYear = DateTime.UtcNow.Year;

            return _context.Convertions
                .Include(c => c.User)
                .Include(c => c.FromCurrency)
                .Include(c => c.ToCurrency)
                .Where(c => c.User.Id == userId && c.Date.Month == month && c.Date.Year == year)
                .ToList();
        }

    }
}
