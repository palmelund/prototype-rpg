using System;
using System.Collections.Generic;
using Assets.Code.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Code
{
    public class TestMenuHandler : MonoBehaviour
    {
        public GameObject CanvasGo; // Set in editor
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

                foreach (var hit in hits)
                {
                    var hgo = hit.collider.gameObject;
                    var hco = hgo.GetComponent<CustomComponentType>();
                    if (hco == null) continue;
                    switch (hco.Type)
                    {
                        case ComponentType.Tile:
                            var tile = hgo.GetComponent<TileComponent>().Tile;
                            if (tile.CanEnter == false) continue;
                            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
                            _obj.Add(go);
                            go.transform.SetParent(Panel.transform, false);
                            go.transform.localScale = new Vector3(1, 1, 1);
                            var b = go.GetComponent<Button>();
                            b.onClick.AddListener(() =>
                            {
                                Debug.Log("Walk");

                                Player.Instance.ActionPathFinder( pos);
                                HideAndClear();
                            });
                            var t = b.GetComponentInChildren<Text>();
                            t.text = "Walk here";
                            break;
                        case ComponentType.Player:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                Panel.gameObject.SetActive(true);
            }
            else if (_active && !EventSystem.current.IsPointerOverGameObject())
            {
                HideAndClear();
            }
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
