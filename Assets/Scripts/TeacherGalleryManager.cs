using ScriptableObjects;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class TeacherGalleryManager : MonoBehaviour
{
    [Header("Данные")]
    [SerializeField] private TeacherPattern[] _allTeachers;
    public int CurrentMoney; 

    [Header("Ссылки на UI Карточки")]
    [SerializeField] private TeacherCardUI _cardUI; 
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;

    private int _viewIndex = 0;

    private void Start()
    {
        SaveManager.Load();
        _viewIndex = SaveManager.Data.CurrentViewIndex;
        
        _nextButton.onClick.AddListener(ShowNext);
        _prevButton.onClick.AddListener(ShowPrev);

        RefreshGallery();
    }

    private void RefreshGallery()
    {
        _viewIndex = Mathf.Clamp(_viewIndex, 0, _allTeachers.Length - 1);
        
        SaveManager.Data.CurrentViewIndex = _viewIndex;
        SaveManager.Save();

        _cardUI.Setup(_allTeachers[_viewIndex], this, _viewIndex);

        _prevButton.interactable = (_viewIndex > 0);
        _nextButton.interactable = (_viewIndex < _allTeachers.Length - 1);
    }

    public void ShowNext()
    {
        _viewIndex++;
        RefreshGallery();
    }

    public void ShowPrev()
    {
        _viewIndex--;
        RefreshGallery();
    }

    public void TryBuyTeacher(TeacherPattern data, int index)
    {
        if (CurrentMoney >= data.Cost && index == SaveManager.Data.LastUnlockedIndex + 1)
        {
            CurrentMoney -= data.Cost;
            SaveManager.Data.LastUnlockedIndex = index;
            SaveManager.Save();

            RefreshGallery();
        }
    }
}