using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using UnityEngine;
using System.Collections;



public class Main : MonoBehaviour
{
    // 変数宣言
    IplImage i_img1, i_img2;
    int[,] ps_arr1, ps_arr2;    // 縦横それぞれの点情報を格納
    int[,,] ps_arr3D, t_arr;
    double[,] pl_arrXZ;
    GameObject refObj;          // 3Dポリゴン取得用
    public GameObject dot;
    CreatePolygonMesh polygon;  // CreatePolygonMesh クラス参照用
    ArrayList vec = new ArrayList();

    IplImage h_img1 = new IplImage();
    IplImage h_img2 = new IplImage();
    Camera cam1 = new Camera();
    Camera cam2 = new Camera();
    Graphic g = new Graphic();

    /*     デバッグ用（FPS）     */
    int   frameCount;
    float prevTime;
    /* ------------------------- */

    // Use this for initialization
    void Start()
    {
        // カメラデバイスの選択、設定
        cam1.setDevice(0);  // 横
        cam2.setDevice(1);  // 縦

        // HSV用画像の初期化
        CvSize WINDOW_SIZE = new CvSize(GlobalVar.CAMERA_WIDTH, GlobalVar.CAMERA_HEIGHT);
        h_img1 = Cv.CreateImage(WINDOW_SIZE, BitDepth.U8, 3);
        h_img2 = Cv.CreateImage(WINDOW_SIZE, BitDepth.U8, 3);

        // データ格納用配列の初期化
        ps_arr1 = new int[GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL, GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL];
        ps_arr2 = new int[GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL, GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL];
        ps_arr3D = new int[GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL, GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL, GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL];
        t_arr = new int[(GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL) + 2, (GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL) + 2, (GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL) + 2];
        pl_arrXZ = new double[GlobalVar.VERTICE_NUM / 2, 2];

        // 3Dポリゴン指定
        refObj = GameObject.Find("Donuts");
        polygon = refObj.GetComponent<CreatePolygonMesh>();

        /*     デバッグ用（FPS）     */
        frameCount = 0;
        prevTime = 0.0f;
        /* ------------------------- */
    }

