using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationLock : MonoBehaviour
{
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        DontDestroyOnLoad(this.gameObject);
    }
}
