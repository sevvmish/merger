using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private AssetManager assetManager;
    [SerializeField] private Ambient ambient;
    public AssetManager GetAssets => assetManager;
    
    public float GameTime;

    

    public bool IsVisualBusy;
    public float PointerClickedCount;

    private List<Frame> baseFrames;
    private HashSet<Frame> buildingToAct;
    private FrameMaker frameMaker;    
    private Dictionary<Vector2, Frame> buildingsLocations;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        ambient.SetData(AmbientType.forest);

        baseFrames = new List<Frame>();
        buildingToAct = new HashSet<Frame> ();
        frameMaker = GetComponent<FrameMaker>();
        frameMaker.SetFrames(Globals.Horizontals, Globals.Verticals, baseFrames);
        buildingsLocations = new Dictionary<Vector2, Frame>();
        for (int i = 0; i < baseFrames.Count; i++)
        {
            buildingsLocations.Add(baseFrames[i].Location, baseFrames[i]);
        }
    }

    public void UpdateState(Frame lastModifiedFrame)
    {
        if (lastModifiedFrame.IsEmpty()) return;

        buildingToAct.Clear();

        Frame frame = lastModifiedFrame;
        FrameTypes _type = frame.FrameType;
        buildingToAct.Add(frame);

        searchAllConnections(frame, buildingToAct);

        print(buildingToAct.Count);

        if (buildingToAct.Count > 2)
        {            
            FrameTypes newFrame = (FrameTypes)((int)lastModifiedFrame.FrameType + 1);
            foreach (var item in buildingToAct)
            {
                item.DeleteVisual(lastModifiedFrame.transform.position);
            }

            lastModifiedFrame.AddBuilding(newFrame, true);
            UpdateState(lastModifiedFrame);
        }
    }

    private void searchAllConnections(Frame frame, HashSet<Frame> allSet)
    {
        //check up vertical
        for (int j = 1; j < Globals.Horizontals; j++)
        {
            Vector2 vec = frame.Location + Vector2.up * j * Globals.BASE_FRAME_OFFSET;

            if (buildingsLocations.ContainsKey(vec) && buildingsLocations[vec].FrameType == frame.FrameType && !allSet.Contains(buildingsLocations[vec]))
            {
                allSet.Add(buildingsLocations[vec]);
                searchAllConnections(buildingsLocations[vec], allSet);
            }
            else
            {
                break;
            }
        }

        //check up vertical
        for (int j = 1; j < Globals.Horizontals; j++)
        {
            Vector2 vec = frame.Location + Vector2.down * j * Globals.BASE_FRAME_OFFSET;
            if (buildingsLocations.ContainsKey(vec) && buildingsLocations[vec].FrameType == frame.FrameType && !allSet.Contains(buildingsLocations[vec]))
            {
                allSet.Add(buildingsLocations[vec]);
                searchAllConnections(buildingsLocations[vec], allSet);
            }
            else
            {
                break;
            }
        }

        //check up vertical
        for (int j = 1; j < Globals.Verticals; j++)
        {
            Vector2 vec = frame.Location + Vector2.left * j * Globals.BASE_FRAME_OFFSET;
            if (buildingsLocations.ContainsKey(vec) && buildingsLocations[vec].FrameType == frame.FrameType && !allSet.Contains(buildingsLocations[vec]))
            {
                allSet.Add(buildingsLocations[vec]);
                searchAllConnections(buildingsLocations[vec], allSet);
            }
            else
            {
                break;
            }
        }

        //check up vertical
        for (int j = 1; j < Globals.Verticals; j++)
        {
            Vector2 vec = frame.Location + Vector2.right * j * Globals.BASE_FRAME_OFFSET;
            if (buildingsLocations.ContainsKey(vec) && buildingsLocations[vec].FrameType == frame.FrameType && !allSet.Contains(buildingsLocations[vec]))
            {
                allSet.Add(buildingsLocations[vec]);
                searchAllConnections(buildingsLocations[vec], allSet);
            }
            else
            {
                break;
            }
        }
    }

    private void Update()
    {
        if (PointerClickedCount > 0) PointerClickedCount -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            print("overall: " + buildingsLocations.Count);
            foreach (var item in buildingsLocations.Keys)
            {
                print(item + ": " + buildingsLocations[item]);
            }
        }


    }


    public bool AddBuildingByClick(Frame frame, FrameTypes _type)
    {
        if (!frame.IsEmpty())
        {
            return false;
        }

        frame.AddBuilding(_type);
                
        UpdateState(frame);

        return true;
    }
   

    

    private IEnumerator playShake(Transform _transform)
    {
        while (true)
        {
            _transform.DOShakeScale(0.5f, 0.3f, 30).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(1.6f);

            //transform.DOPunchScale(Vector3.one*0.2f, 0.3f).SetEase(Ease.OutQuad);
            //yield return new WaitForSeconds(0.7f);

            //transform.DOPunchPosition(Vector3.one * 0.2f, 0.3f).SetEase(Ease.OutQuad);
            //yield return new WaitForSeconds(0.7f);
        }


    }
}
