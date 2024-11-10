namespace Not.Analyzers.Members;

public enum MemberKind
{
    PrivateConst,
    PrivateStaticReadonly,
    PublicConst,
    PublicStaticReadonly,
    PublicStaticEvent,
    PublicStaticMethod,
    PublicImplicitOperator,
    PublicOperator,
    PrivateReadonly,
    PrivateField,
    PrivateEvent,
    PublicField,
    PrivateCtor,
    ProtectedCtor,
    PublicCtor,
    AbstractProperty,
    AbstractMethod,
    PrivateProperty,
    ProtectedProperty,
    InternalProperty,
    PublicIndexDeclarator,
    PublicEvent,
    PublicProperty,
    ProtectedMethod,
    InternalMethod,
    PublicMethod,
    PrivateMethod,
    ProtectedClass,
    InternalClass,
    PrivateClass
}
