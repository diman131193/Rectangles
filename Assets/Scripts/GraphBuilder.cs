using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builds the graph
/// </summary>
public class GraphBuilder : MonoBehaviour
{
    static Graph graph;
    
    GameObject firstObject;

    /// <summary>
    /// Initializes the graph
    /// </summary>
    private void Awake()
    {
        graph = new Graph();
    }

    /// <summary>
    /// Checks if Space button is pressed on the rectangle.
    /// Create a connection between the rectangles. If a connection
    /// already exists the connection is removed.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //get mouse world position
            Vector3 position = Input.mousePosition;
            position = Camera.main.ScreenToWorldPoint(position);
            //get collider that overlap mouse world position
            Collider2D coll = Physics2D.OverlapPoint(new Vector2(position.x, position.y));
            if (coll)
            {
                //Space button is pressed ones on the rectangle
                if (firstObject == null)
                {
                    firstObject = coll.gameObject;
                }
                //Space button is pressed twice on the rectangle
                else if (firstObject.Equals(coll.gameObject))
                {
                    firstObject = null;
                }
                //Space button is pressed on another rectangle.
                else
                {
                    if (!graph.AddEdge(firstObject, coll.gameObject))
                        graph.RemoveEdge(firstObject, coll.gameObject);
                    firstObject = null;
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
