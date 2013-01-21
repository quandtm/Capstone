#pragma once

namespace Capstone
{
	namespace Core
	{
		public ref class ComponentParameterAttribute sealed : Platform::Metadata::Attribute
		{
		public:
			Platform::String^ DisplayName;
		};
	}
}
