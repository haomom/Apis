using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Helper
{
    public static class ObjectComparer
    {
        public static void PropertyValuesAreEquals(object actual, object expected)
        {
            PropertyInfo[] properties = expected.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object expectedValue = property.GetValue(expected, null);

                object actualValue = property.GetValue(actual, null);

                if (actualValue is IList list)
                {
                    AssertListsAreEquals(property, list, (IList) expectedValue);
                }
                else if (!Equals(expectedValue, actualValue))
                {
                    if (property.DeclaringType != null)
                    {
                        Assert.Fail($"Property {property.DeclaringType.Name}.{property.Name} does not match. Expected: {expectedValue} but was: {actualValue}");
                    }
                }
            }
        }

        public static void ListOfPropertiesValuesAreEquals(IList actualList, IList expectedList)
        {
            if (actualList.Count != expectedList.Count)
            {
                Assert.Fail($"Expected IList containing {expectedList.Count} elements but was IList containing {actualList.Count} elements");
            }
            else
            {
                for (int i = 0; i < expectedList.Count; i++)
                {
                    PropertyValuesAreEquals(actualList[i], expectedList[i]);
                }
            }
        }

        private static void AssertListsAreEquals(PropertyInfo property, IList actualList, IList expectedList)
        {
            if (actualList.Count != expectedList.Count)
            {
                Assert.Fail(
                    $"Property {property.PropertyType.Name}.{property.Name} does not match. Expected IList containing {expectedList.Count} elements but was IList containing {actualList.Count} elements");
            }
            else
            { 
                for (int i = 0; i < actualList.Count; i++)
                {
                    if (!Equals(actualList[i], expectedList[i]))
                    {
                        Assert.Fail(
                            $"Property {property.PropertyType.Name}.{property.Name} does not match. Expected IList with element {property.Name} equals to {expectedList[i]} but was IList with element {property.Name} equals to {actualList[i]}");
                    }
                }
            }
        }

        public static void AssertFilteredListByProperty(object expectedObject, IEnumerable actualList, string filteredProperty, string filteredText)
        {
            IList actualObjectList = (IList)actualList;

            PropertyInfo[] properties = expectedObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == filteredProperty)
                {
                    for (int i = 0; i < actualObjectList.Count; i++)
                    {
                        var actualValue = property.GetValue(actualObjectList[i], null);

                        switch (property.PropertyType.FullName)
                        {
                            case "System.String":
                                if (actualValue == null || !actualValue.ToString().Contains(filteredText))
                                {
                                    Assert.Fail(
                                        $"Property {property.PropertyType.Name}.{property.Name} does not match. Expected IList with element {property.Name} at index {i} to have substring {filteredText}.");
                                }

                                break;
                            case "System.Int32":
                            case "System.Int64":
                                break;
                        }
                    }
                    break;
                }
            }
        }
    }
}
