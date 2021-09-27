using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;
using AdapterGenerator.Tests.Utilities.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching.Classes.Properties {
  [TestFixture]
  public class MatchByPropertyNameAndTypeStrategyTests {
    MatchByPropertyNameSimilarityAndTypeStrategy _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new MatchByPropertyNameSimilarityAndTypeStrategy(
        new NameSimilarityDeterminer(new LevenshteinDistanceCalculator()),
        new TypeEquivalencyDeterminer(ImmutableList<IClassMatchWithoutPropertyMatches>.Empty,
          ImmutableList<IEnumMatch>.Empty));
    }

    [TestFixture]
    public class Constructor : MatchByPropertyNameAndTypeStrategyTests {
      [SetUp]
      public override void SetUp() {
        base.SetUp();
      }

      [Test]
      public void ShouldHaveNoOptionalDependencies() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class Matches : MatchByPropertyNameAndTypeStrategyTests {
      private IClass _employeeClass;
      private IClass _managerClass;
      private IProperty _employeeNameProperty;
      private IProperty _employeeAgeProperty;
      private IProperty _employeeAddressProperty;
      private IProperty _managerAddressProperty;
      private IProperty _managerNameProperty;
      private IProperty _managerAgeProperty;
      private IProperty _employeeAddressesProperty;
      private IProperty _managerAddressesProperty;
      private IProperty _managerWorkAddressesProperty;
      private IProperty _employeeWorkAddressesProperty;
      private IProperty _employeeStringsEnumerableProperty;
      private IProperty _employeeStringsArrayProperty;
      private IProperty _managerStringsEnumerableProperty;
      private IProperty _managerStringsArrayProperty;
      private IProperty _employeeBirthDayProperty;
      private IProperty _managerBirthDateProperty;
      private IProperty _employeeIntsEnumerableProperty;
      private IProperty _managerIntsArrayProperty;
      private IProperty _employeeDeliveryAddressesProperty;
      private IProperty _employeeSecondaryAddressesProperty;
      private IProperty _managerDeliveryAddressesProperty;
      private IProperty _managerSecondaryAddressesProperty;
      private IProperty _employeeGenderProperty;
      private IProperty _managerGenderProperty;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        var employeeClasses = TestUtilities.ExtractClasses(TestDataIndex.Core.Employee);
        _employeeClass = employeeClasses.Single(c => c.ClassDeclarationSyntax.Identifier.Text.Equals("Employee"));
        _employeeNameProperty = _employeeClass.FindPropertyByName("Name");
        _employeeAddressProperty = _employeeClass.FindPropertyByName("Address");
        _employeeAddressesProperty = _employeeClass.FindPropertyByName("Addresses");
        _employeeWorkAddressesProperty = _employeeClass.FindPropertyByName("WorkAddresses");
        _employeeDeliveryAddressesProperty = _employeeClass.FindPropertyByName("DeliveryAddresses");
        _employeeSecondaryAddressesProperty = _employeeClass.FindPropertyByName("SecondaryAddresses");
        _employeeStringsEnumerableProperty = _employeeClass.FindPropertyByName("Strings");
        _employeeStringsArrayProperty = _employeeClass.FindPropertyByName("StringsArray");
        _employeeAgeProperty = _employeeClass.FindPropertyByName("Age");
        _employeeBirthDayProperty = _employeeClass.FindPropertyByName("BirthDate");
        _employeeIntsEnumerableProperty = _employeeClass.FindPropertyByName("Ints");
        _employeeGenderProperty = _employeeClass.FindPropertyByName("Gender");

        var managerClasses = TestUtilities.ExtractClasses(TestDataIndex.Core.Manager);
        _managerClass = managerClasses.Single(c => c.ClassDeclarationSyntax.Identifier.Text.Equals("Manager"));
        _managerAddressProperty = _managerClass.FindPropertyByName("Address");
        _managerAddressesProperty = _managerClass.FindPropertyByName("Addresses");
        _managerWorkAddressesProperty = _managerClass.FindPropertyByName("WorkAddresses");
        _managerDeliveryAddressesProperty = _managerClass.FindPropertyByName("DeliveryAddresses");
        _managerSecondaryAddressesProperty = _managerClass.FindPropertyByName("SecondaryAddresses");
        _managerStringsEnumerableProperty = _managerClass.FindPropertyByName("Strings");
        _managerStringsArrayProperty = _managerClass.FindPropertyByName("StringsArray");
        _managerNameProperty = _managerClass.FindPropertyByName("Name");
        _managerAgeProperty = _managerClass.FindPropertyByName("Age");
        _managerBirthDateProperty = _managerClass.FindPropertyByName("BirthDate");
        _managerIntsArrayProperty = _managerClass.FindPropertyByName("Ints");
        _managerGenderProperty = _managerClass.FindPropertyByName("Gender");
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfSameNameAndSamePrimitiveType() {
        _sut.Matches(_employeeNameProperty, _managerNameProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnFalseForPropertiesOfSameNameAndDifferentType() {
        _sut.Matches(_employeeAgeProperty, _managerAgeProperty).Should().BeFalse();
      }

      [Test]
      public void ShouldReturnFalseForPropertiesOfDifferentNameAndSameType() {
        _sut.Matches(_employeeNameProperty, _managerAgeProperty).Should().BeFalse();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfSameMatchedClassType() {
        _sut.Matches(_employeeAddressProperty, _managerAddressProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfSameEnumerableOfMatchedClassType() {
        _sut.Matches(_employeeAddressesProperty, _managerAddressesProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfSameArrayOfMatchedClassType() {
        _sut.Matches(_employeeSecondaryAddressesProperty, _managerSecondaryAddressesProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfSameEnumerableOfPrimitiveType() {
        _sut.Matches(_employeeStringsEnumerableProperty, _managerStringsEnumerableProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfSameArrayOfOfPrimitiveType() {
        _sut.Matches(_employeeStringsArrayProperty, _managerStringsArrayProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfArrayAndEnumerableOfMatchedClassType() {
        _sut.Matches(_employeeDeliveryAddressesProperty, _managerDeliveryAddressesProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfEnumerableAndArrayOfMatchedClassType() {
        _sut.Matches(_employeeWorkAddressesProperty, _managerWorkAddressesProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfSamePrimitiveTypeWhenNullabilityIsDifferent() {
        _sut.Matches(_employeeBirthDayProperty, _managerBirthDateProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfArrayOfNullableIntsAndEnumerableOfNonNullableInts() {
        _sut.Matches(_employeeIntsEnumerableProperty, _managerIntsArrayProperty).Should().BeTrue();
      }

      [Test]
      public void ShouldReturnTrueForPropertiesOfSameEnumType() {
        _sut.Matches(_employeeGenderProperty, _managerGenderProperty).Should().BeTrue();
      }
    }
  }
}