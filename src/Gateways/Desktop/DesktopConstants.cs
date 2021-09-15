using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace EnduranceJudge.Gateways.Desktop
{
    public static class DesktopConstants
    {
        public const string True = "True";
        public const string False = "False";

        public static Assembly[] Assemblies
        {
            get
            {
                var assemblies = ReflectionUtilities.GetAssemblies("EnduranceJudge.Gateways.Desktop");
                return assemblies;
            }
        }

        public const string EntityIdParameter = "Id";
        public const string FormDataParameter = "FormData";
        public const string ChildDataParameter = "ChildData";
        public const string NewChildIdParameter = "ChildId";
        public const string UpdateOnlyParameter = "UpdateOnly";

        public static class Types
        {
            public static readonly Type DelegateCommand = typeof(DelegateCommand);
            public static readonly Type ObservableListItems = typeof(ObservableCollection<ListItemViewModel>);
        }
    }

    public static class Regions
    {
        public const string HEADER_RIGHT = "Region1";
        public const string HEADER_LEFT = "Region2";
        public const string CONTENT_LEFT = "Region3";
        public const string CONTENT_RIGHT = "Region4";
    }
}
