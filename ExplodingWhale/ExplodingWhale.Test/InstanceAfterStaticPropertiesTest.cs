using ExplodingWhale.Rules;

namespace ExplodingWhale.Test;

[TestClass]
public class InstanceAfterStaticPropertiesTest
{
    [TestMethod]
    public void InstanceAfterStaticPropertiesTest_NoIssues() =>
        AnalyzerVerifier.Verify<InstanceAfterStaticProperties>("""
            class Mixed
            {
                static int Static1 => 42;

                Mixed() { }
                static Mixed() { }

                void Method1() { }
                void Method2() { }

                static void Method3() { }
                static void Method4() { }

                static int Static2 => 42;

                int Field1;
            
                int Instance1 { get; set; }

                static int Field2;
            
                Mixed(string x) { }

                int Instance2 => 42;
            }

            class OnlyInstance
            {
                int Instance1 => 42;
                int Instance2 { get; set; }
            }

            class OnlyStatic
            {
                static int Static1 => 42;
                static int Static2 { get; set; }
            }
            """);

    [TestMethod]
    public void InstanceAfterStaticPropertiesTest_ShouldRaise() =>
        AnalyzerVerifier.Verify<InstanceAfterStaticProperties>("""
            class Simple
            {
                static int Static1 => 42;

                int Instance1 { get; }               // Bad
                //  ^^^^^^^^^

                static int Static2 { get; set; }

                int Instance2 { get; init; }
            }

            class Mixed
            {
                static int Static1 => 42;
            
                Mixed() { }
                static Mixed() { }
            
                void Method1() { }
                void Method2() { }
            
                static void Method3() { }
                static void Method4() { }
            
                int Instance1 { get; set; }              // Bad { Move instance property 'Instance1' after all the static properties. }
            
                int Field1;
            
                static int Static2 { get; set; }
            
                static int Field2;
            
                Mixed(string x) { }
            
                int Instance2 { get; }
            }
            """);
}
