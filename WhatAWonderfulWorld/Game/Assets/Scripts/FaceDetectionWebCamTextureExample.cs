#if !(PLATFORM_LUMIN && !UNITY_EDITOR)

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using UnityEngine.Experimental.Rendering.Universal;

namespace OpenCVForUnityExample
{
    [RequireComponent (typeof(WebCamTextureToMatHelper))]
    public class FaceDetectionWebCamTextureExample : MonoBehaviour
    {
        public Scene Game;
        float firstTP = 0;
        float firstTC = 0;
        bool laPatate = false;
        bool Cligne = false;
        int CalibCompt = 5;
        bool Validation = false;
        float timer = 1;

        Mat grayMat;
        Mat grayTest;
        Texture2D texture;
        CascadeClassifier cascade;
        CascadeClassifier eyes_cascade;
        MatOfRect faces;
        MatOfRect eyes;
        WebCamTextureToMatHelper webCamTextureToMatHelper;
        string eyes_cascade_name = "haarcascade_eye_tree_eyeglasses.xml";
        protected static readonly string LBP_CASCADE_FILENAME = "lbpcascade_frontalface.xml";

        #if UNITY_WEBGL && !UNITY_EDITOR
        IEnumerator getFilePath_Coroutine;
        #endif

        void Start ()
        {


            webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper> ();

            #if UNITY_WEBGL && !UNITY_EDITOR
            getFilePath_Coroutine = Utils.getFilePathAsync (LBP_CASCADE_FILENAME, (result) => {
                getFilePath_Coroutine = null;

                cascade = new CascadeClassifier ();
                cascade.load (result);
                if (cascade.empty ()) {
                    Debug.LogError ("cascade file is not loaded. Please copy from “OpenCVForUnity/StreamingAssets/” to “Assets/StreamingAssets/” folder. ");
                }

                webCamTextureToMatHelper.Initialize ();
            });
            StartCoroutine (getFilePath_Coroutine);
            #else
            cascade = new CascadeClassifier ();
            cascade.load (Utils.getFilePath (LBP_CASCADE_FILENAME));
            if (cascade.empty ()) {
                Debug.LogError ("cascade file is not loaded. Please copy from “OpenCVForUnity/StreamingAssets/” to “Assets/StreamingAssets/” folder. ");
            }
            eyes_cascade = new CascadeClassifier();
            if (!eyes_cascade.load(Utils.getFilePath(eyes_cascade_name))) { Debug.LogError("cascade eyes not loaded"); }


#if UNITY_ANDROID && !UNITY_EDITOR
            // Avoids the front camera low light issue that occurs in only some Android devices (e.g. Google Pixel, Pixel2).
            webCamTextureToMatHelper.avoidAndroidFrontCameraLowLightIssue = true;
#endif
            webCamTextureToMatHelper.Initialize ();

            #endif
        }

        public void OnWebCamTextureToMatHelperInitialized ()
        {            
            Mat webCamTextureMat = webCamTextureToMatHelper.GetMat ();

            texture = new Texture2D (webCamTextureMat.cols (), webCamTextureMat.rows (), TextureFormat.RGBA32, false);

            gameObject.GetComponent<Renderer> ().material.mainTexture = texture;

            gameObject.transform.localScale = new Vector3 (webCamTextureMat.cols (), webCamTextureMat.rows (), 1);
            
            float width = webCamTextureMat.width ();
            float height = webCamTextureMat.height ();
            
            float widthScale = (float)Screen.width / width;
            float heightScale = (float)Screen.height / height;
            if (widthScale < heightScale) {
                Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
            } else {
                Camera.main.orthographicSize = height / 2;
            }

            grayMat = new Mat (webCamTextureMat.rows (), webCamTextureMat.cols (), CvType.CV_8UC1);

            faces = new MatOfRect ();
            eyes = new MatOfRect();
        }

        public void OnWebCamTextureToMatHelperDisposed ()
        {
            if (grayMat != null)
                grayMat.Dispose ();

            if (texture != null) {
                Texture2D.Destroy (texture);
                texture = null;
            }

            if (faces != null)
                faces.Dispose ();
        }

