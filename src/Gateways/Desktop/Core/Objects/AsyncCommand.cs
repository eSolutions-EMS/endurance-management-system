using Prism.Commands;
using System;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Core.Objects
{
    public class AsyncCommand : DelegateCommand
    {
        public AsyncCommand(Func<Task> executeMethod) : base(() => executeMethod())
        {
        }

        public AsyncCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base(() => executeMethod(), canExecuteMethod)
        {
        }
    }
}
