﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A graph
/// </summary>
public class Graph
{
    #region Fields

    List<Node> nodes = new List<Node>();

    #endregion

    #region Methods

    /// <summary>
    /// Adds a node with the given value to the graph. If a node
    /// with the same value is in the graph, the value 
    /// isn't added and the method returns false
    /// </summary>
    /// <param name="value">value to add</param>
    /// <returns>true if the value is added, false otherwise</returns>
    public bool AddNode(GameObject value)
    {
        if (Find(value) != null)
        {
            // duplicate value
            return false;
        }
        else
        {
            nodes.Add(new Node(value));
            return true;
        }
    }

    /// <summary>
    /// Adds a edge between the nodes in the graph.
    /// If one or both of the values don't exist 
    /// in the graph the method returns false. If an edge
    /// already exists between the nodes the edge isn't added
    /// and the method retruns false
    /// </summary>
    /// <param name="value1">first value to connect</param>
    /// <param name="value2">second value to connect</param>
    /// <returns>true if the edge is added, false otherwise</returns>
    public bool AddEdge(GameObject value1, GameObject value2)
    {
        Node node1 = Find(value1);
        Node node2 = Find(value2);
        if (node1 == null ||
            node2 == null)
        {
            return false;
        }
        else if (node1.Neighbors.Contains(node2))
        {
            // edge already exists
            return false;
        }
        else
        {
            // add as neighbors to each other
            node1.AddNeighbor(node2);
            node2.AddNeighbor(node1);
            return true;
        }
    }

    /// <summary>
    /// Removes the node with the given value from the graph.
    /// If the node isn't found in the graph, the method 
    /// returns false
    /// </summary>
    /// <param name="value">value to remove</param>
    /// <returns>true if the value is removed, false otherwise</returns>
    public bool RemoveNode(GameObject value)
    {
        Node removeNode = Find(value);
        if (removeNode == null)
        {
            return false;
        }
        else
        {
            // need to remove as neighbor for all nodes
            // in graph
            removeNode.RemoveAllNeighbors();
            nodes.Remove(removeNode);
            foreach (Node node in nodes)
            {
                node.RemoveNeighbor(removeNode);
            }
            return true;
        }
    }

    /// <summary>
    /// Removes an edge between the nodes with the given values 
    /// from the graph. If one or both of the values don't exist 
    /// in the graph the method returns false
    /// </summary>
    /// <param name="value1">first value to disconnect</param>
    /// <param name="value2">second value to disconnect</param>
    /// <returns>true if the edge is removed, false otherwise</returns>
    public bool RemoveEdge(GameObject value1, GameObject value2)
    {
        Node node1 = Find(value1);
        Node node2 = Find(value2);
        if (node1 == null ||
            node2 == null)
        {
            return false;
        }
        else if (!node1.Neighbors.Contains(node2))
        {
            // edge doesn't exist
            return false;
        }
        else
        {
            // undirected graph, so remove as neighbors to each other
            node1.RemoveNeighbor(node2);
            node2.RemoveNeighbor(node1);
            return true;
        }
    }

    /// <summary>
    /// Finds the graph node with the given value
    /// </summary>
    /// <param name="value">value to find</param>
    /// <returns>graph node or null if not found</returns>
    public Node Find(GameObject value)
    {
        foreach (Node node in nodes)
        {
            if (node.Value.Equals(value))
            {
                return node;
            }
        }
        return null;
    }

    #endregion
}
