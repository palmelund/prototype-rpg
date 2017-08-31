using System;
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
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                switch (FindObjectOfType<MapBuilder>().EditorLeftClickActionState)
                {
                    case EditorLeftClickActionState.Move:
                        PlayerController.ControllerInstance.MoveSelectedPlayerCharacterToTarget();
                        break;
                    case EditorLeftClickActionState.BuildWall:
                        FindObjectOfType<MapBuilder>().BuildWall();
                        break;
                    case EditorLeftClickActionState.BuildDoor:
                        FindObjectOfType<MapBuilder>().BuildDoor();
                        break;
                    case EditorLeftClickActionState.Select:

                        //var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);

                        //if (hits.Length == 0)
                        //{
                        //    break;
                        //}

                        //foreach (var hit in hits)
                        //{
                            
                        //}

                        //FindObjectOfType<MapBuilder>().SelectedGameObject = 
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}