using System.Collections.Generic;
using System.IO;
using Items;
using Models.DataModels;
using UnityEngine;

namespace Global
{
    public class GameRegistry : MonoBehaviour
    {
        // TODO: Maybe not static?

        public static readonly Dictionary<string, Sprite> SpriteRegistry = new Dictionary<string, Sprite>();
        public static readonly Dictionary<string, string> MapRegistry = new Dictionary<string, string>();
        public static readonly Dictionary<string, DataModel> ModelRegistry = new Dictionary<string, DataModel>();
        public static readonly Dictionary<string, Item> ItemRegistry = new Dictionary<string, Item>();
        
        // Use this for initialization
        private void Start()
        {
            Serializer.BuildGameSerializableObjectList();
            
            LoadAllSprites();
            LoadAllMaps();

            LoadAllModels<FloorDataModel>("Floor");
            LoadAllModels<WallDataModel>("Walls");
            LoadAllModels<DoorDataModel>("Doors");
            // TODO: Load all items

            CreateTmpItems();
        }

        private void LoadAllSprites()
        {
            foreach (var file in Directory.GetFiles("Data/Resources/Sprites"))
            {
                var fileData = File.ReadAllBytes(file);
                var spriteTexture2D = new Texture2D(2, 2);
                spriteTexture2D.LoadImage(fileData);
                var sprite = Sprite.Create(spriteTexture2D,
                    new Rect(0, 0, spriteTexture2D.width, spriteTexture2D.height), new Vector2(0.5f, 0.5f), 32);
                sprite.name = Path.GetFileNameWithoutExtension(Path.GetFileName(file));

                sprite.texture.filterMode = FilterMode.Point;

                SpriteRegistry.Add(sprite.name, sprite);
            }
        }

        private void LoadAllMaps()
        {
            foreach (var file in Directory.GetFiles("Data/Maps"))
            {
                var mapIdentifier = Path.GetFileNameWithoutExtension(file);
                MapRegistry.Add(mapIdentifier, file);
            }
        }

        private void LoadAllModels<T>(string folder) where T : DataModel
        {
            var pointModelFiles = Directory.GetFiles("Data/Models/" + folder);

            foreach (var file in pointModelFiles)
            {
                var pointModel = Serializer.DeserializeFromFile<T>(file);
                ModelRegistry.Add(pointModel.Identifier, pointModel);
            }
        }

        private void LoadAllItems<T>(string folder) where T : Item
        {
            var pointModelFiles = Directory.GetFiles("Data/Items/" + folder);

            foreach (var file in pointModelFiles)
            {
                var pointModel = Serializer.DeserializeFromFile<T>(file);
                ItemRegistry.Add(pointModel.Identifier, pointModel);
            }
        }

        private void CreateTmpItems()
        {
            var headArmor = new HeadArmor();
            headArmor.Identifier = "item_armor_head";
            headArmor.ItemEquipmentType = ItemEquipmentType.Head;
            GameRegistry.ItemRegistry.Add(headArmor.Identifier, headArmor);

            var bodyArmor = new BodyArmor();
            bodyArmor.Identifier = "item_armor_body";
            bodyArmor.ItemEquipmentType = ItemEquipmentType.Body;
            GameRegistry.ItemRegistry.Add(bodyArmor.Identifier, bodyArmor);

            var legArmor = new LegArmor();
            legArmor.Identifier = "item_armor_leg";
            legArmor.ItemEquipmentType = ItemEquipmentType.Legs;
            GameRegistry.ItemRegistry.Add(legArmor.Identifier, legArmor);
            
            var oneHandItem = new HandItem();
            oneHandItem.Identifier = "item_weapon_any";
            oneHandItem.ItemEquipmentType = ItemEquipmentType.OneHand;
            GameRegistry.ItemRegistry.Add(oneHandItem.Identifier, oneHandItem);

            var dualHandItem = new HandItem();
            dualHandItem.Identifier = "item_weapon_dual";
            dualHandItem.ItemEquipmentType = ItemEquipmentType.TwoHand;
            GameRegistry.ItemRegistry.Add(dualHandItem.Identifier, dualHandItem);

            var junk = new Junk();
            junk.Identifier = "item_junk";
            GameRegistry.ItemRegistry.Add(junk.Identifier, junk);
        }
    }
}