using System;

namespace EnduranceJudge.Core.Models
{
    public class TypeDescriptor<T>
        where T : class
    {
        public TypeDescriptor(Type type)
        {
            this.Type = type;
            var obj = Activator.CreateInstance(type);

            if (obj == null)
            {
                throw new InvalidOperationException($"Cannot instantiate type '{type.FullName}'");
            }

            this.Instance = obj as T
                ?? throw new InvalidOperationException($"Cannot convert object '{obj}' to type '{type.FullName}'");
        }

        public TypeDescriptor(Type type, T instance)
        {
            this.Type = type;
            this.Instance = instance;
        }

        public Type Type { get; }

        public T Instance { get; }
    }
}
