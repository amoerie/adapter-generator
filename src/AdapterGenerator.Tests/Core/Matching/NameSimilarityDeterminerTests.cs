using AdapterGenerator.Core.Matching;
using FluentAssertions;
using NUnit.Framework;

namespace AdapterGenerator.Tests.Core.Matching {
  [TestFixture]
  public class NameSimilarityDeterminerTests {
    NameSimilarityDeterminer _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new NameSimilarityDeterminer(new LevenshteinDistanceCalculator());
    }

    [TestCase("BrokaDepartment", "Department", true)]
    [TestCase("BrokaDepartment", "Site", false)]
    [TestCase("ContactMail", "ContactEmail", true)]
    [TestCase("ContactMail", "ContactPhone", false)]
    public void ShouldReturnExpectedResult(string first, string second, bool expectedResult) {
      _sut.AreSimilar(first, second).Should().Be(expectedResult);
    }
  }
}