using System.Collections.Generic;
using Models.Components;
using UnityEngine;

namespace Models
{
    public class DoorModel : BaseModel
    {
        public Sprite DoorSprite;
        public Sprite FrameSprite;
        public float TurnPoint = -0.45f;
        public override GameObject Instantiate(Vector3 position)
        {
            return Instantiate(position, Vector3.zero);
        }

        public override GameObject Instantiate(Vector3 position, Vector3 rotation)
        {
            var go = new GameObject(IdName);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(rotation);

            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = FrameSprite;
            spriteRenderer.sortingLayerName = "BackgroundConstruction";


            var movingPart = new GameObject();
            movingPart.transform.SetParent(go.transform);
            movingPart.transform.position = position;
            movingPart.transform.rotation = Quaternion.Euler(rotation);

            go.AddComponent<BoxCollider2D>();

            var doorSpriteRenderer = movingPart.AddComponent<SpriteRenderer>();
            doorSpriteRenderer.sprite = DoorSprite;
            doorSpriteRenderer.sortingLayerName = "Door";

            var door = go.AddComponent<DoorComponent>();

            var remaining = position.x - (int)position.x;

            if (remaining == 0f)
            {
                door.Configure(this, new Vector3(TurnPoint, 0) + position, movingPart);
            }
            else if (remaining == 0.5f)
            {
                door.Configure(this, new Vector3(0, TurnPoint) + position, movingPart);
            }
            else
            {
                Debug.LogError("Rounding error!");
            }

            return go;
        }
    }
}