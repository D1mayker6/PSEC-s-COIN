using UnityEngine;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UI;

public class CollectionManager : MonoBehaviour
{
    [Header("Данные")]
    [SerializeField] private TeacherPattern[] _allTeachers; 
    public int CurrentMoney; 

    [Header("UI")]
    [SerializeField] private GameObject _cardPrefab; 
    [SerializeField] private Transform _contentContainer; 

    private List<TeacherCardUI> _spawnedCards = new List<TeacherCardUI>();

    private void Start()
    {
        
        for (int i = 0; i < _allTeachers.Length; i++)
        {
            GameObject newCard = Instantiate(_cardPrefab, _contentContainer);
            TeacherCardUI cardUI = newCard.GetComponent<TeacherCardUI>();
            cardUI.Setup(_allTeachers[i], this, i); 
            _spawnedCards.Add(cardUI);
        }
        
        ApplyAllBonuses(); 
    }

    public void TryBuyTeacher(TeacherPattern data, TeacherCardUI cardUI, int index)
    {
        if (CurrentMoney >= data.Cost && index == SaveManager.Data.LastUnlockedIndex + 1)
        {
            CurrentMoney -= data.Cost;
            
            SaveManager.Data.LastUnlockedIndex = index;
            SaveManager.Save();

            UpdateAllCards();
            ApplyAllBonuses();
        }
    }

    public void UpdateAllCards()
    {
        foreach (var card in _spawnedCards)
        {
            card.UpdateUI();
        }
    }

    public void ApplyAllBonuses()
    {
        float totalClickBonus = 0;
        float totalPassiveBonus = 0;

        for (int i = 0; i <= SaveManager.Data.LastUnlockedIndex; i++)
        {
            if (i < _allTeachers.Length) 
            {
                var teacher = _allTeachers[i];
                if (teacher.BonusType == BonusType.ClickPower)
                    totalClickBonus += teacher.BonusValue;
                else if (teacher.BonusType == BonusType.PassiveIncome)
                    totalPassiveBonus += teacher.BonusValue;
            }
        }

        Debug.Log($"Бонус к клику: {totalClickBonus}, Пассив: {totalPassiveBonus}");
    }
}