using FluentAssertions;
using Friday.Base.Extensions.Reflection;
using Friday.Base.Tests.Reflection.Environment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.Base.Tests.Reflection
{
	[TestClass]
	[TestCategory("Friday.Base/Mapping")]
	public class MappingTests
	{
		[TestMethod]
		public void MappingMethodShouldCopyPropertiesAndFieldsToNewDto()
		{
			var entity = GetTestEntity();
			var dto = entity.MapPropertiesWithFieldsTo<TestDto>();
			dto.Name.Should().Be(entity.Name);
			entity.Name.Should().NotBeNullOrEmpty();
			dto.Id.Should().Be(entity.Id);
		}


		[TestMethod]
		public void ObjectMappingToDtoShouldSuccess()
		{
			var entity = GetTestEntity();
			var dto = new TestDto();
			entity.MapPropertiesWithFieldsTo(dto);
			dto.Name.Should().Be(entity.Name);
			entity.Name.Should().NotBeNullOrEmpty();
			dto.Id.Should().Be(entity.Id);
		}


		



		[TestMethod]
		public void ObjectMappingFromEntityShouldSuccess()
		{
			var entity = GetTestEntity();
			var dto = new TestDto();
			dto.MapPropertiesWithFieldsFrom((object)entity);
			dto.Name.Should().Be(entity.Name);
			entity.Name.Should().NotBeNullOrEmpty();
			dto.Id.Should().Be(entity.Id);
		}




		private TestEntity GetTestEntity()
		{
			return new TestEntity();
		}

		private TestDto GetTestDto()
		{
			return new TestDto();
		}
	}
}