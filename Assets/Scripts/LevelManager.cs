using Assets.Scripts;
using Assets.Scripts.Generators;
using Assets.Scripts.Patterns;
using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private StackController _playerStackScript;

    public List<SerializablePair<Color, int>> Goals;
    public List<SerializablePair<Color, int>> CollectedCubes;
    public List<SerializablePair<Color, int>> CollectStep;

	[SerializeField] public StatsPanelController StatsPanelController; // use it to goals
	[SerializeField] public ColorPallete _colorPallete;

    [SerializeField] private GameObject _endLevelPanel;
	private void Start()
	{
        InitGoals(3, 10, 15);
        InitCollectedStep();
        InitCollectedCubes();
		RedrawStatsUI();
	}

	private void Update()
	{
        if (_playerStackScript == null) 
        {
            Debug.Log("player null");
            FinishLevel(false); 
        }
	}
	private void InitCollectedStep()
    {
        if (Goals.Count > 0)
        {
            foreach (var goal in Goals)
            {
                // [3..6] - the most logical correct for this situation values
                CollectStep.Add(new SerializablePair<Color, int>(goal.First,
                                UnityEngine.Random.Range(3, 7)));
            }
        }
		else
		{
			throw new ArgumentNullException("Goals list has not any element");
		}
	}

	private void InitCollectedCubes()
	{
        if (Goals.Count != 0)
        {
            foreach (var x in Goals)
            {
                CollectedCubes.Add(new SerializablePair<Color, int>(x.First, 0));
            }
        }
        else
        {
            throw new ArgumentNullException("Goals list has not any element");
        }
	}

	private void InitGoals(int goalsCount, int minGoalLimit, int maxGoalsLimit)
	{
        // creates list of unique values
        var uniqueNumbers = new HashSet<int>();

		while (uniqueNumbers.Count < goalsCount && uniqueNumbers.Count < _colorPallete.Pallete.Count)
		{
			int value = UnityEngine.Random.Range(0, _colorPallete.Pallete.Count); // random from all indexes
            uniqueNumbers.Add(value);
		}

        foreach (var index in uniqueNumbers)
        {
            Goals.Add(new SerializablePair<Color, int>(_colorPallete.Pallete[index],
                UnityEngine.Random.Range(minGoalLimit, maxGoalsLimit + 1)));
        }
	}

    public void RedrawStatsUI()
    {
        StatsPanelController.Clear();

		for(int i = 0;i<Goals.Count;i++)
		{
            var color = Goals[i].First;
            var curRes = CollectedCubes.Where(x => x.First.Equals(color)).Select(x => x.Second).First();

			string text = $"{curRes}/{Goals[i].Second}";

            StatsPanelController.AddItem(color, text);
		}
	}//should be in statsPanelController?

	public bool[,] ChooseNextPattern()
    {
        // BUG порядок инициализации нарушен, длина стека вычисляется после выбора паттерна 
        // при начальной генерации карты. Сделать единую точку входа???
        // фиксит отрицательную длину стека
        var amount = _playerStackScript.Stack.Count - 1;
        amount = amount < 0 ? 0 : amount;
		return Patterns.GetPattern(amount);
    }

    public Color[,] SetColorsForCubes(bool[,] pattern)
    {
        Color[,] colors = new Color[pattern.GetLength(0), pattern.GetLength(1)];

        Dictionary<Color, int> goals = Goals.ToDictionary(x => x.First, x => x.Second);
        Dictionary<Color, int> collected = CollectedCubes.ToDictionary(x => x.First, x => x.Second);
        Dictionary<Color, int> remainsToCollect = new Dictionary<Color, int>();

        //init collection
        foreach (var goal in goals)
        {//can be ref with no calc diff 2 times
            if (collected.ContainsKey(goal.Key) && goals[goal.Key] - collected[goal.Key] > 0)
            {
                remainsToCollect.Add(goal.Key, goals[goal.Key] - collected[goal.Key]);
            }
            else
            {
                remainsToCollect.Add(goal.Key, goals[goal.Key]);
            }
        }

        var cubesToSpawn = pattern.Cast<bool>().Where(x => x == true).Count();
        var percentStep = 100 / cubesToSpawn;

        var allRemainColors = remainsToCollect.Keys.ToArray();

        // create either suited color for win or random one
        var colorsToSet = new List<Color>();
        for (int i = 0; i < cubesToSpawn; i++)
        {
            if (UnityEngine.Random.Range(0, 100) <= 100 - percentStep * i)
            {
                var randomSuitColor = allRemainColors[UnityEngine.Random.Range(0, allRemainColors.Length)];
                colorsToSet.Add(randomSuitColor);
            }
            else
            {
                var p = _colorPallete.Pallete;
				colorsToSet.Add(p[UnityEngine.Random.Range(0, p.Count)]);
            }
        }

        var cols = colors.GetLength(1);
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (pattern[i, j])
                {
                    colors[i, j] = colorsToSet[(i * cols + j)%colorsToSet.Count];
                }
            }
        }

        return colors;
    }

    public void AddCollectedCubes(Color color, int value)
    {
        var isInCollectableCubes = false;

        // check if collected cubes contains color
        // othewise returns
		for(int i = 0;i< CollectedCubes.Count;i++)
        {
            var item = CollectedCubes[i];
            if (item.First.Equals(color))
            {
                Debug.Log("pair is founded");
                item.Second += value;
                CollectedCubes[i] = item;
				isInCollectableCubes = true;

                string text = $"{item.Second}/{Goals.Where(x => x.First.Equals(item.First)).Select(x=>x.Second).First()}";
                StatsPanelController.UpdateItem(item.First,text);
                break;
            }
        }
        
        if(isInCollectableCubes) OnCubesAdd();
    }

    private void OnCubesAdd()
    {
        var res = IsGoalsReached();
        if(res) FinishLevel(res);
    }

	private bool IsGoalsReached()
	{
        for(int i = 0;i< Goals.Count; i++)
        {
            // find in collected pair which equals to goals[i] by color
            var match = CollectedCubes.Find(x => x.First.Equals(Goals[i].First));

            //if less that need
            if (match.Second < Goals[i].Second) 
            {
                Debug.Log($"one of goal are not reached\n" +
                    $"Color={match.First} value={match.Second} goal={Goals[i].Second}");
                return false;
            }
        }
        Debug.Log("Goals are reached");
        return true;
	}

    public void FinishLevel(bool result)
    {
        _endLevelPanel.SetActive(true);
        TMP_Text text = _endLevelPanel.GetComponentInChildren<TMP_Text>();
        if (result)
        {
			text.text = "You win!";
        }
        else
        {
			text.text = "You lose!";
		}
	}
}


[Serializable]
public struct SerializablePair<T, Q>
{
	public T First;
	public Q Second;
    public SerializablePair(T val1, Q val2)
    {
        First = val1;
        Second = val2;
    }
}
