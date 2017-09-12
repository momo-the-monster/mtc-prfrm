using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class LoadBGsToQuantizer : MonoBehaviour {

    public Renderer bgRenderer;
    public Texture2D loadedTexture;
    public bool trigger = false;
    public Quantizer quantizer;
    internal List<string> imagePaths;
    internal int currentImage = 0;
    public MMM.FillScreen bgScaler;
    public KeyCode triggerKey = KeyCode.None;

    void Start () {
        imagePaths = GetImageList();
        TriggerRandom();
    }
	
	void Update () {
        if (trigger)
        {
            trigger = false;
            Trigger();
        }
        if (Input.GetKeyDown(triggerKey))
        {
            Trigger();
        }
	}

    public void Trigger()
    {
        currentImage = (currentImage + 1) % imagePaths.Count;
        StartCoroutine( SetTexture());
    }

    public void TriggerRandom()
    {
        currentImage = Mathf.FloorToInt(UnityEngine.Random.Range(0, imagePaths.Count));
        StartCoroutine(SetTexture());
    }

    void OnImagePicked(Texture2D texture)
    {
        loadedTexture = texture;
        loadedTexture.Apply();
        quantizer.inputTexture = loadedTexture;
        quantizer.trigger = true;
        if (bgRenderer != null)
        {
            bgRenderer.sharedMaterial.mainTexture = loadedTexture;
        }
    }

    IEnumerator SetTexture()
    {
        string imagePath = imagePaths[currentImage];
        if(loadedTexture != null)
        {
            loadedTexture = null;
            Resources.UnloadUnusedAssets();
        }
        loadedTexture = new Texture2D(2, 2);
        byte[] bytes;
#if UNITY_ANDROID && !UNITY_EDITOR
         var www = new WWW(imagePath);
         yield return www;
         if (!string.IsNullOrEmpty(www.error))
         {
             Debug.LogError ("Can't read");
         } else {
            Debug.LogFormat("Got the file at {0}", imagePath);
        }
        bytes = www.bytes;
#else
        bytes = File.ReadAllBytes(imagePath);
#endif
        bool success = loadedTexture.LoadImage(bytes);
        loadedTexture.wrapMode = TextureWrapMode.Mirror;

        // Update BG scaler if it exists
        if (success && bgScaler != null)
            bgScaler.AspectRatio = (float)loadedTexture.width / (float)loadedTexture.height;

        quantizer.inputTexture = loadedTexture;
        quantizer.trigger = true;
        if(bgRenderer != null)
        {
            bgRenderer.sharedMaterial.mainTexture = loadedTexture;
        }
        yield return new WaitForEndOfFrame();
    }

    public List<string> GetImageListAndroid()
    {
        List<string> result = new List<string>();
        for (int i = 1; i < 15; i++)
        {
            result.Add("jar:file://" + Application.dataPath + "!/assets/BG/" + i.ToString() + ".jpg");
        }
        return result;
    }

    public List<string> GetImageList()
    {
        string directory = Application.streamingAssetsPath + "/BG";

        // Resolve Relative Paths
        if (!(Path.IsPathRooted(directory)))
            directory = Application.dataPath + directory;
        directory = System.IO.Path.GetFullPath(directory);
        var files = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly)
             .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png")).OrderBy(s => s);
        return files.ToList<string>();
    }
}
