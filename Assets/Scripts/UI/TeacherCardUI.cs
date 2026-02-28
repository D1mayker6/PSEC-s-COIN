using Enums;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TeacherCardUI : MonoBehaviour
    {
        [Header("Ссылки на UI")]
        [SerializeField] private Image _avatarImage;
        [SerializeField] private GameObject _questionMarkIcon; 
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _phraseText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _buyButton;

        private TeacherPattern _data;
        private CollectionManager _manager;
        private int _currentIndex;

        public void Setup(TeacherPattern data, CollectionManager manager, int index)
        {
            _data = data;
            _manager = manager;
            _currentIndex = index;
        
            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(OnBuyClicked);

            UpdateUI();
        }

        public void UpdateUI()
        {

            bool isUnlocked = _currentIndex <= SaveManager.Data.LastUnlockedIndex;
            bool isNextInLine = _currentIndex == SaveManager.Data.LastUnlockedIndex + 1;

            if (isUnlocked)
            {

                _avatarImage.sprite = _data.Sprite;
                _avatarImage.color = Color.white; 
                _questionMarkIcon.SetActive(false);

                _nameText.text = "Имя: " + _data.TeacherName;
                _phraseText.text = "Фраза: \"" + _data.Phrase + "\"";
                _descriptionText.text = "Описание: " + _data.Description;
            
                string bonusStr = _data.BonusType == BonusType.ClickPower ? "+ к клику" : "+ к пассиву";
                _bonusText.text = $"Бонус: {_data.BonusValue} ({bonusStr})";
            
                _buyButton.gameObject.SetActive(false); 
            }
            else
            {

                _avatarImage.sprite = null; 
                _avatarImage.color = Color.black; 
                _questionMarkIcon.SetActive(true); 

                _nameText.text = "Имя: ???";
                _phraseText.text = "Фраза: ???";
                _descriptionText.text = "Описание: ???";
                _bonusText.text = "Бонус: ???";
            
                _buyButton.gameObject.SetActive(true);
                if (isNextInLine)
                {
                    _costText.text = "Купить: " + _data.Cost;
                    _buyButton.interactable = _manager.CurrentMoney >= _data.Cost;
                }
                else
                {
                    _costText.text = "Заблокировано";
                    _buyButton.interactable = false; 
                }
            }
        }

        private void OnBuyClicked()
        {
            _manager.TryBuyTeacher(_data, this, _currentIndex);
        }
    }
}