using UnityEngine;

namespace Assets.Code.Menus
{
    public class DiplomacyMenu : MonoBehaviour {
        public RectTransform Canvas;
        private bool _active;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Canvas.gameObject.SetActive(!_active);
                _active = !_active;
            }
        }
    }
}
