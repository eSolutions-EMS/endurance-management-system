using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.Extensions
{
    public static class NavigationContextExtensions
    {
        public static int? GetId(this NavigationContext context)
        {
            var hasId = context.Parameters.TryGetValue<int>(DesktopConstants.EntityIdParameter, out var id);
            if (!hasId)
            {
                return null;
            }

            return id;
        }

        public static object GetData(this NavigationContext context)
        {
            var hasData = context.Parameters.TryGetValue<object>(DesktopConstants.FormDataParameter, out var data);
            if (!hasData)
            {
                return null;
            }

            return data;
        }

        public static bool HasDependantId(this NavigationContext context)
        {
            return context.Parameters.ContainsKey(DesktopConstants.NewDependantId);
        }

        public static Guid GetDependantId(this NavigationContext context)
        {
            var id = context.Parameters.GetValue<Guid>(DesktopConstants.NewDependantId);
            return id;
        }

        public static bool HasDependant(this NavigationContext context)
        {
            return context.Parameters.ContainsKey(DesktopConstants.DependantDataParameter);
        }

        public static T GetDependant<T>(this NavigationContext context)
        {
            context.Parameters.TryGetValue<T>(DesktopConstants.DependantDataParameter, out var dependant);
            return dependant;
        }
    }
}
