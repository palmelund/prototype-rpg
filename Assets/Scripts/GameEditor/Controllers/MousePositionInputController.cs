using Global;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameEditor.Controllers
{
    public class MousePositionInputController : MonoBehaviour {
        private GameObject _tileMarker;

        private void Start()
        {
            _tileMarker = new GameObject("TileMarker");
            var spriteRenderer = _tileMarker.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameRegistry.SpriteRegistry["mark_tile"];
            spriteRenderer.sortingLayerName = "Marker";
        }

        private void Update ()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _tileMarker.SetActive(true);
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _tileMarker.transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
            }
            else
            {
                _tileMarker.SetActive(false);
            }
        }
    }
}