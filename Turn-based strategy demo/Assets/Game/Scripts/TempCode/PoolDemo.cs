using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolDemo : MonoBehaviour
{
    private const string PoolKey = "Demo.Prefab";
    [SerializeField] private GameObject prefab;
    private readonly List<Poolable> instances = new();

    private void Start()
    {
        Debug.Log(GameObjectPoolController.AddEntry(PoolKey, prefab, 10, 15)
            ? "Pre-populating pool" : "Pool already configured");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Scene 1"))
        {
            ChangeLevel(0);
        }

        if (GUI.Button(new Rect(10, 50, 100, 30), "Scene 2"))
        {
            ChangeLevel(1);
        }

        if (GUI.Button(new Rect(10, 90, 100, 30), "Dequeue"))
        {
            Poolable obj = GameObjectPoolController.Dequeue(PoolKey);
            float x = Random.Range(-10, 10);
            float y = Random.Range(0, 5);
            float z = Random.Range(0, 10);
            obj.transform.position = new Vector3(x, y, z);
            obj.gameObject.SetActive(true);
            instances.Add(obj);
        }

        if (GUI.Button(new Rect(10, 130, 100, 30), "Enqueue"))
        {
            if (instances.Count <= 0) 
                return;
            Poolable obj = instances[0];
            instances.RemoveAt(0);
            GameObjectPoolController.Enqueue(obj);
        }
    }

    private void ChangeLevel(int level)
    {
        ReleaseInstance();
        SceneManager.LoadScene(level);
    }

    private void ReleaseInstance()
    {
        for (int i = instances.Count - 1; i >= 0; --i)
        {
            GameObjectPoolController.Enqueue(instances[i]);
        }
        instances.Clear();
    }
}
