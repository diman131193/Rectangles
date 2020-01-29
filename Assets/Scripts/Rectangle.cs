using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A rectangle
/// </summary>
public class Rectangle : MonoBehaviour
{
    const float DoubleClickTime = 0.5f;
    float lastClickTime;

    Vector3 mouseDownOffset;

    float colliderHalfWidth;
    float colliderHalfHeight;
    BoxCollider2D boxCollider2D;
    Rigidbody2D rb2d;

    private void Start()
    {
        // save for efficiency
        boxCollider2D = GetComponent<BoxCollider2D>();
        colliderHalfWidth = boxCollider2D.size.x / 2;
        colliderHalfHeight = boxCollider2D.size.y / 2;
        rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Destroy the game object by double click
    /// </summary>
    private void Update()
    {
        if (MousePointsToThisObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDownOffset = transform.position - MouseToWorldPoint();
                float timeSinceLastClick = Time.time - lastClickTime;
                if (timeSinceLastClick < DoubleClickTime)
                {
                    //Destroy the game object
                    GraphBuilder.Graph.RemoveNode(gameObject);
                    Destroy(gameObject);
                }
                lastClickTime = Time.time;
            }
        }
    }

    private void OnMouseDown()
    {
        // store the offset between the mouse down position and the GameObject position
        mouseDownOffset = transform.position - MouseToWorldPoint();
        //freez only rotation
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnMouseDrag()
    {
        // move object with mouse
        Vector3 position = mouseDownOffset + MouseToWorldPoint();
        Vector2 position2d = new Vector2(position.x, position.y);
        position2d = CalculateClamped(position2d);
        rb2d.MovePosition(position2d);
        position.x = position2d.x;
        position.y = position2d.y;
        ChangeLineRendererPosition(position);
    }

    
    private void OnMouseUp()
    {
        //freez all constraints
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
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

    /// <summary>
    /// Checks if the mouse points to this rectangle
    /// </summary>
    /// <returns>true if the mouse points to this rectangle, false otherwise</returns>
    bool MousePointsToThisObject()
    {
        Vector3 position = MouseToWorldPoint();
        if (
            (position.x > transform.position.x - colliderHalfWidth) && (
            position.x < transform.position.x + colliderHalfWidth) &&
            (position.y > transform.position.y - colliderHalfHeight) &&
            (position.y < transform.position.y + colliderHalfHeight)
            ) return true;
        return false;
    }

    /// <summary>
    /// Calculates a position to clamp the rectangle in the screen
    /// </summary>
    /// <param name="position">the position to clamp</param>
    /// <returns>the clamped position</returns>
    Vector2 CalculateClamped(Vector2 position)
    {
        // clamp left and right edges
        if (position.x - colliderHalfWidth < ScreenUtils.ScreenLeft)
        {
            position.x = ScreenUtils.ScreenLeft + colliderHalfWidth;
        }
        else if (position.x + colliderHalfWidth > ScreenUtils.ScreenRight)
        {
            position.x = ScreenUtils.ScreenRight - colliderHalfWidth;
        }
        //clamp top and bottom edges
        if (position.y - colliderHalfHeight < ScreenUtils.ScreenBottom)
        {
            position.y = ScreenUtils.ScreenBottom + colliderHalfHeight;
        }
        else if (position.y + colliderHalfHeight > ScreenUtils.ScreenTop)
        {
            position.y = ScreenUtils.ScreenTop - colliderHalfHeight;
        }
        return position;
    }

    /// <summary>
    /// Changes the line renderer position
    /// </summary>
    /// <param name="position">new line renderer position</param>
    void ChangeLineRendererPosition(Vector3 position)
    {
        Node node = GraphBuilder.Graph.Find(gameObject);
        node.SetLineRendererPosition0(position);
    }
}
