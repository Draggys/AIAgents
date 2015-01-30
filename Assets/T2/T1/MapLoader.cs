using UnityEngine;
using System.Collections;
using System;

public class MapLoader{
	private string prefix = "Assets/Data/Discrete/";
	private string postfix = ".txt";

	public string mapName;
	public string startName;
	public string endName;

	public int maxX;
	public int maxY;

	public Vector2 gridWorldSize;
	public float nodeRadius;

	public MapLoader(Vector2 gridWorldSize, float nodeRadius) {
		this.gridWorldSize = gridWorldSize;
		this.nodeRadius = nodeRadius;

		maxX = (int) Math.Round (gridWorldSize.x);
		maxY = (int) Math.Round (gridWorldSize.y);
	}

	public MapData LoadMap(string mapName, string startName, string endName)
	{
		mapName = prefix + mapName + postfix;
		startName = prefix + startName + postfix;
		endName = prefix + endName + postfix;

		MapData mapData = new MapData ();
		mapData.walkable = new bool[(int) Math.Round (gridWorldSize.x), (int) Math.Round (gridWorldSize.y)];

		// Read map
		int x = 0;
		string line;
		System.IO.StreamReader file = new System.IO.StreamReader (mapName);
		while ((line = file.ReadLine ()) != null) {
			string[] walkable = line.Split (' ');
			for(int y = 0; y < walkable.Length; y++) {
				//print ("x: " + x + " y: " + y + " walkable: " + walkable[y]);
				if(walkable[y].Equals ("1"))
					mapData.walkable[x, y] = false;
				else
					mapData.walkable[x, y] = true;
			}
			x++;
		}


		// read start and end pos
		string start = System.IO.File.ReadAllText (startName);
		string[] startList = start.Split (' ');
		mapData.start = new Vector2(float.Parse (startList[0]), float.Parse (startList[1]));
		
		string end = System.IO.File.ReadAllText (startName);
		string[] endList = end.Split (' ');
		mapData.end = new Vector2 (float.Parse (endList [0]), float.Parse (endList [1]));
		
		
		mapData.gridWorldSize = gridWorldSize;
		mapData.nodeRadius = nodeRadius;
		return mapData;
	}
	/*
	public MapData LoadMap(string mapName, string startName, string endName) 
	{
		mapName = prefix + mapName + postfix;
		startName = prefix + startName + postfix;
		endName = prefix + endName + postfix;

		// read map
		string map = System.IO.File.ReadAllText (mapName);
		string[] walkable = map.Split (' ');

		MapData mapData = new MapData();
		mapData.walkable = new bool[(int) Math.Round (gridWorldSize.x), (int) Math.Round (gridWorldSize.y)];

		print (walkable.Length);

		int i = 0;
		int x = 0;
		while (x < maxX) {
			for(int y = 0; y < maxY; y++) {
				mapData.walkable[x, y] = true;
				if(walkable[i].Equals ("0")) {
					mapData.walkable[x, y] = true;
				}
				else {
					mapData.walkable[x, y] = true;
				}
				i++;
			}
			x++;
		}
		for(int f = 0; f < 5; f++) {
			print ("grrr: " + f + " : " + mapData.walkable[0, f].ToString ());
		}

		// read start and end pos
		string start = System.IO.File.ReadAllText (startName);
		string[] startList = start.Split (' ');
		mapData.start = new Vector2(float.Parse (startList[0]), float.Parse (startList[1]));

		string end = System.IO.File.ReadAllText (startName);
		string[] endList = end.Split (' ');
		mapData.end = new Vector2 (float.Parse (endList [0]), float.Parse (endList [1]));


		mapData.gridWorldSize = gridWorldSize;
		mapData.nodeRadius = nodeRadius;
		return mapData;
	}
	*/
}