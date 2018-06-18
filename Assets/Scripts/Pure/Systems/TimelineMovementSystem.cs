using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Pure.Components;
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
			[ReadOnly] public ComponentDataArray<ObjectParams> parametersArray;
			[ReadOnly] ComponentDataArray<TimelineEntity> timelinesArray;
		}

		[Inject]
		Group group;

		struct MovementJob : IJobParallelFor
		{
			public float time;

			public ComponentDataArray<TransformMatrix> matrixArray;
			[ReadOnly] public ComponentDataArray<ObjectParams> parametersArray;

			public void Execute(int index)
			{
				var matrix = matrixArray[index];
				var parameters = parametersArray[index];

				matrix.Value = math.mul(
					math.rottrans(
						math.lookRotationToQuaternion(
							parameters.Orientation, new float3(0f,1f,0f)),
							parameters.InitialPos + new float3 (-1f * time * parameters.Speed * 30f, 0f, 0f)),
						math.scale(new float3(parameters.Scaling)));

				matrixArray[index] = matrix;
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputHandle)
		{
			MovementJob job = new MovementJob
			{
				time = clipTime,
				matrixArray = group.matrixArray,
				parametersArray = group.parametersArray
			};

			return job.Schedule(group.Length, 64, inputHandle);
		}
	}
}