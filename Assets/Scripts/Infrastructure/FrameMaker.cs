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
    [SerializeField] private GameObject[] obstacles;

    private int horizontal = 3;
    private int vertical = 3;
    private Vector3 cameraPos = Vector3.zero;
    private Vector3 backScale = Vector3.one;
    private Vector3 backPos = Vector3.one;

    private List<Frame> frames = new List<Frame>();

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

    public void SetFrames(List<Frame> framesLink, Camera cameraMain)
    {
        frames = framesLink;

        setScreen(cameraMain.transform, Globals.CurrentLevel);
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
        cameraMain.transform.eulerAngles = new Vector3(60, 15, 0);

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

        setObstacles(Globals.CurrentLevel);        
    }

    private void setObstacles(int level)
    {
        if (obstacles.Length == 0) return;

        if (!Globals.IsPlayingSimpleGame && Globals.IsPlayingCustomGame)
        {
            level = 1;
        }
      
        if (level <= 3)
        {

        }
        else if (level > 3 && level < 10)
        {
            switch(level)
            {
                case 5:
                    obstacle(frames[8]);
                    break;
                case 6:
                    obstacle(frames[0]);
                    break;
                case 7:
                    obstacle(frames[2]);
                    break;
                case 8:
                    obstacle(frames[6]);
                    break;
                case 9:
                    obstacle(frames[4]);
                    break;
            }                        
            
        }
        else if (level < 15)
        {
            obstacle(new[] { frames[0], frames[15] });
        }
        else if (level < 20)
        {
            obstacle(new[] { frames[3], frames[12] });
        }
        else if (level < 30)
        {
            obstacle(new[] { frames[0], frames[3], frames[12], frames[15] });
        }
        else if (level < 34)
        {
            obstacle(new[] { frames[3], frames[12], frames[15], frames[UnityEngine.Random.Range(4, 12)] });
        }
        else if (level < 38)
        {
            obstacle(new[] { frames[0], frames[3], frames[12], frames[UnityEngine.Random.Range(4, 12)] });
        }
        else if (level < 50)
        {
            obstacle(new[] { frames[0], frames[3], frames[12], frames[15], frames[UnityEngine.Random.Range(4, 12)] });
        }
        else if (level < 60) //5 * 5
        {
            obstacle(new[] { frames[0], frames[4], frames[20], frames[24] });            
        }
        else if (level < 70) //with centered
        {
            obstacle(new[] { frames[0], frames[4], frames[20], frames[24], frames[12] });            
        }
        else if (level < 80) //random centred 1
        {
            obstacle(new[] { frames[0], frames[4], frames[20], frames[24], frames[UnityEngine.Random.Range(5, 20)] });            
        }
        else if (level < 90)
        {
            obstacle(new[] { frames[0], frames[4], frames[20], frames[24], frames[12], frames[UnityEngine.Random.Range(5, 12)] });            
        }
        else if (level < 100)
        {
            obstacle(new[] { frames[0], frames[4], frames[20], frames[24], frames[UnityEngine.Random.Range(5, 13)], frames[UnityEngine.Random.Range(13, 20)] });
        }
        else if (level < 120)
        {
            obstacle(new[] { frames[0], frames[5], frames[30], frames[35] });
        }
        else if (level < 140)
        {
            obstacle(new[] { frames[0], frames[5], frames[30], frames[35], frames[UnityEngine.Random.Range(6, 20)], frames[UnityEngine.Random.Range(19, 30)] });
        }
        else if (level < 160)
        {
            obstacle(new[] { frames[0], frames[5], frames[30], frames[35], frames[14], frames[15], frames[20], frames[21] });
        }

    }

    private void obstacle(Frame[] framesToDel)
    {
        if (obstacles.Length == 0) return;

        for (int i = 0; i < framesToDel.Length; i++)
        {
            Vector3 pos = new Vector3(framesToDel[i].Location.x, 0, framesToDel[i].Location.y);
            Destroy(framesToDel[i].gameObject);
            
            GameObject g = Instantiate(obstacles[UnityEngine.Random.Range(0, obstacles.Length)], location);
            g.transform.position = pos;
            g.transform.localEulerAngles = new Vector3(0, 90 * UnityEngine.Random.Range(0, 4), 0);
        }

        for (int i = 0; i < framesToDel.Length; i++)
        {
            frames.Remove(framesToDel[i]);
        }            
    }

    private void obstacle(Frame framesToDel)
    {
        if (obstacles.Length == 0) return;
                
        Vector3 pos = new Vector3(framesToDel.Location.x, 0, framesToDel.Location.y);
        Destroy(framesToDel.gameObject);

        GameObject g = Instantiate(obstacles[UnityEngine.Random.Range(0, obstacles.Length)], location);
        g.transform.position = pos;
        g.transform.localEulerAngles = new Vector3(0, 90 * UnityEngine.Random.Range(0, 4), 0);
                        
        frames.Remove(framesToDel);        
    }

    private void setScreen(Transform screenTransform, int level)
    {
        if (!Globals.IsPlayingSimpleGame && Globals.IsPlayingCustomGame)
        {
            level = 1000;
        }

        if (level < 10)
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
        else if (level < 50)
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
        else if (level < 100)        
        {
            horizontal = 5;
            vertical = 5;

            backFace.localScale = Vector3.one;

            if (Globals.IsMobilePlatform)
            {
                cameraPos = new Vector3(-8.5f, 70, -41.2f);
            }
            else
            {
                cameraPos = new Vector3(-12f, 84f, -49.2f);
            }

            backScale = Vector3.one * 26;
            backPos = new Vector3(0, -0.1f, 0);
        }
        else
        {
            horizontal = 6;
            vertical = 6;

            backFace.localScale = Vector3.one;

            if (Globals.IsMobilePlatform)
            {
                cameraPos = new Vector3(-9f, 83, -48f);
            }
            else
            {
                cameraPos = new Vector3(-10.8f, 99.6f, -57.6f);
            }

            backScale = Vector3.one * 31;
            backPos = new Vector3(0.5f, -0.1f, 0.5f);
        }


        Globals.Horizontals = horizontal;
        Globals.Verticals = vertical;
    }

}
