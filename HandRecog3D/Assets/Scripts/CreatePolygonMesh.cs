﻿using UnityEngine;
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

    int num = 39;
    int y_num = 4;
    private float[] xdata;
    private float[] ydata;
    private float[] zdata;

    private int f_interval = 3; // ポリゴンの輪郭の点間の距離

    public void Init()
    {
        m = new Mesh();

        // 図形の座標を格納
        xdata = new float[num];
        zdata = new float[num];
        ydata = new float[y_num];

        // 外部
        xdata[0] = 120; zdata[0] = 180;
        xdata[1] = 120; zdata[1] = 160;
        xdata[2] = 120; zdata[2] = 140;
        xdata[3] = 120; zdata[3] = 120;
        xdata[4] = 120; zdata[4] = 100;
        xdata[5] = 120; zdata[5] = 80;

        xdata[6] = 120; zdata[6] = 60;
        xdata[7] = 140; zdata[7] = 60;
        xdata[8] = 160; zdata[8] = 60;
        xdata[9] = 180; zdata[9] = 60;
        xdata[10] = 200; zdata[10] = 60;
        xdata[11] = 220; zdata[11] = 60;

        xdata[12] = 240; zdata[12] = 60;
        xdata[13] = 240; zdata[13] = 80;
        xdata[14] = 240; zdata[14] = 100;
        xdata[15] = 240; zdata[15] = 120;
        xdata[16] = 240; zdata[16] = 140;
        xdata[17] = 240; zdata[17] = 160;

        xdata[18] = 240; zdata[18] = 180;
        xdata[19] = 220; zdata[19] = 180;
        xdata[20] = 200; zdata[20] = 180;
        xdata[21] = 180; zdata[21] = 180;
        xdata[22] = 160; zdata[22] = 180;
        xdata[23] = 140; zdata[23] = 180;

        // 内部
        xdata[24] = 240; zdata[24] = 80;
        xdata[25] = 220; zdata[25] = 80;
        xdata[26] = 200; zdata[26] = 80;
        xdata[27] = 180; zdata[27] = 80;
        xdata[28] = 160; zdata[28] = 80;
        xdata[29] = 140; zdata[29] = 80;

        xdata[30] = 140; zdata[30] = 100;
        xdata[31] = 140; zdata[31] = 120;
        xdata[32] = 140; zdata[32] = 140;

        xdata[33] = 140; zdata[33] = 160;
        xdata[34] = 160; zdata[34] = 160;
        xdata[35] = 180; zdata[35] = 160;
        xdata[36] = 200; zdata[36] = 160;
        xdata[37] = 220; zdata[37] = 160;
        xdata[38] = 240; zdata[38] = 160;

        ydata[0] = 70; ydata[1] = 90; ydata[2] = 150; ydata[3] = 170;

        for (int i = 0; i < num; i++)
        {
            zdata[i] = zdata[i] - 240;
        }

        // 頂点座標の指定
        Vector3[] vertices = new Vector3[]
        {
            // 底面
            new Vector3(xdata[0], ydata[0], zdata[0]),
            new Vector3(xdata[6], ydata[0], zdata[6]),
            new Vector3(xdata[12], ydata[0], zdata[12]),
            new Vector3(xdata[18], ydata[0], zdata[18]),

            // 下から二層目
            new Vector3(xdata[24], ydata[1], zdata[24]),
            new Vector3(xdata[29], ydata[1], zdata[29]),
            new Vector3(xdata[33], ydata[1], zdata[33]),
            new Vector3(xdata[38], ydata[1], zdata[38]),

            // 上から二層目
            new Vector3(xdata[24], ydata[2], zdata[24]),
            new Vector3(xdata[29], ydata[2], zdata[29]),
            new Vector3(xdata[33], ydata[2], zdata[33]),
            new Vector3(xdata[38], ydata[2], zdata[38]),

            // 上面
            new Vector3(xdata[0], ydata[3], zdata[0]),
            new Vector3(xdata[6], ydata[3], zdata[6]),
            new Vector3(xdata[12], ydata[3], zdata[12]),
            new Vector3(xdata[18], ydata[3], zdata[18])
        };
        // 三角形ごとの頂点インデックスを指定
        int[] triangles = new int[]
        {
            // 底面
            0,1,2,
            2,3,0,
            
            // 下から二層目
            4,5,7,
            7,5,6,

            // 上から二層目
            9,8,11,
            9,11,10,

            // 上面 
            12,14,13,
            14,12,15,

            // 横（外）
            0,12,13,
            0,13,1,
            0,3,15,
            0,15,12,
            1,14,2,
            1,13,14,

            // 横（内）
            4,9,5,
            4,8,9,
            5,10,6,
            5,9,10,
            6,11,7,
            6,10,11,
            
            // 横（それ以外）
            2,4,3,
            3,4,7,
            3,7,15,
            7,11,15,
            15,11,14,
            11,8,14,
            14,8,2,
            8,4,2
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
            new Vector2(1.0f, 0.0f),
        };

        m.vertices = vertices;
        m.uv = uv;
        m.triangles = triangles;

        m.RecalculateBounds();
        m.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
    }

    // Use this for initialization
    /*
    void Start()
    {
        m = new Mesh();

        // 図形の座標を格納
        xdata = new float[num];
        zdata = new float[num];
        ydata = new float[y_num];

        // 外部
        xdata[0] = 130; zdata[0] = 170;
        xdata[1] = 130; zdata[1] = 150;
        xdata[2] = 130; zdata[2] = 130;
        xdata[3] = 130; zdata[3] = 110;
        xdata[4] = 130; zdata[4] = 90;
        xdata[5] = 130; zdata[5] = 70;
        xdata[6] = 150; zdata[6] = 70;
        xdata[7] = 170; zdata[7] = 70;
        xdata[8] = 190; zdata[8] = 70;
        xdata[9] = 210; zdata[9] = 70;
        xdata[10] = 230; zdata[10] = 70;
        xdata[11] = 230; zdata[11] = 90;
        xdata[12] = 230; zdata[12] = 110;
        xdata[13] = 230; zdata[13] = 130;
        xdata[14] = 230; zdata[14] = 150;
        xdata[15] = 230; zdata[15] = 170;
        xdata[16] = 210; zdata[16] = 170;
        xdata[17] = 190; zdata[17] = 170;
        xdata[18] = 170; zdata[18] = 170;
        xdata[19] = 150; zdata[19] = 170;

        // 内部
        xdata[20] = 150; zdata[20] = 150;
        xdata[21] = 150; zdata[21] = 130;
        xdata[22] = 150; zdata[22] = 110;
        xdata[23] = 150; zdata[23] = 90;
        xdata[24] = 150; zdata[24] = 70;

        xdata[25] = 170; zdata[25] = 150;
        xdata[26] = 190; zdata[26] = 150;
        xdata[27] = 210; zdata[27] = 150;
        xdata[28] = 210; zdata[28] = 130;
        xdata[29] = 210; zdata[29] = 110;
        xdata[30] = 210; zdata[30] = 90;
        xdata[31] = 210; zdata[31] = 70;

        ydata[0] = 70; ydata[1] = 90; ydata[2] = 150; ydata[3] = 170;

        for (int i = 0; i < num; i++)
        {
            zdata[i] = zdata[i] - 240;
        }

        // 頂点座標の指定
        Vector3[] vertices = new Vector3[]
        {
            // 底面
            new Vector3(xdata[0], ydata[0], zdata[0]),
            new Vector3(xdata[5], ydata[0], zdata[5]),
            new Vector3(xdata[10], ydata[0], zdata[10]),
            new Vector3(xdata[15], ydata[0], zdata[15]),

            // 下から二層目
            new Vector3(xdata[24], ydata[1], zdata[24]),
            new Vector3(xdata[20], ydata[1], zdata[20]),
            new Vector3(xdata[27], ydata[1], zdata[27]),
            new Vector3(xdata[31], ydata[1], zdata[31]),

            // 上から二層目
            new Vector3(xdata[24], ydata[2], zdata[24]),
            new Vector3(xdata[20], ydata[2], zdata[20]),
            new Vector3(xdata[27], ydata[2], zdata[27]),
            new Vector3(xdata[31], ydata[2], zdata[31]),

            // 上面
            new Vector3(xdata[0], ydata[3], zdata[0]),
            new Vector3(xdata[5], ydata[3], zdata[5]),
            new Vector3(xdata[10], ydata[3], zdata[10]),
            new Vector3(xdata[15], ydata[3], zdata[15])
        };
        // 三角形ごとの頂点インデックスを指定
        int[] triangles = new int[]
        {
            // 底面
            0,1,2,
            2,3,0,
            
            // 下から二層目
            4,5,7,
            7,5,6,

            // 上から二層目
            9,8,11,
            9,11,10,

            // 上面 
            12,14,13,
            14,12,15,

            // 横（外）
            0,12,13,
            0,13,1,
            0,3,15,
            0,15,12,
            3,2,14,
            3,14,15,

            // 横（内）
            4,9,5,
            4,8,9,
            5,10,6,
            5,9,10,
            6,11,7,
            6,10,11,
            
            // 横（それ以外）
            4,2,1,
            4,7,2,
            1,13,4,
            4,13,8,
            8,13,14,
            14,11,8,
            14,2,7,
            14,7,11
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
            new Vector2(1.0f, 0.0f),
        };

        m.vertices = vertices;
        m.uv = uv;
        m.triangles = triangles;

        m.RecalculateBounds();
        m.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
    }
    */

    // Update is called once per frame
    //void Update()
    //{
    /* オブジェクトの動かし方
    // メッシュ情報を保存
    t_vertices = m.vertices;
    t_triangles = m.triangles;
    t_uv = m.uv;

    // 情報書き換え
    for (int i = 0; i < t_vertices.Length; i++)
    {
      t_vertices[i].x = t_vertices[i].x - 1.0f;
    }
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
    /* ------------------------*/
    //}



    //---------------------------------------------------------
    // 関数名 : overrideXZData
    // 機能   : 物体情報との内部外部判定結果から、図形のXYデータを書き換える
    // 引数   : x/図形のXデータ y/図形のYデータ flag/観測点の内部外部値
    // 戻り値 : なし
    //---------------------------------------------------------
    public void overrideXYData(int[,,] flag, int y)
    {
        // メッシュ情報を保存
        t_vertices = m.vertices;
        t_triangles = m.triangles;
        t_uv = m.uv;

        int m_z = GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL;
        int m_x = GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL;

        for (int t_z = 0; t_z < m_z; t_z++)
        {
            for (int t_x = 0; t_x < m_x; t_x++)
            {
                if (flag[t_x, y, t_z] == 3 &&
                    t_x != 0 && t_x != m_x &&
                    t_z != 0 && t_z != m_z)
                {
                    // 3の周辺の情報を調べる
                    // 下から
                    if (flag[t_x, y, t_z - 1] == 1)
                    {
                        // 図形が端まで達しているときは移動しない
                        for (int i = 0; i < num; i++)
                        {
                            if (zdata[i] > -10)
                            {
                                return;
                            }
                        }

                        for (int i = 0; i < num; i++)
                        {
                            zdata[i] = zdata[i] + 3;
                        }
                        for (int i = 0; i < t_vertices.Length; i++)
                        {
                            t_vertices[i].z = t_vertices[i].z + 3.0f;
                        }
                    }
                    // 上
                    else if (flag[t_x, y, t_z + 1] == 1)
                    {
                        // 図形が端まで達しているときは移動しない
                        for (int i = 0; i < num; i++)
                        {
                            if (zdata[i] < - GlobalVar.CAMERA_WIDTH + 10)
                            {
                                return;
                            }
                        }

                        for (int i = 0; i < num; i++)
                        {
                            zdata[i] = zdata[i] - 3;
                        }
                        for (int i = 0; i < t_vertices.Length; i++)
                        {
                            t_vertices[i].z = t_vertices[i].z - 3.0f;
                        }
                    }
                    // 左
                    if (flag[t_x - 1, y, t_z] == 1)
                    {
                        // 図形が端まで達しているときは移動しない
                        for (int i = 0; i < num; i++)
                        {
                            if (xdata[i] > GlobalVar.CAMERA_WIDTH - 10)
                            {
                                return;
                            }
                        }

                        for (int i = 0; i < num; i++)
                        {
                            xdata[i] = xdata[i] + 3;
                        }
                        for (int i = 0; i < t_vertices.Length; i++)
                        {
                            t_vertices[i].x = t_vertices[i].x + 3.0f;
                        }
                    }
                    // 右
                    else if (flag[t_x + 1, y, t_z] == 1)
                    {
                        // 図形が端まで達しているときは移動しない
                        for (int i = 0; i < num; i++)
                        {
                            if (xdata[i] < 10)
                            {
                                return;
                            }
                        }

                        for (int i = 0; i < num; i++)
                        {
                            xdata[i] = xdata[i] - 3;
                        }
                        for (int i = 0; i < t_vertices.Length; i++)
                        {
                            t_vertices[i].x = t_vertices[i].x - 3.0f;
                        }
                    }
                }
            }
        }

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


    public int[,,] getIODMonitoringPoint(int[,,] flag)
    {
        int m_x, m_y, m_z;

        m_x = GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL;
        m_y = GlobalVar.CAMERA_HEIGHT;
        m_z = GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL;

        for (int y = GlobalVar.POINT_INTERVAL / 2; y < m_y; y=y + GlobalVar.POINT_INTERVAL)
        {
            ArrayList m_vec = new ArrayList();
            ArrayList p_vec = new ArrayList();

            if (y > ydata[0] && y < ydata[y_num - 1])
            {
                // X,Z軸の観測点を配列に格納
                insertMonitoringDataToArray(m_vec);

                // それぞれのYの際の、ポリゴンの座標情報を配列に格納
                insertPolygonDataToArray(p_vec, y);

                // 内部外部判定実行
                getIODResult(p_vec, m_vec, flag, y);
            }
        }

        return flag;
    }
    //---------------------------------------------------------
    // 関数名 : getIODResult
    // 機能   : 図形に対しての、全ての観測点における内部外部判定値を格納
    // 引数   : f_v/図形座標 m_v/観測点座標 flag/観測点の内部外部値
    // 戻り値 : flag/観測点の内部外部値
    //---------------------------------------------------------
    private int[,,] getIODResult(ArrayList p_v, ArrayList m_v, int[,,] flag, int y)
    {
        for (int i = 0; i < m_v.Count; i++)
        {
            Complex m_com = (Complex)m_v[i];

            for (int j = 0; j < p_v.Count - 1; j++)
            {
                Complex f_com1 = (Complex)p_v[j];
                Complex f_com2 = (Complex)p_v[j + 1];
                Complex sub1 = f_com1.sub(m_com);
                Complex sub2 = f_com2.sub(f_com1);

                double ch3 = Mathf.Sqrt(Mathf.Pow((float)sub1.x, 2) + Mathf.Pow((float)sub1.y, 2));

                if (ch3 < 20)
                {
                    double ch4 = sub1.x * sub2.y - sub1.y * sub2.x;
                    
                    if (ch4 >= 0)
                    {
                        //Debug.LogFormat("x:{0} y:{1} z:{2}", i % 32, (y - 5) / 10, i / 32);
                        flag[i % 32, (y - 5) / 10, i / 32] = 2;
                    }
                    else {
                        //Debug.LogFormat("x:{0} y:{1} z:{2}", i % 32, (y - 5) / 10, i / 32);
                        flag[i % 32, (y - 5) / 10, i / 32] = 1;
                    }
                    break;
                }
            }
        }

        // 計算ミスによるノイズを除去
        delArrNoise(flag, (y - 5) / 10);

        // 補完
        completionIOFlag(flag, (y - 5) / 10);


        return flag;
    }

    private int[,,] delArrNoise(int[,,] flag, int y)
    {
        int m_x = GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL;
        int m_z = GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL;

        for (int z = 0; z < m_z - 1; z++)
        {
            for (int x = 0; x < m_x - 1; x++)
            {
                if (x != 0 && y != 0)
                {
                    if (flag[x, y, z] == 2)
                    {
                        int f = 0;

                        if (flag[x - 1, y, z - 1] != 2) f++;
                        if (flag[x - 1, y, z] != 2) f++;
                        if (flag[x - 1, y, z + 1] != 2) f++;
                        if (flag[x, y, z - 1] != 2) f++;
                        if (flag[x, y, z + 1] != 2) f++;
                        if (flag[x + 1, y, z - 1] != 2) f++;
                        if (flag[x + 1, y, z] != 2) f++;
                        if (flag[x + 1, y, z + 1] != 2) f++;

                        if (f >= 7)
                        {
                            flag[x, y, z] = 1;
                        }
                    }
                }
            }
        }
        for (int z = 0; z < m_z - 1; z++)
        {
            for (int x = m_x - 1; x > 0; x--)
            {
                if (x != 0 && y != 0)
                {
                    if (flag[x, y, z] == 2)
                    {
                        int f = 0;

                        if (flag[x - 1, y, z - 1] != 2) f++;
                        if (flag[x - 1, y, z] != 2) f++;
                        if (flag[x - 1, y, z + 1] != 2) f++;
                        if (flag[x, y, z - 1] != 2) f++;
                        if (flag[x, y, z + 1] != 2) f++;
                        if (flag[x + 1, y, z - 1] != 2) f++;
                        if (flag[x + 1, y, z] != 2) f++;
                        if (flag[x + 1, y, z + 1] != 2) f++;

                        if (f >= 6)
                        {
                            flag[x, y, z] = 1;
                        }
                    }
                }
            }
        }

        return flag;
    }

    //---------------------------------------------------------
    // 関数名 : completionIOFlag
    // 機能   : 図形に対する内部判定(近)から内部判定(遠)を補完する
    // 引数   : flag/観測点の内部外部値
    // 戻り値 : flag/観測点の内部外部値
    //---------------------------------------------------------
    private int[,,] completionIOFlag(int[,,] flag, int y)
    {
        int m_x = GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL;
        int m_z = GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL;

        for (int z = 0; z < m_z; z++)
        {
            for (int x = 0; x < m_x; x++)
            {
                if (x != 0 && y != 0)
                {
                    if (flag[x, y, z] == 0 && flag[x - 1, y, z] == 2)
                    {
                        flag[x, y, z] = 2;
                    }
                }
            }
        }

        return flag;
    }


    //---------------------------------------------------------
    // 関数名 : insertMonitoringDataToArray
    // 機能   : 観測点の座標を配列に格納
    // 引数   : v/配列
    // 戻り値 : v/配列
    //---------------------------------------------------------
    private ArrayList insertMonitoringDataToArray(ArrayList v)
    {
        int m_x, m_z, t_x, t_z;
        m_x = GlobalVar.CAMERA_WIDTH / GlobalVar.POINT_INTERVAL;
        m_z = GlobalVar.CAMERA_HEIGHT / GlobalVar.POINT_INTERVAL;

        for (int z = 0; z < m_z; z++)
        {
            for (int x = 0; x < m_x; x++)
            {
                t_x = x * GlobalVar.POINT_INTERVAL + (GlobalVar.POINT_INTERVAL / 2);
                t_z = z * GlobalVar.POINT_INTERVAL + (GlobalVar.POINT_INTERVAL / 2);

                v.Add(new Complex(t_x, t_z));
            }
        }

        return v;
    }

    //---------------------------------------------------------
    // 関数名 : insertPolygonDataToArray
    // 機能   : 
    // 引数   : 
    // 戻り値 : v/配列
    //---------------------------------------------------------
    private ArrayList insertPolygonDataToArray(ArrayList v, int y)
    {
        // ydata[0] = 70; ydata[1] = 90; ydata[2] = 150; ydata[3] = 170;
        // 第一層
        if (y > ydata[0] && y < ydata[1])
        {
            for (int i = 0; i < 24; i++)
            {
                v.Add(new Complex(xdata[i], 240 + zdata[i]));
            }

            v.Add(new Complex(xdata[0], 240 + zdata[0]));
        }
        // 第二層
        else if (y > ydata[1] && y < ydata[2])
        {
            for (int i = 0; i < 13; i++)
            {
                v.Add(new Complex(xdata[i], 240 + zdata[i]));
            }

            for (int i = 24; i < 39; i++)
            {
                v.Add(new Complex(xdata[i], 240 + zdata[i]));
            }

            for (int i = 18; i < 24; i++)
            {
                v.Add(new Complex(xdata[i], 240 + zdata[i]));
            }

            v.Add(new Complex(xdata[0], 240 + zdata[0]));
        }

        // 第三層
        if (y > ydata[2] && y < ydata[3])
        {
            for (int i = 0; i < 24; i++)
            {
                // 配列に格納
                v.Add(new Complex(xdata[i], 240 + zdata[i]));
            }

            v.Add(new Complex(xdata[0], 240 + zdata[0]));
        }

        return v;
    }

    //---------------------------------------------------------
    // 関数名 : getPolygonData
    // 機能   : 
    // 引数   : 
    // 戻り値 : 
    //---------------------------------------------------------   
    public void getPolygonData(float[] x, float[]y, float[] z)
    {
        // 変数初期化
        x = new float[num];
        z = new float[num];
        y = new float[y_num];

        for (int i = 0; i < num; i++)
        {
            //x[i] = xdata[i];
            //z[i] = zdata[i];
        }

        for (int i = 0; i < y_num; i++)
        {
            //y[i] = ydata[i];
        }

        return;
    }

    //---------------------------------------------------------
    // 関数名 : isExsistPolygon
    // 機能   : 
    // 引数   : y/
    // 戻り値 : 
    //---------------------------------------------------------   
    public bool isExsistPolygon(float y)
    {
        if (ydata[0] < y && ydata[y_num - 1] > y)
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
    // 関数名 : getPolygonY
    // 機能   : 
    // 引数   : 
    // 戻り値 : 
    //---------------------------------------------------------   
    public double[] getPolygonY(double[] y_arr)
    {
        // 底面のY座標
        y_arr[0] = t_vertices[0].y;
        // 上面のY座標
        y_arr[1] = t_vertices[8].y;
        

        return y_arr;
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
    // 機能   : ポリゴンのベクトル、および点の配列情報から内部外部判定を行う
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

    /*
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