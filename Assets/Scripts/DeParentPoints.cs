using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeParentPoints : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
    }
}
