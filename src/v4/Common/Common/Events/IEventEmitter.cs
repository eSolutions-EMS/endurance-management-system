using Common.Conventions;

namespace Common.Events;
public interface IEventEmitter : ITransientService
{
    void Emit(object sender);
}

public interface IEventEmitter<T> : ITransientService
{
    void Emit(object sender, T data);
}

public interface IEventEmitter<T1, T2> : ITransientService
{
    void Emit(object sender, T1 data1, T2 data2);
}
