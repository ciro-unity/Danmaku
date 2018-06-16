using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Pure.Components;
using UnityEngine;
using Unity.Mathematics;
using Unity.Rendering;

namespace Pure.Systems
{
	[ExecuteInEditMode]
	public class TimelineMovementSystem : JobComponentSystem
	{
		public float clipTime;

		private struct MovementJob : IJobProcessComponentData<ObjectParams, TransformMatrix, TimelineEntity>
		{
			public float time;

			public void Execute(ref ObjectParams parameters, ref TransformMatrix matrix, ref TimelineEntity tag)
			{
				matrix.Value = math.mul(
					math.rottrans(
						math.lookRotationToQuaternion(
							parameters.Orientation, new float3(0f,1f,0f)),
							parameters.InitialPos + new float3 (-1f * time * parameters.Speed * 30f, 0f, 0f)),
						math.scale(new float3(parameters.Scaling)));
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputHandle)
		{
			MovementJob job = new MovementJob
			{
				time = clipTime,
			};

			return job.Schedule(this, 64, inputHandle);
		}
	}
}