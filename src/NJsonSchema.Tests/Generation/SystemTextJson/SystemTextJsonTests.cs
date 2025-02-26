﻿using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using NJsonSchema.Annotations;
using NJsonSchema.Generation;
using Xunit;

namespace NJsonSchema.Tests.Generation.SystemTextJson
{
    public class SystemTextJsonTests
    {
        public class HealthCheckResult
        {
            [Required]
            public string Name { get; }

            public string Description { get; }
        }

        [Fact]
        public async Task When_property_is_readonly_then_its_in_the_schema()
        {
            //// Act
            var schema = JsonSchema.FromType<HealthCheckResult>();
            var data = schema.ToJson();

            //// Assert
            Assert.NotNull(data);
            Assert.Contains(@"Name", data);
            Assert.Contains(@"Description", data);
        }
        
        public class ContainerType1
        {
            public int Property { get; set; }
            public NestedType1 NestedType { get; set; }
        }
        
        public class NestedType1
        {
            public int NestedProperty { get; set; }
        }

        [Fact]
        public async Task When_type_is_excluded_then_it_should_not_be_in_the_schema()
        {
            //// Act
            var schema = JsonSchema.FromType<ContainerType1>(new SystemTextJsonSchemaGeneratorSettings
            {
                ExcludedTypeNames = [typeof(NestedType1).FullName]
            });
            var data = schema.ToJson();
            
            //// Assert
            Assert.NotNull(data);
            Assert.DoesNotContain(@"NestedType1", data);
            Assert.Contains(@"Property", data);
        }
        
        public class ContainerType2
        {
            public int Property { get; set; }
            
            [JsonSchemaIgnore]
            public NestedType2 NestedType { get; set; }
        }
        
        public class NestedType2
        {
            public int NestedProperty { get; set; }
        }
        
        [Fact]
        public async Task When_type_is_excluded_with_json_schema_ignore_attribute_then_it_should_not_be_in_the_schema()
        {
            //// Act
            var schema = JsonSchema.FromType<ContainerType2>();
            var data = schema.ToJson();
            
            //// Assert
            Assert.NotNull(data);
            Assert.DoesNotContain(@"NestedType2", data);
            Assert.Contains(@"Property", data);
        }
    }
}
