using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rectangle spawner
/// </summary>
public class RectangleSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefabRectangle;

    float colliderHalfWidth;
    float colliderHalfHeight;

    void Start()
    {
        GameObject tempRectangle = Instantiate<GameObject>(prefabRectangle);
        BoxCollider2D boxCollider2D = tempRectangle.GetComponent<BoxCollider2D>();
        // save for efficiency
        colliderHalfWidth = boxCollider2D.size.x / 2;
        colliderHalfHeight = boxCollider2D.size.y / 2;
        Destroy(tempRectangle);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && FreeArea())
        {
            Spawn();
        }
    }

    /// <summary>
    /// Checks if rectangle is spawned in free area
    /// </summary>
    /// <returns>true if spawn area is free, false otherwise</returns>
    bool FreeArea()
    {
        Vector3 position = MouseToWorldPoint();
        Vector2 locationMin =
            new Vector2(position.x - colliderHalfWidth, position.y - colliderHalfHeight);
        Vector2 locationMax =
            new Vector2(position.x + colliderHalfWidth, position.y + colliderHalfHeight);
        if (
            (Physics2D.OverlapArea(locationMin, locationMax) == null) &&
            (locationMin.x > ScreenUtils.ScreenLeft) &&
            (locationMin.y > ScreenUtils.ScreenBottom) &&
            (locationMax.x < ScreenUtils.ScreenRight) &&
            (locationMax.y < ScreenUtils.ScreenTop)
            ) return true;
        return false;
    }

    /// <summary>
    /// Spawns a rectangle
    /// </summary>
    void Spawn()
    {
        Vector3 position = MouseToWorldPoint();
        GameObject rectangle =
            Instantiate<GameObject>(prefabRectangle, position, Quaternion.identity);
        rectangle.GetComponent<SpriteRenderer>().color =
            new Color(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f)
                );
        // add the rectangle to the graph
        GraphBuilder.Graph.AddNode(rectangle);
    }

    /// <summary>
    /// Returns the mouse position as a world space position
    /// </summary>
    /// <returns>world mouse position</returns>
    Vector3 MouseToWorldPoint()
    {
        Vector3 position = Input.mousePosition;
        position.z = -Camera.main.transform.position.z;
        position = Camera.main.ScreenToWorldPoint(position);
        return position;
    }
}
