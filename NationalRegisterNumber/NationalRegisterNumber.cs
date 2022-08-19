﻿namespace NationalRegisterNumber;

public static class NationalRegisterNumber
{
    private const int Divisor = 97;
    private const int FollowNumberMin = 1;
    private const int FollowNumberMax = 998;

    private static readonly DateTime AbsoluteMinDate = new(1900, 1, 1);
    private static readonly Random Randomizer = new();

    /// <summary>
    /// Validates strings if it's a valid national register number for Belgium.
    /// </summary>
    /// <param name="nationalRegisterNumber"></param>
    /// <returns>true if it's valid, otherwise false</returns>
    public static bool IsValid(string nationalRegisterNumber)
    {
        // Sanitize input
        if (string.IsNullOrEmpty(nationalRegisterNumber))
            return false;

        // Filter input
        string numbersOnly = new(nationalRegisterNumber.Trim().Where(x => char.IsDigit(x)).ToArray());

        // Check null and length
        if (string.IsNullOrEmpty(numbersOnly) || numbersOnly.Length != 11)
            return false;

        // Check control number
        if (!long.TryParse(nationalRegisterNumber[..9], out var dividend))
            return false;

        var remainder = dividend % Divisor;
        var controlNumber = Divisor - remainder;

        if (!long.TryParse(nationalRegisterNumber[9..], out var actualControlNumber))
            return false;

        // Born before 2000/01/01
        if (controlNumber == actualControlNumber)
            return true;

        if (!long.TryParse($"2{nationalRegisterNumber[..9]}", out dividend))
            return false;

        remainder = dividend % Divisor;
        controlNumber = Divisor - remainder;

        // Born after 1999/12/31
        return controlNumber == actualControlNumber;
    }

    public static string Generate(DateTime birthDate, int followNumber)
    {
        // Sanitize
        if (birthDate < AbsoluteMinDate)
            throw new ArgumentException($"Birthdate can't be before {AbsoluteMinDate.ToShortDateString()}", nameof(birthDate));

        if (birthDate > DateTime.Today)
            throw new ArgumentException($"Date of birth should be smaller than today {DateTime.Today.ToShortDateString()}", nameof(birthDate));

        if (followNumber < FollowNumberMin || followNumber > FollowNumberMax)
            throw new ArgumentException("Follow number should be (inclusive) between 1 and 998", nameof(followNumber));

        // Calculate control number
        var birthDatePart = birthDate.ToString("yyMMdd");
        var followNumberPart = followNumber.ToString().PadLeft(3, '0');

        long dividend;
        if (birthDate.Year > 1999)
            dividend = long.Parse($"2{birthDatePart}{followNumberPart}");
        else
            dividend = long.Parse($"{birthDatePart}{followNumberPart}");

        var remainder = dividend % Divisor;
        var controlNumber = Divisor - remainder;
        var controlNumberPart = controlNumber.ToString().PadLeft(2, '0');

        return $"{birthDatePart}{followNumberPart}{controlNumberPart}";
    }

    public static string Generate() => Generate(GenerateBirthDate(), Randomizer.Next(FollowNumberMin, FollowNumberMax));
    public static string Generate(DateTime birthDate) => Generate(birthDate, Randomizer.Next(FollowNumberMin, FollowNumberMax));
    public static string Generate(BiologicalSex sex) => Generate(GenerateBirthDate(), GenerateFollowNumber(sex));
    public static string Generate(DateTime birthDate, BiologicalSex sex) => Generate(birthDate, GenerateFollowNumber(sex));

    private static int GenerateFollowNumber(BiologicalSex sex)
    {
        var followNumber = Randomizer.Next(FollowNumberMin, FollowNumberMax);
        if (sex == BiologicalSex.Female)
        {
            if (followNumber % 2 != 0)
                followNumber++;
        }
        else
        {
            if (followNumber % 2 == 0)
                followNumber--;
        }
        return followNumber;
    }

    private static DateTime GenerateBirthDate()
    {
        var range = (DateTime.Today - AbsoluteMinDate).Days;
        return AbsoluteMinDate.AddDays(Randomizer.Next(range));
    }
}
