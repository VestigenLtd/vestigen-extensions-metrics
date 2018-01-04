using System;
using System.Collections.Generic;
using Vestigen.Extensions.Metrics.Abstractions.Internal;
using Xunit;

namespace Vestigen.Extensions.Metrics.UnitTests.Internal
{
    public class TypeNameSimplifierTest
    {
        public static TheoryData<Type, string> FullTypeNameData => new TheoryData<Type, string>
        {
            // Predefined Types
            { typeof(int), "int" },
            { typeof(List<int>), "System.Collections.Generic.List" },
            { typeof(Dictionary<int, string>), "System.Collections.Generic.Dictionary" },
            { typeof(Dictionary<int, List<string>>), "System.Collections.Generic.Dictionary" },
            { typeof(List<List<string>>), "System.Collections.Generic.List" },

            // Classes inside NonGeneric class
            { typeof(A), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.A" },
            { typeof(B<int>), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.B" },
            { typeof(C<int, string>), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.C" },
            { typeof(C<int, B<string>>), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.C" },
            { typeof(B<B<string>>), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.B" },

            // Classes inside Generic class
            { typeof(Outer<int>.D), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.Outer.D" },
            { typeof(Outer<int>.E<int>), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.Outer.E" },
            { typeof(Outer<int>.F<int, string>), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.Outer.F" },
            { typeof(Outer<int>.F<int, Outer<int>.E<string>>),"Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.Outer.F" },
            { typeof(Outer<int>.E<Outer<int>.E<string>>), "Holroyd.Extensions.Metrics.Test.Internal.TypeNameSimplifierTest.Outer.E" }
        };

        [Theory]
        [MemberData(nameof(FullTypeNameData))]
        public void Can_PrettyPrint_FullTypeName(Type type, string expectedTypeName)
        {
            // Arrange & Act
            var displayName = TypeNameSimplifier.GetTypeDisplayName(type);

            // Assert
            Assert.Equal(expectedTypeName, displayName);
        }

        public static TheoryData<Type, string> BuiltInTypesData => new TheoryData<Type, string>
        {
            // Predefined Types
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(object), "object" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(string), "string" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(ushort), "ushort" },
        };

        [Theory]
        [MemberData(nameof(BuiltInTypesData))]
        public void ReturnsCommonName_ForBuiltinTypes(Type type, string expectedTypeName)
        {
            // Arrange & Act
            var displayName = TypeNameSimplifier.GetTypeDisplayName(type);

            // Assert
            Assert.Equal(expectedTypeName, displayName);
        }

        private class A { }

        private class B<T> { }

        private class C<T1, T2> { }

        private class Outer<T>
        {
            public class D { }

            public class E<T1> { }

            public class F<T1, T2> { }
        }
    }
}
