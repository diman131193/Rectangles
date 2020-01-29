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

    /// <summary>
    /// Saves BoxCollider2D half sizes and Rigidbody2D component
    /// </summary>
    private void Start()
    {
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
                    // destroy the game object
                    GraphInitializer.Graph.RemoveNode(gameObject);
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
        // freeze only rotation
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnMouseDrag()
    {
        // move object with mouse
        Vector3 rectanglePosition = mouseDownOffset + MouseToWorldPoint();
        Vector2 rectanglePosition2d = new Vector2(rectanglePosition.x, rectanglePosition.y);
        rectanglePosition2d = CalculateClamped(rectanglePosition2d);
        rb2d.MovePosition(rectanglePosition2d);
        rectanglePosition.x = rectanglePosition2d.x;
        rectanglePosition.y = rectanglePosition2d.y;
        ChangeLineRendererPosition(rectanglePosition);
    }

    
    private void OnMouseUp()
    {
        // freeze all constraints
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
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

    /// <summary>
    /// Checks if the mouse points to this rectangle
    /// </summary>
    /// <returns>true if the mouse points to this rectangle, false otherwise</returns>
    bool MousePointsToThisObject()
    {
        Vector3 mouseWorldPosition = MouseToWorldPoint();
        if (
            (mouseWorldPosition.x > transform.position.x - colliderHalfWidth) && (
            mouseWorldPosition.x < transform.position.x + colliderHalfWidth) &&
            (mouseWorldPosition.y > transform.position.y - colliderHalfHeight) &&
            (mouseWorldPosition.y < transform.position.y + colliderHalfHeight)
            ) return true;
        return false;
    }

    /// <summary>
    /// Calculates a position to clamp the rectangle in the screen
    /// </summary>
    /// <param name="rectanglePosition">the position to clamp</param>
    /// <returns>the clamped position</returns>
    Vector2 CalculateClamped(Vector2 rectanglePosition)
    {
        // clamp left and right edges
        if (rectanglePosition.x - colliderHalfWidth < ScreenUtils.ScreenLeft)
        {
            rectanglePosition.x = ScreenUtils.ScreenLeft + colliderHalfWidth;
        }
        else if (rectanglePosition.x + colliderHalfWidth > ScreenUtils.ScreenRight)
        {
            rectanglePosition.x = ScreenUtils.ScreenRight - colliderHalfWidth;
        }
        // clamp top and bottom edges
        if (rectanglePosition.y - colliderHalfHeight < ScreenUtils.ScreenBottom)
        {
            rectanglePosition.y = ScreenUtils.ScreenBottom + colliderHalfHeight;
        }
        else if (rectanglePosition.y + colliderHalfHeight > ScreenUtils.ScreenTop)
        {
            rectanglePosition.y = ScreenUtils.ScreenTop - colliderHalfHeight;
        }
        return rectanglePosition;
    }

    /// <summary>
    /// Changes the line renderer position
    /// </summary>
    /// <param name="position">new line renderer position</param>
    void ChangeLineRendererPosition(Vector3 position)
    {
        Node node = GraphInitializer.Graph.Find(gameObject);
        node.SetLineRendererPosition0(position);
    }
}
