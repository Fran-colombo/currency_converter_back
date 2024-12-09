using Common.Enum;
using Common.Exceptions;
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
                try
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
                catch (Exception ex)
                {
                    throw new NotAbleCreateCurrencyException("Something went wrong creating the currency.");
                }

            }
            else
                {
                    throw new CurrencyAlreadyExistException("The Currency already exists.");
                }
        }
        public void DeleteCurrency(string code)
        {
            var curToDelete = _repository.GetCurrencyByCode(code);
            if (curToDelete != null)
            {
                try
                {
                    _repository.DeleteCurrency(code);
                }
                catch (Exception ex)
                {
                    throw new NotAbleDeleteCurrency("We couldn´t delete the currency");
                }
            }
            else
            {
                throw new CurrencyNotExistException("The currency does not exist");
            }
        }

        public void UpdateCurrencyByCode(string code, float Ic)
        {
            var CurrencyToUpdate = _repository.GetCurrencyByCode(code);
            if (CurrencyToUpdate != null & Ic > 0)
            {
                try
                {
                    _repository.UpdateCurrencyIC(code, Ic);
                }
                catch (Exception ex)
                {
                    throw new NotAbleUpdateCurrencyException("Something went wrong while we were updating the currency");
                }
            }
            else if (Ic < 0)
            {
                throw new ConvertionIndexLowerThanZeroException("You can´t have a convertion index lower than 0!");
            }
            else
            {
                throw new CurrencyNotExistException("Code not found");
            }
        }

        public IEnumerable<CurrencyForDto>? GetAllCurrencies()
        {
            try
            {
                var currencyList = _repository.GetActiveCurrencies();
                List<CurrencyForDto> currenciesForView = new List<CurrencyForDto>();
                foreach (var Currency in currencyList)
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
            catch (Exception ex)
            {
                throw new NotAbleFindCurrencyException("We couldn´t find the currencies.");
            }
        }

        public CurrencyForDto? GetCurrencyByCode(string code)
        {
            try
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
            catch (Exception ex)
            {
                throw new CurrencyNotExistException("The currency that you are looking for does not exist");
            }
        }



        public float? MakeConvertion(int userId, MakeConvertionDto conv)
       
        {
            User user = _userRepository.GetUserById(userId)!;
            if (user.conversions < user.Subscription.MaxConversions)
            {
                Currency sourceCurrency = _repository.GetCurrencyByCode(conv.Code1)!;
                Currency targetCurrency = _repository.GetCurrencyByCode(conv.Code2)!;
                float convertedAmount = conv.Amount;
                try
                {
                    float convertedOutput = convertedAmount * (targetCurrency.ConvertionIndex / sourceCurrency.ConvertionIndex);

                    _userRepository.SumConversion();

                    return convertedOutput;
                }
                catch (Exception ex)
                {
                    throw new SomethingWentWrongInTheConvertionException("Sth went wrong");
                }
            }
            else
            {
                return null;
            }
        }
    }
}
