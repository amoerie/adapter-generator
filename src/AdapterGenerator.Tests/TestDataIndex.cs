using System;
using System.IO;

namespace AdapterGenerator.Tests {
  public static class TestDataIndex {
    public static DirectoryInfo RootFolder
      => new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase));

    private static readonly Func<string, FileInfo> TestDataFile =
      fileName => new FileInfo(Path.Combine(RootFolder.FullName, fileName));

    public static class Core {
      public static FileInfo HelloWorldTxt => TestDataFile("Core/Parsing/TestData/HelloWorld.txt");
      public static FileInfo OneClass => TestDataFile("Core/Parsing/TestData/OneClass.cs");

      public static FileInfo OneClassWithNestedClass
        => TestDataFile("Core/Parsing/TestData/OneClassWithNestedClass.cs");

      public static FileInfo MultipleClassesInOneFile
        => TestDataFile("Core/Parsing/TestData/MultipleClassesInOneFile.cs");

      public static FileInfo SimpleInputClass => TestDataFile("Core/Matching/Classes/TestData/SimpleInputClass.cs");
      public static FileInfo SimpleOutputClass => TestDataFile("Core/Matching/Classes/TestData/SimpleOutputClass.cs");

      public static FileInfo OtherSimpleOutputClass
        => TestDataFile("Core/Matching/Classes/TestData/OtherSimpleOutputClass.cs");

      public static FileInfo SimpleInputEnum => TestDataFile("Core/Matching/Enums/TestData/SimpleInputEnum.cs");
      public static FileInfo SimpleOutputEnum => TestDataFile("Core/Matching/Enums/TestData/SimpleOutputEnum.cs");

      public static FileInfo OtherSimpleOutputEnum
        => TestDataFile("Core/Matching/Enums/TestData/OtherSimpleOutputEnum.cs");

      public static FileInfo Employee => TestDataFile("Core/Matching/Classes/Properties/TestData/Employee.cs");
      public static FileInfo Manager => TestDataFile("Core/Matching/Classes/Properties/TestData/Manager.cs");

      public static FileInfo Source => TestDataFile("Core/Generation/Adapters/TestData/Source.cs");
      public static FileInfo SourceEnum => TestDataFile("Core/Generation/Adapters/TestData/SourceEnum.cs");
      public static FileInfo Target => TestDataFile("Core/Generation/Adapters/TestData/Target.cs");
      public static FileInfo TargetEnum => TestDataFile("Core/Generation/Adapters/TestData/TargetEnum.cs");

      public static FileInfo SimpleAdapter
        => TestDataFile("Core/Generation/AdapterTests/NUnit/TestData/SimpleAdapter.cs");
    }

    public static class IntegrationTests {
      public static FileInfo SourcePerson => TestDataFile("IntegrationTests/TestData/Sources/Person.cs");
      public static FileInfo SourceEmployee => TestDataFile("IntegrationTests/TestData/Sources/Employee.cs");
      public static FileInfo SourceCompany => TestDataFile("IntegrationTests/TestData/Sources/Company.cs");
      public static FileInfo SourceSpeaker => TestDataFile("IntegrationTests/TestData/Sources/Speaker.cs");
      public static FileInfo SourceGender => TestDataFile("IntegrationTests/TestData/Sources/Gender.cs");
      public static FileInfo SourceTrainer => TestDataFile("IntegrationTests/TestData/Sources/Trainer.cs");
      public static FileInfo SourceDepartment => TestDataFile("IntegrationTests/TestData/Sources/BrokaDepartment.cs");
      public static FileInfo TargetPerson => TestDataFile("IntegrationTests/TestData/Targets/Person.cs");
      public static FileInfo TargetEmployee => TestDataFile("IntegrationTests/TestData/Targets/Employee.cs");
      public static FileInfo TargetCompany => TestDataFile("IntegrationTests/TestData/Targets/Company.cs");
      public static FileInfo TargetSpeaker => TestDataFile("IntegrationTests/TestData/Targets/Speaker.cs");
      public static FileInfo TargetGender => TestDataFile("IntegrationTests/TestData/Targets/Gender.cs");
      public static FileInfo TargetTrainer => TestDataFile("IntegrationTests/TestData/Targets/Trainer.cs");
      public static FileInfo TargetDepartment => TestDataFile("IntegrationTests/TestData/Targets/Department.cs");
    }
  }
}