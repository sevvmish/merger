using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMaker : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private Transform location;
    
        
    public void SetFrames(int hor, int ver, List<Frame> frames)
    {        
        //clean
        if (location.childCount > 0)
        {
            for (int i = 0; i < location.childCount; i++)
            {
                Destroy(location.GetChild(i).gameObject);
            }
        }

        //make
        for (int i = 0; i < hor; i++)
        {
            for (int j = 0; j < ver; j++)
            {
                GameObject g = Instantiate(GameManager.Instance.GetAssets.Frame, location);
                g.transform.localPosition = new Vector3(
                    i * Globals.BASE_FRAME_OFFSET - ((float)(hor * Globals.BASE_FRAME_OFFSET) / 2.5f),
                    0,
                    j * Globals.BASE_FRAME_OFFSET - ((float)(ver * Globals.BASE_FRAME_OFFSET) / 2.5f));
                g.transform.localEulerAngles = Vector3.zero;
                frames.Add(g.GetComponent<Frame>());
            }            
        }
    }
}
