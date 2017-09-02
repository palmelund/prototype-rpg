using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using Models.DataModels;
using UnityEngine;

public class GameRegistry : MonoBehaviour
{
    public static readonly Dictionary<string, Sprite> SpriteRegistry = new Dictionary<string, Sprite>();
    public static readonly Dictionary<string, FloorDataModel> FloorDataModelRegistry = new Dictionary<string, FloorDataModel>();
    public static readonly Dictionary<string, WallDataModel> WallDataModelRegistry = new Dictionary<string, WallDataModel>();
    public static readonly Dictionary<string, DoorDataModel> DoorDataModelRegistry = new Dictionary<string, DoorDataModel>();

    // Use this for initialization
    private void Start()
    {
        LoadAllSprites();
        LoadAllFloorModels();
        LoadAllWallModels();
        LoadAllDoorModels();
    }

    private void LoadAllSprites()
    {
        var directories = Directory.GetDirectories("Data/Resources/Sprites");
        var tmp = directories.ToList();
        tmp.Add("Data/Resources/Sprites");
        directories = tmp.ToArray();

        foreach (var directory in directories)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                var fileData = File.ReadAllBytes(file);
                var spriteTexture2D = new Texture2D(2, 2);
                spriteTexture2D.LoadImage(fileData);
                Sprite sprite = Sprite.Create(spriteTexture2D, new Rect(0, 0, spriteTexture2D.width, spriteTexture2D.height), new Vector2(0.5f, 0.5f), 32);
                sprite.name = Path.GetFileNameWithoutExtension(Path.GetFileName(file));
                
                sprite.texture.filterMode = FilterMode.Point;
                
                SpriteRegistry.Add(sprite.name, sprite);
            }
        }
    }

    private void LoadAllFloorModels()
    {
        var floorModelFiles = Directory.GetFiles("Data/Models/Floor");

        foreach (var file in floorModelFiles)
        {
            var floorModel = DataModel.DeserializeFromFile<FloorDataModel>(file);
            FloorDataModelRegistry.Add(floorModel.Identifier, floorModel);
        }
    }

    private void LoadAllWallModels()
    {
        var wallModelFiles = Directory.GetFiles("Data/Models/Walls");

        foreach (var file in wallModelFiles)
        {
            var wallModel = DataModel.DeserializeFromFile<WallDataModel>(file);
            WallDataModelRegistry.Add(wallModel.Identifier, wallModel);
        }
    }

    private void LoadAllDoorModels()
    {
        var doorModelFiles = Directory.GetFiles("Data/Models/Doors");

        foreach (var file in doorModelFiles)
        {
            var doorModel = DataModel.DeserializeFromFile<DoorDataModel>(file);
            DoorDataModelRegistry.Add(doorModel.Identifier, doorModel);
        }
    }
}
