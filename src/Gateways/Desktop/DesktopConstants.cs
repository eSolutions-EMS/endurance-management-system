using EnduranceJudge.Core.Utilities;
using System.Reflection;

namespace EnduranceJudge.Gateways.Desktop
{
    public static class DesktopConstants
    {
        public const string TIME_FORMAT = "HH:mm:ss";

        public static Assembly[] Assemblies
        {
            get
            {
                var assemblies = ReflectionUtilities.GetAssemblies("EnduranceJudge.Gateways.Desktop");
                return assemblies;
            }
        }

        public static class NavigationParametersKeys
        {
            public const string DOMAIN_ID = "DomainId";
            public const string VIEW_ID = "ViewId";
            public const string PARENT_VIEW_ID = "ParentViewId";
            public const string DATA = "DataParameter";
        }
    }

    public static class Regions
    {
        public const string HEADER_RIGHT = "Region0";
        public const string HEADER_LEFT = "Region1";
        public const string CONTENT_RIGHT = "Region2";
        public const string CONTENT_LEFT = "Region3";
    }
}
