namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ParentFormBase<TView> : FormBase<TView>
        where TView : IView
    {
        protected void NewForm<T>()
            where T : IView
        {
            this.Navigation.ChangeToNewForm<T>(this.Id);
        }
    }
}
