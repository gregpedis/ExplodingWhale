using ExplodingWhale.Rules;

namespace ExplodingWhale.Test;

[TestClass]
public class StaticAfterInstanceMethodsTest
{
    [TestMethod]
    public void StaticAfterInstanceMethods_Success() =>
        AnalyzerVerifier.Verify<StaticAfterInstanceMethods>("""
            class Mixed
            {
                Mixed() { }
                static Mixed() { }

                void Instance1() { }
                void Instance2() { }

                static void Static1() { }
                static void Static2() { }

                int Property1 { get; set; } 
            
                static int Property2 { get; set; } 
            
                int Field1 = 42;
            
                static int Field2  = 42;
            
                Mixed(string x) { }
            }

            class OnlyInstance
            {
                void Instance1() { }
                void Instance2() { }
            }

            class OnlyStatic
            {
                static void Static1() { }
                static void Static2() { }
            }
            """);

    [TestMethod]
    public void StaticAfterInstanceMethods_ShouldRaise() =>
        AnalyzerVerifier.Verify<StaticAfterInstanceMethods>("""
            class Simple
            {
                void Instance1() { }

                static void Static1() { } // Bad
                //          ^^^^^^^

                void Instance2() { }

                static void Static2() { }
            }

            class Complex
            {
                SomeClass() { }

                static SomeClass() { }

                SomeClass(int x) { }
            
                void Instance1() { }

                static void Static1() { } // Bad {Move static method 'Static1' after all the instance methods}

                void Instance2() { }
            
                static void Static2() { }

                int Property1 { get; set; } 

                static int Property2 { get; set; } 

                int Field1 = 42;

                static int Field2  = 42;

                SomeClass(string x) { }
            }
            """);
}
