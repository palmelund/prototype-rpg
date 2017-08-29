using Characters.Player;
using GameEditor.MapEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameEditor.Controllers
{
    public class LeftClickInputController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && FindObjectOfType<MapBuilder>().MoveMode == MoveMode.Move)
            {
                PlayerController.ControllerInstance.MoveSelectedPlayerCharacterToTarget();
            }
            else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() &&
                     FindObjectOfType<MapBuilder>().MoveMode == MoveMode.BuildWall)
            {
                FindObjectOfType<MapBuilder>().BuildWall();
            }
            else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() &&
                     FindObjectOfType<MapBuilder>().MoveMode == MoveMode.BuildDoor)
            {
                FindObjectOfType<MapBuilder>().BuildDoor();
            }
        }
    }
}