    // Update is called once per frame
    void Update()
    {
        /*     デバッグ用（FPS）     */
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;
        /* ------------------------- */

        // カメラ画像の取得
        i_img1 = cam1.getCameraImage();
        i_img2 = cam2.getCameraImage();

        // カメラ画像をBGRからHSVへ変換
        g.convertBgrToHsv(i_img1, h_img1);
        g.convertBgrToHsv(i_img2, h_img2);

        // 平滑化
        g.convertSmooothing(h_img1);
        g.convertSmooothing(h_img2);

        // カメラ画像の任意の点からデータ取得
        g.getPointData(h_img1, ps_arr1);
        g.getPointData(h_img2, ps_arr2);

        /*     デバッグ用     */
        //d_img = g.getPointImage2(d_img, ps_arr1, ps_arr2);
        //using (var r_img = Cv2.CvArrToMat(d_img))
        //{
        //    texture.LoadImage(r_img.ImEncode(".jpeg"));
        //    texture.Apply();
        //}
        /* ------------------ */

        // 縦横の点情報を結合して3次元配列に格納
        bondPosStaArr(ps_arr2, ps_arr1, ps_arr3D);

        // 情報が存在するレイヤー(Y軸方向)において、内部外部判定を行う
        for (int y = 0; y < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; y++)
        {
            // 情報が存在する場合
            if (isExsistInfo3DArrY(ps_arr3D, y))
            {
                // 同じ階層にポリゴンがある場合
                if (polygon.isExsistPolygon(y * GlobalVar.POINT_INTERVAL))
                {
                    // ポリゴンの座標を取得
                    polygon.getPolygonXZ(pl_arrXZ);
                    // ベクトル取得
                    polygon.getPolygonVector(vec, pl_arrXZ);
                    // 取得したベクトルと手情報から内部外部判定を行う
                    //polygon.getIEDecisionArr(vec, ps_arr3D, y);

                    // 内部に手情報がある場合
                    if (polygon.isIEDecision(vec, ps_arr3D, y))
                    {
                        Debug.Log("内部に手有り");

                    }
                }
                // 同じ階層にポリゴンがない場合
                else
                {

                }
            }
            // 情報が存在しない場合
            else
            {

            }
        }


        // 3D表示
        display3Ddot(ps_arr3D);

        /*     デバッグ用（FPS）     */
        if (time >= 0.5f)
        {
            //Debug.LogFormat("{0}fps", frameCount/time);
            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
        /* ------------------------- */
    }

    //---------------------------------------------------------
    // 関数名 : isExsistInfo3DArrY
    // 機能   : 三次元配列について、指定されたYの時、情報が格納されているか確認する
    // 引数   : xyz_arr/ y/
    // 戻り値 : true/ false/
    //---------------------------------------------------------
    private bool isExsistInfo3DArrY(int[,,] xyz_arr, int y)
    {
        for (int x = 0; x < GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL; x++)
        {
            for (int z = 0; z < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; z++)
            {
                if (xyz_arr[x, y, z] == 1) {
                    return true;
                }
            }
        }

        return false;
    }

    //---------------------------------------------------------
    // 関数名 : bondPosStaArr
    // 機能   : 青以外の情報が格納されている数を取得
    // 引数   : x_arr/X情報 y_arr/Y情報
    // 戻り値 : xyz_arr/
    //---------------------------------------------------------
    private int[,,] bondPosStaArr(int[,] xz_arr, int[,] xy_arr, int[,,] xyz_arr)
    {
        // 配列を初期化
        init3DArr(xyz_arr);

        // 下から情報があるか確認している
        for (int y = 0; y < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; y++)
        {
            for (int x = 0; x < GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL; x++)
            {
                // 情報を発見したとき
                if (xy_arr[(GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL) - 1 - y, x] == 1) {
                    for (int z = 0; z < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; z++)
                    {
                        if (xz_arr[z, x] == 1)
                        {
                            xyz_arr[x, y, (GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL) - 1 - z] = 1;
                        }
                        else
                        {
                            xyz_arr[x, y, (GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL) - 1 - z] = 0;
                        }
                    }
                }
            }
        }

        // ノイズを除去
        // delArrNoise(xyz_arr);

        return xyz_arr;
    }

    //---------------------------------------------------------
    // 関数名 : delArrNoise
    // 機能   : 情報が格納されている配列からノイズ情報を除去する
    // 引数   : arr/情報
    // 戻り値 : なし
    //---------------------------------------------------------
    private int[,,] delArrNoise(int[,,] arr)
    {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              

        return arr;
    }

    //---------------------------------------------------------
    // 関数名 : display3Ddot
    // 機能   : 情報が格納されている配列から3Dに起こす
    // 引数   : arr/情報
    // 戻り値 : なし
    //---------------------------------------------------------
    private void display3Ddot(int[,,] arr)
    {
        // 変数宣言
        float i_x = 0;
        float i_y = 0;
        float i_z = -240;
        float fps = 0.05F;
        float t_x, t_y, t_z;

        for (int y = 0; y < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; y++)
        {
            for (int x = 0; x < GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL; x++)
            {
                for (int z = 0; z < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; z++)
                {
                    if (arr[x,y,z] == 1)
                    {
                        t_x = i_x;
                        t_y = i_y;
                        t_z = i_z;

                        t_x = t_x + (GlobalVar.POINT_INTERVAL / 2) + (GlobalVar.POINT_INTERVAL * x);
                        t_y = t_y + (GlobalVar.POINT_INTERVAL / 2) + (GlobalVar.POINT_INTERVAL * y);
                        t_z = t_z + (GlobalVar.POINT_INTERVAL / 2) + (GlobalVar.POINT_INTERVAL * z);

                        GameObject dots = Instantiate(dot, new Vector3(t_x, t_y, t_z), transform.rotation) as GameObject;
                        Destroy(dots, fps);
                    }
                }
            }
        }
    }


    //---------------------------------------------------------
    // 関数名 : init3DArr
    // 機能   : 情報が格納されている配列から3Dに起こす
    // 引数   : arr/情報
    // 戻り値 : なし
    //---------------------------------------------------------
    private void init3DArr(int[,,] arr)
    {
        for (int y = 0; y < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; y++)
        {
            for (int x = 0; x < GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL; x++)
            {
                for (int z = 0; z < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; z++)
                {
                    arr[x, y, z] = 0;
                }
            }
        }
    }

    //---------------------------------------------------------
    // 関数名 : init3DArr2
    // 機能   : 情報が格納されている配列から3Dに起こす
    // 引数   : arr/情報
    // 戻り値 : なし
    //---------------------------------------------------------
    private void init3DArr2(int[,,] arr)
    {
        for (int y = 0; y < (GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL) + 2; y++)
        {
            for (int x = 0; x < (GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL) + 2; x++)
            {
                for (int z = 0; z < (GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL) + 2; z++)
                {
                    arr[x, y, z] = 0;
                }
            }
        }
    }

        /// <summary>
        /// 終了処理
        /// </summary>
        void OnApplicationQuit()
    {
        // リソースの破棄
        Cv.ReleaseImage(i_img1);
        Cv.ReleaseImage(i_img2);
        Cv.ReleaseImage(h_img1);
        Cv.ReleaseImage(h_img2);
        cam1.Release();
        cam2.Release();
    }
}