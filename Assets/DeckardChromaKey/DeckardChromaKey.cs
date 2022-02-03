using UnityEngine;
using System.Collections;
using UnityEngine.Video;
//using UnityEngine.PlayerLoop;
using System;


#if UNITY_EDITOR
using UnityEditor;

#endif

[ExecuteInEditMode]
public class DeckardChromaKey : MonoBehaviour
{
    public int deviceNumber = 7;
    public bool webcamOpened = false;
    public bool saveTexture = false;
    public WebCamTexture webcamTexture;
    public RenderTexture inputRenderTexture;
    public string frameNr;
    public Material chromaMat;
    public Material blurringMat;
    public Material refinementMat;
    public RenderTexture rt;
    public RenderTexture rt2;
    public Material[] materials;
    public string[] properties;
    public bool ResetWebcam = false;
    public Color KeyColor;
    public Texture2D backgorundTex;
    public Texture2D customMask;
    public float threshold = -0.28f;
    public float smooth = 0.4f;
    public float border = 0.00048f;
    public float maskPower = 10;
    public bool useMateKey = true;
    public bool roll1 = false;
    public bool roll2 = false;
    public bool roll3 = false;
    public bool roll4 = false;
    public bool roll5 = false;
    public VideoPlayer inputVideo;
    public Texture rtTextureInput;

    public enum SourceType { Webcam, RenderTexture, Video };
    public SourceType sourceType;

    public enum OutputType { DynamicRenderTexture, AssetRenderTexture };
    public OutputType outputType;

    public enum ChromaKeyMethod { GarbageKey, ColorKey};
    public ChromaKeyMethod chromakeyMethod;

    public enum TrackingResolution { _128, _256, _512, _1024 };
    public TrackingResolution trackingResolution;

    public bool useTracking = true;

    public Material trackingMat;

    public RenderTexture trackingRT { get; private set; }
    

    public bool portraitMode = true;

    Color[] trackingDataX;
    Color[] trackingDataY;
    public Vector4 trackResult;
    public int trackerResolutiuon = 128;
    public bool findFace = true;
    public Vector2 headCentroid;

    public float trackingTreshold = 0.01f;
    public float headTrackingTrehold = 0f;
    public float shoulderTrackingTreshold = 0f;
    public bool findShoulders = false;
    public float shouldersTrack;

    Texture2D trackingTexX;
    Texture2D trackingTexY;
    Texture2D faceSearchTex;


    public enum FilterType { ChromaPass, Sharpen, Beauty, CustomFilter };
    public FilterType[] filterType;

    public Material[] filterPasses;
    public int actualPasses;
    public Transform projectionPlane;
    public float cutoff = 0.5f;
    public bool generateNormalMap = true;
    public RenderTexture normalMap;
    public Material normalMaterial;
    public Material depthToNormalMat;
    public int _iteration;
    public int maskSoftnessTaps = 8;
    public float colorInfluence = 0.2f;
    public bool usingHDRP = false;
    public Material cleanupMat;
    public bool useCleanup;
    public float cleanup;
    public float cleanupRange;
    

    public Color _BaseColor;
    public Color _EmissiveColor;
    public float _Smoothness;
    public float _Metallic;
    public float _NormalScale;
    public MeshRenderer mrMask;
    public MeshRenderer mrTransparent;
    public bool UsingDeckardRender = false;
    public bool screenSpaceProjection = false;
    public RenderTexture histogram;
    public bool useHistogram = false;
    public Material histogramMaterial;
    public int minAnimation = 0;
    public int maxAnimation = 100;
    public int animationFrame;
    public int lenght;
    public bool useAutoTune = false;
    public RenderTexture rtAutotune;
    public Texture2D rtTexture2D;
    public Color[] autotuneData;
    public Color autotuneResult;
    public Color oldAutotuneResult;
    public float thresholdAutotune = 0.1f;
    public int oldWebcamIndex;

