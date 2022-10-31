using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Reflection;
using System.Security.Claims;
using System.Xml.Linq;

namespace Tare_Problem1
{

//	Problem 1
//You are part of the Resistance in the Star Wars universe.
//	You are at an old rebel base on planet Craig.
//First Order forces will land on the planet in the next few hours.
//You must write an algorithm in C# to load the right number of laser cannons into the only functional 
//drone the Alliance has left in order to deploy them to the nearby peaks.
//The algorithm is fed with a collection containing a finite number of radar sourced terrain heights.
//A peak is an element in the list with both lower preceding and following neighbouring heights.
//The first and last elements in the list are not peaks by definition, because they only have one
//neighbouring height.
//For example, the list:
//var heights = new int[] { 1, 6, 4, 5, 4, 5, 1, 2, 3, 4, 7, 2 };
//	Can be depicted as:
//And we find 4 peaks, corresponding to indexes 1, 3, 5 and 10 (heights 6, 5, 5 and 7, respectively).
//You have to choose how many laser cannons the drone needs to load.The aim is to set the maximum
//number of cannons on the peaks, complying with some rules stated by R2D2:
//1. The cannons must be deployed to peaks, and each peak admits a single cannon.
//2. If you choose to load k cannons, and k > 1, the distance between any two cannons(once
//deployed) must be greater than or equal to k.If two cannons are deployed to the peaks on
//indexes P and Q, the distance between them is defined as the absolute value |P – Q|.

	internal class Program 
	{
		static void Main(string[] args)
		{
			
			var heights = new int[] { 1, 6, 4, 5, 4, 5, 1, 2, 3, 4, 7, 2 };
			StarWars starWars = new StarWars();
			Console.Write("Maximum number of cannons is : " + starWars.GetMaxCannons(heights));
			Console.Read();

		}
	}
}
