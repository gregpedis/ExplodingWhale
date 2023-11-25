using ExplodingWhale.Rules;

namespace ExplodingWhale.Test;

[TestClass]
public class InstanceAfterStaticFieldsTest
{
    [TestMethod]
    public void InstanceAfterStaticFieldsTest_NoIssues() =>
        AnalyzerVerifier.Verify<InstanceAfterStaticFields>("""
            class Mixed
            {
                static int Static1 = 42;

                Mixed() { }
                static Mixed() { }

                void Method1() { }
                void Method2() { }

                static void Method3() { }
                static void Method4() { }

                static int Static2 = 42;

                int Property1 { get; set; } 
            
                int Instance1 = 42;

                static int Property2 { get; set; } 
            
                Mixed(string x) { }

                int Instance2 = 42;
            }

            class OnlyInstance
            {
                int Instance1;
                int Instance2;
            }

            class OnlyStatic
            {
                static int Static1;
                static int Static2;
            }
            """);

    [TestMethod]
    public void InstanceAfterStaticFieldsTest_ShouldRaise() =>
        AnalyzerVerifier.Verify<InstanceAfterStaticFields>("""
            class Simple
            {
                static int Static1;

                int Instance1;               // Bad
                //  ^^^^^^^^^

                static int Static2;

                int Instance2;
            }

            class Mixed
            {
                static int Static1 = 42;
            
                Mixed() { }
                static Mixed() { }
            
                void Method1() { }
                void Method2() { }
            
                static void Method3() { }
                static void Method4() { }
            
                int Instance1;              // Bad { Move instance field 'Instance1' after all the static fields. }
            
                int Property1 { get; set; } 
            
                static int Static2;
            
                static int Property2 { get; set; } 
            
                Mixed(string x) { }
            
                int Instance2 = 42;
            }
            """);
}
