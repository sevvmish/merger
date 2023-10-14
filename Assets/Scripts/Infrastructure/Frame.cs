using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    [SerializeField] private GameObject visualsPack;
    [SerializeField] private GameObject appearEffect;
    [SerializeField] private GameObject greatEffect;
    [SerializeField] private GameObject deleteEffect;
    [SerializeField] private GameObject[] ghosts;
    public FrameTypes FrameType { get; private set; } = FrameTypes.none;
    public Vector2 Location => new Vector2(transform.localPosition.x, transform.localPosition.z);
    public bool IsEmpty() => FrameType == FrameTypes.none;
    public bool IsBuildingGhostShown { get; private set; }

    private GameManager gm;
    private bool isShaking;

    private void Start()
    {
        gm = GameManager.Instance;
        appearEffect.SetActive(false);
        greatEffect.SetActive(false);
        deleteEffect.SetActive(false);
    }

    public bool ShowGhost()
    {
        if (!IsEmpty() || IsBuildingGhostShown) return false;

        FrameTypes _type = GameManager.IsEventBuildingType(gm.CurrentGameEventToProceed);

        if (_type != FrameTypes.none)
        {
            IsBuildingGhostShown = true;
            activateGhostByType(_type);
        }

        return true;
    }

    public void HideGhost()
    {
        if (!IsBuildingGhostShown) return;
        resetGhost();
    }
       
    public void SetShake(bool isOK)
    {
        if (isOK)
        {
            isShaking = true;
            StartCoroutine(playShake());
        }
        else
        {
            isShaking = false;
        }
    }

    private IEnumerator playShake()
    {
        while (isShaking)
        {
            visualsPack.transform.DOShakeScale(0.3f, 0.3f, 30).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.5f);
        }        
    }


    private void resetVisual()
    {
        for (int i = 0; i < visualsPack.transform.childCount; i++)
        {
            visualsPack.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void resetGhost()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SetActive(false);
        }
        IsBuildingGhostShown = false;
        isShaking = false;
    }

    private void hideVisual()
    {
        resetVisual();
        resetGhost();
    }


    public void UpdateBuilding()
    {
        FrameTypes newType = (FrameTypes)((int)FrameType + 1);

        resetGhost();

        SoundController.Instance.PlayUISound(SoundsUI.positive);
        FrameType = newType;
        hideVisual();
        StartCoroutine(playShow(true, 6));
    }

    public void ChangeBuilding(FrameTypes newFrame)
    {        
        resetGhost();

        SoundController.Instance.PlayUISound(SoundsUI.positive);
        FrameType = newFrame;
        hideVisual();
        StartCoroutine(playShow(true, 6));
    }

    public void DeleteBuilding()
    {
        resetGhost();
        FrameType = FrameTypes.none;

        SoundController.Instance.PlayUISound(SoundsUI.swallow);

        StartCoroutine(playDelete());
    }
    private IEnumerator playDelete()
    {
        gm.IsVisualBusy = true;
        deleteEffect.SetActive(false);
        deleteEffect.SetActive(true);
        visualsPack.transform.DOScale(Vector3.zero, Globals.CREATE_DELETE_TIME);
        yield return new WaitForSeconds(Globals.CREATE_DELETE_TIME);
        hideVisual();
        activateVisualByType(FrameTypes.none);

        gm.IsVisualBusy = false;

        yield return new WaitForSeconds(1f);        
        visualsPack.transform.localScale = Vector3.one;
        deleteEffect.SetActive(false);
    }

    public void AddBuilding(FrameTypes _type)
    {
        AddBuilding(_type, false, 0);

    }
    public void AddBuilding(FrameTypes _type, bool isRemake, int amount)
    {
        if (!IsEmpty()) return;

        resetGhost();

        if (isRemake)
        {
            if (amount > 3)
            {
                SoundController.Instance.PlayUISound(SoundsUI.positive);
            }
            else
            {
                SoundController.Instance.PlayUISound(SoundsUI.swallow);
            }
            
        }
        else
        {
            SoundController.Instance.PlayUISound(SoundsUI.pop);
        }

        FrameType = _type;
        StartCoroutine(playShow(isRemake, amount));
    }
    private IEnumerator playShow(bool isRemake, int amount)
    {
        while(gm.IsVisualBusy)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (appearEffect.activeSelf) appearEffect.SetActive(false);
        if (greatEffect.activeSelf) greatEffect.SetActive(false);

        if (isRemake)
        {
            print("small");
            appearEffect.SetActive(true);
        }

        if (amount > 3)
        {
            print("big");
            greatEffect.SetActive(true);
        }

        visualsPack.transform.localPosition = Vector3.zero;
        activateVisualByType(FrameType);
        visualsPack.transform.localScale = Vector3.zero;
        visualsPack.transform.DOScale(Vector3.one, Globals.CREATE_DELETE_TIME);
        yield return new WaitForSeconds(Globals.CREATE_DELETE_TIME * 0.8f);

        visualsPack.transform.DOKill();
        visualsPack.transform.localScale = Vector3.one;
        visualsPack.transform.DOShakeScale(0.2f, 0.8f, 30).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(1.5f);

        if (appearEffect.activeSelf) appearEffect.SetActive(false);
        if (greatEffect.activeSelf) greatEffect.SetActive(false);
    }

    

    public void DeleteVisual(Vector3 pointToSet)
    {
        FrameType = FrameTypes.none;
        StartCoroutine(playDelete(pointToSet));
    }
    private IEnumerator playDelete(Vector3 pointToSet)
    {
        gm.IsVisualBusy = true;
        visualsPack.transform.DOMove(pointToSet, Globals.CREATE_DELETE_TIME);
        visualsPack.transform.DOScale(Vector3.one * 0.5f, Globals.CREATE_DELETE_TIME);

        yield return new WaitForSeconds(Globals.CREATE_DELETE_TIME);
        hideVisual();
        visualsPack.transform.DOLocalMove(Vector3.zero, 0);
        visualsPack.transform.localScale = Vector3.one;
        gm.IsVisualBusy = false;
    }

    private void activateVisualByType(FrameTypes _type)
    {        
        visualsPack.transform.GetChild((int)_type).gameObject.SetActive(true);
    }

    private void activateGhostByType(FrameTypes _type)
    {
        ghosts[(int)(_type - 1)].SetActive(true);
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
