using UnityEngine;
using UnityEngine.EventSystems;
using World;

namespace GameEditor.Controllers
{
    public class RightClickInputController : MonoBehaviour {

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        private void Update () {
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                
            }
        }
    }
}
