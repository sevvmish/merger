using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMaker : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private Transform location;
    [SerializeField] private Transform backFace;
    [SerializeField] private GameObject environment;
    [SerializeField] private GameObject one;
    [SerializeField] private GameObject two;

    private int horizontal = 3;
    private int vertical = 3;
    private Vector3 cameraPos = Vector3.zero;
    private Vector3 backScale = Vector3.one;
    private Vector3 backPos = Vector3.one;

    public void Off()
    {
        environment.SetActive(false);
        //clean
        if (location.childCount > 0)
        {
            for (int i = 0; i < location.childCount; i++)
            {
                Destroy(location.GetChild(i).gameObject);
            }
        }
    }

    public void SetFrames(List<Frame> frames, Camera cameraMain)
    {
        setScreen(cameraMain.transform);
        environment.SetActive(true);

        //clean
        if (location.childCount > 0)
        {
            for (int i = 0; i < location.childCount; i++)
            {
                Destroy(location.GetChild(i).gameObject);
            }
        }

        cameraMain.transform.localPosition = cameraPos;

        GameObject g = Instantiate(backFace.gameObject, location);
        g.transform.localPosition = backPos;
        g.transform.localEulerAngles = new Vector3(90, 0, 0);
        g.transform.localScale = backScale;

        bool isOne = true;

        //make
        for (int i = 0; i < horizontal; i++)
        {
            for (int j = 0; j < vertical; j++)
            {
                g = Instantiate(GameManager.Instance.GetAssets.Frame, location);
                g.transform.localPosition = new Vector3(
                    i * Globals.BASE_FRAME_OFFSET - ((float)(horizontal * Globals.BASE_FRAME_OFFSET) / 2.5f),
                    0,
                    j * Globals.BASE_FRAME_OFFSET - ((float)(vertical * Globals.BASE_FRAME_OFFSET) / 2.5f));
                g.transform.localEulerAngles = Vector3.zero;
                frames.Add(g.GetComponent<Frame>());

                if (isOne)
                {
                    GameObject f = Instantiate(one, location);
                    f.transform.localPosition = g.transform.localPosition;

                    isOne = false;
                }
                else
                {
                    GameObject f = Instantiate(two, location);
                    f.transform.localPosition = g.transform.localPosition;

                    isOne = true;
                }
            }
            
            if (horizontal % 2 == 0)
            {
                if (isOne)
                {                    
                    isOne = false;
                }
                else
                {                    
                    isOne = true;
                }
            }
        }

        print(frames[1].Location);
        Destroy(frames[1].gameObject);
        frames.Remove(frames[1]);
    }

    private void setScreen(Transform screenTransform)
    {
        if (Globals.CurrentLevel < 5)
        {
            horizontal = 3;
            vertical = 3;
            
            backFace.localScale = Vector3.one;

            if (Globals.IsMobilePlatform)
            {
                cameraPos = new Vector3(-8, 68, -40);
            }
            else
            {
                cameraPos = new Vector3(-9.6f, 81.6f, -48);
            }
                        
            backScale = Vector3.one * 16;
            backPos = new Vector3(-1, -0.1f, -1);
        }
        else if (Globals.CurrentLevel < 15)
        {
            horizontal = 4;
            vertical = 4;

            backFace.localScale = Vector3.one;

            if (Globals.IsMobilePlatform)
            {
                cameraPos = new Vector3(-8, 68, -40);
            }
            else
            {
                cameraPos = new Vector3(-9.6f, 81.6f, -48);
            }

            backScale = Vector3.one * 21;
            backPos = new Vector3(-0.5f, -0.1f, -0.5f);
        }
        else if (Globals.CurrentLevel < 20)        
        {
            horizontal = 5;
            vertical = 5;

            backFace.localScale = Vector3.one;

            if (Globals.IsMobilePlatform)
            {
                cameraPos = new Vector3(-8, 68, -40);
            }
            else
            {
                cameraPos = new Vector3(-9.6f, 81.6f, -48);
            }

            backScale = Vector3.one * 26;
            backPos = new Vector3(0, -0.1f, 0);
        }


        Globals.Horizontals = horizontal;
        Globals.Verticals = vertical;
    }

}
