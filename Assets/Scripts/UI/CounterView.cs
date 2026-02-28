using System;
using UnityEngine;
using UnityEngine.UI;

public class CounterView : MonoBehaviour
{
    [SerializeField] private MoneyButton _moneyButton;
    [SerializeField] private Text _moneyText;


    private void Start()
    {
        _moneyButton.OnMoneyChanged += UpdateView;
    }

    private void UpdateView()
    {
        _moneyText.text = $"{_moneyButton.Money}";
    }
}
