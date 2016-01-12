using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using UnityEngine;



public class Sub : MonoBehaviour
{
    // 変数宣言
    public int index;
    public GameObject dot;

    IplImage i_img;
    int[,] ps_arr;
    //int nb_num;

    IplImage h_img = new IplImage();
    Camera cam = new Camera();
    Graphic g = new Graphic();
    //ThreeDimension td = new ThreeDimension();

    /*     デバッグ用     */
    Texture2D texture;
    IplImage d_img = new IplImage();
    /* ------------------ */

    // Use this for initialization
    void Start()
    {
        // カメラデバイスの選択、設定
        cam.setDevice(index);

        // HSV用画像の初期化
        CvSize WINDOW_SIZE = new CvSize(GlobalVar.CAMERA_WIDTH, GlobalVar.CAMERA_HEIGHT);
        h_img = Cv.CreateImage(WINDOW_SIZE, BitDepth.U8, 3);

        // データ格納用配列の初期化
        ps_arr = new int[GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL, GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL];

        /*     デバッグ用     */
        CvSize D_WINDOW_SIZE = new CvSize(GlobalVar.CAMERA_WIDTH, GlobalVar.CAMERA_HEIGHT);
        d_img = Cv.CreateImage(D_WINDOW_SIZE, BitDepth.U8, 3);
        texture = new Texture2D(GlobalVar.CAMERA_WIDTH, GlobalVar.CAMERA_HEIGHT, TextureFormat.RGB24, false);
        GetComponent<Renderer>().material.mainTexture = texture;
        /* ------------------ */
    }

    // Update is called once per frame
    void Update()
    {
        // カメラ画像の取得
        i_img = cam.getCameraImage();

        // カメラ画像をBGRからHSVへ変換
        g.convertBgrToHsv(i_img, h_img);

        // 平滑化
        g.convertSmooothing(h_img);

        // カメラ画像の任意の点からデータ取得
        g.getPointData(h_img, ps_arr);


        /*     デバッグ用（HSV画像表示表示）     */
        /*---------------------------------------*/
        //using (var r_img = Cv2.CvArrToMat(h_img))
        //{
        //    texture.LoadImage(r_img.ImEncode(".jpeg"));
        //    texture.Apply();
        //}
        /*---------------------------------------*/
        /*     デバッグ用（点情報表示）     */
        /*----------------------------------*/
        d_img = g.getPointImage1(d_img, ps_arr);
        using (var r_img = Cv2.CvArrToMat(d_img))
        {
            texture.LoadImage(r_img.ImEncode(".jpeg"));
            texture.Apply();
        }
        /*----------------------------------*/

        /*       テスト       */
        //display3Ddot(ps_arr);
        /* ------------------ */
    }

    //---------------------------------------------------------
    // 関数名 : display3Ddot
    // 機能   : 情報が格納されている配列から3Dに起こす
    // 引数   : arr/情報
    // 戻り値 : なし
    //---------------------------------------------------------
    public void display3Ddot(int[,] arr)
    {
        // 変数宣言
        int index, x_tmp, y_tmp;
        float i_x = -160;
        float i_y = 40;
        float i_z = 120;
        float fps = 0.05F;
        float t_x, t_y, t_z;

        for (int y = GlobalVar.POINT_INTERVAL / 2; y < GlobalVar.CAMERA_HEIGHT; y = y + GlobalVar.POINT_INTERVAL)
        {
            for (int x = GlobalVar.POINT_INTERVAL / 2; x < GlobalVar.CAMERA_WIDTH; x = x + GlobalVar.POINT_INTERVAL)
            {
                // インデックス計算
                index = (GlobalVar.CAMERA_WIDTH * 3) * y + (x * 3);
                x_tmp = (x - (GlobalVar.POINT_INTERVAL / 2)) / GlobalVar.POINT_INTERVAL;
                y_tmp = (y - (GlobalVar.POINT_INTERVAL / 2)) / GlobalVar.POINT_INTERVAL;

                // 青以外の場合，白
                if (arr[y_tmp, x_tmp] == 1)
                {
                    t_x = i_x;
                    t_y = i_y;
                    t_z = i_z;

                    t_x = t_x + (GlobalVar.POINT_INTERVAL / 2) + (GlobalVar.POINT_INTERVAL * x_tmp);
                    t_z = t_z - (GlobalVar.POINT_INTERVAL / 2) - (GlobalVar.POINT_INTERVAL * y_tmp);

                    GameObject dots = Instantiate(dot, new Vector3(t_x, t_y , t_z), transform.rotation) as GameObject;
                    Destroy(dots, fps);
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
        Cv.ReleaseImage(i_img);
        Cv.ReleaseImage(h_img);
        cam.Release();
    }
}