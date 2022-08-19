using System;
using FluentAssertions;
using NUnit.Framework;

namespace NationalRegisterNumberTests;

[TestFixture]
public class NationalRegisterNumberTests
{
    [TestCase("90022742191")]
    public void ValidateShouldReturnTrue(string nationalRegisterNumber)
    {
        // Act
        var result = NationalRegisterNumber.NationalRegisterNumber.IsValid(nationalRegisterNumber);

        // Assert
        result.Should().BeTrue();
    }

    [TestCase("12345678910")]
    [TestCase("Test")]
    [TestCase("00000000000")]
    [TestCase("99999999999")]
    [TestCase("!@#!@%^@^@$^&@$^@sdfasdf")]
    [TestCase("$^@#^@##$44")]
    [TestCase("15435#$%4354dfsg")]
    [TestCase("90022742192")]
    public void ValidateShouldReturnFalse(string nationalRegisterNumber)
    {
        // Act
        var result = NationalRegisterNumber.NationalRegisterNumber.IsValid(nationalRegisterNumber);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void GenerateWithBirthdateAndFollowNumberShouldWork()
    {
        // Arrange
        var birthDate = new DateTime(1990, 1, 1);
        var followNumber = 16;

        // Act
        var result = NationalRegisterNumber.NationalRegisterNumber.Generate(birthDate, followNumber);

        // Assert
        var assertion = NationalRegisterNumber.NationalRegisterNumber.IsValid(result);
        assertion.Should().BeTrue();
    }

    [Test]
    public void GenerateShouldWork()
    {
        // Act
        var result = NationalRegisterNumber.NationalRegisterNumber.Generate();

        // Assert
        var assertion = NationalRegisterNumber.NationalRegisterNumber.IsValid(result);
        assertion.Should().BeTrue();
    }

    [Test]
    public void GenerateWithOldDateShouldThrowException()
    {
        // Arrange
        var reallyOldDate = new DateTime(1889, 12, 5);

        // Act
        var result = () => NationalRegisterNumber.NationalRegisterNumber.Generate(reallyOldDate);

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Test]
    public void GenerateWithFutureDateShouldThrowException()
    {
        // Arrange
        var futureDate = DateTime.Today.AddDays(10);

        // Act
        var result = () => NationalRegisterNumber.NationalRegisterNumber.Generate(futureDate);

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [TestCase(0)]
    [TestCase(999)]
    [TestCase(10234)]
    [TestCase(-50)]
    public void GenerateWithWrongFollowNumberShouldThrowException(int followNumber)
    {
        // Arrange
        var validDate = new DateTime(1998, 1, 1);

        // Act
        var result = () => NationalRegisterNumber.NationalRegisterNumber.Generate(validDate, followNumber);

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Test]
    public void GenerateWithBirthDateShouldWork()
    {
        // Arrange
        var birthdate = new DateTime(2000, 1, 1);

        // Act
        var result = NationalRegisterNumber.NationalRegisterNumber.Generate(birthdate);

        // Assert
        var assertion = NationalRegisterNumber.NationalRegisterNumber.IsValid(result);
        assertion.Should().BeTrue();
    }

    [Test]
    public void GenerateWithBiologicalSexFemaleShouldWork()
    {
        // Act
        var result = NationalRegisterNumber.NationalRegisterNumber.Generate(NationalRegisterNumber.BiologicalSex.Female);

        // Assert
        var assertion = NationalRegisterNumber.NationalRegisterNumber.IsValid(result);
        assertion.Should().BeTrue();
        var digit = int.Parse(result[8].ToString());
        var even = digit % 2 == 0;
        even.Should().BeTrue();
    }

    [Test]
    public void GenerateWithBiologicalSexmaleShouldWork()
    {
        // Act
        var result = NationalRegisterNumber.NationalRegisterNumber.Generate(NationalRegisterNumber.BiologicalSex.Male);

        // Assert
        var assertion = NationalRegisterNumber.NationalRegisterNumber.IsValid(result);
        assertion.Should().BeTrue();
        var digit = int.Parse(result[8].ToString());
        var even = digit % 2 != 0;
        even.Should().BeTrue();
    }

    [Test]
    public void GenerateWithBirthDateAndBiologicalSexShouldWork()
    {
        // Arrange
        var birthdate = new DateTime(2000, 1, 1);
        var sex = NationalRegisterNumber.BiologicalSex.Male;

        // Act
        var result = NationalRegisterNumber.NationalRegisterNumber.Generate(birthdate, sex);

        // Assert
        var assertion = NationalRegisterNumber.NationalRegisterNumber.IsValid(result);
        assertion.Should().BeTrue();
        var digit = int.Parse(result[8].ToString());
        var even = digit % 2 != 0;
        even.Should().BeTrue();
    }
}
