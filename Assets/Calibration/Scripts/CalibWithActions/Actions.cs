using UnityEngine;
using System;

using System.Collections;
using System.Collections.Generic;

public class Actions {
    public Vector3 Point1;
    public Vector3 Point2;
    public List<Vector3> PointList1;
    public List<Vector3> PointList2;

    public string Description_Point1;
    public string Description_Point2;

    protected Vector3 PointDistance;
    protected Vector3 MidPoint;

    private float Distance1;
    private float Distance2;

    private float calculatedDistancePoint1;
    private float calculatedDistancePoint2;

    private Vector3 DirectionVector;
    private Vector3 perpendicularVector;
    private Vector3 NormalizedVector;

    public Actions()
    {
        PointList1 = new List<Vector3>();
        PointList2 = new List<Vector3>();
        ClearList1();
        ClearList2();
    }

    public void AddtoList1(float X, float Y, float Z)
    {
        PointList1.Add(new Vector3(X,Y,Z));
    }
    public void AddtoList2(float X, float Y, float Z)
    {
        PointList2.Add(new Vector3(X, Y, Z));
    }
    public void Calibrate()
    {
        MidPoint = (Point1 + Point2) / 2;
     /*   MidPoint = (Point1+Point2) / 2 ;
        Distance1 = Vector3.Distance(MidPoint, Point1);
        Distance2 = Vector3.Distance(MidPoint, Point2);
     //   Debug.Log("Distance 1 " + Distance1 + " Distance 2 " + Distance2);
        DirectionVector = Point2 - Point1;
        perpendicularVector.X = DirectionVector.y;
        perpendicularVector.y = -DirectionVector.X;
        perpendicularVector.z = DirectionVector.z;
        NormalizedVector = perpendicularVector.normalized;
        R1.X = Point1.X + NormalizedVector.X * width / 2;
        R1.y = Point1.y + NormalizedVector.y * width / 2;
        R2.X = Point1.X - NormalizedVector.X * width / 2;
        R2.y = Point1.y - NormalizedVector.y * width / 2;
        R3.X = Point2.X + NormalizedVector.X * width / 2;
        R3.y = Point2.y + NormalizedVector.y * width / 2;
        R4.X = Point2.X - NormalizedVector.X * width / 2;
        R4.y = Point2.y - NormalizedVector.y * width / 2;
        Debug.Log("Rectangle R1 " + R1 + " R2 " + R2 + " R3 " + R3 + " R4 " + R4);*/

    }

    #region Rectangle Boundary
  /*  private float RectangleArea;
    private float TriangleArea;
    float R1R2P, R1R3P, R3R4P, R4R2P; 
    private bool IsInRectangle(Vector3 ReceivedPosition)
    {
        RectangleArea = 0.5F * Math.Abs(((R2.y-R3.y)*(R4.X-R1.X))+((R1.y-R4.y)*(R2.X-R3.X)));
        R1R2P = 0.5f * (R1.X * (R2.y - ReceivedPosition.y) + R2.X * (ReceivedPosition.y - R1.y) + ReceivedPosition.X * (R1.y - R2.y));
        R1R3P = 0.5f * (R1.X * (R3.y - ReceivedPosition.y) + R3.X * (ReceivedPosition.y - R1.y) + ReceivedPosition.X * (R1.y - R3.y));
        R3R4P = 0.5f * (R3.X * (R4.y - ReceivedPosition.y) + R4.X * (ReceivedPosition.y - R3.y) + ReceivedPosition.X * (R3.y - R4.y));
        R4R2P = 0.5f * (R4.X * (R2.y - ReceivedPosition.y) + R2.X * (ReceivedPosition.y - R4.y) + ReceivedPosition.X * (R4.y - R2.y));
        TriangleArea = R1R2P + R1R3P + R3R4P + R4R2P;

        return RectangleArea <= TriangleArea;
    }

    private float triangleArea(Vector3 A, Vector3 B, Vector3 C)
    {
        return (C.X * B.y - B.X * C.y) - (C.X * A.y - A.X * C.y) + (B.X * A.y - A.X * B.y);
    }
    private bool IsInRectangle2(Vector3 P)
    {
        Debug.Log(triangleArea(R1, R2, P));
        if (triangleArea(R1, R2, P) > 0 || triangleArea(R1, R3, P) > 0 || triangleArea(R3, R4, P) > 0 || triangleArea(R4, R2, P) > 0)
        {
            return false;
        }
        return true;

    }
    private bool IsInRectangle3(Vector3 P)
    {
        if (((P.X <= R1.X) && (P.X <= R2.X)) && ((P.y <= R1.y) && (P.y >= R2.y)))
        {
            if (((P.X >= R3.X) && (P.X >= R4.X)) && ((P.y <= R3.y) && (P.y >= R4.y)))
            {
                return true;
            }
        }
        return false;
    }*/
    #endregion


