using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SimpleSolution.Test
{
    /* 
    * Description
    * ===========
    * 
    * This test will try get instance member information for a specified type. Please
    * note that the order of report should be in a constructor -> properties -> methods
    * manner. The constructors are ordered by Non-public/public and number of parameters.
    * The properties are ordered by name and the methods are ordered by name and number
    * of parameters.
    * 
    * Difficulty: Super Hard
    * 
    * Knowledge Point
    * ===============
    * 
    * - GetProperties(), GetMethods(), GetConstructors().
    * - BindingFlags enum,
    * - MemberInfo, MethodBase class
    * - Special named methods.
    */
    public class GetMemberInformation
    {
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        class ForTestCtorProperty
        {
            public ForTestCtorProperty(string name) : this(name, null)
            {
            }

            ForTestCtorProperty(string name, string optional)
            {
                Name = name;
            }

            public string Name { get; }
            public int this[int index] => index;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ForTestMethod
        {
            public int CalculateSomething(int @base, string name)
            {
                return @base + name.Length;
            }
        }

        #region Please modifies the code to pass the test

        static IEnumerable<string> GetInstanceMemberInformation(Type type)
        {
            string[] typeName = { $"Member information for {type.FullName}"};

            var constructorOfNonPublic = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(mi => mi.GetParameters().Length > 0)
                .Select(mi => "Non-public constructor: " + string.Join(", ", mi.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}")))
                .ToArray();

            var constructorOfPublic = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Where(mi => mi.GetParameters().Length > 0)
                .Select(mi => "Public constructor: " + string.Join(", ", mi.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}")))
                .ToArray();

            //type.GetConstructors().orderBy().thenBy().select(toDescription);
            //getMembers will get all properties and methd;

            var nameOfIndexField = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetIndexParameters().Length > 0)
                .Select(mi => "Indexed property Item: Public getter.")
                .ToArray();

            var nameOfNoIndexField = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetIndexParameters().Length == 0)
                .Select(p => $"Normal property {p.Name}: Public getter.")
                .ToArray();

            var methodOfPublic = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(mi => !mi.IsSpecialName)
                .Select(mi => $"Public method {mi.Name}: " + string.Join(", ", mi.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}")))
                .ToArray();

            if (constructorOfPublic.Length == 0)
            {
                constructorOfPublic = new[]{"Public constructor: no parameter" };
            }

            return typeName.Concat(constructorOfNonPublic).Concat(constructorOfPublic).Concat(methodOfPublic).Concat(nameOfIndexField).Concat(nameOfNoIndexField).ToArray();
        }

        #endregion

        static IEnumerable<object[]> GetMemberTestCases()
        {
            return new[]
            {
                new object[]
                {
                    typeof(ForTestCtorProperty), new[]
                    {
                        "Member information for SimpleSolution.Test.GetMemberInformation+ForTestCtorProperty",
                        "Non-public constructor: String name, String optional",
                        "Public constructor: String name",
                        "Indexed property Item: Public getter.",
                        "Normal property Name: Public getter."
                    }
                },
                new object[]
                {
                    typeof(ForTestMethod), new []
                    {
                        "Member information for SimpleSolution.Test.GetMemberInformation+ForTestMethod",
                        "Public constructor: no parameter",
                        "Public method CalculateSomething: Int32 base, String name",
                    }
                },
            };
        }

        [Theory]
        [MemberData(nameof(GetMemberTestCases))]
        public void should_get_member_information(Type type, string[] expected)
        {
            Assert.Equal(expected, GetInstanceMemberInformation(type));
        }
    }
}