using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Pure.Components;
using UnityEngine;
using Unity.Mathematics;

namespace Pure.Systems
{
	public class MovementSystem : JobComponentSystem
	{
		private struct MovementJob : IJobProcessComponentData<ObjectParams, TransformMatrix>
		{
			public float deltaTime;
			public float time;

			public void Execute(ref ObjectParams parameters, ref TransformMatrix matrix)
			{
				matrix.Value = math.mul(
					math.rottrans(
						math.lookRotationToQuaternion(
							parameters.Orientation, new float3(0f,1f,0f)),
							parameters.InitialPos + new float3 (-1f * time * parameters.Speed * 10f, 0f, 0f)),
						math.scale(new float3(parameters.Scaling)));
			}
		}

		
		protected override JobHandle OnUpdate(JobHandle inputHandle)
		{
			MovementJob job = new MovementJob
			{
				deltaTime = Time.deltaTime,
				time = Time.time,
			};

			return job.Schedule(this, 64, inputHandle);
		}
	}
}