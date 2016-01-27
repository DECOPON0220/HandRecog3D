using UnityEngine;
using System.Collections;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

class ThreeDimension
{
    // 変数宣言
    private int[,,] xyz_arr;

    //---------------------------------------------------------
    // 関数名 : createXYZArr
    // 機能   : 三次元配列を作成
    // 引数   : 
    // 戻り値 : 
    //---------------------------------------------------------
    public void createXYZArr()
    {
        // x,y,z軸の値を設定
        int x = GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL;
        int y = GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL;
        int z = GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL;

        // 三次元配列を作成
        xyz_arr = new int[x, y, z];

        // 値を初期化
        initXYZArr();

        return;
    }

    //---------------------------------------------------------
    // 関数名 : initXYZArr()
    // 機能   : 三次元配列の値を初期化
    // 引数   : 
    // 戻り値 : 
    //---------------------------------------------------------
    public void initXYZArr()
    {
        for (int x = 0; x < xyz_arr.GetLength(0); x++)
        {
            for (int y = 0; y < xyz_arr.GetLength(1); y++)
            {
                for (int z = 0; z < xyz_arr.GetLength(2); z++)
                {
                    xyz_arr[x, y, z] = 0;
                }
            }
        }

        return;
    }

    //---------------------------------------------------------
    // 関数名 : setXZArr();
    // 機能   : オブジェクトのXZ面の情報をもとに、内部外部判定を行い
    //          点情報を三次元配列に登録
    // 引数   : p_xz/ポリゴンのxz座標ベクトル
    // 戻り値 : 
    //--------------------------------------------------------
    public void setXZArr(ArrayList p_xz, int y)
    {
        Debug.Log("Test");

        // 判定用の座標を配列に格納
        ArrayList vec = new ArrayList();
        for (int z = 0; z < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; z++)
        {
            for (int x = 0; x < GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL; x++)
            {
                vec.Add(new Complex(x * GlobalVar.POINT_INTERVAL, z * (-GlobalVar.POINT_INTERVAL)));
            }
        }

        // ポリゴン近くの点について内部外部判定
        for (int k = 0; k < vec.Count; k++)
        {
            Complex com = (Complex)vec[k];

            for (int j = 0; j < p_xz.Count - 1; j++)
            {
                Complex c1 = (Complex)p_xz[j];
                Complex c2 = (Complex)p_xz[j + 1];
                Complex sub1 = c1.sub(com);
                Complex sub2 = c2.sub(c1);

                Debug.Log(sub1.x);

                double ch3 = Mathf.Sqrt(Mathf.Pow((float)sub1.x, 2) + Mathf.Pow((float)sub1.y, 2));

                if (ch3 < 10)
                {
                    double ch4 = sub1.x * sub2.y - sub1.y * sub2.x;
                    if (ch4 >= 0)
                    {
                        //近くの外
                        //printSDot(d_img, (int)(com.x) - 1, (int)(com.y) - 1, 1);
                        //flag[k] = 1;

                        
                    }
                    else {
                        //近くの内
                        //printSDot(d_img, (int)(com.x) - 1, (int)(com.y) - 1, 2);
                        //flag[k] = 2;
                    }
                    break;
                }
            }
        }
    }

    /*
    public bool isIEDecision(ArrayList p_vec, int[,,] xyz_arr, int y)
    {
        // 手の点座標を配列に格納
        ArrayList vec = new ArrayList();
        for (int z = 0; z < GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL; z++)
        {
            for (int x = 0; x < GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL; x++)
            {
                if (xyz_arr[x, y, z] == 1)
                {
                    vec.Add(new Complex(x * GlobalVar.POINT_INTERVAL, z * (-GlobalVar.POINT_INTERVAL)));
                }
            }
        }

        // ポリゴン近くの点について内部外部判定
        for (int k = 0; k < vec.Count; k++)
        {
            Complex com = (Complex)vec[k];

            //Debug.Log(com.x);

            for (int j = 0; j < p_vec.Count - 1; j++)
            {
                Complex c1 = (Complex)p_vec[j];
                Complex c2 = (Complex)p_vec[j + 1];
                Complex sub1 = c1.sub(com);
                Complex sub2 = c2.sub(c1);

                double ch3 = Mathf.Sqrt(Mathf.Pow((float)sub1.x, 2) + Mathf.Pow((float)sub1.y, 2));

                if (ch3 < 10)
                {
                    double ch4 = sub1.x * sub2.y - sub1.y * sub2.x;
                    if (ch4 >= 0)
                    {
                        //近くの外
                        //printSDot(d_img, (int)(com.x) - 1, (int)(com.y) - 1, 1);
                        //flag[k] = 1;
                    }
                    else {
                        //近くの内
                        //printSDot(d_img, (int)(com.x) - 1, (int)(com.y) - 1, 2);
                        //flag[k] = 2;
                        return true;
                    }
                    break;
                }
            }
        }

        return false;
    }
    */

    class Complex
    {
        public double x;
        public double y;
        bool flag;

        public Complex(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.flag = false;
        }

        Complex add(Complex c2)
        {//複素数足し算
            return new Complex(this.x + c2.x, this.y + c2.y);
        }
        public Complex sub(Complex c2)
        {//複素数引き算
            return new Complex(this.x - c2.x, this.y - c2.y);
        }
        Complex mul(Complex c2)
        {//複素数掛け算
            return new Complex(this.x * c2.x - this.y * c2.y, this.x * c2.y + this.y * c2.x);
        }
        Complex div(Complex c2)
        {//複素数割り算
            double d = (c2.x * c2.x) + (c2.y * c2.y);
            return new Complex(((this.x * c2.x) + (this.y * c2.y)) / d, (-(this.x * c2.y) + (this.y * c2.x)) / d);
        }

        double realPart()
        {
            return this.x;
        }
        double realAbs()
        {
            if (this.x >= 0)
            {
                return this.x;
            }
            return this.x * (-1);
        }
        double imaginaryPart()
        {
            return this.y;
        }
        double imaginaryAbs()
        {
            if (this.x >= 0) { return this.y; }
            return this.y * (-1);
        }
    }
}
