using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackController : MonoBehaviour
{
	[SerializeField] private LevelManager _levelManager;
	[SerializeField] private List<GameObject> _cubeStack = new List<GameObject>();
	public int MaxStackCount = 15;

	private GameObject _last;
	private Queue<GameObject> _addBuffer = new Queue<GameObject>();
	private Queue<GameObject> _removingBuffer = new Queue<GameObject>();

	private OnDestroyDispatcher _onDestroyDispatcher;

	public List<GameObject> Stack => _cubeStack;

	void Start()
	{
		_onDestroyDispatcher = GetComponent<OnDestroyDispatcher>();
		_cubeStack.Add(gameObject);
		UpdateLastCube();
	}

	public void AddCube(GameObject cube)
	{
		_addBuffer.Enqueue(cube);
		lock (_cubeStack)
		{
			if (_cubeStack.Count + 1 > MaxStackCount)
			{
				_onDestroyDispatcher.DestroyObject(_addBuffer.Dequeue());
				return;
			}

			transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
			cube.transform.position = new Vector3(_last.transform.position.x, _last.transform.position.y - 1f, _last.transform.position.z);
			cube.transform.SetParent(transform);
			_cubeStack.Add(_addBuffer.Dequeue());

			UpdateLastCube();

			ProcessCubes();
			ClearRemovingBuffer();
			//если порог есть, то добавить в собранные весь порог соответствующему цвету и удалить весь этот цвет из стопки
			//если нет - то просто оставляемв стопке
		}
	}

	public void RemoveCube(GameObject cube)
	{
		cube.transform.parent = null;
		_cubeStack.Remove(cube);
		UpdateLastCube();
	}

	public void UpdateLastCube() => _last = _cubeStack.Last();

	public void ProcessCubes0()
	{
		// Группируем кубы по цвету и считаем количество каждого цвета
		var groupedCubes = _cubeStack
			.Select(cube => cube.GetComponent<CubeController>())
			.Where(controller => controller != null)
			.GroupBy(controller => controller.Color)
			.Select(group => new SerializablePair<Color, int>(group.Key, group.Count()))
			.ToList();

		// Проверяем совпадения с CollectStep и удаляем кубы
		foreach (var groupItem in groupedCubes)
		{
			try
			{
				// Находим соответствующий элемент в CollectStep
				SerializablePair<Color, int> match = _levelManager.CollectStep
					.Where(x => x.Equals(groupItem.First) && x.Second == groupItem.Second).First();

				//???
				Debug.Log("remove cubes");
				// Удаляем все кубы данного цвета из _cubeStack
				var cubesToRemove = _cubeStack.Where(cube => cube.GetComponent<CubeController>().Color.Equals(groupItem.First)).ToList();
				foreach (var cube in cubesToRemove)
				{
					RemoveCube(cube);
					_levelManager.CollectedCubes.Add(new SerializablePair<Color, int>(groupItem.First, groupItem.Second)); // Добавляем в собранные кубы
				}

			}
			catch
			{
				continue;
			}

			//if (match != null)
			//{
			//	Debug.Log("remove cubes");
			//	Удаляем все кубы данного цвета из _cubeStack
			//   var cubesToRemove = _cubeStack.Where(cube => cube.GetComponent<CubeController>().Color.Equals(groupItem.First)).ToList();
			//	foreach (var cube in cubesToRemove)
			//	{
			//		RemoveCube(cube);
			//		_levelManager.CollectedCubes.Add(new SerializablePair<Color, int>(groupItem.First, groupItem.Second)); // Добавляем в собранные кубы
			//	}
			//}
		}
	}

	public void ProcessCubes()
	{
		var colorsToRemove = new List<Color>();

		var cubeControllers = _cubeStack
			.Select(cube => cube.GetComponent<CubeController>())
			.Where(controller => controller != null)
			.ToList();

		var groupedCubes = cubeControllers
			.GroupBy(controller => controller.Color)
			.Select(group => new SerializablePair<Color, int>(group.Key, group.Count()))
			.ToList();

		foreach (var cube in groupedCubes)
		{
			var color = cube.First;
			var amount = cube.Second;

			var pair = new SerializablePair<Color, int>(color, amount);
			// collect step - min limit for add cubes in stats
			if (_levelManager.CollectStep.Contains(pair))
			{
				colorsToRemove.Add(color);
				_levelManager.AddCollectedCubes(pair.First, pair.Second);
			}
		}

		foreach (var cc in cubeControllers)
		{
			if (colorsToRemove.Contains(cc.Color))
			{
				_removingBuffer.Enqueue(cc.gameObject);
			}
		}

		ClearRemovingBuffer();
	}

	private void ClearRemovingBuffer()
	{
		foreach(var c in _removingBuffer)
		{
			if (c.gameObject != null)
			{
				RemoveCube(c);
				_onDestroyDispatcher.CollectObjectForUI(c, _levelManager.StatsPanelController.transform); // add particles
			}
		}
	}
}
