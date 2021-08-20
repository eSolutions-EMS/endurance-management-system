using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Gateways.Desktop.Core;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public interface INavigationService : ISingletonService
    {
        void NavigateToImport();

        void NavigateToEvent();

        void ChangeTo<T>()
            where T : IView;

        void ChangeTo<T>(int id)
            where T : IView;

        void ChangeTo<TView>(Action<object> action)
            where TView : IView;

        void ChangeTo(Type viewType, Action<object> action);

        void ChangeTo<TView>(object data, Action<object> action)
            where TView : IView;

        void ChangeTo(Type viewType, object data, Action<object> action);
    }
}
