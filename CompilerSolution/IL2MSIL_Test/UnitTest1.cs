using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IL2MSIL_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var lines = @"class Kek
    public static int Jij
end

public class Program
public static int Index

public static func void Main()
    Index = 3 + 5
    Kek.Jij = 4
    string str = Index.ToString()
    ret
end
end".Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            IL2MSIL.ILTranslator.Compile("kek", true, lines);
        }
    }
}
