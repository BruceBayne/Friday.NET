using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Friday.Base.ValueTypes;
using Friday.Base.ValueTypes.Token;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.Base.Tests.ValueType
{
	[TestClass]
	public class SerializationTests
	{
		[TestMethod]
		public void SubscriptionIdShouldBeSerializable()
		{
			CheckInstanceSerialization(new SubscriptionId(1));
		}


		[TestMethod]
		public void AuthTokenShouldBeSerializable()
		{
			CheckInstanceSerialization(AuthToken.Create());
		}


		private static IEnumerable<Type> EnumerateNonGenericValueTypes()
		{

			return typeof(FridayDebugger).Assembly.GetTypes().Where(t =>
				t.IsValueType && !t.IsAbstract && !t.IsGenericType);
		}

		[TestMethod]
		public void ValueTypesMarkedAsSerializableShouldBeSerializable()
		{
			var types = EnumerateNonGenericValueTypes().Where(t => t.GetCustomAttribute<SerializableAttribute>() != null).ToList();
			foreach (var type in types)
				CheckInstanceSerialization(Activator.CreateInstance(type));
		}


		[TestMethod]
		public void TwoSameNonGenericValueTypesMustEquals()
		{
			var types = EnumerateNonGenericValueTypes();
			foreach (var type in types)
			{
				var t1 = Activator.CreateInstance(type);
				var t2 = Activator.CreateInstance(type);
				t1.Should().BeEquivalentTo(t1);
				t1.Should().Be(t2);
			}
		}



		[TestMethod]
		public void PercentShouldBeSerializable()
		{
			CheckInstanceSerialization(Percent.Fifty);
		}


		private static void CheckInstanceSerialization(object instance)
		{
			instance.Should().BeBinarySerializable(instance.GetType().Name);
		}
	}
}