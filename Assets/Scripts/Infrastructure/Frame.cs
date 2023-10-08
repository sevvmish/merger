using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    [SerializeField] private GameObject visualsPack;
    public FrameTypes FrameType { get; private set; } = FrameTypes.none;

    public Vector2 Location => new Vector2(transform.localPosition.x, transform.localPosition.z);
    public bool IsEmpty() => FrameType == FrameTypes.none;

    public void AddBuilding(FrameTypes _type)
    {
        if (!IsEmpty()) return;

        ShowNewVisual(_type);
    }


    private void resetVisual()
    {
        for (int i = 0; i < visualsPack.transform.childCount; i++)
        {
            visualsPack.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void hideVisual()
    {
        resetVisual();
    }

    private void showVisual()
    {
        activateVisualByType(FrameType);
    }

    public void ShowNewVisual(FrameTypes _type)
    {
        hideVisual();
        FrameType = _type;
        showVisual();
    }

    public void DeleteVisual()
    {
        FrameType = FrameTypes.none;
        hideVisual();
    }

    private void activateVisualByType(FrameTypes _type)
    {        
        visualsPack.transform.GetChild((int)_type).gameObject.SetActive(true);
    }

}

public enum FrameTypes
{
    none,
    one,
    two,
    three,
    four,
    five,
    six,
    seven
}
