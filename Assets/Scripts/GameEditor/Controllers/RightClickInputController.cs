using System;
using Characters.Player;
using GameEditor.MapEditor;
using Models.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameEditor.Controllers
{
    public class RightClickInputController : MonoBehaviour
    {

        private GameObject _rightClickMenu;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        private void Update () {
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                switch (FindObjectOfType<MapBuilder>().EditorLeftClickActionState)
                {
                    // TODO: Build should be a result of clicking to build something!
                    case EditorLeftClickActionState.Move:
                        MovementModeRightClick();
                        break;
                    case EditorLeftClickActionState.Select:
                        Debug.Log("Selecting items!");
                        BuildGameObjectSelectorDialog();
                        break;
                    case EditorLeftClickActionState.Build:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (_rightClickMenu != null && !EventSystem.current.IsPointerOverGameObject())
            {
                ClearRightClickMenu();
            }
        }

        private void MovementModeRightClick()
        {
            Debug.Log("Right Click!");
            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);

            if (hits.Length == 0)
            {
                return;
            }

            _rightClickMenu = Instantiate( Resources.Load<GameObject>("Prefabs/BasePanel"));
            _rightClickMenu.transform.SetParent(GameObject.Find("GlobalCanvas").transform);
            _rightClickMenu.transform.position = Input.mousePosition;

            var height = hits.Length * 35;
            _rightClickMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(175, height);

            var verticalPositionOffset = (height - 35) / 2;

            foreach (var hit in hits)
            {
                var colliderGameObject = hit.collider.gameObject;

                if (colliderGameObject.GetComponent<FloorComponent>() != null)
                {
                    RightClickTileButtonBuilder(colliderGameObject.GetComponent<FloorComponent>(), verticalPositionOffset, Input.mousePosition);
                }
                else if (colliderGameObject.GetComponent<DoorComponent>() != null)
                {
                    RightClickDoorButtonBuilder(colliderGameObject.GetComponent<DoorComponent>(), verticalPositionOffset);
                }

                verticalPositionOffset -= 35;
            }
        }
        private void RightClickTileButtonBuilder(FloorComponent floorComponent, int verticalPositionOffSet, Vector3 pos)
        {
            if (floorComponent.CanEnter == false) return;
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            go.transform.SetParent(_rightClickMenu.transform, false);
            go.transform.localScale = Vector3.one;
            var b = go.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                FindObjectOfType<PlayerController>().SetSelectedPlayerCharacterPath(pos);
                ClearRightClickMenu();
            });
            b.GetComponentInChildren<Text>().text = "Walk here";

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffSet);
        }

        private void RightClickDoorButtonBuilder(DoorComponent doorComponent, int verticalPositionOffSet)
        {
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            go.transform.SetParent(_rightClickMenu.transform, false);
            go.transform.localScale = Vector3.one;
            var b = go.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                doorComponent.ToggleDoor();
                ClearRightClickMenu();
            });
            b.GetComponentInChildren<Text>().text = doorComponent.IsOpen ? "Close Door" : "Open Door";

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffSet);
        }

        private void ClearRightClickMenu()
        {
            foreach (Transform menuItem in _rightClickMenu.transform)
            {
                Destroy(menuItem.gameObject);
            }

            Destroy(_rightClickMenu);
            _rightClickMenu = null;
        }

        private void BuildGameObjectSelectorDialog()
        {
            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            Debug.Log(hits.Length);
            
        }
    }
}
