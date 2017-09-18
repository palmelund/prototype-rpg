using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class BasicWindow : MonoBehaviour
    {
        public RectTransform Panel;
        public Text Header;
        public GameObject DragZone;
        public GameObject ResizeZone;
        public Button CloseButton;
        public Button MinimizeButton;
        public GameObject Content;

        private bool _isMinimized;
        private float _height;

        private void Start()
        {
            CloseButton.onClick.AddListener(() =>
            {
                GameObject.Destroy(this);
            });

            MinimizeButton.onClick.AddListener(Minimize);
        }

        public void Minimize()
        {
            // TODO: Do we have to disable Content?

            if (_isMinimized)
            {
                Content.SetActive(true);
                Panel.sizeDelta = new Vector2(Panel.sizeDelta.x, _height);

                _isMinimized = !_isMinimized;
            }
            else
            {
                _height = Panel.sizeDelta.y;
                Panel.sizeDelta = new Vector2(Panel.sizeDelta.x, 40);

                Content.SetActive(false);

                _isMinimized = !_isMinimized;
            }
        }
    }
}
