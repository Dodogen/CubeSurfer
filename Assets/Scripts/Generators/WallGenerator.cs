using UnityEngine;
using Assets.Scripts.Patterns;
using System.Collections.Generic;
public class WallGenerator : MonoBehaviour
{
	[SerializeField] private float  _spacing = 1f;
	[SerializeField] private GameObject cubePrefab;
	[SerializeField] private GameObject obstaclePrefab;

	public List<GameObject> GenerateCubeWall(Vector3 point, Color[,] colorPattern, bool[,] pattern)
	{
		var walls = new List<GameObject>();
		var rowCorrector = colorPattern.GetLength(0) - 1;

		for (int i = colorPattern.GetLength(0) - 1; i >= 0; i--)
		{
			for (int j = 0; j < colorPattern.GetLength(1); j++)
			{
				if (pattern[i, j])
				{
					var item = Instantiate(cubePrefab,
						new Vector3(point.x + j * _spacing, point.y + (rowCorrector - i) * _spacing, point.z),
						Quaternion.identity);
					item.GetComponent<CubeController>().ApplyMaterial(colorPattern[i, j]);

					walls.Add(item);
				}
			}
		}

		return walls;
	}
	public List<GameObject> GenerateObstacleWall(Vector3 point, bool[,] pattern)
	{
		var walls = new List<GameObject>();
		var rowCorrector = pattern.GetLength(0) - 1;

		for(int i = pattern.GetLength(0) - 1; i >= 0 ; i--)
        {
			for (int j = 0; j < pattern.GetLength(1); j++)
			{
				if (pattern[i, j])
				{
					var item = Instantiate(obstaclePrefab, 
						new Vector3(point.x + j * _spacing, point.y + (rowCorrector - i) * _spacing, point.z), 
						Quaternion.identity);

					walls.Add(item);
				}
			}
		}

		return walls;
	}
}
