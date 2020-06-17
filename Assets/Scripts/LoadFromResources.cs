using UnityEngine;
using System.IO;
using UnityEditor;

public class LoadFromResources : MonoBehaviour
{
    [SerializeField] private GameObject elementPrefab;
    [SerializeField] private Transform content;

    private Object[] images;

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
        foreach (var image in images)
        {
            GameObject newElement = Instantiate(elementPrefab);
            newElement.transform.parent = content;
            newElement.GetComponent<ListElement>().image.sprite = (Sprite)image;
            newElement.GetComponent<ListElement>().fileName.text = "Name: " + image.name;

            var timeCreated = System.DateTime.Now - File.GetCreationTime(AssetDatabase.GetAssetPath(image));
            var formatTimeCreated = string.Format("{0:%d}d {0:%h}h {0:%m}min {0:%s}sec", timeCreated);
            newElement.GetComponent<ListElement>().timeSinceCreated.text = "Time since created: " + formatTimeCreated;
        }
    }

    private void LoadImages()
    {
        images = Resources.LoadAll("Images", typeof(Sprite));
    }

    private void DestroyOldList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
