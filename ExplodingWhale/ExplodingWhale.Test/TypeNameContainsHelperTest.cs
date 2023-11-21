
using ExplodingWhale.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExplodingWhale.Test;

[TestClass]
public class TypeNameContainsHelperTest
{
    [TestMethod]
    public void Test_Success() =>
        AnalyzerVerifier.Verify<TypeNameContainsHelper>("""
            public class NotAHelpER { }
            """);

    [TestMethod]
    public void Test_ShouldRaise() =>
        AnalyzerVerifier.Verify<TypeNameContainsHelper>("""
            public class ThingyHelper  // Bad
            { } 

            public class ThingyHelPEr // Only check with correct casing
            { } 

            public class ThingyHelperExtensions  // Bad { Type name 'ThingyHelperExtensions' contains the word 'Helper' }
            //                 ^^^^^^
            { } 

            public struct StructHelperExtensions  // Bad { Type name 'StructHelperExtensions' contains the word 'Helper' }
            //                  ^^^^^^
            { } 

            public class HelperThingy
            //           ^^^^^^
            { } 

            // Bad@-4 { Type name 'HelperThingy' contains the word 'Helper' }
            """);
}
