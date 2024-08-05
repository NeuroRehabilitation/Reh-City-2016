using UnityEngine;

public class AutomaticParameters : MonoBehaviour {

    public static int[] CalculateGrid(int elements)
    {
        var grid = new int[2];

        grid[1] = (int)Mathf.Sqrt(elements);//rows
        grid[0] = (int)Mathf.Ceil(elements / (float)grid[1]);//columns

        if (elements == 5)
        {
            grid[1] = 1;
            grid[0] = 5;
        }
        else if (elements == 10)
        {
            grid[1] = 2;
            grid[0] = 5;
        }
        else if (elements == 13 || elements == 14)
        {
            grid[1] = 2;
            grid[0] = 7;
        }
        else if (elements == 17 || elements == 18)
        {
            grid[1] = 3;
            grid[0] = 6;
        }
        else if (elements == 21)
        {
            grid[1] = 3;
            grid[0] = 7;
        }
        else if (elements >= 26 && elements <= 28)
        {
            grid[1] = 4;
            grid[0] = 7;
        }
        else if (elements == 31 || elements == 32)
        {
            grid[1] = 4;
            grid[0] = 8;
        }
        else if (elements == 36)
        {
            grid[1] = 4;
            grid[0] = 9;
        }
        else if (elements >= 37 && elements <= 40)
        {
            grid[1] = 5;
            grid[0] = 8;
        }
        else if (elements >= 41)
        {
            grid[1] = 5;
            grid[0] = 9;
        }

        return grid;
    }
}
