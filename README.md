# National Register Number
National Register Number is a package that can generate and validate Belgian national register numbers. The logic is based on [Official Documentation by the Belgian Government](https://www.ibz.rrn.fgov.be/fileadmin/user_upload/nl/rr/instructies/IT-lijst/IT000_Rijksregisternummer.pdf)

## Installation
TODO

## Usage
After installation you can use the static class `NationalRegisterNumberGenerator` to generate or validate your national register numbers.

**Validate**

This will return true or false based on input.

`NationalRegisterNumberGenerator.IsValid(string here)`

**Generate**

This will return a string that's a valid national register number.
Following overloads are possible on `NationalRegisterNumberGenerator.Generate()`

```
Generate()
Generate(DateTime birthDate)
Generate(BiologicalSex sex)
Generate(DateTime birthDate, BiologicalSex sex)
Generate(DateTime minDate, DateTime maxDate)
Generate(DateTime minDate, DateTime maxDate, BiologicalSex sex)
Generate(DateTime birthDate, int followNumber)
```

`followNumber` is a number (inclusive) between 1 and 998. If your parameters are invalid, it will throw an `ArgumentException` with a message why `Generate()` has failed. 
Keep in mind that the absolute `minDate` is `1900/01/01` and the absolute `maxDate` is `Datetime.Today` on the time that the code runs. If you pick a date outside this range, it will also throw an `ArgumentException`.

## !!WARNING!!
Only use the package for test and research purposes. Do **not** use it in a live/production environment. It should only be used for unit/integration testing.

