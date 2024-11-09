namespace Not.Analyzers.Objects;

public enum MemberKind
{
    PrivateConst = 0,
    PrivateStaticReadonly = 1,
    PublicConst = 2,
    PublicStaticReadonly = 3,

    PublicStaticMethod = 4,

    PrivateReadonly = 5,
    PrivateField = 6,
    PublicField = 7,

    PrivateCtor = 8,
    ProtectedCtor = 9,
    PublicCtor = 10,

    AbstractProperty = 11,
    AbstractMethod = 12,

    PrivateProperty = 13,
    ProtectedProperty = 14,
    InternalProperty = 15,
    PublicProperty = 16,

    ProtectedMethod = 17,
    InternalMethod = 18,
    PublicMethod = 19,
    PrivateMethod = 20,

    ProtectedClass = 21,
    InternalClass = 22,
    PrivateClass = 23,
}
