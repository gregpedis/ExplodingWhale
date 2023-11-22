using ExplodingWhale.Rules;

namespace ExplodingWhale.Test;

[TestClass]
public class LocalFunctionsEndOfBodyTest
{
    [TestMethod]
    public void LocalFunctionsEndOfBody_Success() =>
        AnalyzerVerifier.Verify<LocalFunctionsEndOfBody>("""
            public class SomeClass
            {
                public int Method()
                {
                    var x = 1;
                    return AddOne(x) + AddTwo(x);

                    int AddOne(x) => x+1;
                    int AddTwo(x) => x+2;
                }

                public int Property
                {
                    get
                    {
                        var x = 1;
                        return AddOne(x) + AddTwo(x);

                        int AddOne(x) => x+1;
                        int AddTwo(x) => x+2;
                    }
                }
            }
            """);

    [TestMethod]
    public void LocalFunctionsEndOfBody_ShouldRaise() =>
        AnalyzerVerifier.Verify<LocalFunctionsEndOfBody>("""
            public class SomeClass
            {
                public SomeClass()
                {
                    var x = AddOne(x) + AddTwo(x);
            
                    int AddOne(int x) => x+1;                   // Bad { Move 'AddOne' at the end of the body. }
                    //  ^^^^^^
                    int AddTwo(int x) => x+2;                   // Bad
                    //  ^^^^^^
            
                    var shouldNotBeHere = 42;
                }

                public int Method()
                {
                    var x = 1;
                    return AddOne(x) + AddTwo(x);
            
                    int AddOne(int x) => x+1;                   // Bad { Move 'AddOne' at the end of the body. }
                    //  ^^^^^^
                    int AddTwo(int x) => x+2;                   // Bad
                    //  ^^^^^^

                    var shouldNotBeHere = 42;
                }
            
                public int Property
                {
                    get
                    {
                        var x = 1;
                        return AddOne(x) + AddTwo(x);

                        int AddOne(int x) => x+1;               // Bad
                        //  ^^^^^^
                        var shouldNotBeHere = 42;

                        int AddTwo(int x) => x+2;
                    }
                }
            }
            """);

    [TestMethod]
    public void LocalFunctionsEndOfBody_ShouldRaise_TopLevel() => // Top level statements are ignored. They are a mess anyways.
        AnalyzerVerifier.Verify<LocalFunctionsEndOfBody>("""
            public int Method() => 42;
            var x = 1;
            """, OutputKind.ConsoleApplication);
}
