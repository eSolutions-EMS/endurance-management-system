using EMS.Judge.Core;
using EMS.Core.ConventionalServices;
using System;

namespace EMS.Judge.Services;

public interface INavigationService : ISingletonService
{
    void NavigateToImport();
    void NavigateToEvent();
    void NavigateToManager();
    void NavigateToRanking();
    void NavigateToHardware();
    void ChangeTo<T>()
        where T : IView;
    void ChangeToNewConfiguration<T>(int principalId, int childViewId)
        where T : IView;
    void ChangeToUpdateConfiguration<T>(int id)
        where T : IView;
    void ChangeTo<T>(params NavigationParameter[] parameters);
    void ChangeTo(Type view, params NavigationParameter[] parameters);
}
