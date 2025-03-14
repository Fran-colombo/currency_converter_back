﻿using Common.Models;
using Data.Entities;

namespace Services.Interfaces
{
    public interface IConvertionService
    {
        IEnumerable<ConvertionToShowDto> GetUserConvertions(int id);
        bool canConvert(int userId);
        string? MakeConvertion(int userId, MakeConvertionDto conv);
        IEnumerable<ConvertionToShowDto>? getUserConvertionsForMonth(string username, int month, int year);
        IEnumerable<ConvertionToShowDto>? getUserConvertionsForMonth(int userId, int month, int year);
    }
}