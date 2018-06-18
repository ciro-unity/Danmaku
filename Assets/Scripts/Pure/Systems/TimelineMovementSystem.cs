using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Pure.Components;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;
using Unity.Rendering;

namespace Pure.Systems
{
	[ExecuteInEditMode]
	public class TimelineMovementSystem : JobComponentSystem
	{
		public float clipTime;

		struct Group
		{
			public readonly int Length;
			public ComponentDataArray<TransformMatrix> matrixArray;

			[ReadOnly] public ComponentDataArray<Orientation> orientationArray;
			[ReadOnly] public ComponentDataArray<InitialPos> initialPosArray;
			[ReadOnly] public ComponentDataArray<Speed> speedArray;
			[ReadOnly] public ComponentDataArray<Scaling> scalingArray;

			[ReadOnly] ComponentDataArray<TimelineEntity> timelinesArray;
		}

		[Inject]
		Group group;

		[BurstCompile]
		struct MovementJob : IJobParallelFor
		{
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
					math.rottrans(orientation, initialPos + new float3(-1f * time * speed * 30f, 0f, 0f)),
					math.scale(new float3(scaling))
				);

				matrixArray[index] = matrixComponent;
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputHandle)
		{
			MovementJob job = new MovementJob
			{
				time = clipTime,
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