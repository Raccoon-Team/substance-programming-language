namespace CompilerUtilities.Exceptions
{
    public enum ErrorCode
    {
        UnexpectedToken,
        EntryPointAlreadyExists,
        EntryPointNotExists,
        ClosingBraceNotFound,
        NotPossibleToSetValue,
        TypeExpected,
        AccessModifierAlreadySet,
        UnexpectedModifier,
        NameExpected,
        ModifierExpected,
    }
}