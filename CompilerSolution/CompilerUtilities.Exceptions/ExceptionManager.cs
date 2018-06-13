using System.Collections.Generic;

namespace CompilerUtilities.Exceptions
{
    public static class ExceptionManager
    {
        private static readonly Dictionary<int, (string, bool)> compilerErrors;

        static ExceptionManager()
        {
            compilerErrors = new Dictionary<int, (string, bool)>
            {
                [(int) ErrorCode.UnexpectedToken] = ("Unexpected token at index {0}", true),
                [(int) ErrorCode.EntryPointAlreadyExists] = ("Entry point already exists", false),
                [(int) ErrorCode.EntryPointNotExists] = ("Entry point does not exists", false),
                [(int) ErrorCode.ClosingBraceNotFound] = ("Closing brace not found for opening brace, line: {0}", true),
                [(int) ErrorCode.NotPossibleToSetValue] = ("Is not possible to set value of method or constant. Index: {0}", true),
                [(int) ErrorCode.TypeExpected] = ("Type expected at index {0}", true),
                [(int) ErrorCode.AccessModifierAlreadySet] = ("Access modifier already set. Index: {0}", true),
                [(int) ErrorCode.UnexpectedModifier] = ("Unexpected modifier at index {0}", true),
                [(int) ErrorCode.NameExpected] = ("Name expected at index {0}", true),
                [(int) ErrorCode.ModifierExpected] = ("Modifier expected at index {0}", true),
            };
        }

        public static void ThrowCompiler(int code, string file, int line = -1)
        {
            var message = compilerErrors[code].Item1;
            if (compilerErrors[code].Item2)
                message = string.Format(message, line);

            throw new CompileException(message, code, line, file);
        }

        public static void ThrowCompiler(ErrorCode code, string file, int line = -1)
        {
            ThrowCompiler((int) code, file, line);
        }
    }
}