using UnityEngine;
using System.Collections;

public class Location {

    public string Name;
    public int Crosses;
    public float Distance;
	
    public Location(string name, int cross, float dist)
    {
        Name = name;
        Crosses = cross;
        Distance = dist;
    }
}
