
using Unity.Entities;
using Unity.Mathematics;

namespace Pure.Components
{
	public struct ObjectParams : IComponentData
	{
		public float Speed;
		public float Scaling;
		public float3 InitialPos;
		public float3 Orientation;
	}
}