using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public event Action OnMoneyChanged;

    [SerializeField] private int _money;
    public int Money
    {
        get => _money;
        private set
        {
            _money = value;
            OnMoneyChanged?.Invoke();
        }
    }

    public void Add(int amount)
    {
        Money += amount;
    }

    public bool TrySpend(int amount)
    {
        if (Money < amount) return false;
        Money -= amount;
        return true;
    }

    public void Set(int amount)
    {
        Money = amount;
    }
}