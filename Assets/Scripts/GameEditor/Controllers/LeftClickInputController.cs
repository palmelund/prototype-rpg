using System;
using Characters.Player;
using GameEditor.MapEditor;
using Models.Components;
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
                        FindObjectOfType<PlayerController>().MoveSelectedPlayerCharacterToTarget();
                        break;
                    case EditorLeftClickActionState.Select:
                        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                        if (hit.collider?.gameObject.GetComponent<IWorldComponent>() != null)
                        {
                            FindObjectOfType<MapBuilder>().SelectedGameObject = hit.collider.gameObject;
                        }
                        break;
                    case EditorLeftClickActionState.Build:
                        FindObjectOfType<MapBuilder>().BuildSelectedObject();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}