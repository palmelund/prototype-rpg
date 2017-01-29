using UnityEngine;

namespace Assets.Code.Menus
{
    public class InventoryMenu : MonoBehaviour
    {
        public RectTransform Canvas;
        private bool _active;

        void Start () {
		
        }
	
        void Update ()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Canvas.gameObject.SetActive(!_active);
                _active = !_active;
            }
        }

        public void CloseMenu()
        {
            Canvas.gameObject.SetActive(false);
            _active = false;
        }
    }
}
