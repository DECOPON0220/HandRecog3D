using System;
using UnityEngine;
using System.Collections;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp;



public class WebCamera : MonoBehaviour
{
    // 変数宣言
    public int Width;       // 320px
    public int Height;      // 240px
    public int FPS;         // 24fps
    public int VideoIndex;  // 任意のビデオデバイス
    public int INTERVAL;    // 10px
    string ext = ".jpeg";   // エンコードする形式  
    int[,] pointResult;     // 指定した点から得た情報を格納する配列
    VideoCapture video;
    Texture2D    texture;


    //---------------------------------------------------------
    // 関数名 : Start
    // 機能   : オブジェクト起動時に実行される
    // 引数   : なし
    // 戻り値 : なし
    //---------------------------------------------------------
    void Start()
    {
        // カメラを列挙する
        // 使いたいカメラのインデックスをVideoIndexに入れる
        // 列挙はUnityで使うのはOpenCVだけど、インデックスは同じらしい
        var devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            print(string.Format("index {0}:{1}", i, devices[i].name));
        }

        // ビデオの設定
        video = new VideoCapture(VideoIndex);
        video.Set(CaptureProperty.FrameWidth, Width);
        video.Set(CaptureProperty.FrameHeight, Height);

        // テクスチャの作成
        texture = new Texture2D(Width, Height, TextureFormat.RGB24, false);
        GetComponent<Renderer>().material.mainTexture = texture;

