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
        
        foreach (var teacher in _allTeachers)
        {
            GameObject newCard = Instantiate(_cardPrefab, _contentContainer);
            TeacherCardUI cardUI = newCard.GetComponent<TeacherCardUI>();
            cardUI.Setup(teacher, this);
            _spawnedCards.Add(cardUI);
        }
        
        ApplyAllBonuses(); 
    }

    public void TryBuyTeacher(TeacherPattern data, TeacherCardUI cardUI)
    {
        if (CurrentMoney >= data.Cost)
        {
            
            CurrentMoney -= data.Cost;

            PlayerPrefs.SetInt(data.TeacherID, 1);
            PlayerPrefs.Save();

            cardUI.UpdateUI();

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

        foreach (var teacher in _allTeachers)
        {
            if (PlayerPrefs.GetInt(teacher.TeacherID, 0) == 1) // Если куплен
            {
                if (teacher.BonusType == BonusType.ClickPower)
                    totalClickBonus += teacher.BonusValue;
                else if (teacher.BonusType == BonusType.PassiveIncome)
                    totalPassiveBonus += teacher.BonusValue;
            }
        }

        Debug.Log($"Бонус к клику: {totalClickBonus}, Пассив: {totalPassiveBonus}");
    }
}