using System;
using System.Collections.Immutable;
using System.Linq;
using AdapterGenerator.Core.Matching.Classes;
using AdapterGenerator.Core.Matching.Classes.Properties;
using AdapterGenerator.Core.Matching.Enums;
using AdapterGenerator.Core.Parsing;

namespace AdapterGenerator.Core.Matching {
  public class Matcher : IMatcher {
    private readonly IClassMatcher _classMatcher;
    private readonly IEnumMatcher _enumMatcher;
    private readonly IPropertyMatcher _propertyMatcher;

    public Matcher(IClassMatcher classMatcher, IEnumMatcher enumMatcher, IPropertyMatcher propertyMatcher) {
      if (classMatcher == null) throw new ArgumentNullException(nameof(classMatcher));
      if (enumMatcher == null) throw new ArgumentNullException(nameof(enumMatcher));
      _classMatcher = classMatcher;
      _enumMatcher = enumMatcher;
      _propertyMatcher = propertyMatcher;
    }

    public IMatches Match(ITypeDeclarations sources, ITypeDeclarations targets) {
      var initialClassMatches = _classMatcher.Match(sources.Classes, targets.Classes);
      var enumMatches = _enumMatcher.Match(sources.Enums, targets.Enums);
      var classMatches = initialClassMatches.Select(cm =>
        new ClassMatch {
          Source = cm.Source,
          Target = cm.Target,
          PropertyMatches = _propertyMatcher.Match(initialClassMatches, enumMatches, cm.Source, cm.Target)
        }).ToImmutableList<IClassMatch>();
      return new Matches(classMatches, enumMatches);
    }
  }
}