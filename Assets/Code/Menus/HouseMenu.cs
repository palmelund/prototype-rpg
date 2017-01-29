using UnityEngine;

namespace Assets.Code.Menus
{
    public class HouseMenu : MonoBehaviour {
        public RectTransform Canvas;
        private bool _active;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Canvas.gameObject.SetActive(!_active);
                _active = !_active;
            }
        }
    }
}
