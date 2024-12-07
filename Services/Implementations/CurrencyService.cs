using Common.Enum;
using Common.Models;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class CurrencyService : ICurrencyService
    {

        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICurrencyRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public CurrencyService(IUserRepository userRepository,
                               ISubscriptionRepository subscriptionRepository,
                               ICurrencyRepository currencyRepository,
                               IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _repository = currencyRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public string AddCurrency(CurrencyForDto CreationDTO)
        {
            if (!_repository.GetActiveCurrencies().Any(e => e.Code == CreationDTO.Code))
            {
                var newCurrency = _repository.CreateCurrency(
                new Currency
                {
                    Code = CreationDTO.Code,
                    Legend = CreationDTO.Legend,
                    Symbol = CreationDTO.Symbol,
                    ConvertionIndex = CreationDTO.ConvertionIndex,
                });

                return newCurrency;
            }
            else
            {
                throw new ArgumentException("The Currency already exists.");
            }

        }
        public void DeleteCurrency(string code)
        {
            var curToDelete = _repository.GetCurrencyByCode(code);
            if(curToDelete == null)
            {
                throw new ArgumentException("The currency does not exist");
            }

            _repository.DeleteCurrency(code);
        }
        public void UpdateCurrencyByCode(string code, float Ic)
        {
            var CurrencyToUpdate = _repository.GetCurrencyByCode(code);
            if (CurrencyToUpdate == null)
            {
                throw new ArgumentException("Code not found");
            }


            _repository.UpdateCurrencyIC(code, Ic);

        }

        public IEnumerable<CurrencyForDto>? GetAllCurrencies()
        {
            var currencyList = _repository.GetActiveCurrencies();
            List<CurrencyForDto> currenciesForView = new List<CurrencyForDto>();
            foreach(var Currency in currencyList)
            {
                var currencyDto = new CurrencyForDto
                {
                    Code = Currency.Code,
                    Legend = Currency.Legend,
                    Symbol = Currency.Symbol,
                    ConvertionIndex = Currency.ConvertionIndex,
                };
                currenciesForView.Add(currencyDto);
            }
            return currenciesForView;
        }

        public CurrencyForDto? GetCurrencyByCode(string code)
        {
            var curr = _repository.GetCurrencyByCode(code);
            if (curr == null)
            {
                throw new ArgumentException("The code you are looking for doens´t exist.");
            }
            var currForView = new CurrencyForDto
            {
                Code = curr.Code,
                Legend = curr.Legend,
                Symbol = curr.Symbol,
                ConvertionIndex = curr.ConvertionIndex,
            };

            return currForView;
        }




        public float? MakeConvertion(int userId, MakeConvertionDto conv)
       
        {
            User user = _userRepository.GetUserById(userId)!;
            if (user.conversions < user.Subscription.MaxConversions)
            {
                Currency sourceCurrency = _repository.GetCurrencyByCode(conv.Code1)!;
                Currency targetCurrency = _repository.GetCurrencyByCode(conv.Code2)!;
                float convertedAmount = conv.Amount;

                float convertedOutput = convertedAmount * (targetCurrency.ConvertionIndex / sourceCurrency.ConvertionIndex);

                _userRepository.SumConversion();

                return convertedOutput;
            }
            else
            {
                return null;
            }
        }

        //    var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


        //    var user = _userRepository.GetUserById(userId);
        //    if (user.conversions < user.Subscription.MaxConversions)
        //    {

        //        float? cur1 = _repository.GetCurrencyByCode(conv.Code1)?.ConvertionIndex;
        //        float? cur2 = _repository.GetCurrencyByCode(conv.Code2)?.ConvertionIndex;

        //        if (cur1 == null || cur2 == null)
        //        {
        //            throw new ArgumentException("You have to fulfill both currencies camps");
        //        }
        //        else if (conv.Amount < 0)
        //        {
        //            throw new ArgumentException("You can´t convert negative values");
        //        }
        //        float? amountInC2 = conv.Amount * (cur1 / cur2);
        //        _userRepository.SumConversion();

        //        return amountInC2;
        //    }
        //     throw new ArgumentException("You can´t make more convertions");
        //}





    }
}
