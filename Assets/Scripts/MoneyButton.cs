using UnityEngine;
using UnityEngine.EventSystems;

public class MoneyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Animator _buttonAnimator;
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private float _clickCooldown = 0.05f;

    private float _lastClickTime;

    public void OnPointerDown(PointerEventData eventData)
    {
        _buttonAnimator.SetBool("pressed", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _buttonAnimator.SetBool("pressed", false);
        if (Time.unscaledTime - _lastClickTime < _clickCooldown) return;
        _lastClickTime = Time.unscaledTime;
        if (_currencyManager != null) _currencyManager.Add(1);
    }
}