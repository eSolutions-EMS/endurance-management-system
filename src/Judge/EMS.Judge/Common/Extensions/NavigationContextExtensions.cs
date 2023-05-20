using Prism.Regions;
using System;
using static EMS.Judge.DesktopConstants;

namespace EMS.Judge.Common.Extensions;

public static class NavigationContextExtensions
{
    private const string MISSING_PARAMETER_TEMPLATE = "Missing '{0}' from navigation context";

    public static bool IsExistingConfiguration(this NavigationContext context)
    {
        var hasDomainId = context.Parameters.ContainsKey(DesktopConstants.NavigationParametersKeys.DOMAIN_ID);
        return hasDomainId;
    }

    public static int GetDomainId(this NavigationContext context)
    {
        var key = DesktopConstants.NavigationParametersKeys.DOMAIN_ID;
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
        var hasPrincipalId = context.Parameters.TryGetValue<int>(DesktopConstants.NavigationParametersKeys.PARENT_VIEW_ID, out var id);
        if (!hasPrincipalId)
        {
            return null;
        }

        return id;
    }

    public static int GetViewId(this NavigationContext context)
    {
        var key = DesktopConstants.NavigationParametersKeys.VIEW_ID;
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
        var hasData = context.Parameters.TryGetValue<object>(DesktopConstants.NavigationParametersKeys.DATA, out var data);
        if (!hasData)
        {
            return null;
        }

        return data;
    }
}
