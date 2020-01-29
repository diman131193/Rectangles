using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A graph node
/// </summary>
public class Node
{
    #region Fields

    GameObject value;
    List<Node> neighbors;
    List<GameObject> lineRenderers;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">value for the node</param>
    public Node(GameObject value)
    {
        this.value = value;
        neighbors = new List<Node>();
        lineRenderers = new List<GameObject>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the value stored in the node
    /// </summary>
    public GameObject Value
    {
        get { return value; }
    }

    /// <summary>
    /// Gets a read-only list of the neighbors of the node
    /// </summary>
    public IList<Node> Neighbors
    {
        get { return neighbors.AsReadOnly(); }
    }

    /// <summary>
    /// Gets a read-only list of the lineRenderers of the node
    /// </summary>
    public IList<GameObject> LineRenderers
    {
        get { return lineRenderers.AsReadOnly(); }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the given node as a neighbor for this node
    /// </summary>
    /// <param name="neighbor">neighbor to add</param>
    public bool AddNeighbor(Node neighbor)
    {
        // don't add duplicate nodes
        if (neighbors.Contains(neighbor))
        {
            return false;
        }
        else
        {
            neighbors.Add(neighbor);
            lineRenderers.Add(AddLineRenderer(value.transform.position, neighbor.Value.transform.position));
            return true;
        }
    }

    public GameObject AddLineRenderer(Vector3 position1, Vector3 position2)
    {
        // add line renderer and draw line
        GameObject lineObj = new GameObject("LineObj");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        //Set color
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

        //Set width
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition(0, position1);
        lineRenderer.SetPosition(1, position2);

        return lineObj;
    }

    /// <summary>
    /// Removes the given node as a neighbor for this node
    /// </summary>
    /// <param name="neighbor">neighbor to remove</param>
    /// <returns>true if the neighbor was removed, false otherwise</returns>
    public bool RemoveNeighbor(Node neighbor)
    {
        int index = neighbors.IndexOf(neighbor);
        if (index == -1)
        {
            // neighbor not in list
            return false;
        }
        else
        {
            // remove neighbor
            neighbors.RemoveAt(index);
            GameObject lineRenderer = lineRenderers[index];
            lineRenderers.RemoveAt(index);
            Object.Destroy(lineRenderer);
            return true;
        }
    }

    /// <summary>
    /// Removes all the neighbors for the node
    /// </summary>
    /// <returns>true if the neighbors were removed, false otherwise</returns>
    public bool RemoveAllNeighbors()
    {
        for (int i = neighbors.Count - 1; i >= 0; i--)
        {
            neighbors.RemoveAt(i);
            GameObject lineRenderer = lineRenderers[i];
            lineRenderers.RemoveAt(i);
            Object.Destroy(lineRenderer);
        }
        return true;
    }

    public void SetLineRendererPosition0(Vector3 position)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            lineRenderers[i].GetComponent<LineRenderer>().SetPosition(0, position);
            neighbors[i].SetLineRendererPosition1(position, this);
        }
    }

    public void SetLineRendererPosition1(Vector3 position, Node node)
    {
        int index = neighbors.IndexOf(node);
        lineRenderers[index].GetComponent<LineRenderer>().SetPosition(1, position);
    }

    #endregion
}