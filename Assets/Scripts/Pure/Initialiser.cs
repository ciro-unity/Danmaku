using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Pure.Components;
using Unity.Collections;
using UnityEngine.Rendering;
using Pure.Systems;

namespace Pure
{
	public class Initialiser : MonoBehaviour
	{
		public int numberOfInstances;
		public float speed;
		public Mesh shipMesh;
		public Material shipMaterial;

		private NativeArray<Entity> instances;

		private void Start ()
		{
			EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();

			Entity playerShipEntity = entityManager.CreateEntity(
				ComponentType.Create<Scaling>(),
				ComponentType.Create<Speed>(),
				ComponentType.Create<InitialPos>(),
				ComponentType.Create<Orientation>(),
				ComponentType.Create<TransformMatrix>(),
				ComponentType.Create<MeshInstanceRenderer>()
			);

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
				entityManager.SetComponentData(e, new Speed(Random.Range(.5f, 2f) * speed));
				entityManager.SetComponentData(e, new InitialPos(Random.insideUnitSphere * 245f));
				entityManager.SetComponentData(e, new Orientation(Random.rotation));
			}

			entityManager.DestroyEntity(playerShipEntity);
		}

		private void OnDisable()
		{
			instances.Dispose();
		}
	}
}
