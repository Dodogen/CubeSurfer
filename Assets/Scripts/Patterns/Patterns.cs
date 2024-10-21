using System;
using System.Collections.Generic;

namespace Assets.Scripts.Patterns
{
	public class Patterns
	{
		private const bool f = false;
		private const bool t = true;

		public static bool[,] LAYOUT_1 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,t,f},
		{ f,t,t},
		{ f,t,f},
		{ f,t,f}
		};
		public static bool[,] LAYOUT_2 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,f,f},
		{ f,t,f},
		{ f,t,f},
		{ f,t,f}
		};
		public static bool[,] LAYOUT_3 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,f,f},
		{ f,f,t},
		{ t,f,t},
		{ f,t,t}
		};
		public static bool[,] LAYOUT_4 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,f,f},
		{ t,t,f},
		{ t,f,f},
		{ t,f,t}
		};
		public static bool[,] LAYOUT_5 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,t,t},
		{ f,t,f},
		{ f,t,t},
		{ f,t,f}
		};
		public static bool[,] LAYOUT_6 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,f,f},
		{ f,t,f},
		{ f,t,t},
		{ f,t,t}
		};
		public static bool[,] LAYOUT_7 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,f,f},
		{ f,f,f},
		{ t,f,t},
		{ f,t,f}
		};
		public static bool[,] LAYOUT_8 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,t,t},
		{ f,f,t},
		{ t,f,t},
		{ f,t,f}
		};
		public static bool[,] LAYOUT_9 = new bool[5, 3]
		{
		{ f,t,t},
		{ f,f,t},
		{ f,f,t},
		{ f,f,t},
		{ t,t,f}
		};
		public static bool[,] LAYOUT_10 = new bool[5, 3]
		{
		{ t,f,f},
		{ t,f,f},
		{ t,f,t},
		{ t,f,t},
		{ t,f,t}
		};
		public static bool[,] LAYOUT_11 = new bool[5, 3]
		{
		{ t,f,f},
		{ t,f,f},
		{ t,f,t},
		{ f,f,t},
		{ f,f,t}
		};
		public static bool[,] LAYOUT_12 = new bool[5, 3]
		{
		{ f,f,f},
		{ f,f,f},
		{ f,t,t},
		{ f,t,t},
		{ f,t,t}
		};


		private static List<bool[,]> patterns = new List<bool[,]>();
		private static List<bool[,]> patternsFor0Cubes = new List<bool[,]>();
		private static List<bool[,]> patternsFor1Cubes = new List<bool[,]>();
		private static List<bool[,]> patternsFor2Cubes = new List<bool[,]>();

		public static bool[,] GetRandomPattern()
		{
			if (patternsFor0Cubes.Count == 0)
			{
				patternsFor0Cubes.Add(LAYOUT_1);
				patternsFor0Cubes.Add(LAYOUT_2);
				patternsFor0Cubes.Add(LAYOUT_3);
				patternsFor0Cubes.Add(LAYOUT_4);
				patternsFor0Cubes.Add(LAYOUT_5);
				patternsFor0Cubes.Add(LAYOUT_6);
				patternsFor0Cubes.Add(LAYOUT_7);
				patternsFor0Cubes.Add(LAYOUT_8);
				patternsFor0Cubes.Add(LAYOUT_9);
				patternsFor0Cubes.Add(LAYOUT_10);
				patternsFor0Cubes.Add(LAYOUT_11);
				patternsFor0Cubes.Add(LAYOUT_12);
			}
			return patternsFor0Cubes[new Random().Next(patternsFor0Cubes.Count)];
		}
		public static bool[,] GetPattern(int cubesAmount) => cubesAmount switch
		{
			0 => GetPatternForZeroCubes(),
			1 => GetPatternForOneCube(),
			2 => GetPatternForTwoCubes(),
			_ => GetRandomPattern(),
		};
		private static bool[,] GetPatternForZeroCubes()
		{
			if (patternsFor0Cubes.Count == 0)
			{
				patternsFor0Cubes.Add(LAYOUT_1);
				patternsFor0Cubes.Add(LAYOUT_2);
				patternsFor0Cubes.Add(LAYOUT_3);
				//patternsFor0Cubes.Add(LAYOUT_4);
				patternsFor0Cubes.Add(LAYOUT_5);
				patternsFor0Cubes.Add(LAYOUT_6);
				//patternsFor0Cubes.Add(LAYOUT_7);
				patternsFor0Cubes.Add(LAYOUT_8);
				//patternsFor0Cubes.Add(LAYOUT_9);
				patternsFor0Cubes.Add(LAYOUT_10);
				patternsFor0Cubes.Add(LAYOUT_11);
				patternsFor0Cubes.Add(LAYOUT_12);
			}
			return patternsFor0Cubes[UnityEngine.Random.Range(0, patternsFor0Cubes.Count)];

		}
		private static bool[,] GetPatternForOneCube()
		{
			if (patternsFor1Cubes.Count == 0)
			{
				patternsFor1Cubes.Add(LAYOUT_1);
				patternsFor1Cubes.Add(LAYOUT_2);
				//patternsFor1Cubes.Add(LAYOUT_3);
				patternsFor1Cubes.Add(LAYOUT_4);
				patternsFor1Cubes.Add(LAYOUT_5);
				patternsFor1Cubes.Add(LAYOUT_6);
				patternsFor1Cubes.Add(LAYOUT_7);
				//patternsFor1Cubes.Add(LAYOUT_8);
				patternsFor0Cubes.Add(LAYOUT_9);
				patternsFor1Cubes.Add(LAYOUT_10);
				patternsFor1Cubes.Add(LAYOUT_11);
				patternsFor1Cubes.Add(LAYOUT_12);
			}
			return patternsFor1Cubes[UnityEngine.Random.Range(0, patternsFor1Cubes.Count)];

		}
		private static bool[,] GetPatternForTwoCubes()
		{
			if (patternsFor2Cubes.Count == 0)
			{
				//patterns.Add(LAYOUT_1);
				patternsFor2Cubes.Add(LAYOUT_2);
				patternsFor2Cubes.Add(LAYOUT_3);
				patternsFor2Cubes.Add(LAYOUT_4);
				patternsFor2Cubes.Add(LAYOUT_5);
				patternsFor2Cubes.Add(LAYOUT_6);
				patternsFor2Cubes.Add(LAYOUT_7);
				patternsFor2Cubes.Add(LAYOUT_8);
				patternsFor2Cubes.Add(LAYOUT_9);
				//patternsFor2Cubes.Add(LAYOUT_10);
				//patternsFor2Cubes.Add(LAYOUT_11);
				patternsFor2Cubes.Add(LAYOUT_12);
			}
			return patternsFor2Cubes[UnityEngine.Random.Range(0, patternsFor2Cubes.Count)];

		}
	}
}
