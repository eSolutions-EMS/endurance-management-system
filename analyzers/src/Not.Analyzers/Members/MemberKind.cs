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
    
    ProtectedMethod = 16,
    InternalMethod = 17,
    PublicMethod = 18,
    PrivateMethod = 19,
    
    InternalClass = 20,
    PrivateClass = 21,
} 
