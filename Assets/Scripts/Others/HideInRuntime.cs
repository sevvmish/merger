using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInRuntime : MonoBehaviour
{    
    private void Start()
    {
        if (gameObject.TryGetComponent(out MeshRenderer m))
        {
            m.enabled = false;
        }
    }
}
