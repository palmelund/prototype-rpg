using System;
using System.Collections.Generic;
using Code.Characters.Player;
using Code.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Menus
{
    public class RightClickMenu : MonoBehaviour
    {
        public RectTransform Panel; // Set in editor
        private bool _active;

        private readonly List<GameObject> _obj = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            Panel.gameObject.SetActive(false);
            Panel.sizeDelta = new Vector2(175, 40);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _active = !_active;
                var pos = Input.mousePosition;
                Panel.transform.position = pos;

                var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                Debug.Log(hits.Length);

                if (hits.Length == 0)
                {
                    HideAndClear();
                    return;
                }

                var hight = hits.Length * 35;
                Panel.sizeDelta = new Vector2(175, hight);

                var verticalPositionOffSet = (hight - 35)/2;

                var log = $"Hits: {hits.Length}";

                foreach (var raycastHit2D in hits)
                {
                    log += $"\n{raycastHit2D.transform.gameObject.name}";
                }

                Debug.Log(log);

                foreach (var hit in hits)
                {
                    var colliderGameObject = hit.collider.gameObject;

                    if (colliderGameObject.GetComponent<Tile>() != null)
                    {
                        RightClickTileButtonBuilder(colliderGameObject.GetComponent<Tile>(), verticalPositionOffSet, pos);
                    }
                    else if (colliderGameObject.GetComponent<DoorComponent>() != null)
                    {
                        RightClickDoorButtonBuilder(colliderGameObject.GetComponent<DoorComponent>(), verticalPositionOffSet);
                    }
                    // Expand here


                    verticalPositionOffSet -= 35;
                }

                //var r = Panel.GetComponent<RectTransform>();

                // Panel.sizeDelta = new Vector2(175, verticalPositionOffSet + 5);

                Panel.gameObject.SetActive(true);
            }
            else if (_active && !EventSystem.current.IsPointerOverGameObject())
            {
                HideAndClear();
            }
        }

        //private void RightClickFocusButtonBuilder(GameObject collidedGameObject, int verticalPositionOffset)
        //{
        //    var enemy = collidedGameObject.GetComponent<NpcComponent>().Npc;
        //    var Go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
        //    _obj.Add(Go);
        //    Go.transform.SetParent(Panel.transform, false);
        //    Go.transform.localScale = new Vector3(1, 1, 1);
        //    var button = Go.GetComponent<Button>();
        //    button.onClick.AddListener(() =>
        //    {
        //        CombatController.Instance.ActionPlayerFocus(PlayerController.PlayableCharacterInstance, enemy);

        //    });
        //    button.GetComponentInChildren<Text>().text = "Focus on " + enemy.Name;

        //    var rt = Go.GetComponent<RectTransform>();
        //    rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffset);
        //}

        private void RightClickTileButtonBuilder(Tile tile, int verticalPositionOffSet, Vector3 pos)
        {
            if (tile.CanEnter == false) return;
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            _obj.Add(go);
            go.transform.SetParent(Panel.transform, false);
            go.transform.localScale = new Vector3(1, 1, 1);
            var b = go.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                PlayerController.ControllerInstance.Move(pos);
                HideAndClear();
            });
            b.GetComponentInChildren<Text>().text = "Walk here";

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffSet);
        }

        private void RightClickDoorButtonBuilder(DoorComponent door, int verticalPositionOffSet)
        {
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            _obj.Add(go);
            go.transform.SetParent(Panel.transform, false);
            go.transform.localScale = new Vector3(1,1,1);
            var b = go.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                door.ToggleDoor();
                HideAndClear();
            });
            b.GetComponentInChildren<Text>().text = door.IsOpen ? "Close DoorComponent" : "Open DoorComponent";

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffSet);
        }

        //private void RightClickPlayerButtonBuilder(GameObject collidedGameObject, int verticalPositionOffSet)
        //{
        //    //var player = collidedGameObject.GetComponent<PlayerComponent>().PlayableCharacter;
        //    var Go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
        //    _obj.Add(Go);
        //    Go.transform.SetParent(Panel.transform, false);
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
        //    _obj.Add(Go);
        //    Go.transform.SetParent(Panel.transform, false);
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
            _active = !_active;
            Panel.gameObject.SetActive(false);
            foreach (var o in _obj)
            {
                Destroy(o);
            }
        }
    }
}
