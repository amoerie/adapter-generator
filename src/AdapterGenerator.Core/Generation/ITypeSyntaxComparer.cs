using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation {
  public interface ITypeSyntaxComparer : IEqualityComparer<TypeSyntax> {}
}