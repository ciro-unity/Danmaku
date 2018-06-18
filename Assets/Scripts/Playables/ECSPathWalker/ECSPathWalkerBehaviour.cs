using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

using Unity.Collections;
using Unity.Entities;
using Pure.Components;
using Pure.Systems;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

[System.Serializable]
public class ECSPathWalkerBehaviour : PlayableBehaviour
{

    public PathObjectDefinition objectDefinition;
    public int numberOfInstances;

	private World w;
    private NativeArray<Entity> instances;
	private EntityManager entityManager;
	private TimelineMovementSystem timelineMovSys;
	private MeshInstanceRendererSystem meshInstRendSys;
	private TransformSystem transfSys;

    public override void OnPlayableCreate (Playable playable)
    {
		Debug.Log("Creating world");
		w = new World("EditorWorld");

        entityManager = w.GetOrCreateManager<EntityManager>();
		timelineMovSys = w.GetOrCreateManager<TimelineMovementSystem>();
		meshInstRendSys = w.GetOrCreateManager<MeshInstanceRendererSystem>();

		Entity playerShipEntity = entityManager.CreateEntity(
			ComponentType.Create<Scaling>(),
			ComponentType.Create<Speed>(),
			ComponentType.Create<InitialPos>(),
			ComponentType.Create<Orientation>(),
			ComponentType.Create<TransformMatrix>(),
			ComponentType.Create<MeshInstanceRenderer>(),
			ComponentType.Create<TimelineEntity>()
		);

		//stealing mesh and material from the Prefab
		Material shipMaterial = objectDefinition.prefab.GetComponent<MeshRenderer>().sharedMaterial;
		Mesh shipMesh = objectDefinition.prefab.GetComponent<MeshFilter>().sharedMesh;

		entityManager.SetSharedComponentData(playerShipEntity, new MeshInstanceRenderer
		{
			mesh = shipMesh,
			material = shipMaterial,
		});

		instances = new NativeArray<Entity>(numberOfInstances, Allocator.Persistent);
		entityManager.Instantiate(playerShipEntity, instances);
		foreach(Entity e in instances)
		{
			entityManager.SetComponentData(e, new Scaling(Random.Range(.1f, .8f)));
			entityManager.SetComponentData(e, new Speed(Random.Range(.5f, 2f) * objectDefinition.Speed));
			entityManager.SetComponentData(e, new InitialPos(Random.insideUnitSphere * 245f));
			entityManager.SetComponentData(e, new Orientation(Random.rotation));
		}

		entityManager.DestroyEntity(playerShipEntity);
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		meshInstRendSys.Update();
		timelineMovSys.clipTime = (float)playable.GetTime();
		timelineMovSys.Update();
	}

    public override void OnPlayableDestroy (Playable playable)
    {
        instances.Dispose();
		w.Dispose();
    }
}