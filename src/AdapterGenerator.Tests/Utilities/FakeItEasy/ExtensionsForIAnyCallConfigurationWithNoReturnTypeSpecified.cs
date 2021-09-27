using System;
using System.Linq.Expressions;
using FakeItEasy;
using FakeItEasy.Configuration;

namespace AdapterGenerator.Tests.Utilities.FakeItEasy {
  public static class ExtensionsForIAnyCallConfigurationWithNoReturnTypeSpecified {
    public static IVoidConfiguration SetProperty<TFake, TProperty>(this IAnyCallConfigurationWithNoReturnTypeSpecified config, Expression<Func<TFake, TProperty>> property, TProperty value) {
      return config.Where(_ => _.Method.Name.Equals($"set_{((MemberExpression)property.Body).Member.Name}")).WhenArgumentsMatch(args => args[0].Equals(value));
    }

    public static IVoidConfiguration SetProperty<TFake, TProperty>(this IAnyCallConfigurationWithNoReturnTypeSpecified config, Expression<Func<TFake, TProperty>> property) {
      return config.Where(_ => _.Method.Name.Equals($"set_{((MemberExpression)property.Body).Member.Name}")).WithAnyArguments();
    }

    public static IVoidConfiguration SetProperty<TProperty>(this IAnyCallConfigurationWithNoReturnTypeSpecified config, string propertyName, TProperty value) {
      return config.Where(_ => _.Method.Name.Equals($"set_{propertyName}")).WhenArgumentsMatch(args => args[0].Equals(value));
    }

    public static IVoidConfiguration SetProperty(this IAnyCallConfigurationWithNoReturnTypeSpecified config, string propertyName) {
      return config.Where(_ => _.Method.Name.Equals($"set_{propertyName}")).WithAnyArguments();
    }
  }
}