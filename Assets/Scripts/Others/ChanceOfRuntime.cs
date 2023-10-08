using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceOfRuntime : MonoBehaviour
{
    public float Chance = 50;

    private void Awake()
    {
        int c = UnityEngine.Random.Range(0, 100);

        if (c < Chance)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
