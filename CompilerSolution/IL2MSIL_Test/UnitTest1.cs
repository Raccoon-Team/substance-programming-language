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

class Program
public static int Index

static func void Main()
    Index = 3
    Kek.Jij = 4
    ret
end
end".Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            IL2MSIL.ILTranslator.Compile("kek", true, lines);
        }
    }
}
