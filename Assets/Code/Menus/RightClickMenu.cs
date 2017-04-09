using System;
using System.Collections.Generic;
using Assets.Code.Characters;
using Assets.Code.Characters.Npc;
using Assets.Code.Characters.Player;
using Assets.Code.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Code.Menus
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

                int rpos = 0;

                foreach (var hit in hits)
                {
                    var hgo = hit.collider.gameObject;
                    var hco = hgo.GetComponent<CustomComponentType>();
                    if (hco == null) continue;
                    switch (hco.Type)
                    {
                        case ComponentType.Tile:
                            {
                                RightClickTileButtonBuilder(hgo, rpos, pos);
                                break;
                            }
                        case ComponentType.Player:
                            {
                                RightClickPlayerButtonBuilder(hgo, rpos);
                                break;
                            }
                        case ComponentType.Npc:
                            {
                                RightClickNpcButtonBuilder(hgo, rpos);
                                break;
                            }

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    rpos += 35;
                }

                var r = Panel.GetComponent<RectTransform>();

                Panel.sizeDelta = new Vector2(175, rpos + 5);

                Panel.gameObject.SetActive(true);
            }
            else if (_active && !EventSystem.current.IsPointerOverGameObject())
            {
                HideAndClear();
            }
        }

        private void RightClickTileButtonBuilder(GameObject hgo, int rpos, Vector3 pos)
        {
            var tile = hgo.GetComponent<TileComponent>().Tile;
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
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - rpos);
        }

        private void RightClickPlayerButtonBuilder(GameObject hgo, int rpos)
        {
            var player = hgo.GetComponent<PlayerComponent>().Player;
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            _obj.Add(go);
            go.transform.SetParent(Panel.transform, false);
            go.transform.localScale = new Vector3(1, 1, 1);
            var b = go.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                Menu.Instance.OpenWindow();
                HideAndClear();
            });
            b.GetComponentInChildren<Text>().text = "Open Menu";

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - rpos);
        }

        private void RightClickNpcButtonBuilder(GameObject hgo, int rpos)
        {
            var enemy = hgo.GetComponent<NpcComponent>().Npc;
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            _obj.Add(go);
            go.transform.SetParent(Panel.transform, false);
            go.transform.localScale = new Vector3(1, 1, 1);
            var b = go.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                PlayerController.ControllerInstance.SetTarget(enemy);
                HideAndClear();
            });
            b.GetComponentInChildren<Text>().text = "Set Target";

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - rpos);
        }

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
