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
                            Username = convertion.User.Username, // Asumiendo que tienes Username en User
                            Code1 = convertion.FromCurrency.Code, // Cambia a la propiedad correcta
                            Code2 = convertion.ToCurrency.Code,   // Cambia a la propiedad correcta
                            Amount = convertion.Amount,
                            Result = convertion.ConvertedAmount,
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

    




        public float? MakeConvertion(int userId, MakeConvertionDto conv)

        {
            User user = _userRepository.GetUserById(userId)!;
            int userConv = _repository.GetConvertionsByMonths(userId);
            if (userConv < user.Subscription.MaxConversions)
            {
                Currency sourceCurrency = _curRepository.GetCurrencyByCode(conv.Code1)!;
                Currency targetCurrency = _curRepository.GetCurrencyByCode(conv.Code2)!;
                float amount = conv.Amount;
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

                        return convertedOutput;
                    }
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
