using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            dto.Id.Should().Be(entity.Id);
        }

        [TestMethod]
        public void MappingMethodShouldCopyPropertiesAndFieldsToExistingDto()
        {
            var entity = GetTestEntity();
            var dto = GetTestDto();
            dto = dto.MapPropertiesWithFieldsFrom(entity);
            dto.Name.Should().Be(entity.Name);
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
