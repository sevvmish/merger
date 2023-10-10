using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
        
    private Ray ray;
    private RaycastHit hit;
    private GameManager gm;
    private Frame currentFrame;

    private void Start()
    {
        gm = GameManager.Instance;
    }


    void Update()
    {         
        if (Input.GetMouseButtonDown(0) && !gm.IsVisualBusy && gm.PointerClickedCount <= 0)
        {            
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                        
            if (Physics.Raycast(ray, out hit, 100))
            {             
                
                if (hit.collider.gameObject.TryGetComponent(out Frame frame))
                {
                    gm.ReactOnFrameClick(frame);
                    frame.HideGhost();
                    currentFrame = frame;
                }
                
            }
        }

        if (!Globals.IsMobilePlatform) check();
    }

    private void check()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.TryGetComponent(out Frame frame))
            {
                if (currentFrame == null || currentFrame != frame)
                {
                    if (currentFrame != null) currentFrame.HideGhost();
                    frame.ShowGhost();
                    currentFrame = frame;
                }                
            }
        }
        else
        {
            if (currentFrame != null) currentFrame.HideGhost();
        }
    }
}
