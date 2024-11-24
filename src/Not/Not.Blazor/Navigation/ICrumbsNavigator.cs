﻿using Not.Injection;

namespace Not.Blazor.Navigation;

public interface ICrumbsNavigator : ITransient
{
    void NavigateTo(string endpoint);
    void NavigateTo<T>(string endpoint, T parameter);
    bool CanNavigateBack();
    void NavigateBack();
    T ConsumeParameter<T>();
}