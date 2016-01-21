using UnityEngine;
using System.Collections;

public class CreatePolygonMesh : MonoBehaviour
{
    // 変数宣言
    private Mesh m;
    private float ori_x, ori_z;
    private float bottom;
    private float inner;
    private float outer;
    private float top;
    private Vector3[] t_vertices;
    private int[] t_triangles;
    private Vector2[] t_uv;

    private int f_interval = 3; // ポリゴンの輪郭の点間の距離

    // Use this for initialization
    void Start()
    {
        m = new Mesh();

        // 初期位置代入
        ori_x = 160.0f;
        ori_z = -120.0f;
        bottom = 70.0f;
        inner  = 30.0f;
        outer  = 50.0f;
        top  = bottom + 30.0f;

        // 頂点座標の指定
        Vector3[] vertices = new Vector3[]
        {
            // 底面外
            new Vector3(ori_x-outer, bottom, ori_z+outer),
            new Vector3(ori_x-outer, bottom, ori_z-outer),
            new Vector3(ori_x+outer, bottom, ori_z-outer),
            new Vector3(ori_x+outer, bottom, ori_z+outer),
            // 底面内
            new Vector3(ori_x-inner, bottom, ori_z+inner),
            new Vector3(ori_x-inner, bottom, ori_z-inner),
            new Vector3(ori_x+inner, bottom, ori_z-inner),
            new Vector3(ori_x+inner, bottom, ori_z+inner),
            // 上面外
            new Vector3(ori_x-outer, top, ori_z+outer),
            new Vector3(ori_x-outer, top, ori_z-outer),
            new Vector3(ori_x+outer, top, ori_z-outer),
            new Vector3(ori_x+outer, top, ori_z+outer),
            // 上面内
            new Vector3(ori_x-inner, top, ori_z+inner),
            new Vector3(ori_x-inner, top, ori_z-inner),
            new Vector3(ori_x+inner, top, ori_z-inner),
            new Vector3(ori_x+inner, top, ori_z+inner),
        };
        // 三角形ごとの頂点インデックスを指定
        int[] triangles = new int[]
        {
            // 底面
            0,1,4,
            4,1,5,
            5,1,2,
            2,6,5,
            2,3,6,
            6,3,7,
            7,3,0,
            0,4,7,
            // 横面外
            0,8,9,
            9,1,0,
            1,9,10,
            10,2,1,
            2,10,11,
            11,3,2,
            3,11,8,
            8,0,3,
            // 横面内
            4,5,13,
            13,12,4,
            5,6,14,
            14,13,5,
            6,7,15,
            15,14,6,
            7,4,12,
            12,15,7,
            // 上面
            8,12,9,
            9,12,13,
            9,13,10,
            10,13,14,
            10,14,11,
            11,14,15,
            11,15,8,
            8,15,12
        };
        // UVの指定 (頂点数と同じ数を指定すること)
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        };

        m.vertices = vertices;
        m.uv = uv;
        m.triangles = triangles;

        m.RecalculateBounds();
        m.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";

        //m.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        // メッシュ情報を保存
        t_vertices = m.vertices;
        t_triangles = m.triangles;
        t_uv = m.uv;

        // 情報書き換え
        //for (int i = 0; i < t_vertices.Length; i++)
        //{
        //  t_vertices[i].x = t_vertices[i].x - 1.0f;
        //}
        //Debug.Log(t_vertices.Length);

        // メッシュ情報クリア
        m.Clear();

        // 変更した情報をコピー
        m.vertices = t_vertices;
        m.triangles = t_triangles;
        m.uv = t_uv;

        // 表示
        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
    }

    //---------------------------------------------------------
    // 関数名 : getPolygonLayerInfo
    // 機能   : 
    // 引数   : y/
    // 戻り値 : 
    //---------------------------------------------------------   
    public bool isExsistPolygon(float y)
    {
        if (bottom < y && top > y)
        {
            return true;
        }

        return false;
    }

    //---------------------------------------------------------
    // 関数名 : getPolygonXZ
    // 機能   : 
    // 引数   : xz_arr/ y/
    // 戻り値 : 
    //---------------------------------------------------------   
    public double[,] getPolygonXZ(double[,] xz_arr)
    {
        for (int i = 0; i < t_vertices.Length / 2; i++)
        {
            xz_arr[i, 0] = t_vertices[i].x;
            xz_arr[i, 1] = t_vertices[i].z; 
        }

        return xz_arr;
    }

    //---------------------------------------------------------
    // 関数名 : getPolygonVector
    // 機能   : ポリゴンをレイヤーとして見た時の、ベクトル取得
    // 引数   : 
    // 戻り値 : 
    //---------------------------------------------------------  
    public ArrayList getPolygonVector(ArrayList vec, double[,] pl_arr)
    {
        for (int i = 0; i < GlobalVar.VERTICE_NUM / 2 - 1; i++)
        {
            // 二点間の距離を取得
            int d = (int)Mathf.Sqrt(Mathf.Pow((float)pl_arr[i + 1, 0] - (float)pl_arr[i, 0], 2)
                                  + Mathf.Pow((float)pl_arr[i + 1, 1] - (float)pl_arr[i, 1], 2));
            // 座標生成
            for (int j = 0; j < d / f_interval; j++)
            {
                double gx = f_interval * j * (pl_arr[i + 1, 0] - pl_arr[i, 0]) / d + pl_arr[i, 0];
                double gz = f_interval * j * (pl_arr[i + 1, 1] - pl_arr[i, 1]) / d + pl_arr[i, 1];

                // 配列に格納
                vec.Add(new Complex(gx, gz));
            }
        }

        return vec;
    }

    //---------------------------------------------------------
    // 関数名 : isIEDecision
    // 機能   : ポリゴンのベクトル、および配列情報から内部外部判定を行う
    // 引数   : 
    // 戻り値 : 
    //---------------------------------------------------------
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

    //---------------------------------------------------------
    // 関数名 : getIEDecisionArr
    // 機能   : ポリゴンのベクトル、および配列情報から内部外部判定を行う
    // 引数   : 
    // 戻り値 : 
    //---------------------------------------------------------
    public int[,,] getIEDecisionArr(ArrayList vec, int[,,] xyz_arr, int y)
    {

        return xyz_arr;
    }

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

    //void print()
    //{
    //    System.out.println(this.x + "+" + this.y + "*i");
    //}

    /*boolean caushy(Complex c1,Complex c2){
	}*/
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