using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.DataModels;
using UnityEngine;

public class GameRegistry : MonoBehaviour
{
    // TODO: Maybe not static?

    public static readonly Dictionary<string, Sprite> SpriteRegistry = new Dictionary<string, Sprite>();
    public static readonly Dictionary<string, string> MapRegistry = new Dictionary<string, string>();
    public static readonly Dictionary<string, DataModel> ModelRegistry = new Dictionary<string, DataModel>();

    // Use this for initialization
    private void Start()
    {
        LoadAllSprites();
        LoadAllMaps();

        LoadAllModels<FloorDataModel>("Floor");
        LoadAllModels<WallDataModel>("Walls");
        LoadAllModels<DoorDataModel>("Doors");
    }

    public static List<T> GetModelsListOfType<T>() where T : DataModel
    {
        return ModelRegistry.OfType<T>().ToList();
    }

    public static Dictionary<string, T> GetModelsDictionaryOfType<T>() where T : DataModel
    {
        return ModelRegistry.OfType<T>().ToDictionary(model => model.Identifier);
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
            Debug.Log(file);
            MapRegistry.Add(mapIdentifier, file);
        }
    }

    private void LoadAllModels<T>(string folder) where T : DataModel
    {
        var pointModelFiles = Directory.GetFiles("Data/Models/" + folder);

        foreach (var file in pointModelFiles)
        {
            var pointModel = DataModel.DeserializeFromFile<T>(file);
            ModelRegistry.Add(pointModel.Identifier, pointModel);
        }
    }
}