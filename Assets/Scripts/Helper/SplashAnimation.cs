using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashAnimation : MonoBehaviour
{
    [SerializeField]
    float duration;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, duration);
    }
}
