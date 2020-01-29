using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initializes the game
/// </summary>
public class GameInitializer : MonoBehaviour
{
 
    private void Awake()
    {
        // initialize screen utils
        ScreenUtils.Initialize();
    }
}
