using Common.Exceptions;
using Common.Models;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.VisualBasic;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Exceptions.ConvertionException;

namespace Services.Implementations
{
    public class ConvertionService : IConvertionService
    {
        public IConvertionsRepository _repository;
        public IUserRepository _userRepository;
        public ICurrencyRepository _curRepository;

        public ConvertionService(IConvertionsRepository repository, IUserRepository userRepository, ICurrencyRepository curRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _curRepository = curRepository;
        }


        public IEnumerable<ConvertionToShowDto>? GetUserConvertions(int id)
        {
            try
            {
                var convertions = _repository.GetUserConvertions(id);
                if (convertions != null)
                {
                    List<ConvertionToShowDto> convertionList = new List<ConvertionToShowDto>();

                    foreach (var convertion in convertions)
                    {
                        var newConvertion = new ConvertionToShowDto
                        {
                            Username = convertion.User.Username, 
                            Code1 = convertion.FromCurrency.Code, 
                            Code2 = convertion.ToCurrency.Code,  
                            Amount = convertion.Amount,
                            Result = convertion.ToCurrency.Symbol + convertion.ConvertedAmount,
                            Date = convertion.Date,
                        };
                        convertionList.Add(newConvertion);
                    }

                    return convertionList;
                }
                else
                {
                    throw new UserNotFoundException("The user id you are looking for doesn´t exist");
                }
            }
            catch (Exception ex)
            {
                throw new NotAbleGetUserConvertions("We could not get the historial of convertions");
            }
        }

        public IEnumerable<ConvertionToShowDto>? getUserConvertionsForMonth(string username, int month)
        {
            if (1 <= month && month <= 12)
            {
                try
                {
                    var convertions = _repository.getConvertionsForMonth(username, month);
                    if (convertions != null)
                    {
                        List<ConvertionToShowDto> convertionList = new List<ConvertionToShowDto>();

                        foreach (var convertion in convertions)
                        {
                            var newConvertion = new ConvertionToShowDto
                            {
                                Username = convertion.User.Username,
                                Code1 = convertion.FromCurrency.Code,
                                Code2 = convertion.ToCurrency.Code,
                                Amount = convertion.Amount,
                                Result = convertion.ToCurrency.Symbol + convertion.ConvertedAmount,
                                Date = convertion.Date,
                            };
                            convertionList.Add(newConvertion);
                        }

                        return convertionList;
                    }
                    else
                    {
                        throw new UserNotFoundException("The user id you are looking for doesn´t exist");
                    }
                }
                catch (Exception ex)
                {
                    throw new NotAbleGetUserConvertions("We could not get the historial of convertions");
                }
            }
            else
            {
                throw new Exception("You only have 12 month, come on, you are betten than this");
            }
            
        }

        public IEnumerable<ConvertionToShowDto>? getUserConvertionsForMonth(int userId, int month)
        {
            if (1 <= month && month <= 12)
            {
                try
                {
                    var convertions = _repository.getConvertionsForMonth(userId, month);
                    if (convertions != null)
                    {
                        List<ConvertionToShowDto> convertionList = new List<ConvertionToShowDto>();

                        foreach (var convertion in convertions)
                        {
                            var newConvertion = new ConvertionToShowDto
                            {
                                Username = convertion.User.Username,
                                Code1 = convertion.FromCurrency.Code,
                                Code2 = convertion.ToCurrency.Code,
                                Amount = convertion.Amount,
                                Result = convertion.ToCurrency.Symbol + convertion.ConvertedAmount,
                                Date = convertion.Date,
                            };
                            convertionList.Add(newConvertion);
                        }

                        return convertionList;
                    }
                    else
                    {
                        throw new UserNotFoundException("The user id you are looking for doesn´t exist");
                    }
                }
                catch (Exception ex)
                {
                    throw new NotAbleGetUserConvertions("We could not get the historial of convertions");
                }
            }
            else
            {
                throw new Exception("You only have 12 month, come on, you are betten than this");
            }

        }


        public bool canConvert(int userId)
        {
            try
            {
                User user = _userRepository.GetUserById(userId)!;
                int userConv = _repository.GetConvertionsByMonths(userId);
                if (userConv < user.Subscription.MaxConversions)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (UserNotFoundException ex)
            {
                throw ex;
            }
        }




        public string? MakeConvertion(int userId, MakeConvertionDto conv)

        {
            User user = _userRepository.GetUserById(userId)!;
            bool cancon = canConvert(userId);
            if (cancon == true) {
                Currency sourceCurrency = _curRepository.GetCurrencyByCode(conv.Code1)!;
                Currency targetCurrency = _curRepository.GetCurrencyByCode(conv.Code2)!;
                float amount = conv.Amount;
                if (amount > 0 && sourceCurrency.isActive == true && targetCurrency.isActive == true)
                {
                    try
                    {
                        float convertedOutput = amount * (sourceCurrency.ConvertionIndex / targetCurrency.ConvertionIndex);

                        //_userRepository.SumConversion();

                        Convertions newConversion = new Convertions
                        {
                            User = user,
                            FromCurrency = sourceCurrency,
                            ToCurrency = targetCurrency,
                            Amount = amount,
                            ConvertedAmount = convertedOutput,
                            Date = DateTime.UtcNow
                        };

                        if (newConversion.Date == DateTime.MinValue)
                        {
                            throw new InvalidOperationException("La fecha de la conversión no se ha asignado correctamente.");
                        }
                        else
                        {
                            _repository.AddConversion(newConversion);

                            return targetCurrency.Symbol + convertedOutput;
                        }
                    }

                    catch (SomethingWentWrongInTheConvertionException)
                    {
                        throw;
                    }
                }
                else if (amount <= 0)
                {
                    throw new InvalidOperationException($"You can´t have an amount lowet than 0, so how are you going to calculate: {amount}");
                }
                else if(sourceCurrency.isActive == false)
                {
                    throw new CurrencyNotExistException($"{sourceCurrency} does not exist");
                }
                else
                {
                    throw new CurrencyNotExistException($"{targetCurrency} does not exist");
                }
            }
            else
            {
                return null;
            }
        }
    }
}
