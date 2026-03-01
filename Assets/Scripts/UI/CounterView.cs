using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CounterView : MonoBehaviour
    {
        [SerializeField] private CurrencyManager _currencyManager;
        [SerializeField] private TextMeshProUGUI _moneyText;

        private void Start()
        {
            if (_currencyManager != null) _currencyManager.OnMoneyChanged += UpdateView;
            UpdateView();
        }

        private void UpdateView()
        {
            if (_currencyManager == null) return;
            _moneyText.text = $"{_currencyManager.Money}";
        }

        private void OnDestroy()
        {
            if (_currencyManager != null) _currencyManager.OnMoneyChanged -= UpdateView;
        }
    }
}