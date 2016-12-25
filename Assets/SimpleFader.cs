using UnityEngine;
using System.Collections;

public class SimpleFader : MonoBehaviour {

    // based on http://answers.unity3d.com/questions/341350/how-to-fade-out-a-scene.html

    private Texture2D black;
    public float fadeSpeed = 0.3f;
    public int drawDepth = -1000;

    private float alpha = 1f;
    private float fadeDir = -1;
    private Rect rect;

    public static SimpleFader instance;

    // todo optimize
    
    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        Color curColor = GUI.color;
        curColor.a = alpha;
        GUI.color = curColor;
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
    }

	// Use this for initialization
	void Start () {
        black = new Texture2D(1, 1);
        black.SetPixel(0, 0, Color.black);
        black.Apply();
	}

    public static void BeginFadeOut()
    {
        if (instance == null)
        {      
            instance = GameObject.FindObjectOfType<SimpleFader>(); 
        }
        if (instance != null)
        {
            instance.FadeOut();
        }
       
    }

    public void FadeOut()
    {
        fadeDir = 1;
    }

    public void FadeIn()
    {
        fadeDir = -1;
    }

    public static void BeginFadeIn()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<SimpleFader>();
        }
        if (instance != null)
        {
            instance.FadeIn();
        }
    }
}
