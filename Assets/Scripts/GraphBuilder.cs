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

    void Awake()
    {
        graph = new Graph();

        // add edges to graph
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 position = Input.mousePosition;
            position = Camera.main.ScreenToWorldPoint(position);
            Collider2D coll = Physics2D.OverlapPoint(new Vector2(position.x, position.y));
            if (coll)
            {
                if (firstObject == null)
                {
                    firstObject = coll.gameObject;
                }
                else if (firstObject.Equals(coll.gameObject))
                {
                    firstObject = null;
                }
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
    /// <value>graph</value>
    public static Graph Graph
    {
        get { return graph; }
    }
}
