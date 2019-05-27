using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;


public class ScreenshotScript : MonoBehaviour
{

    private Camera myCamera;
    private static ScreenshotScript instance;
    private bool takeScreenshotOnNextFrame;
    Material blueMat;
    Material bluePrevMat;
    Material redMat;
    Material redPrevMat;
    string scene_name;

    int state_machine = 0;
    int img_counter = 0;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();

        blueMat = Resources.Load("BlueMask", typeof(Material)) as Material;
        redMat = Resources.Load("RedMask", typeof(Material)) as Material;

        state_machine = 0;

        scene_name = SceneManager.GetActiveScene().name;
        if (!Directory.Exists(scene_name))
        {
            Directory.CreateDirectory(scene_name);
        }
    }

    IEnumerator captureScreenshot(string imagePath)
    {
        yield return new WaitForEndOfFrame();
        //about to save an image capture
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);


        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        Debug.Log(" screenImage.width" + screenImage.width + " texelSize" + screenImage.texelSize);
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        Debug.Log("imagesBytes=" + imageBytes.Length);

        //Save image to file
        System.IO.File.WriteAllBytes(imagePath, imageBytes);
    }

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;
            Debug.Log(renderTexture.width.ToString() + " " + renderTexture.height.ToString());
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            renderResult.Apply();
            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
            Debug.Log(Application.dataPath + "Saved CameraScreenshot.png");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }
    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }
    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (state_machine == 0)
            {
                Time.timeScale = 0;
                Debug.Log(Application.dataPath + "Saved CameraScreenshot.png");
                ScreenCapture.CaptureScreenshot(scene_name + "/textured_smush" + img_counter.ToString() + ".png");
                //StartCoroutine(captureScreenshot("masked_scene.png"));
                state_machine = 1;
            }
            else if (state_machine == 2)
            {
                // restore old materials and restart simulation
                GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (GameObject go in allObjects)
                {
                    if (go.activeInHierarchy && go.name == "default" && go.transform.parent.name == "pillow_5")
                    {
                        //MeshFilter viewedModelFilter = (MeshFilter)go.GetComponent("default");
                        //Mesh viewedModel = viewedModelFilter.mesh
                        go.GetComponent<Renderer>().material = bluePrevMat;
                        //go.GetComponent<MaterialEditor2>().UpdateMaterial(blueMat);
                        //gameObject.BroadcastMessage("UpdateMaterial", blueMat);
                    }

                    if (go.activeInHierarchy && go.name == "default" && go.transform.parent.name == "pillow_5 (1)")
                    {
                        go.GetComponent<Renderer>().material = redPrevMat;
                    }
                }
                Time.timeScale = 1;

                state_machine = 0;
            }
        }

        else if (state_machine == 1)
        {
            // set the damn pillows to be masked materials
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            int active_total = 0;
            foreach (GameObject go in allObjects)
            {
                if (go.activeInHierarchy && go.name == "default" && go.transform.parent.name == "pillow_5")
                {
                    //MeshFilter viewedModelFilter = (MeshFilter)go.GetComponent("default");
                    //Mesh viewedModel = viewedModelFilter.mesh
                    bluePrevMat = go.GetComponent<Renderer>().material;
                    go.GetComponent<Renderer>().material = blueMat;
                    //go.GetComponent<MaterialEditor2>().UpdateMaterial(blueMat);
                    //gameObject.BroadcastMessage("UpdateMaterial", blueMat);
                }

                if (go.activeInHierarchy && go.name == "default" && go.transform.parent.name == "pillow_5 (1)")
                {
                    redPrevMat = go.GetComponent<Renderer>().material;
                    go.GetComponent<Renderer>().material = redMat;
                }
            }
            ScreenCapture.CaptureScreenshot(scene_name + "/masked_smush" + img_counter.ToString() + ".png");
            img_counter += 1;
            state_machine = 2;
        }
    }
}