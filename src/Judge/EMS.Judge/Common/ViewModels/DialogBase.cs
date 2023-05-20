using Prism.Services.Dialogs;
using System;

namespace EMS.Judge.Common.ViewModels;

public abstract class DialogBase : ViewModelBase, IDialogAware
{
    public virtual string Title { get; protected set; }

    public abstract void OnDialogOpened(IDialogParameters parameters);

    public virtual bool CanCloseDialog()
        => true;

    public virtual void OnDialogClosed()
    {
    }

    public event Action<IDialogResult> RequestClose;

    protected void Close(IDialogResult result)
    {
        this.RequestClose.Invoke(result);
    }
}
