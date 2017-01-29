using UnityEngine;

namespace Assets.Code.Menus
{
    public class Menu : MonoBehaviour
    {

        public RectTransform MenuWindow;    // Set in editor
        private bool _active;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void OpenWindow()
        {
            _active = true;
            gameObject.SetActive(true);
        }

        public void CloseWindow()
        {
            _active = false;
            gameObject.SetActive(false);
        }
    }
}
