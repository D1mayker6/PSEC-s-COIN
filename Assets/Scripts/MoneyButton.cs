using System;
using UnityEngine;
using UnityEngine.UI;

public class MoneyButton : MonoBehaviour
{
        [SerializeField] private Animator _buttonAnimator;
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
        
        public event Action OnMoneyChanged;

        private void Update()
        {
                if (Input.GetMouseButtonDown(0))
                {
                        Money += 1;
                        _buttonAnimator.SetTrigger("click");
                }
        }
}
