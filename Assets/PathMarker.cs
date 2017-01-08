using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public static class PathMarker
    {
        private static readonly Queue<GameObject> UsedMarkers = new Queue<GameObject>();
        private static readonly Queue<GameObject> FreeMarkers = new Queue<GameObject>();

        public static void CreatePath(Stack<PathMember> path)
        {
            var l = path.ToList();
            foreach (var pathMember in l)
            {
                var pos = pathMember.Destination.GameObject.transform.position;
                GetMarker(pos.x, pos.y);
            }
        }

        private static GameObject BuildMarker()
        {
            var go = new GameObject();
            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>("dot");
            renderer.sortingLayerName = "Marker";
            return go;
        }

        private static GameObject GetMarker(float x, float y)
        {
            var marker = FreeMarkers.Count > 0 ? FreeMarkers.Dequeue() : BuildMarker();

            UsedMarkers.Enqueue(marker);
            marker.transform.position = new Vector3(x,y);
            marker.GetComponent<Renderer>().enabled = true;
            return marker;
        }

        public static void ClearPath()
        {
            while (UsedMarkers.Count > 0)
            {
                ClearNext();
            }
        }

        public static void ClearNext()
        {
            var marker = UsedMarkers.Dequeue();
            marker.GetComponent<Renderer>().enabled = false;
            FreeMarkers.Enqueue(marker);
        }
    }
}
