using Characters.Player;
using UnityEngine;
using World;

namespace Models.MapModels
{
    // This model is used for debugging only. Another set of models will be used later

    public class DebugActorSpawner : IMapModel
    {
        public string Identifier { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public DebugActorSpawner()
        {
            var character = new PlayableCharacter();
            Object.FindObjectOfType<PlayerController>().SelectedPlayerCharacter = character;

            character.GameObject = new GameObject("player_go");
            var spriteRenderer = character.GameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameRegistry.SpriteRegistry["player"];
            spriteRenderer.sortingLayerName = "Characters";
            character.GameObject.transform.position = new Vector3(0, 0);

            character.NextVertice = Object.FindObjectOfType<WorldComponent>().GetTileAt(character.GameObject.transform.position)?.Vertice;

            character.GameObject.AddComponent<BoxCollider2D>();
        }

        public DebugActorSpawner(string identifier, float x, float y) : this()
        {
            Identifier = identifier;
            X = x;
            Y = y;
        }
    }
}
