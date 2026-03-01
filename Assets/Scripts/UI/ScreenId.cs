using UnityEngine;

namespace UI
{
    public class ScreenId : MonoBehaviour
    {
        [SerializeField] private int _id;
        public int Id => _id;
    }
}