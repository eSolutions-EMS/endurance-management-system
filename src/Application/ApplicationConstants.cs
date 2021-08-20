using EnduranceJudge.Core.Utilities;
using System.Reflection;

namespace EnduranceJudge.Application
{
    public static class ApplicationConstants
    {
        public static Assembly[] Assemblies
        {
            get
            {
                var assemblies = ReflectionUtilities.GetAssemblies("EnduranceJudge.Application");
                return assemblies;
            }
        }

        public const string WorkFileName = "endurance-judge-file";

        public static class FileExtensions
        {
            public const string Xml = ".xml";
            public const string SupportedExcel = ".xlsx";
        }

        public static class ExcelMaps
        {
            public static class ImportNational
            {
                public const int FirstEntryRow = 4;
                public const int FeiIdColumn = 27;
                public const int NameColumn = 2;
                public const int BreedColumn = 22;
            }
        }
    }
}
