using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Pure.Components;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;

namespace Pure.Systems
{
	public class MovementSystem : JobComponentSystem
	{
		struct Group
		{
			public readonly int Length;
			public ComponentDataArray<TransformMatrix> matrixArray;

			[ReadOnly] public ComponentDataArray<Orientation> orientationArray;
			[ReadOnly] public ComponentDataArray<InitialPos> initialPosArray;
			[ReadOnly] public ComponentDataArray<Speed> speedArray;
			[ReadOnly] public ComponentDataArray<Scaling> scalingArray;
		}

		[Inject]
		Group group;

		private struct MovementJob : IJobParallelFor
		{
			public float deltaTime;
			public float time;
			public ComponentDataArray<TransformMatrix> matrixArray;

			[ReadOnly] public ComponentDataArray<Orientation> orientationArray;
			[ReadOnly] public ComponentDataArray<InitialPos> initialPosArray;
			[ReadOnly] public ComponentDataArray<Speed> speedArray;
			[ReadOnly] public ComponentDataArray<Scaling> scalingArray;

			public void Execute(int index)
			{
				var matrixComponent = matrixArray[index];

				var orientation = orientationArray[index].Value;
				var initialPos = initialPosArray[index].Value;
				var speed = speedArray[index].Value;
				var scaling = scalingArray[index].Value;

				matrixComponent.Value = math.mul
				(
					math.rottrans(orientation, initialPos + new float3(-1f * time * speed * 10f, 0f, 0f)),
					math.scale(new float3(scaling))
				);

				matrixArray[index] = matrixComponent;
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputHandle)
		{
			MovementJob job = new MovementJob
			{
				deltaTime = Time.deltaTime,
				time = Time.time,
				matrixArray = group.matrixArray,
				orientationArray = group.orientationArray,
				initialPosArray = group.initialPosArray,
				speedArray = group.speedArray,
				scalingArray = group.scalingArray
			};

			return job.Schedule(group.Length, 64, inputHandle);
		}
	}
}