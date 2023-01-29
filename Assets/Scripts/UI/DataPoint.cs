using TMPro;
using UnityEngine;

namespace UI
{
    public class DataPoint : MonoBehaviour
    {
        #region Public Properties

        public int Value { get; private set; }

        #endregion

        [field: SerializeField]
        private RectTransform Transform { get; set; }
        [field: SerializeField]
        private TMP_Text ValueLabel { get; set; }

        #region Public Methods

        public void Initialize(int value, RectTransform parent)
        {
            Transform.parent = parent;
            Transform.anchoredPosition = Vector2.zero;
            
            Value = value;
            ValueLabel.text = Value.ToString();
        }

        public void SetPosition(Vector2 position)
        {
            Transform.anchoredPosition = position;
        }

        #endregion
    }
}