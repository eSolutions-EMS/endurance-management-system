using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EnduranceJudge.Core
{
    public static class CoreConstants
    {
        public const char StringSplitChar = ';';

        public static Assembly[] Assemblies
        {
            get
            {
                var assemblies = ReflectionUtilities.GetAssemblies("EnduranceJudge.Core");
                return assemblies;
            }
        }

        public static class Types
        {
            public static readonly Type ListGeneric = typeof(List<>);
            public static readonly Type ObjectExtensions = typeof(IObjectExtensions);
        }
    }
}
