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

    /// <summary>
    /// Instantiates and destroys a temp rectangle to save
    /// BoxCollider2D half sizes
    /// </summary>
    void Start()
    {
        GameObject tempRectangle = Instantiate<GameObject>(prefabRectangle);
        BoxCollider2D boxCollider2D = tempRectangle.GetComponent<BoxCollider2D>();
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
        Vector3 mouseWorldPosition = MouseToWorldPoint();
        Vector2 lowerLeftCorner =
            new Vector2(mouseWorldPosition.x - colliderHalfWidth, mouseWorldPosition.y - colliderHalfHeight);
        Vector2 upperRightCorner =
            new Vector2(mouseWorldPosition.x + colliderHalfWidth, mouseWorldPosition.y + colliderHalfHeight);
        if (
            (Physics2D.OverlapArea(lowerLeftCorner, upperRightCorner) == null) &&
            (lowerLeftCorner.x > ScreenUtils.ScreenLeft) &&
            (lowerLeftCorner.y > ScreenUtils.ScreenBottom) &&
            (upperRightCorner.x < ScreenUtils.ScreenRight) &&
            (upperRightCorner.y < ScreenUtils.ScreenTop)
            ) return true;
        return false;
    }

    /// <summary>
    /// Spawns a rectangle
    /// </summary>
    void Spawn()
    {
        Vector3 mouseWorldPosition = MouseToWorldPoint();
        GameObject rectangle =
            Instantiate<GameObject>(prefabRectangle, mouseWorldPosition, Quaternion.identity);
        rectangle.GetComponent<SpriteRenderer>().color =
            new Color(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f)
                );
        // add the rectangle to the graph
        GraphInitializer.Graph.AddNode(rectangle);
    }

    /// <summary>
    /// Returns the mouse position as a world space position
    /// </summary>
    /// <returns>world mouse position</returns>
    Vector3 MouseToWorldPoint()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return mouseWorldPosition;
    }
}
