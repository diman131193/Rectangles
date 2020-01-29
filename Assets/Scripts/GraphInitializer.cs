using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builds the graph
/// </summary>
public class GraphInitializer : MonoBehaviour
{
    static Graph graph;
    
    GameObject rectangle;

    /// <summary>
    /// Initializes the graph
    /// </summary>
    private void Awake()
    {
        graph = new Graph();
    }

    /// <summary>
    /// Checks if space button is pressed on the rectangle.
    /// Create a connection between the rectangles. If a connection
    /// already exists the connection is removed.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // get mouse world position
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // get collider that overlap mouse world position
            Collider2D rectangleCollider = Physics2D.OverlapPoint(new Vector2(mouseWorldPosition.x, mouseWorldPosition.y));
            if (rectangleCollider)
            {
                // space button is pressed ones on the rectangle
                if (rectangle == null)
                {
                    rectangle = rectangleCollider.gameObject;
                }
                // space button is pressed twice on the rectangle
                else if (rectangle.Equals(rectangleCollider.gameObject))
                {
                    rectangle = null;
                }
                // space button is pressed on another rectangle
                else
                {
                    if (!graph.AddEdge(rectangle, rectangleCollider.gameObject))
                    {
                        graph.RemoveEdge(rectangle, rectangleCollider.gameObject);
                    }
                        
                    rectangle = null;
                }
            }
        }
    }

    /// <summary>
    /// Gets the graph
    /// </summary>
    public static Graph Graph
    {
        get { return graph; }
    }
}
