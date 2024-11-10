namespace Not.Analyzers.Members;

public enum MemberKind
{
    PrivateConst,
    PrivateStaticReadonly,
    PublicConst,
    PublicStaticReadonly,
    PublicStaticMethod,
    PublicImplicitOperator,
    PublicOperator,
    PrivateReadonly,
    PrivateField,
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
    PublicProperty,
    ProtectedMethod,
    InternalMethod,
    PublicMethod,
    PrivateMethod,
    ProtectedClass,
    InternalClass,
    PrivateClass
}
