using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    public void Update()
    {
        Debug.Log(Input.touchCount);

        foreach (Touch touch in Input.touches)
        {

        }

    }
}
