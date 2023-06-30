using System.Collections.Generic;
using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    [SerializeField] private Transform minAnchor, maxAnchor;

    public List<Vector2> PlacePoints(int count)
    {
        List<Vector2> points = new List<Vector2>();

        int c = Mathf.FloorToInt(Mathf.Sqrt(count)) + 1;

        Vector2 fantamas = maxAnchor.position - minAnchor.position;

        //float aspectRatio = fantamas.x / fantamas.y;
        //int xCount = 0;
        //int yCount = 0;
        //float nearAspectRatio = float.MaxValue;

        //for (int x = 1; x < count; x++)
        //{
        //    for (int y = 1; y < count; y++)
        //    {
        //        float d = Mathf.Abs(nearAspectRatio - aspectRatio);
        //        float f = (float)x / y;

        //        if (d > Mathf.Abs(f - aspectRatio) && x * y > count)
        //        {
        //            nearAspectRatio = f;
        //            xCount = x;
        //            yCount = y;

        //            if (Mathf.Abs(nearAspectRatio - aspectRatio) < 1) break;
        //        }
        //    }
        //}

        Vector2 offset = fantamas / c;

        for (int x = 0; x < c; x++)
        {
            for (int y = 0; y < c; y++)
            {
                points.Add(new Vector2(x * offset.x + offset.x * 0.5f, y * offset.y + offset.y * 0.5f) + (Vector2)minAnchor.position);
            }
        }

        return points;
    }
}
