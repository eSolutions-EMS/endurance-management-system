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
            var hasData = context.Parameters.TryGetValue<object>(DesktopConstants.DataParameter, out var data);
            if (!hasData)
            {
                return null;
            }

            return data;
        }

        public static bool HasChildId(this NavigationContext context)
        {
            return context.Parameters.ContainsKey(DesktopConstants.NewChildIdParameter);
        }

        public static Guid GetChildId(this NavigationContext context)
        {
            var id = context.Parameters.GetValue<Guid>(DesktopConstants.NewChildIdParameter);
            return id;
        }

        public static bool HasChild(this NavigationContext context)
        {
            return context.Parameters.ContainsKey(DesktopConstants.ChildDataParameter);
        }

        public static T GetChild<T>(this NavigationContext context)
        {
            context.Parameters.TryGetValue<T>(DesktopConstants.ChildDataParameter, out var child);
            return child;
        }

        public static bool IsUpdate(this NavigationContext context)
        {
            var isUpdate = context.Parameters.ContainsKey(DesktopConstants.UPDATE_PARAMETER);
            return isUpdate;
        }

        public static bool IsRemove(this NavigationContext context)
        {
            var isRemove = context.Parameters.ContainsKey(DesktopConstants.REMOVE_PARAMETER);
            return isRemove;
        }

        public static string GetMessage(this NavigationContext context)
        {
            context.Parameters.TryGetValue<string>(DesktopConstants.MESSAGE_PARAMETER, out var message);
            return message;
        }
    }
}
