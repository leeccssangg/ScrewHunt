using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRepeatButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Button btnChoose;

    bool onHove;
    bool onChoose;

    float currentTimeHove;
    [SerializeField] float timeHoveSetting;

    float currentTimeRechoose;
    [SerializeField] float timeReChooseSetting;

    private UnityAction m_OnChooseAction;

    private void OnEnable()
    {
        onHove = false;
        onChoose = false;
    }

    private void Awake()
    {
        btnChoose.onClick.AddListener(() => {
            if (!onChoose)
            {
                OnChoose();
            }
        });
        //Setup(() => Debug.Log("Choose"));
    }
    public void Setup(UnityAction action)
    {
        m_OnChooseAction = action;
    }

    private void Update()
    {
        if (onHove)
        {
            if (currentTimeHove < timeHoveSetting)
                currentTimeHove += Time.deltaTime;
            else
            {
                if (currentTimeRechoose >= timeReChooseSetting && !onChoose)
                {
                    if (!btnChoose.interactable)    
                    {
                        onHove = false;
                        return;
                    }
                    OnChoose();
                    currentTimeRechoose = 0;
                }
                else
                    currentTimeRechoose += Time.deltaTime;
            }
        }
    }
    void OnChoose()
    {
        transform.DOScale(1f, .25f).From(.9f).SetEase(Ease.OutBounce);
        m_OnChooseAction?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onHove = true;
        currentTimeHove = 0;
        currentTimeRechoose = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onHove = false;
    }
    public void SetInteractable(bool value)
    {
        btnChoose.interactable = value;
    }
}
