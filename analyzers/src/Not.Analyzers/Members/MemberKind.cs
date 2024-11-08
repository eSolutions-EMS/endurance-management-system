namespace Not.Analyzers.Objects;

public enum MemberKind
{
    // Constants and Static Readonly Fields
    PrivateConst,
    PrivateStaticReadonly,
    PublicConst,
    PublicStaticReadonly,
    
    // Instance Fields
    PrivateReadonly,
    PrivateField,
    
    // Constructors
    PrivateCtor,
    ProtectedCtor,
    PublicCtor,

    // Abstract members
    AbstractProperty,
    AbstractMethod,
    
    // Properties
    PrivateProperty,
    ProtectedProperty,
    InternalProperty,
    PublicProperty,
    
    // Methods
    ProtectedMethod,
    InternalMethod,
    PublicMethod,
    PrivateMethod,
    
    // Nested Types
    InternalClass,
    PrivateClass
} 
