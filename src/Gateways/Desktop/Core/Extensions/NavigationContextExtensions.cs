using Prism.Regions;
using System;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;

namespace EnduranceJudge.Gateways.Desktop.Core.Extensions
{
    public static class NavigationContextExtensions
    {
        private const string MISSING_PARAMETER_TEMPLATE = "Missing '{0}' from navigation context";

        public static bool IsExistingConfiguration(this NavigationContext context)
        {
            var hasDomainId = context.Parameters.ContainsKey(Parameters.DOMAIN_ID);
            return hasDomainId;
        }

        public static int GetDomainId(this NavigationContext context)
        {
            var key = Parameters.DOMAIN_ID;
            var hasId = context.Parameters.TryGetValue<int>(key, out var id);
            if (!hasId)
            {
                var message = string.Format(MISSING_PARAMETER_TEMPLATE, key);
                throw new ArgumentException(message);
            }

            return id;
        }

        public static int? LookForParentViewId(this NavigationContext context)
        {
            var hasPrincipalId = context.Parameters.TryGetValue<int>(Parameters.PARENT_VIEW_ID, out var id);
            if (!hasPrincipalId)
            {
                return null;
            }

            return id;
        }

        public static int GetViewId(this NavigationContext context)
        {
            var key = Parameters.VIEW_ID;
            var hasPrincipalId = context.Parameters.TryGetValue<int>(key, out var id);
            if (!hasPrincipalId)
            {
                var message = string.Format(MISSING_PARAMETER_TEMPLATE, key);
                throw new ArgumentException(message);
            }

            return id;
        }

        public static object GetData(this NavigationContext context)
        {
            var hasData = context.Parameters.TryGetValue<object>(DesktopConstants.DataParameter, out var data);
            if (!hasData)
            {
                return null;
            }

            return data;
        }

        public static string GetMessage(this NavigationContext context)
        {
            context.Parameters.TryGetValue<string>(DesktopConstants.MESSAGE_PARAMETER, out var message);
            return message;
        }
    }
}
