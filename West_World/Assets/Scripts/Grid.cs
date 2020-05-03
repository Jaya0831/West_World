using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 三种坐标：
    1，世界坐标，以相机为原点
    2，tilemap内定义坐标（第一象限最左下角的一个格子为原点）
    3，Grid类内定义的原点：整个网格最左下角的格子为原点
 */
public class Grid : MonoBehaviour
{
    /// <summary>
    /// 每个格子的长和宽
    /// </summary>
    /// 
    public float grid_x = 1f, grid_y = 1f;
    /// <summary>
    /// 网格的长和宽
    /// </summary>
    ///
    public float x = 10f, y = 10f;
    /// <summary>
    /// 网格
    /// </summary>
    ///
    public Node[,] grid;
    /// <summary>
    /// 两个方向上的网格数
    /// </summary>
    ///
    public int x_node, y_node;
    /// <summary>
    /// 网格的左下角
    /// </summary>
    ///
    public Vector3 gridStart;

    /// <summary>
    /// 开放列表
    /// </summary>
    ///
    public List<Node> openList;

    /// <summary>
    /// 地图
    /// </summary>
    ///
    public Tilemap tilemap;
    /// <summary>
    /// 可走地形对应的贴图
    /// </summary>
    ///
    public List<Sprite> sprite_open = new List<Sprite>();
    /// <summary>
    /// 别走比分对应的贴图
    /// </summary>
    public List<Sprite> sprite_obstacle = new List<Sprite>();
    /// <summary>
    /// 地点位置信息（0到3是miner的四个目的地，4，5是厕所和floor）
    /// </summary>
    public List<Vector3>[] objectInf = new List<Vector3>[6];


