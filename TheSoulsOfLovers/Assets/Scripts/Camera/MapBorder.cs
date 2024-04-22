using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBorder : MonoBehaviour
{
    public Tilemap tilemapBase;
    public Vector2 min;
    public Vector2 max;

    void Awake()
    {
        if (tilemapBase != null)
        {
            tilemapBase.CompressBounds();
            Vector3 minTB = tilemapBase.cellBounds.min;
            Vector3 maxTB = tilemapBase.cellBounds.max;
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(minTB.x, minTB.y));
            points.Add(new Vector2(minTB.x, maxTB.y));
            points.Add(new Vector2(maxTB.x, maxTB.y));
            points.Add(new Vector2(maxTB.x, minTB.y));
            points.Add(new Vector2(minTB.x, minTB.y));
            GetComponent<EdgeCollider2D>().SetPoints(points);
            GetComponent<EdgeCollider2D>().enabled = true;
        }
        else if(min != null && max != null)
        {
            // Лень делать
        }
        else
            GetComponent<Collider2D>().enabled = false;
    }
}
