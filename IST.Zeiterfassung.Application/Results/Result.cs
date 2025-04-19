using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Application.Results;

public class Result<T>
{
    public bool Success { get; }
    public string? ErrorMessage { get; }
    public T? Value { get; }

    private Result(bool success, T? value, string? errorMessage)
    {
        Success = success;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Ok(T value) => new(true, value, null);
    public static Result<T> Fail(string errorMessage) => new(false, default, errorMessage);
}