        // 変数初期化
        pointResult = new int[Height / INTERVAL, Width / INTERVAL];
    }

    //---------------------------------------------------------
    // 関数名 : Update
    // 機能   : 1フレーム毎に呼び出される
    // 引数   : なし
    // 戻り値 : なし
    //---------------------------------------------------------
    void Update()
    {
        using (Mat m_img = new Mat())
        {
            // Webカメラから画像を取得する
            video.Read(m_img);

            // Mat から IplImage に変換
            using (var i_img = m_img.ToIplImage())
            {
                // OpenCVのデータがBGRなのでHSVに変換
                Cv.CvtColor(i_img, i_img, ColorConversion.BgrToHsv);

                // 画像のヒストグラム平滑化
                Cv.Smooth(i_img, i_img, SmoothType.Median, 5, 0, 0, 0);

                // 画像の任意の点からデータ取得
                getPointData(i_img, pointResult);

                // 点情報から画像を生成（デバッグ用）
                getPointImage(i_img, pointResult);

                // 画像を画面に表示
                using (var r_img = Cv2.CvArrToMat(i_img))
                {
                    // 画像を jpeg にエンコード
                    texture.LoadImage(r_img.ImEncode(ext));
                    texture.Apply();
                }
            }
        }
    }

    //---------------------------------------------------------
    // 関数名 : getPointData
    // 機能   : データ取得
    // 引数   : img/HSV画像 arr/データ
    // 戻り値 : img/HSV画像
    //---------------------------------------------------------
    unsafe public IplImage getPointData(IplImage img, int[,] arr)
    {
        byte* pxe = (byte*)img.ImageData;

        // 縦横一定間隔ごとにデータ取得
        for (int y = INTERVAL / 2; y < Height; y = y + INTERVAL)
        {
            for (int x = INTERVAL / 2; x < Width; x = x + INTERVAL)
            {
                // 指定位置のデータ取得
                // 青の処理
                if (checkPoint(pxe, x, y))
                {
                    // データ取得位置表示（デバッグ用）
                    visualizPoint(pxe, x, y, true);

                    // データ格納
                    setPointData(pxe, x, y, arr, true);
                }
                // 青以外の処理
                else
                {
                    // データ取得位置表示（デバッグ用）
                    visualizPoint(pxe, x, y, false);

                    // データ格納
                    setPointData(pxe, x, y, arr, false);
                }
            }
        }

        return img;
    }

    //---------------------------------------------------------
    // 関数名 : setPointData
    // 機能   : データを取得、配列に保存
    // 引数   : pxe/ピクセル情報 x/インデックス y/インデックス arr/データ
    // 戻り値 : なし
    //---------------------------------------------------------
    unsafe private void setPointData(byte* pxe, int x, int y, int[,] arr, bool flag)
    {
        // 青
        if (flag)
        {
            arr[(y / INTERVAL), (x / INTERVAL)] = 0;
        }
        // 青以外
        else
        {
            arr[(y / INTERVAL), (x / INTERVAL)] = 1;
        }
    }

    //---------------------------------------------------------
    // 関数名 : checkPoint
    // 機能   : 画像にデータ習得位置を表示
    // 引数   : pxe/ピクセル情報 x/横ピクセル y/縦ピクセル
    // 戻り値 : true/青 false/青以外
    //---------------------------------------------------------
    unsafe private bool checkPoint(byte* pxe, int x, int y)
    {
        // インデックス計算
        int index = (Width * 3) * y + (x * 3);

        int h = pxe[index];
        int s = pxe[index];
        int v = pxe[index];

        // 青の場合
        if (h >= 85 && h <= 110
         && s >= 5 && s <= 255
         && v >= 5 && v <= 255)
        {
            return true;
        }
        // その他の場合
        return false;
    }

    //---------------------------------------------------------
    // 関数名 : visualizPoint
    // 機能   : 画像にデータ取得位置を表示（デバッグ用）
    // 引数   : pxe/ピクセル情報 x/横ピクセル y/縦ピクセル flag/青かそれ以外か
    // 戻り値 : なし
    //---------------------------------------------------------
    unsafe private void visualizPoint(byte* pxe, int x, int y, bool flag)
    {
        for (int y_ad = -1; y_ad <= 1; y_ad++) {
            for (int x_ad = -1; x_ad <= 1; x_ad++) {
                // インデックス計算
                int index = (Width * 3) * (y + y_ad) + ((x + x_ad) * 3);

                // 青の場合
                if (flag)
                {
                    pxe[index] = 0;
                    pxe[index + 1] = 0;
                    pxe[index + 2] = 0;
                }
                // 青以外の場合
                else {
                    pxe[index] = 255;
                    pxe[index + 1] = 255;
                    pxe[index + 2] = 255;
                }
            }
        }
    }

    //---------------------------------------------------------
    // 関数名 : getPointImage
    // 機能   : 画像取得（デバッグ用）
    // 引数   : name/ウィンドウ名 img/HSV画像                             
    // 戻り値 : なし
    //---------------------------------------------------------
    unsafe public IplImage getPointImage(IplImage img, int[,] arr)
    {
        // 変数宣言
        int index, x_tmp, y_tmp;

        // 画像初期化（すべて黒）
        byte* pxe = (byte*)img.ImageData;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                index = (Width * 3) * y + (x * 3);
                pxe[index] = 0;
                pxe[index + 1] = 0;
                pxe[index + 2] = 0;
            }
        }

        // 肌色の点を白
        for (int y = INTERVAL / 2; y < Height; y = y + INTERVAL)
        {
            for (int x = INTERVAL / 2; x < Width; x = x + INTERVAL)
            {
                for (int y_ad = -1; y_ad <= 1; y_ad++)
                {
                    for (int x_ad = -1; x_ad <= 1; x_ad++)
                    {
                        // インデックス計算
                        index = (Width * 3) * (y + y_ad) + ((x + x_ad) * 3);
                        x_tmp = (x - (INTERVAL / 2)) / INTERVAL;
                        y_tmp = (y - (INTERVAL / 2)) / INTERVAL;

                        // 青以外の場合，白
                        if (arr[y_tmp, x_tmp] == 1)
                        {
                            pxe[index] = 255;
                            pxe[index + 1] = 255;
                            pxe[index + 2] = 255;
                        }
                    }

                }
            }
        }

        return img;
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    void OnApplicationQuit()
    {
        if (video != null)
        {
            video.Dispose();
            video = null;
        }
    }
}