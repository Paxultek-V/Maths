using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionDeterminer 
{
    public enum Direction
    {
        Forward,
        Right,
        Back,
        Left,
        None
    }


    public static Direction DetermineDirection(float x, float y, float minDistance)
    {
        if (new Vector2(x, y).magnitude < minDistance)
            return Direction.None;


        float positiveX = Mathf.Abs(x);
        float positiveY = Mathf.Abs(y);

        if (positiveX > positiveY)
        {
            if (x > 0)
                return Direction.Right;
            else
                return Direction.Left;
        }
        else
        {
            if (y > 0)
                return Direction.Forward;
            else
                return Direction.Back;
        }
    }
}