    public Vector3 GetCurrentPosition(Vector3 ReceivedPosition,bool MinusOnetoOne, bool Analog, bool Digital)
    {
        Vector3 pos = Vector3.zero;

        pos.x = MinusOnetoOne ? (ReceivedPosition.x - MidPoint.x) / (Point1.x - Point2.x) : (ReceivedPosition.x - Point2.x) / (Point1.x - Point2.x);
        pos.y = MinusOnetoOne ? (ReceivedPosition.y - MidPoint.y) / (Point1.y - Point2.y) : (ReceivedPosition.y - Point2.y) / (Point1.y - Point2.y);
        pos.z = MinusOnetoOne ? (ReceivedPosition.z - MidPoint.z) / (Point1.z - Point2.z) : (ReceivedPosition.z - Point2.z) / (Point1.z - Point2.z);
        if (Point1.x == Point2.x)
        {
            pos.x = 0;
        }
        if (Point1.y == Point2.y)
        {
            pos.y = 0;
        }

        if (Point1.z == Point2.z)
        {
            pos.z = 0;
        }
        if (Digital)
        {
            // ignore values between 0 - 0.7f
            pos = (pos/0.8f);
            //scaley = (int)(scaley / 0.8f);
        }
        else if (Analog)
        {
            // ignore values between 0 - 0.2f
            pos.x = (Math.Abs(pos.x) > 0.2f) ? pos.x : 0;
            pos.y = (Math.Abs(pos.y) > 0.2f) ? pos.y : 0;
            pos.z = (Math.Abs(pos.z) > 0.2f) ? pos.z : 0;
        }
        return pos;
    }
    
    //Normalized Distance
    float normalized = 0;
    public float Direction(Vector3 ReceivedPosition,bool Digital)
    {
      //  if (!IsInRectangle3(ReceivedPosition)) return 0;
        calculatedDistancePoint1 = Vector3.Distance(ReceivedPosition, Point1);
        calculatedDistancePoint2 = Vector3.Distance(ReceivedPosition, Point2);
        normalized = 2 * (calculatedDistancePoint1 / (calculatedDistancePoint1 + calculatedDistancePoint2)) - 1;
        if (Digital)
        {
            normalized = (int)(normalized / 0.8f);
            return normalized;
        }
        else
        {
            normalized = Mathf.Abs(normalized) > 0.2f ? normalized : 0;
            return normalized;
        }   
    }

    #region Sort
    public void SortList1()
    {
      // InsertionSort(PointList1);
        for (int i = PointList1.Count - 1; i > PointList1.Count-3; i--)
        {
            Point1 = PointList1[i] + PointList1[i - 1];
            //Debug.Log("averaged " + i + " terms");
        }
      // Point1 = PointList1[PointList1.Count - 1];
        Point1 = Point1 / 2;
       
    }
    public void SortList2()
    {
      //  InsertionSort(PointList2);
        for (int i = PointList2.Count - 1; i > PointList2.Count - 6; i--)
        {
            Point2 = PointList2[i] + PointList2[i - 1];
        }
        Point2 = Point2 / 2;
       // Point2 = PointList2[PointList2.Count - 1];
    }

    void InsertionSort(List<Vector3> Sortarray)
    {
        for (int i = 0; i < Sortarray.Count; i++)
        {
            for (int j = i; (j > 0 && less(Sortarray[j].magnitude, Sortarray[j - 1].magnitude)); j--)
            {
                exchange(Sortarray, j, j - 1);
            }
        }
    }

    bool less(float a, float b)
    {
        if (a <= b) return true;
        return false;
    }

    void exchange(List<Vector3> arr , int a, int b)
    {
        Vector3 temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }
    #endregion

    public void ClearList1()
    {
        if (PointList1.Count != 0)
        {
            PointList1.Clear();
        }
    }
    public void ClearList2()
    {
        if (PointList2.Count != 0)
        {
            PointList2.Clear();
        }
    }
}
