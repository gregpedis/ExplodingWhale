
using ExplodingWhale.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExplodingWhale.Test;

[TestClass]
public class MakeUppercaseTest
{
    [TestMethod]
    public void Test_Success() => 
        AnalyzerVerifier.Verify<MakeUppercase>("""
            public class THINGY { }
            """);

    [TestMethod]
    public void Test_ShouldRaise() => 
        AnalyzerVerifier.Verify<MakeUppercase>("""
            public class Thingy1  // Bad
            { } 

            public class Thingy2  // Bad
            //           ^^^^^^^
            { } 

            public class Thingy3  // Bad { Type name 'Thingy3' contains lowercase letters }
            //           ^^^^^^^
            { } 


            public class Thingy4  
            //           ^^^^^^^
            { } 

            // Bad@-4 { Type name 'Thingy4' contains lowercase letters }
            """);
}
