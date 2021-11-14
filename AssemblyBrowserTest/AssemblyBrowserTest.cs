using System.Collections.Generic;
using System.Reflection;
using AssemblyBrowserCore.Tools;
using NUnit.Framework;

namespace AssemblyTest
{
    [TestFixture]
    public class Tests
    {
        private const BindingFlags BindingFlagsAll = BindingFlags.Instance | BindingFlags.NonPublic |
                                                     BindingFlags.Static | BindingFlags.Public |
                                                     BindingFlags.FlattenHierarchy;

        [Test]
        public void TestConstructorSignatureTool()
        {
            var constructor = typeof(TestClass).GetConstructors(BindingFlagsAll)[0];
            Assert.AreEqual("public  .ctor(System.Double testField, string testProperty)",
                constructor.GetSignature());
        }

        [Test]
        public void TestFieldSignatureTool()
        {
            var field = typeof(TestClass).GetFields(BindingFlagsAll)[0];
            Assert.AreEqual("private System.Double _testField",
                field.GetSignature());
        }

        [Test]
        public void TestMethodSignatureTool()
        {
            var method = typeof(TestClass).GetMethods(BindingFlagsAll)[2];
            Assert.AreEqual("public System.Type GetType()",
                method.GetSignature(false));
        }
    }

    public class TestClass
    {
        private double _testField;

        private string TestProperty { get; }

        public TestClass(double testField, string testProperty)
        {
            _testField = testField;
            TestProperty = testProperty;
        }

        public void TestMethod(List<List<int>> list, int a, int b)
        {
            a = b;
            b = a;
        }
    }
}