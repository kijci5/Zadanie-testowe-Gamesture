using UnityEngine;
using System.IO;

public class LoadFromDirectory : MonoBehaviour
{
    [SerializeField] GameObject elementPrefab;
    [SerializeField] Transform content;

    private FileInfo[] images;

    private void Awake()
    {
        DestroyOldList();
        LoadImages();
        CreateNewList();
    }

    public void Refresh()
    {
        Awake();
    }

    private void CreateNewList()
    {
        foreach (FileInfo f in images)
        {
            byte[] bytes;
            bytes = File.ReadAllBytes(f.FullName);
            Texture2D textureToLoad = new Texture2D(100, 100, TextureFormat.RGB24, false);
            textureToLoad.LoadImage(bytes);
            Sprite sprite = Sprite.Create(textureToLoad, new Rect(0, 0, textureToLoad.width, textureToLoad.height), new Vector2(0.5f, 0.5f));

            GameObject newElement = Instantiate(elementPrefab);
            newElement.transform.parent = content;
            newElement.GetComponent<ListElement>().image.sprite = sprite;
            newElement.GetComponent<ListElement>().fileName.text = "Name: " + f.Name;

            var timeCreated = System.DateTime.Now - f.CreationTime;
            var formatTimeCreated = string.Format("{0:%d}d {0:%h}h {0:%m}min {0:%s}sec", timeCreated);
            newElement.GetComponent<ListElement>().timeSinceCreated.text = "Time since created: " + formatTimeCreated;
        }
    }

    private void LoadImages()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Images");
        images = dir.GetFiles("*.png");
    }

    private void DestroyOldList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
