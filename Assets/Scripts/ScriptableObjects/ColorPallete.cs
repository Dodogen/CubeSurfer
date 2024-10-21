using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
	[CreateAssetMenu(fileName = "New color pallete", menuName = "Color Pallete")]
	public class ColorPallete : ScriptableObject
	{
		[SerializeField] private List<Color> _colorPallete = new List<Color>
		{
			Color.blue,
			Color.cyan,
			Color.gray,
			Color.green,
			Color.grey,
			Color.magenta,
			Color.red,
			Color.yellow,
			new Color(1f, 0.5f, 0f), // Orange
            new Color(0.5f, 0f, 0.5f), // Purple
            new Color(0.5f, 0.25f, 0f), // Brown
			};


		public List<Color> Pallete
		{
			get => _colorPallete;
		}

		// if accidently smn put same colors in pallete
		private void Awake()
		{
			_colorPallete = _colorPallete.Distinct().ToList();
		}
	}
}
