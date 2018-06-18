
using Unity.Entities;
using Unity.Mathematics;

namespace Pure.Components
{
	struct Speed : IComponentData
	{
		public float Value;

		public Speed(float value)
		{
			Value = value;
		}
	}

	struct Scaling : IComponentData
	{
		public float Value;

		public Scaling(float value)
		{
			Value = value;
		}
	}

	struct InitialPos : IComponentData
	{
		public float3 Value;

		public InitialPos(float3 value)
		{
			Value = value;
		}
	}

	struct Orientation : IComponentData
	{
		public quaternion Value;

		public Orientation(quaternion value)
		{
			Value = value;
		}
	}
}