    void Start()
    {
        if (inputVideo != null && Application.isPlaying == true && sourceType == DeckardChromaKey.SourceType.Video)
        {
            inputVideo.playbackSpeed = 1;
        }

        if (Application.isPlaying)
        {


            mrMask.material = Instantiate(mrMask.material);
            mrTransparent.material = Instantiate(mrTransparent.material);
            materials[0] = mrTransparent.material;
            materials[1] = mrMask.material;





            if (sourceType == DeckardChromaKey.SourceType.Webcam)
            {
                OpenWebcam();
            }

        }
        else
        {
            if (sourceType == DeckardChromaKey.SourceType.Webcam)
            {
                if (webcamOpened) CloseWebcam();
                OpenWebcam();
            }
        }
        if (useAutoTune)
        {
            rtAutotune = new RenderTexture(16, 16, 24);
            rtTexture2D = new Texture2D(16, 16);
        }
        oldWebcamIndex = deviceNumber;
    }



    public void Update()
    {
        if (useHistogram && histogram == null) histogram = new RenderTexture(256, 64, 32, RenderTextureFormat.ARGB32);

        UpdateMaterials();
        if (chromakeyMethod == (DeckardChromaKey.ChromaKeyMethod.ColorKey)) chromaMat.SetFloat("_MatteKey", 0f);
        else chromaMat.SetFloat("_MatteKey", 1f);

        if (UsingDeckardRender)
        {
            mrTransparent.enabled = false;
        }
        else mrTransparent.enabled = true;
        // materials[1].CopyPropertiesFromMaterial(materials[0]);
        //  materials[1].SetFloat("_AlphaCutoffEnable", 1f);

        //   materials[1].SetFloat("_SurfaceType", 0f);


        if (inputVideo != null && Application.isPlaying == false && sourceType == DeckardChromaKey.SourceType.Video)
        {
            inputVideo.playbackSpeed = 0;
            
            SkipToFrame(animationFrame);
            inputVideo.Play();
        }

        if (rtAutotune == null && useAutoTune)
        {
            rtAutotune = new RenderTexture(16, 16, 24);
            rtTexture2D = new Texture2D(16, 16);

        }
        if (useAutoTune)
        {
            Autotune();

        }
        if ((oldWebcamIndex != deviceNumber && webcamOpened))
        {
            CloseWebcam();
            OpenWebcam();
            oldWebcamIndex = deviceNumber;
        }
        if (sourceType != DeckardChromaKey.SourceType.Webcam && webcamOpened)
        {
            CloseWebcam();
        }

    }

    private void SkipToFrame(int frame)
    {

        maxAnimation = (int)inputVideo.frameCount;
        inputVideo.frame = (long)frame;
        // Debug.Log(videoPlayer.frame);
    }

    public void OpenWebcam()
    {




        webcamTexture = new WebCamTexture();
        WebCamDevice[] devices = WebCamTexture.devices;
        // webcamTexture = new WebCamTexture();

        webcamOpened = true;

        for (int i = 0; i < devices.Length; i++)
            Debug.Log(devices[i].name);


        if (devices.Length > 0)
        {
            webcamTexture.deviceName = devices[deviceNumber].name;

            webcamTexture.Play();
            if (outputType != DeckardChromaKey.OutputType.AssetRenderTexture)
            {
                if (rt == null)
                    Debug.Log("Please Assign output render Texture");
            }
            else rt = new RenderTexture(webcamTexture.width, webcamTexture.height, 32, RenderTextureFormat.ARGB32);

            rt2 = new RenderTexture(webcamTexture.width, webcamTexture.height, 32, RenderTextureFormat.ARGB32);

            trackingRT = new RenderTexture(trackerResolutiuon, trackerResolutiuon, 32, RenderTextureFormat.ARGB32);

        }

        UpdateMaterials();
    }

