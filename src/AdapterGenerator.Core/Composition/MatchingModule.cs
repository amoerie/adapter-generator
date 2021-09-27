using System.Collections.Generic;
using AdapterGenerator.Core.Matching;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Matching.Enums.Values;
using Autofac;

namespace AdapterGenerator.Core.Composition {
  public class MatchingModule : Module {
    protected override void Load(ContainerBuilder builder) {
      /* Classes */
      builder.RegisterType<ClassMatcher>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<SingleClassMatcher>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<MatchByClassNameStrategy>().AsSelf().SingleInstance();
      builder.RegisterType<MatchByClassNamesAndPropertiesSimilarityStrategy>().AsSelf().SingleInstance();
      builder.Register(ctx => new IClassMatchingStrategy[] {
        ctx.Resolve<MatchByClassNameStrategy>(),
        ctx.Resolve<MatchByClassNamesAndPropertiesSimilarityStrategy>(),
      }).As<IEnumerable<IClassMatchingStrategy>>().SingleInstance();

      /* Class properties */
      builder.RegisterType<PropertyMatcher>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<SinglePropertyMatcher>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<PropertyMatchingStrategiesFactory>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<MatchByPropertyNameSimilarityAndTypeStrategy>().AsSelf().SingleInstance();

      /* Enums */
      builder.RegisterType<EnumMatcher>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<SingleEnumMatcher>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<MatchByEnumNameStrategy>().AsSelf().SingleInstance();
      builder.RegisterType<MatchByEnumNameSimilarityStrategy>().AsSelf().SingleInstance();
      builder.RegisterType<MatchByEnumNamesAndPropertiesSimilarityStrategy>().AsSelf().SingleInstance();
      builder.Register(ctx => new IEnumMatchingStrategy[] {
        ctx.Resolve<MatchByEnumNameStrategy>(),
        ctx.Resolve<MatchByEnumNameSimilarityStrategy>(),
        ctx.Resolve<MatchByEnumNamesAndPropertiesSimilarityStrategy>(),
      }).As<IEnumerable<IEnumMatchingStrategy>>().SingleInstance();

      /* Enum values */
      builder.RegisterType<EnumValueMatcher>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<SingleEnumValueMatcher>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<MatchByEnumValueNameStrategy>().AsSelf().SingleInstance();
      builder.RegisterType<MatchByEnumValueNameSimilarityStrategy>().AsSelf().SingleInstance();

      builder.Register(ctx => new IEnumValueMatchingStrategy[] {
        ctx.Resolve<MatchByEnumValueNameStrategy>(),
        ctx.Resolve<MatchByEnumValueNameSimilarityStrategy>(),
      }).As<IEnumerable<IEnumValueMatchingStrategy>>().SingleInstance();

      /* Other */
      builder.RegisterType<TypeEquivalencyDeterminer>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<NameSimilarityDeterminer>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<LevenshteinDistanceCalculator>().AsImplementedInterfaces().SingleInstance();
      builder.RegisterType<Matcher>().AsImplementedInterfaces().SingleInstance();
    }
  }
}