        public void OnWebCamTextureToMatHelperErrorOccurred (WebCamTextureToMatHelper.ErrorCode errorCode)
        {
            Debug.Log ("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
        }

        void Update ()
        {
            Scene Game = SceneManager.GetActiveScene();
            
            if (webCamTextureToMatHelper.IsPlaying () && webCamTextureToMatHelper.DidUpdateThisFrame ()) {
                
                Mat rgbaMat = webCamTextureToMatHelper.GetMat ();
                Imgproc.cvtColor (rgbaMat, grayMat, Imgproc.COLOR_RGBA2GRAY);
                Imgproc.equalizeHist (grayMat, grayMat);
                
                
                if (cascade != null)
                {
                    cascade.detectMultiScale(grayMat, faces, 1.1, 2, 2, new Size(grayMat.cols() * 0.2, grayMat.rows() * 0.2), new Size());
                }

                grayTest = grayMat;

                if (eyes_cascade != null)
                {
                    eyes_cascade.detectMultiScale(grayTest, eyes, 1.1, 2, 0, new Size(grayTest.cols() * 0.13, grayTest.rows() * 0.13), new Size(30, 30));
                }

                OpenCVForUnity.CoreModule.Rect[] rects_y = eyes.toArray();
                for (int j = 0; j < rects_y.Length; j++)
                {
                    Imgproc.rectangle(rgbaMat, new Point(rects_y[j].x, rects_y[j].y), new Point(rects_y[j].x + rects_y[j].width, rects_y[j].y + rects_y[j].height), new Scalar(255, 255, 0, 0), 2);
                }

                //Affichage du cadrillage pour le calibrage
                if (Game.name == "Calibrage" && CalibCompt < 4)
                {
                    Imgproc.rectangle(rgbaMat, new Point(0, 0), new Point(640, 480), new Scalar(0, 0, 0, 0), 500);
                    Imgproc.line(rgbaMat, new Point(200, 0), new Point(200, 480), new Scalar(255, 0, 0, 255), 1);
                    Imgproc.line(rgbaMat, new Point(440, 0), new Point(440, 480), new Scalar(255, 0, 0, 255), 1);
                    Imgproc.line(rgbaMat, new Point(200, 150), new Point(440, 150), new Scalar(255, 0, 0, 255), 1);
                }else if(Game.name == "Calibrage" && CalibCompt == 4)
                {
                    Imgproc.rectangle(rgbaMat, new Point(0, 0), new Point(640, 480), new Scalar(0, 0, 0, 0), 500);
                    Imgproc.rectangle(rgbaMat, new Point(205, 125), new Point(435, 355), new Scalar(255, 0, 0, 255), 1);
                }
                else
                {
                    Imgproc.rectangle(rgbaMat, new Point(0, 0), new Point(640, 480), new Scalar(0, 0, 0, 0), 500);
                }

                OpenCVForUnity.CoreModule.Rect[] rects = faces.toArray ();
                for (int i = 0; i < rects.Length; i++) {
                    //Lance la fonction selon la scene qui est joué
                    if (Game.name == "Jeu")
                    {
                        Deplacement(rects[i], rects_y);
                    }
                    else if(Game.name == "Calibrage")
                    {
                        rgbaMat = CalibFace(rects[i], rgbaMat);
                    }
                }
                Utils.fastMatToTexture2D (rgbaMat, texture);
            }
        }


        //Gestion du calibrage de la tête
        Mat CalibFace(OpenCVForUnity.CoreModule.Rect rects, Mat rgbaMat)
        {
            Text TextCalib;
            TextCalib = GameObject.Find("Indication").GetComponent<Text>();
            Text Gauche;
            Gauche = GameObject.Find("Gauche").GetComponent<Text>();
            Text Droite;
            Droite = GameObject.Find("Droite").GetComponent<Text>();
            Text Haut;
            Haut = GameObject.Find("Jump").GetComponent<Text>();
            Text Milieu;
            Milieu = GameObject.Find("Milieu").GetComponent<Text>();
            int LimG = 0;
            int LimD = 0;
            int LimH = 0;
            int LimB = 640;
            
            if(CalibCompt<4)
            {
                Imgproc.circle(rgbaMat, new Point(rects.x + rects.width / 2, rects.y + rects.height / 2), 10, new Scalar(38, 193, 231, 255), 20);
            }
            else if(CalibCompt == 4)
            {
                Imgproc.rectangle(rgbaMat, new Point(rects.x, rects.y), new Point(rects.x + rects.width, rects.y + rects.height), new Scalar(38, 193, 231, 255), 2);
            }

            switch (CalibCompt)
            {
                //Milieu
                case 0:
                    TextCalib.text = "Bienvenue sur le calibrage. Placer le point au Milieu";
                    LimG = 100;
                    LimD = 340;
                    LimH = 60;
                    break;

                //Gauche
                case 1:
                    TextCalib.text = "Parfait ! Maintenant rester a gauche";
                    LimG = 0;
                    LimD = 100;
                    LimH = 60;
                    break;

                //Droite
                case 2:
                    TextCalib.text = "Parfait ! Maintenant rester a droite";
                    LimG = 340;
                    LimD = 480;
                    LimH = 60;
                    break;

                //Saut
                case 3:
                    TextCalib.text = "Parfait ! Maintenant rester en haut";
                    LimG = 100;
                    LimD = 340;
                    LimH = 0;
                    LimB = 60;
                    break;

                //Patate
                case 4:
                    TextCalib.text = "Parfait ! Maintenant bouger le rectangle bleue pour qu'il soit plus petit que le rouge et ensuite plus grand";
                    Gauche.text = "";
                    Droite.text = "";
                    Haut.text = "";
                    Milieu.text = "";

                    if (rects.x > 205 && rects.y > 125 && rects.x + rects.width < 435 && rects.y + rects.height < 355  && rects.width < 230 && Validation == false)
                    {
                        Validation = true;
                    }
                    if (rects.x < 205 && rects.y < 125 && rects.x + rects.width > 435 && rects.y + rects.height > 355 && rects.width > 320 && Validation == true)
                    {
                        CalibCompt++;
                        Validation = false;
                    }
                    break;

                //Finalisation
                case 5:
                    TextCalib.text = "Tout est calibre ! Mettez vous au milieu et appuyez sur espace pour lancer le jeux.";
                    LimG = 100;
                    LimD = 340;
                    LimH = 60;
                    if (Input.GetKeyDown("space") && rects.x > LimG && rects.x < LimD && rects.y > LimH && rects.y < LimB)
                    {
                        TextCalib.text = "Chargement...";
                        SceneManager.LoadScene("Jeu");
                    }
                    break;
            }

            if (CalibCompt < 4)
            {
                if (rects.x > LimG && rects.x < LimD && rects.y > LimH && rects.y < LimB && Validation == false)
                {
                    firstTP = Time.time;
                    Validation = true;
                }
                if (rects.x < LimG || rects.x > LimD || rects.y < LimH || rects.y > LimB)
                {
                    Validation = false;
                }
                if (Validation == true)
                {
                    Imgproc.ellipse(rgbaMat, new Point(320, 380), new Size(20, 20), 10.0, -90.0, ((Time.time - firstTP) * 120) - 90, new Scalar(0, 255, 0, 255), 2);
                }
                if (Time.time - firstTP > 3 && Validation == true)
                {
                    CalibCompt++;
                    Validation = false;
                }
            }

            return rgbaMat;
        }

        //Gestion du déplacement du personnage
        void Deplacement(OpenCVForUnity.CoreModule.Rect rects, OpenCVForUnity.CoreModule.Rect[] rects_y)
        {
            GameObject slime;
            slime = GameObject.Find("Blob");
            GameObject sol;
            sol = GameObject.Find("plat1");
            DeplacerSlime contact;
            contact = slime.GetComponent<DeplacerSlime>();
            GameObject Coffre;
            Coffre = GameObject.Find("Coffre");
            FrapperCoffre openC;
            openC = Coffre.GetComponent<FrapperCoffre>();
            GameObject lumiere;
            lumiere = GameObject.Find("Global Light 2D");
            Light2D light;
            light = lumiere.GetComponent<Light2D>();


            if (contact.isDead == false)
            {
                //Mouvement
                if (rects.x < 100 && rects.y > 60)
                {
                    //Milieu - Gauche
                    slime.transform.Translate(-10, 0, 0);
                    contact.deplacement = -1;
                }
                else if (rects.x > 300 && rects.y > 60)
                {
                    //Milieu - Droit
                    slime.transform.Translate(10, 0, 0);
                    contact.deplacement = 1;
                }
                else if (rects.x > 100 && rects.x < 300 && rects.y < 60 && contact.auSol == true)
                {
                    //Tete.text = "Haut - Milieu";
                    slime.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 1500.0f), ForceMode2D.Impulse);
                    contact.deplacement = 0;
                }
                else if (rects.x > 300 && rects.y < 60 && contact.auSol == true)
                {
                    //Haut - Droit
                    slime.GetComponent<Rigidbody2D>().AddForce(new Vector2(200.0f, 1500.0f), ForceMode2D.Impulse);
                    contact.deplacement = 1;
                }
                if (rects.x < 100 && rects.y < 60 && contact.auSol == true)
                {
                    //Haut - Gauche
                    slime.GetComponent<Rigidbody2D>().AddForce(new Vector2(-200.0f, 1500.0f), ForceMode2D.Impulse);
                    contact.deplacement = -1;
                }
                else
                {
                    contact.deplacement = 0;
                }

                //Patate
                if (laPatate == true)
                {
                    contact.frapper = false;
                }
                if (rects.width < 210 && rects.width > 180 && laPatate == false)
                {
                    firstTP = Time.time;
                }
                if (rects.width > 300 && Time.time - firstTP < 0.5 && laPatate == false)
                {
                    contact.frapper = true;
                    laPatate = true;
                    firstTP = Time.time;
                }

                if (Time.time - firstTP > 1 && laPatate == true)
                {
                    laPatate = false;
                }

                //Clignement
                if (rects_y.Length == 0)
                {
                    timer -= Time.deltaTime;
                    if (timer > 0.9833 && Cligne == false)
                    {
                        firstTC = Time.time;
                        Cligne = true;
                        timer = 1;

                        light.intensity = 0;
                    }
                    else if(Time.time - firstTC > 0.5)
                    {
                        timer = 1;
                        light.intensity = 0.1f;
                    }

                    if(Time.time - firstTC > 5 && Cligne == true)
                    {
                        Cligne = false;
                    }
                }
            }
        }

        void OnDestroy ()
        {
            webCamTextureToMatHelper.Dispose ();

            if (cascade != null)
                cascade.Dispose ();

            #if UNITY_WEBGL && !UNITY_EDITOR
            if (getFilePath_Coroutine != null) {
                StopCoroutine (getFilePath_Coroutine);
                ((IDisposable)getFilePath_Coroutine).Dispose ();
            }
            #endif
        }
    }
}

#endif