using System;

namespace AdapterGenerator.Tests.Utilities.Net {
  public static partial class ExtensionsForString {
    public static string GenerateRandomString(this string reference, int length) {
      if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
      reference = GenerateRandomString(length);
      return reference;
    }

    public static string GenerateRandomString(int length) {
      if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
      var stringGenerator = new RandomStringGenerator(length);
      return stringGenerator.GenerateString();
    }
  }
}