    private void UpdateMaterials()
    {
        foreach (Material m in materials)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                if (i != 2)
                    m.SetTexture(properties[i], rt);
                else m.SetTexture(properties[i], normalMap);
            }

            
            m.SetColor("_BaseColor", chromaMat.GetColor("_BaseColor"));
            m.SetColor("_EmissiveColor", chromaMat.GetColor("_EmissiveColor"));
            m.SetFloat("_Smoothness", chromaMat.GetFloat("_Smoothness"));
            m.SetFloat("_Metallic", chromaMat.GetFloat("_Metallic"));
            m.SetFloat("_NormalScale", chromaMat.GetFloat("_NormalScale"));
            m.SetFloat("_DespillLumaComposite", chromaMat.GetFloat("_DespillLumaComposite"));
            m.SetFloat("_DespillComposite", chromaMat.GetFloat("_DespillComposite"));
            cutoff = chromaMat.GetVector("_SoftTransition").y;
            maskSoftnessTaps = Mathf.FloorToInt(chromaMat.GetFloat("_iterations"));
            colorInfluence = chromaMat.GetFloat("_ColorInfluence");
            m.SetFloat("_ScreenSpace", chromaMat.GetFloat("_ScreenSpaceProjection"));
            if (useHistogram)
            histogramMaterial.SetFloat("_PreCorrectGamma", chromaMat.GetFloat("_PreCorrectGamma"));
            





        }
        cleanupMat.SetFloat("_Cleanup", chromaMat.GetFloat("_Cleanup"));
        cleanupMat.SetFloat("_Range", chromaMat.GetFloat("_Range"));
    }

    public void LateUpdate()
    {

        materials[1].SetFloat("_AlphaCutoff", chromaMat.GetVector("_SoftTransition").y);

        if (saveTexture)
        {
            CaptureImage(webcamTexture);
            saveTexture = false;
        }

        if (backgorundTex != null)
        {
            chromaMat.SetTexture("_garbageMate", backgorundTex);
            refinementMat.SetTexture("_ultimatte", backgorundTex);
        }

        blurringMat.SetVector("_BlurDirection", new Vector2(1f, 0f));
        if (sourceType == DeckardChromaKey.SourceType.Video)
        {
            if (inputVideo != null)
            {
                if (inputVideo.isPlaying && inputVideo.isPrepared)
                {
                    if (rt == null) InitializeBuffers(inputVideo.texture);

                    if (inputVideo.texture.width != rt.width || inputVideo.texture.height != rt.height)
                    {
                        InitializeBuffers(inputVideo.texture);

                    }

                }

                if (useHistogram)
                {
                    
                    Graphics.Blit(inputVideo.texture, histogram, histogramMaterial);
                }
                Graphics.Blit(inputVideo.texture, rt, chromaMat);
            }
            else
            {
                RenderTexture tex = new RenderTexture(16, 16, 32, RenderTextureFormat.ARGB32);
                InitializeBuffers(tex);
            }
            
        }
        else if (sourceType == DeckardChromaKey.SourceType.Webcam)
        {
            if (webcamTexture != null && webcamTexture.isPlaying)
            {
                if (rt != null)
                {
                    if (webcamTexture.width != rt.width || webcamTexture.height != rt.height)
                    {
                        InitializeBuffers(webcamTexture);
                    }
                }
                else InitializeBuffers(webcamTexture);
                if (useHistogram)
                {
                    Graphics.Blit(webcamTexture, histogram, histogramMaterial);
                }
                UnityEngine.RenderTexture.active = rt;
                GL.Clear(true, true, Color.black);
                Graphics.Blit(webcamTexture, rt, chromaMat);
            }
        }

        else
        {
            {
                if (rt != null) { 
                if (rtTextureInput.width != rt.width || rtTextureInput.height != rt.height )
                {
                    InitializeBuffers(rtTextureInput);
                }
                }
                else InitializeBuffers(rtTextureInput);
                if (useHistogram)
                {
                    Graphics.Blit(rtTextureInput, histogram, histogramMaterial);
                }
                Graphics.Blit(rtTextureInput, rt, chromaMat);
            }

        }


        //if (useAutoTune)
        //{

        //    if (sourceType == DeckardChromaKey.SourceType.Webcam)
        //    {
        //        Graphics.Blit(webcamTexture, rtAutotune);
        //    }
                
        //    else if (sourceType == DeckardChromaKey.SourceType.Video)
        //    {
        //        Graphics.Blit(inputVideo.texture, rtAutotune);
        //    }
        //}


        if (useTracking)
        {

            if (trackingRT != null)
            {
                if (trackingRT.width != trackerResolutiuon) trackingRT = null;
            }

            if (trackingRT == null) trackingRT = new RenderTexture(trackerResolutiuon, trackerResolutiuon, 32, RenderTextureFormat.ARGB32);

            //Check if Using Webcam
            if (sourceType == DeckardChromaKey.SourceType.Webcam && webcamTexture.didUpdateThisFrame)
            {
                Graphics.Blit(rt, trackingRT, trackingMat);
                TrackImage();
            }

            else
            {
                Graphics.Blit(rt, trackingRT, trackingMat);
                TrackImage();
            }
        }



        //Add adittional filtering passes
        if (generateNormalMap) GenerateNormalMap();
        int passCount;
        if (useCleanup)
        {
            cleanupMat.SetTexture("_normalMap", normalMap);
            Graphics.Blit(rt, rt2, cleanupMat);
            passCount = 1;
        }
        else passCount = 0;
        //Check for actual active passes (all materials assigned)
        
        for (int i = 0; i < filterPasses.Length; i++)
        {
            if (filterPasses[i] != null)
                passCount++;
        }

        if (passCount -1 > 0 && rt != null)
        {
            for (int i = 0; i < passCount-1; i++)
            {

                if (i % 2 == 0 && filterPasses[i] != null)
                {
                    Graphics.Blit(rt2, rt, filterPasses[i]);
                }
                else if (filterPasses[i] != null)
                {
                    Graphics.Blit(rt, rt2, filterPasses[i]);
                }
            }

            
            
        }

        if (filterPasses.Length % 2 == 0)
        {

            Graphics.Blit(rt2, rt);

        }


    }

    private void Autotune()
    {
        //Texture buffer;
        //if (sourceType == SourceType.Webcam) buffer = webcamTexture;
        //else if (sourceType == SourceType.RenderTexture) buffer = rtTextureInput;
        //else buffer = inputVideo.texture;
        if (sourceType == SourceType.Webcam)
            Graphics.Blit(webcamTexture, rtAutotune);
        else if (sourceType == SourceType.RenderTexture)
            Graphics.Blit(rtTextureInput, rtAutotune);
        else Graphics.Blit(inputVideo.texture, rtAutotune);

        RenderTexture.active = rtAutotune;
        rtTexture2D.ReadPixels(new Rect(0, 0, 16, 16), 0, 0, false);
        RenderTexture.active = null;
        autotuneData = rtTexture2D.GetPixels(0, 0, 16, 16);
        float accumulation = 0f;

        oldAutotuneResult = autotuneResult;
        if (autotuneData[0].g > thresholdAutotune)
        {
            autotuneResult = autotuneData[0];
            accumulation += 1;
            Debug.Log("smaller1");
        }
        if (autotuneData[1].g > thresholdAutotune)
        {
            autotuneResult += autotuneData[1];
            accumulation += 1;
            Debug.Log("smaller2");
        }

        if (autotuneData[2].g > thresholdAutotune)
        {
            autotuneResult += autotuneData[2];
            accumulation += 1;
            Debug.Log("smaller3");
        }

        if (autotuneData[0].g > thresholdAutotune)
        {
            autotuneResult += autotuneData[3];
            accumulation += 1;
            Debug.Log("smaller4");
        }

        autotuneResult = autotuneResult / accumulation;
        
        //autotuneResult = Color.Lerp(oldAutotuneResult, autotuneResult, 0.1f);
        chromaMat.SetColor("_KeyColor", autotuneResult);
    }

    public void GenerateNormalMap()
    {
        if (normalMap == null)
        {
            normalMap = new RenderTexture(rt.width / 4, rt.height / 4, 24, RenderTextureFormat.ARGB32);
        }
        if (normalMaterial == null)
        {
            normalMaterial = new Material(normalMaterial);
            normalMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        _iteration = maskSoftnessTaps;
        depthToNormalMat.SetFloat("_ColorInfluence", colorInfluence);
        RenderTexture normalBuffer1, normalBuffer2;

        
        normalBuffer1 = RenderTexture.GetTemporary(rt.width / 4, rt.height / 4);
        normalBuffer2 = RenderTexture.GetTemporary(rt.width / 4, rt.height / 4);
        Graphics.Blit(rt, normalBuffer1, normalMaterial, 0);



        for (var i = 0; i < _iteration; i++)
        {
            Graphics.Blit(normalBuffer1, normalBuffer2, normalMaterial, 1);
            Graphics.Blit(normalBuffer2, normalBuffer1, normalMaterial, 2);
        }

        Graphics.Blit(normalBuffer1, normalMap, depthToNormalMat);

        RenderTexture.ReleaseTemporary(normalBuffer1);
        RenderTexture.ReleaseTemporary(normalBuffer2);
    
    }

    private void InitializeBuffers(Texture tex)
    {
        if (rt) rt.Release();
        if (rt2) rt2.Release();
        if (normalMap) normalMap.Release();
        if (histogram) histogram.Release();
        if (rt) DestroyImmediate(rt);
        if (rt2) DestroyImmediate(rt2);
        if (normalMap) DestroyImmediate(normalMap);
        if (histogram) DestroyImmediate(histogram);

        rt = new RenderTexture(tex.width , tex.height , 32, RenderTextureFormat.ARGB32);
        rt2 = new RenderTexture(tex.width , tex.height , 32, RenderTextureFormat.ARGB32);
        if (useHistogram)
        histogram = new RenderTexture(256, 64, 32, RenderTextureFormat.ARGB32);
        UpdateMaterials();

    }

    public void TrackImage()
    {
        if (trackingTexX != null)
        {
            if (trackingTexX.width != trackerResolutiuon)
            {
                trackingTexX = null;
            }

            if (trackingTexY.height != trackerResolutiuon)
            {
                trackingTexY = null;

            }
        }


        if (trackingTexX == null)
            trackingTexX = new Texture2D(trackerResolutiuon, 1);
        if (trackingTexY == null)
            trackingTexY = new Texture2D(1, trackerResolutiuon);

        RenderTexture.active = trackingRT;
        trackingTexX.ReadPixels(new Rect(0, trackerResolutiuon - 1, trackerResolutiuon, 1), 0, 0);
        trackingTexY.ReadPixels(new Rect(0, 0, 1, trackerResolutiuon), 0, 0);
        trackingTexX.Apply();
        trackingTexY.Apply();
        RenderTexture.active = null;
        trackingDataX = trackingTexX.GetPixels(0, 0, trackingTexX.width, trackingTexX.height);
        //track Left extent (Top in Vertical)
        for (int i = 0; i < trackerResolutiuon - 1; i++)
        {
            if (trackingDataX[i].g > trackingTreshold)
            {
                trackResult.x = i;
                break;
            }

            if (i == i - 2)
            {

                trackResult = new Vector4(0, 0, 0, 0);
                break;
            }

        }
        //track Shoulders (in Portrait)
        if (findShoulders && portraitMode)
        {
            for (int i = 0; i < trackerResolutiuon - 1; i++)
            {
                if (trackingDataX[i].g > shoulderTrackingTreshold)
                {
                    shouldersTrack = i;
                    break;
                }

            }
        }
        //track Shoulders (in Landscape)
        else if (findShoulders && !portraitMode)
        {
            for (int i = 0; i < trackerResolutiuon - 1; i++)
            {
                if (trackingDataX[i].g > shoulderTrackingTreshold)
                {
                    shouldersTrack = i;
                    break;
                }
            }
        }
        //track right extents (bottom In vertical)
        for (int i = 0; i < trackerResolutiuon; i++)
        {
            if (trackingDataX[trackerResolutiuon - i - 1].g > trackingTreshold)
            {
                trackResult.y = trackerResolutiuon - i;
                break;
            }
        }

        trackingDataY = trackingTexY.GetPixels(0, 0, trackingTexY.width, trackingTexY.height);
        //track bottom
        for (int i = 0; i < trackerResolutiuon - 1; i++)
        {
            if (trackingDataY[i].r > trackingTreshold)
            {
                trackResult.z = i;
                break;
            }
        }
        //track Top
        for (int i = 0; i < trackerResolutiuon; i++)
        {
            if (trackingDataY[trackerResolutiuon - i - 1].r > trackingTreshold)
            {
                trackResult.w = trackerResolutiuon - i;
                break;
            }
        }
 

#region //Find Head Top (in Portrait Mode)

        if (findFace && portraitMode)
        {
            if (faceSearchTex != null)
            {
                if (faceSearchTex.height != trackerResolutiuon)
                {
                    faceSearchTex = null;
                }
            }

            if (faceSearchTex == null)
                faceSearchTex = new Texture2D(1, trackerResolutiuon);

            RenderTexture.active = trackingRT;
            if (!portraitMode)
                faceSearchTex.ReadPixels(new Rect(0, trackerResolutiuon - 1, trackerResolutiuon, 1), 0, 0);
            else
                faceSearchTex.ReadPixels(new Rect(Mathf.CeilToInt(trackResult.x), 0, 1, trackerResolutiuon), 0, 0);
            //   Graphics.CopyTexture(trackingRT, faceSearchTex);
            faceSearchTex.Apply();
            RenderTexture.active = null;
            trackingDataX = faceSearchTex.GetPixels(0, 0, faceSearchTex.width, faceSearchTex.height);

            for (int i = 0; i < trackerResolutiuon - 1; i++)
            {
                if (trackingDataX[i].b > headTrackingTrehold)
                {
                    headCentroid.x = i;
                    break;
                }
            }

        }
        #endregion

        #region //Find Head Top (in Landscape Mode)

        else if (findFace && !portraitMode)
        {
            if (faceSearchTex != null)
            {
                if (faceSearchTex.width != trackerResolutiuon)
                {
                    faceSearchTex = null;
                }
            }

            if (faceSearchTex == null)
                faceSearchTex = new Texture2D(trackerResolutiuon, 1);

            RenderTexture.active = trackingRT;

                faceSearchTex.ReadPixels(new Rect(0, Mathf.CeilToInt(trackResult.x), trackerResolutiuon, 1), 0, 0);

            faceSearchTex.Apply();
            RenderTexture.active = null;
            trackingDataY = faceSearchTex.GetPixels(0, 0, faceSearchTex.width, faceSearchTex.height);

            for (int i = 1; i < trackerResolutiuon; i++)
            {
                if (trackingDataY[i].b > headTrackingTrehold)
                {
                    headCentroid.x = i;
                    
                    break;
                }
            }

        }
        #endregion
    }

    public void CaptureImage(Texture wcam)
    {
        Texture2D finalTexture = new Texture2D(wcam.width, wcam.height);
        RenderTexture rt = new RenderTexture(wcam.width, wcam.height, 32);
        Graphics.Blit(wcam, rt);
        RenderTexture.active = rt;
        finalTexture.ReadPixels(new Rect(0, 0, wcam.width, wcam.height), 0, 0);
        finalTexture.Apply();
        byte[] _bytes = finalTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes("Assets/DeckardChromaKey/Resources/Profiles/GarbageMatte.png", _bytes);

#if UNITY_EDITOR
        AssetDatabase.ImportAsset("Assets/DeckardChromaKey/Resources/Profiles/GarbageMatte.png", ImportAssetOptions.ForceUpdate);
        backgorundTex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DeckardChromaKey/Resources/Profiles/GarbageMatte.png", typeof(Texture2D));
#endif
    }

    public void CloseWebcam()
    {
        if (webcamTexture != null)
            webcamTexture.Stop();
        webcamOpened = false;
    }

    public void UpdateValues()
    {
        int key;
        if (useMateKey) key = 1;
        else key = 0;
        chromaMat.SetFloat("_treshold", threshold);
        chromaMat.SetFloat("_smooth", smooth);
        // chromaMat.SetFloat("_Border", border);
        chromaMat.SetFloat("_maskPower", maskPower);
        chromaMat.SetInt("_useMatteKey", key);






    }
    private void OnDestroy()
    {
        CloseWebcam();
        DestroyImmediate(webcamTexture);
        if (rt) rt.Release();
        if (rt2) rt2.Release();
        if (normalMap) normalMap.Release();
        DestroyImmediate(rt);
        DestroyImmediate(rt2);
        DestroyImmediate(normalMap);
    }

    public void SetPortraitMode(bool isPortrait)
    {
        if (isPortrait)
        {
            projectionPlane.localEulerAngles = new Vector3(0, 0, -90);
            chromaMat.SetFloat("_PortraitMode", 1f);
        }
        else
        {
            projectionPlane.localEulerAngles = new Vector3(0, 0, 0);
            chromaMat.SetFloat("_PortraitMode", 0f);
        }
    }



    void OnDrawGizmos()
    {
        // Your gizmo drawing thing goes here if required...

#if UNITY_EDITOR
        // Ensure continuous Update calls.
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
#endif
    }
}