﻿using System.Collections.Generic;
using Characters.Player;
using Models.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InputControllers
{
    public class RightClickInputController : MonoBehaviour
    {
        public RectTransform Panel; // Set in editor
        private bool _activeFrame1;
        private bool _activeFrame2; // Temp hacky solution

        private readonly List<GameObject> _rightClickMenuObjects = new List<GameObject>();

        // Use this for initialization
        private void Start()
        {
            Panel.gameObject.SetActive(false);
            Panel.sizeDelta = new Vector2(175, 40);
        }

        // Update is called once per frame
        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(1))
            {
                _activeFrame1 = !_activeFrame1;
                var pos = UnityEngine.Input.mousePosition;
                Panel.transform.position = pos;

                var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);

                if (hits.Length == 0)
                {
                    HideAndClear();
                    return;
                }

                var hight = hits.Length * 35;
                Panel.sizeDelta = new Vector2(175, hight);

                var verticalPositionOffSet = (hight - 35)/2;
                
                foreach (var hit in hits)
                {
                    var colliderGameObject = hit.collider.gameObject;

                    if (colliderGameObject.GetComponent<FloorComponent>() != null)
                    {
                        RightClickTileButtonBuilder(colliderGameObject.GetComponent<FloorComponent>(), verticalPositionOffSet, pos);
                    }
                    else if (colliderGameObject.GetComponent<DoorComponent>() != null)
                    {
                        RightClickDoorButtonBuilder(colliderGameObject.GetComponent<DoorComponent>(), verticalPositionOffSet);  // TODO: Opening the doorComponent from a distance should set a path to the doorComponent, and then open it. That means that paths should allow other actions as well, or some other way of queueing actions.
                    }
                    // Expand here


                    verticalPositionOffSet -= 35;
                }

                //var r = WindowPanel.GetComponent<RectTransform>();

                // WindowPanel.sizeDelta = new Vector2(175, verticalPositionOffSet + 5);

                Panel.gameObject.SetActive(true);
            }
            else if (_activeFrame1 && !_activeFrame2)
            {
                _activeFrame2 = !_activeFrame2;
            }
            else if (_activeFrame1 && _activeFrame2 && !EventSystem.current.IsPointerOverGameObject())
            {
                HideAndClear();
            }
        }

        //private void RightClickFocusButtonBuilder(GameObject collidedGameObject, int verticalPositionOffset)
        //{
        //    var enemy = collidedGameObject.GetComponent<NpcComponent>().Npc;
        //    var Go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
        //    _rightClickMenuObjects.Add(Go);
        //    Go.transform.SetParent(WindowPanel.transform, false);
        //    Go.transform.localScale = new Vector3(1, 1, 1);
        //    var button = Go.GetComponent<Button>();
        //    button.onClick.AddListener(() =>
        //    {
        //        CombatController.Instance.ActionPlayerFocus(PlayerController.PlayableCharacterInstance, enemy);

        //    });
        //    button.GetComponentInChildren<Text>().text = "Focus on " + enemy.DisplayName;

        //    var rt = Go.GetComponent<RectTransform>();
        //    rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffset);
        //}

        private void RightClickTileButtonBuilder(FloorComponent floorComponent, int verticalPositionOffSet, Vector3 pos)
        {
            if (floorComponent.CanEnter == false) return;
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            _rightClickMenuObjects.Add(go);
            go.transform.SetParent(Panel.transform, false);
            go.transform.localScale = new Vector3(1, 1, 1);
            var b = go.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                FindObjectOfType<PlayerController>().SetSelectedPlayerCharacterPath(pos);
                HideAndClear();
            });
            b.GetComponentInChildren<Text>().text = "Walk here";

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffSet);
        }

        private void RightClickDoorButtonBuilder(DoorComponent doorComponent, int verticalPositionOffSet)
        {
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            _rightClickMenuObjects.Add(go);
            go.transform.SetParent(Panel.transform, false);
            go.transform.localScale = new Vector3(1,1,1);
            var b = go.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                doorComponent.ToggleDoor();
                HideAndClear();
            });
            b.GetComponentInChildren<Text>().text = doorComponent.IsOpen ? "Close DoorMapModel" : "Open DoorMapModel";

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffSet);
        }

        //private void RightClickPlayerButtonBuilder(GameObject collidedGameObject, int verticalPositionOffSet)
        //{
        //    //var player = collidedGameObject.GetComponent<PlayerComponent>().PlayableCharacter;
        //    var Go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
        //    _rightClickMenuObjects.Add(Go);
        //    Go.transform.SetParent(WindowPanel.transform, false);
        //    Go.transform.localScale = new Vector3(1, 1, 1);
        //    var b = Go.GetComponent<Button>();
        //    b.onClick.AddListener(() =>
        //    {
        //        Menu.Instance.OpenWindow();
        //        HideAndClear();
        //    });
        //    b.GetComponentInChildren<Text>().text = "Open Menu";

        //    var rt = Go.GetComponent<RectTransform>();
        //    rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffSet);
        //}

        //private void RightClickNpcButtonBuilder(GameObject collidedGameObject, int verticalPositionOffSet)
        //{
        //    var enemy = collidedGameObject.GetComponent<NpcComponent>().Npc;
        //    var Go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
        //    _rightClickMenuObjects.Add(Go);
        //    Go.transform.SetParent(WindowPanel.transform, false);
        //    Go.transform.localScale = new Vector3(1, 1, 1);
        //    var b = Go.GetComponent<Button>();
        //    b.onClick.AddListener(() =>
        //    {
        //        PlayerController.ControllerInstance.SetTarget(enemy);
        //        CombatController.Instance.StartCombat();
        //        HideAndClear();
        //    });
        //    b.GetComponentInChildren<Text>().text = "Set Target";

        //    var rt = Go.GetComponent<RectTransform>();
        //    rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffSet);
        //}

        private void HideAndClear()
        {
            _activeFrame1 = !_activeFrame1;
            _activeFrame2 = !_activeFrame2;
            Panel.gameObject.SetActive(false);
            foreach (var o in _rightClickMenuObjects)
            {
                Destroy(o);
            }
        }
    }
}
