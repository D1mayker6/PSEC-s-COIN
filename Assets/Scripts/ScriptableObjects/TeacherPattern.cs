using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TeacherPattern", menuName = "Scriptable Objects/New Teacher")]
    public class TeacherPattern : ScriptableObject
    {
        [Header("Идентификатор для сохранений")]
        public int TeacherID; 

        [Header("Основная информация")]
        public string TeacherName;
        public string Phrase;
        public Sprite Sprite;

        [Header("Экономика")]
        public int Cost;
        public BonusType BonusType;
        public float BonusValue;
    }
}
