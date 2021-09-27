using System.Collections.Generic;
using System.IO;

namespace AdapterGenerator.Core.Parsing {
  public interface IFileParser {
    ITypeDeclarations ParseFiles(IEnumerable<FileInfo> files);
  }
}