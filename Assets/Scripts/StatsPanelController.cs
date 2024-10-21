using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Drawing;
using Color = UnityEngine.Color;
using System.Linq;

public class StatsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _panelItemPrefab;

	private List<GameObject> _items = new List<GameObject>();
	private List<(Image, TMP_Text)> _statPairs = new List<(Image, TMP_Text)>();

	public void AddItem(Color color, string text)
	{
		GameObject newElement = Instantiate(_panelItemPrefab, transform);
		//RectTransform rectTransform = newElement.GetComponent<RectTransform>();
		//rectTransform.anchorMin = new Vector2(0, 0.5f);
		//rectTransform.anchorMax = new Vector2(0, 0.5f);
		//rectTransform.anchoredPosition = new Vector2(0, 0);

		TMP_Text tmText = newElement.GetComponentInChildren<TMP_Text>();
		Image image = newElement.GetComponentInChildren<Image>();

		if(tmText != null && image != null)
		{
			image.color = color;
			tmText.text = text;
		}

		_items.Add(newElement);
		_statPairs.Add((image,tmText));
		Debug.Log($"add new item with color {color}");
	}

	public void Clear()
	{
		foreach(var i in _items)
		{
			_items.Remove(i);
			Destroy(i);
			Debug.Log($"remove item. Count = {_items.Count}");
		}
	}

	public void UpdateItem(Color color, string text)
	{
		var elem = _statPairs.Where(x=>x.Item1.color.Equals(color)).First();
		elem.Item2.text = text;
	}
}