    private void Awake()
    {
        gridStart = this.transform.position - new Vector3(x / 2, y / 2, 1);
        x_node = Mathf.RoundToInt(x / grid_x);
        y_node = Mathf.RoundToInt(y / grid_y);
        grid = new Node[x_node, y_node];
        CreateTheGrid();
        TransformTilemapToGrid(tilemap, -8, 7, -5, 4);
        //Debug.Log(FromPositionToVector(new Vector3(4.5f, 4.5f, 1)));
        //Debug.Log(FromPositionToVector(new Vector3(4, 4, 1)));
        //Debug.Log(FromPositionToVector(new Vector3(3.5f, 3.5f, 1)));

    }
    /// <summary>
    /// 网格化
    /// </summary>
    private void CreateTheGrid()
    {
        for (int i = 0; i < x_node; i++)
        {
            for (int j = 0; j < y_node; j++)
            {
                Vector3 nodePosition = gridStart + Vector3.right * i + Vector3.right * 0.5f + Vector3.up * j + Vector3.up * 0.5f;
                grid[i, j] = new Node(nodePosition, i, j);
            }
        }
    }
    /// <summary>
    /// 将世界位置转化为grid坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2Int FromPositionToVector(Vector3 position)
    {
        int i = (int)((position.x - gridStart.x) / grid_x);
        int j = (int)((position.y - gridStart.y) / grid_y);
        // 下面的四个if是为了确保smooth中的边界状态成立（就是刚好在边缘线上的坐标转化）如果之后要改动超出范围的处理规则，记得保证边缘状况！！！
        if (i >= x_node) i = x_node - 1;
        if (i < 0) i = 0;
        if (j >= y_node) j = y_node - 1;
        if (j < 0) j = 0;
        return new Vector2Int(i, j);
    }
    /// <summary>
    /// 将grid坐标转化为世界坐标 
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public Vector3 FromVectorToPosition(Vector2Int vector2)
    {
        float position_x = gridStart.x + grid_x * (vector2.x + 0.5f);
        float position_y = gridStart.y + grid_y * (vector2.y + 0.5f);
        return new Vector3(position_x, position_y, 1);
    }
    /// <summary>
    /// 画网格
    /// </summary>
    private void OnDrawGizmos()
    {
        //Awake函数中代码
        gridStart = this.transform.position - new Vector3(x / 2, y / 2, 1);
        x_node = Mathf.RoundToInt(x / grid_x);
        y_node = Mathf.RoundToInt(y / grid_y);
        grid = new Node[x_node, y_node];
        CreateTheGrid();
        //画网格
        Gizmos.DrawWireCube(this.transform.position, new Vector3(x, y));
        if (grid == null) return;
        Gizmos.color = new Color(1, 1, 1, 0.3f);
        for (int i = 0; i < x_node; i++)
        {
            for (int j = 0; j < y_node; j++)
            {
                Gizmos.DrawWireCube(grid[i, j].nodePosition, new Vector3(grid_x, grid_y));
            }
        }
    }
    /// <summary>
    /// A*寻路
    /// </summary>
    /// <param name="now"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public List<Vector3> FindWay(Vector3 now, Vector3 destination)
    {
        List<Vector3> path = new List<Vector3>();
        Vector2Int dest_vec2 = FromPositionToVector(destination);
        Vector2Int now_vec2 = FromPositionToVector(now);
        // 如果目的地是m_Location_Type中的一种，先把它加入openlist中，处理完后在放回closelist
        // HACK:扩展性
        if ((int)grid[dest_vec2.x, dest_vec2.y].type < 5)
        {
            grid[dest_vec2.x, dest_vec2.y].status = Node.Status.undiscovered;
        }
        if ((int)grid[now_vec2.x, now_vec2.y].type < 5)
        {
            grid[now_vec2.x, now_vec2.y].status = Node.Status.undiscovered;
        }
        // 保存node的状态信息
        Node.Status[,] grid_record = new Node.Status[x_node, y_node];
        for (int i = 0; i < x_node; i++)
        {
            for (int j = 0; j < y_node; j++)
            {
                grid_record[i, j] = grid[i, j].status;
            }
        }
        // 将起点状态设为open并放入openlist
        grid[now_vec2.x, now_vec2.y].status = Node.Status.open;
        openList.Add(grid[now_vec2.x, now_vec2.y]);
        // 目标节点在openlist中时（被发现时）
        while (grid[dest_vec2.x, dest_vec2.y].status != Node.Status.open)
        {
            // F最小的格子
            Node minnode;
            // 找到openlist中F最小的节点作为当前节点
            if (openList.Count != 0)
            {
                minnode = openList[0];
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].F < minnode.F) minnode = openList[i];
                }
                now_vec2 = new Vector2Int(minnode.nodeI, minnode.nodeJ);
            }
            else break;//当openlist为空时退出循环（此时目标节点无法抵达）
            // 将当前节点从openlist中移除，并将状态设置为close
            grid[now_vec2.x, now_vec2.y].status = Node.Status.close;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].nodeI == now_vec2.x && openList[i].nodeJ == now_vec2.y)
                {
                    openList.RemoveAt(i);
                }
            }
            // 更新当前节点周围八个方向节点的G，H，F，parent
            for (int i = now_vec2.x - 1; i <= now_vec2.x + 1; i++)
            {
                for (int j = now_vec2.y - 1; j <= now_vec2.y + 1; j++)
                {
                    // 更新周围格子的字段
                    if (i > -1 && i < x_node && j > -1 && j < y_node && grid[i, j].status != Node.Status.close && grid[i, j].status != Node.Status.obstacle)
                    {
                        int g;
                        // 斜边：14，直边：10
                        if ((int)Mathf.Abs(i - now_vec2.x) + (int)Mathf.Abs(j - now_vec2.y) == 1)
                        {
                            g = grid[now_vec2.x, now_vec2.y].G + 10;
                        }
                        else
                        {
                            // 蹩马腿
                            if (grid[i, now_vec2.y].status == Node.Status.obstacle || grid[now_vec2.x, j].status == Node.Status.obstacle)
                            {
                                continue;
                            }
                            g = grid[now_vec2.x, now_vec2.y].G + 14;
                        }
                        // 如果当前节点更新后的g比周围节点目前的g小，就更新该节点
                        if (grid[i, j].status == Node.Status.undiscovered || grid[i, j].G > g)
                        {
                            grid[i, j].G = g;
                            grid[i, j].H = Mathf.Abs(dest_vec2.x - i) + Mathf.Abs(dest_vec2.y - j);
                            grid[i, j].F = grid[i, j].G + grid[i, j].H;
                            grid[i, j].parent = grid[now_vec2.x, now_vec2.y];
                            // 如果不在openlist中，添加，并更新status
                            if (grid[i, j].status == Node.Status.undiscovered)
                            {
                                grid[i, j].status = Node.Status.open;
                                openList.Add(grid[i, j]);
                            }
                        }
                    }
                }
            }
        }
        // 如果终点可到达
        if (grid[dest_vec2.x, dest_vec2.y].status == Node.Status.open)
        {
            // 加载路线
            path = LoadPath(now, destination);
            //平滑处理
            SmoothThePath(path);
        }
        // 恢复grid信息到寻路之前
        while (openList.Count != 0) openList.RemoveAt(0);
        for (int i = 0; i < x_node; i++)
        {
            for (int j = 0; j < y_node; j++)
            {
                grid[i, j].status = grid_record[i, j];
                grid[i, j].G = 0;
                grid[i, j].F = 0;
                grid[i, j].H = 0;
                grid[i, j].parent = null;
            }
        }
        //HACK:扩展性
        if ((int)grid[dest_vec2.x, dest_vec2.y].type < 5)
        {
            grid[dest_vec2.x, dest_vec2.y].status = Node.Status.obstacle;
        }
        if ((int)grid[now_vec2.x, now_vec2.y].type < 5)
        {
            grid[now_vec2.x, now_vec2.y].status = Node.Status.obstacle;
        }
        return path;
    }
    /// <summary>
    /// 加载路线
    /// </summary>
    /// <param name="now"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    private List<Vector3> LoadPath(Vector3 now, Vector3 destination)
    {
        Vector2Int dest_vec2 = FromPositionToVector(destination);
        List<Vector3> path = new List<Vector3>();
        Node nowNode = grid[dest_vec2.x, dest_vec2.y];
        path.Add(FromVectorToPosition(dest_vec2));
        while (new Vector2Int(nowNode.nodeI, nowNode.nodeJ) != FromPositionToVector(now)) //为什么写nowNode.parent不行啊，就一直是null
        {
            path.Add(FromVectorToPosition(new Vector2Int(nowNode.parent.nodeI, nowNode.parent.nodeJ)));
            nowNode = nowNode.parent;
        }
        return path;
    }
    /// <summary>
    /// 将tile中的信息输入网格中
    /// </summary>
    /// <param name="mytilemap"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="up"></param>
    /// <param name="down"></param>
    private void TransformTilemapToGrid(Tilemap mytilemap, int left, int right, int down, int up)//最（左/右/上/下）的坐标
    {
        for (int i = 0; i < 6; i++)//HACK: 扩展性
        {
            objectInf[i] = new List<Vector3>();
        }
        for (int i = left; i <= right; i++)
        {
            for (int j = down; j <= up; j++)
            {
                if (sprite_obstacle.Exists(sp => sp.Equals(mytilemap.GetSprite(new Vector3Int(i, j, 0)))))
                {
                    grid[i - left, j - down].status = Node.Status.obstacle;
                    if (mytilemap.GetSprite(new Vector3Int(i, j, 0))==sprite_obstacle[1])
                    {
                        objectInf[(int)Node.Location_Type.Mine].Add(FromVectorToPosition(new Vector2Int(i - left, j - down)));
                        grid[i - left, j - down].type = Node.Location_Type.Mine;
                    }
                    else if (mytilemap.GetSprite(new Vector3Int(i, j, 0)) == sprite_obstacle[2])
                    {
                        objectInf[(int)Node.Location_Type.Bar].Add(FromVectorToPosition(new Vector2Int(i - left, j - down)));
                        grid[i - left, j - down].type = Node.Location_Type.Bar;
                    }
                    else if (mytilemap.GetSprite(new Vector3Int(i, j, 0)) == sprite_obstacle[3])
                    {
                        objectInf[(int)Node.Location_Type.Home].Add(FromVectorToPosition(new Vector2Int(i - left, j - down)));
                        grid[i - left, j - down].type = Node.Location_Type.Home;
                    }
                    else if (mytilemap.GetSprite(new Vector3Int(i, j, 0)) == sprite_obstacle[4])
                    {
                        objectInf[(int)Node.Location_Type.Bank].Add(FromVectorToPosition(new Vector2Int(i - left, j - down)));
                        grid[i - left, j - down].type = Node.Location_Type.Bank;
                    }
                    else if(mytilemap.GetSprite(new Vector3Int(i, j, 0)) == sprite_obstacle[5])
                    {
                        objectInf[(int)Node.Location_Type.Bathroom].Add(FromVectorToPosition(new Vector2Int(i - left, j - down)));
                        grid[i - left, j - down].type = Node.Location_Type.Bathroom;
                    }
                    else
                    {
                        grid[i - left, j - down].type = Node.Location_Type.Block;
                    }
                }
                else
                {
                    if (mytilemap.GetSprite(new Vector3Int(i, j, 0)) == sprite_open[0])
                    {
                        grid[i - left, j - down].type = Node.Location_Type.Ground;
                    }
                    else if (mytilemap.GetSprite(new Vector3Int(i, j, 0)) == sprite_open[2])
                    {
                        grid[i - left, j - down].type = Node.Location_Type.Floor;
                        objectInf[(int)Node.Location_Type.Floor].Add(FromVectorToPosition(new Vector2Int(i - left, j - down)));
                    }
                }
            }
        }
    }
    /// <summary>
    /// 平滑处理路线
    /// </summary>
    /// <param name="path"></param>
    private void SmoothThePath(List<Vector3> path)
    {
        int flag = 0;//指向平滑处理的三个点中的第一个的下标
        //确保最少有三个点并且flag后面还有至少两个点
        while (path.Count > 2 && path.Count - flag > 2)
        {
            bool smooth = true;// 是否可平滑处理这三个节点（即删除中间的节点）
            // 将世界坐标转化为网格坐标
            Vector2Int vector2Int_1 = FromPositionToVector(path[flag]);
            Vector2Int vector2Int_2 = FromPositionToVector(path[flag + 2]);
            // 确定两个节点围成的矩形的上下左右边界
            int left = vector2Int_1.x > vector2Int_2.x ? vector2Int_2.x : vector2Int_1.x;
            int right = vector2Int_1.x > vector2Int_2.x ? vector2Int_1.x : vector2Int_2.x;
            int high = vector2Int_1.y > vector2Int_2.y ? vector2Int_1.y : vector2Int_2.y;
            int low = vector2Int_1.y > vector2Int_2.y ? vector2Int_2.y : vector2Int_1.y;
            if (vector2Int_1.x == vector2Int_2.x) // k不存在的情况
            {
                for (int i = low; i < high; i++)
                {
                    // 首尾两点之间没有障碍物时，中间的点可删除
                    if (grid[left, i].status == Node.Status.obstacle) //left=right
                    {
                        smooth = false;
                        break;
                    }
                }
            }
            else// k存在
            {
                // 计算k，b_up，b_down
                float k = (path[flag].y - path[flag + 2].y) / (path[flag].x - path[flag + 2].x);
                float b_up, b_down;
                if (k >= 0)
                {
                    b_up = (path[flag].y + grid_y / 2) - k * (path[flag].x - grid_x / 2);
                    b_down = (path[flag].y - grid_y / 2) - k * (path[flag].x + grid_x / 2);
                }
                else
                {
                    b_up = (path[flag].y + grid_y / 2) - k * (path[flag].x + grid_x / 2);
                    b_down = (path[flag].y - grid_y / 2) - k * (path[flag].x - grid_x / 2);
                }
                if (Mathf.Abs(k) <= grid_y / grid_x)
                {
                    float check_x = path[flag].x;
                    do
                    {
                        // 直线与斜线交点转化为grid坐标
                        Vector2Int vector2Int_high = FromPositionToVector(new Vector3(check_x, k * check_x + b_up, 1));
                        Vector2Int vector2Int_low = FromPositionToVector(new Vector3(check_x, k * check_x + b_down, 1));
                        // 判断是否有障碍物在该列必经的格子上
                        for (int i = vector2Int_low.y; i <= vector2Int_high.y; i++)
                        {
                            //Debug.Log(vector2Int_high.x + "," + i + ":" + grid[vector2Int_high.x, i].status);
                            if (i <= high && i >= low && grid[vector2Int_high.x, i].status == Node.Status.obstacle)//保证在矩形框内
                            {
                                smooth = false;
                                break;
                            }
                        }
                        if (smooth == false) break;
                        check_x += (((path[flag].x > path[flag + 2].x) ? -1 : 1) / 2f);
                    } while ((check_x - path[flag + 2].x) / (check_x - path[flag].x) <= 0);
                }
                else
                {
                    float check_y = path[flag].y;
                    do
                    {
                        Vector2Int vector2Int_left;
                        Vector2Int vector2Int_right;
                        if (k >= 0)// 也可以算出来比较
                        {
                            vector2Int_left = FromPositionToVector(new Vector3((check_y - b_up) / k, check_y, 1));
                            vector2Int_right = FromPositionToVector(new Vector3((check_y - b_down) / k, check_y, 1));
                        }
                        else
                        {
                            vector2Int_left = FromPositionToVector(new Vector3((check_y - b_down) / k, check_y, 1));
                            vector2Int_right = FromPositionToVector(new Vector3((check_y - b_up) / k, check_y, 1));
                        }
                        for (int i = vector2Int_left.x; i <= vector2Int_right.x; i++)
                        {
                            if (i <= right && i >= left && grid[i, vector2Int_left.y].status == Node.Status.obstacle)
                            {
                                smooth = false;
                                break;
                            }
                        }
                        if (smooth == false) break;
                        check_y += (((path[flag].y > path[flag + 2].y) ? -1 : 1) / 2f);
                    } while ((check_y - path[flag + 2].y) / (check_y - path[flag].y) <= 0);
                }
            }
            if (smooth) path.RemoveAt(flag + 1);
            else flag++;//首节点向前移
        }
    }
}
