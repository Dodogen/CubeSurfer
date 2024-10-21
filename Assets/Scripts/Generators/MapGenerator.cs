using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Generators
{
	public class MapGenerator : MonoBehaviour
	{
		[SerializeField] GameObject _player;
		[SerializeField, Range(0,1)] float _wallSpawnChance = 0f;

		private LevelManager _levelManager;
		private float lastPlatformPositionZ;
		private WallGenerator _wallGenerator;
		
		public GameObject platformPrefab; // Префаб платформы
		public Transform spawnPoint; // Точка появления новых платформ
		public int initialPlatformCount = 10; // Количество начальных платформ
		public float platformSpawnDistance = 10f; // Расстояние, после которого появляется новая платформа
		public List<Color> wallColors = new List<Color>(); // Набор цветов для стен
		public List<GameObject> platforms = new List<GameObject>();

		void Start()
		{
			_levelManager = GetComponent<LevelManager>();
			_wallGenerator = GetComponent<WallGenerator>();
			GenerateInitialPlatforms();
		}

		void Update()
		{
			if (_player != null && ShouldSpawnNewPlatform())
			{
				SpawnNewPlatform();
			}
		}

		private void GenerateInitialPlatforms()
		{
			for (int i = 0; i < initialPlatformCount; i++)
			{
				SpawnNewPlatform();
			}
		}

		private bool ShouldSpawnNewPlatform()
		{
			float playerPositionZ = _player.transform.position.z;
			return playerPositionZ > 
				(lastPlatformPositionZ + platforms[0].transform.position.z)/2;
		}

		private void SpawnNewPlatform()
		{
			GameObject newPlatform = Instantiate(platformPrefab, spawnPoint.position, Quaternion.identity);
			platforms.Add(newPlatform);
			lastPlatformPositionZ = newPlatform.transform.position.z;

			//removes old platform
			if (platforms.Count > initialPlatformCount)
			{
				GameObject oldPlatform = platforms[0];
				platforms.RemoveAt(0);
				Destroy(oldPlatform); // add particles??
			}

			//creates objects on platform
			var pattern = _levelManager.ChooseNextPattern();
			List<GameObject> walls;
			//if (UnityEngine.Random.Range(0f,1f) <= 0.5)
			if (UnityEngine.Random.Range(0f, 1f) <= _wallSpawnChance)
			{
				walls = _wallGenerator.GenerateCubeWall(spawnPoint.position - new Vector3(1, -1, 0),
					_levelManager.SetColorsForCubes(pattern),
					pattern);
			}
			else
			{
				walls = _wallGenerator.GenerateObstacleWall(spawnPoint.position - new Vector3(1, -1, 0), pattern);
			}

			SetObjectsCollectionAsChildFor(newPlatform, walls);

			spawnPoint.position += new Vector3(0, 0, platformSpawnDistance);
		}

		private void SetObjectsCollectionAsChildFor(GameObject parent, List<GameObject> children)
		{
			foreach (var child in children) child.transform.parent = parent.transform;
		}
	}
}
