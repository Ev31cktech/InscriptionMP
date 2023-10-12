using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace Inscryption_Server
{
	internal class IgnoreFieldsContractResolver : DefaultContractResolver
	{
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			var properties = base.CreateProperties(type, memberSerialization);

			// Ignore all fields
			foreach (var property in properties)
			{
				if (type.GetRuntimeProperty(property.PropertyName) == null)
				{ property.ShouldSerialize = instance => false; }
			}
			return properties;
		}
	}
}
