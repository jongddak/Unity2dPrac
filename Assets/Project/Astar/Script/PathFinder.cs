using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;


    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    [SerializeField] List<Vector2Int> path;  // ���� ���, float ��ǥ�� �ϸ� �̼��� ������ ������� �־ 

    private void Start()
    {
        Vector2Int st = new Vector2Int((int)startPos.position.x,(int)startPos.position.y);
        Vector2Int ed = new Vector2Int((int)endPos.position.x, (int)endPos.position.y);

       
        bool succ = Astar(st, ed, out path);
        if (succ)
        {
            Debug.Log("��� Ž�� ����");
        }
        else 
        {
            Debug.Log("��� Ž�� ����");
        }
          
    }
    private void Update()
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 from = new Vector3(path[i].x, path[i].y, 0);
            Vector3 to = new Vector3(path[i + 1].x, path[i + 1].y, 0);
            Debug.DrawLine(from, to);
        }
    }

    Vector2Int[] direction =
    {
        new Vector2Int(0, +1),
        new Vector2Int(0, -1),
        new Vector2Int(+1, 0),
        new Vector2Int(-1, 0),
    };

    public bool Astar(Vector2Int stpos, Vector2Int endpos, out List<Vector2Int> path)
    {
        //���� ���� 
        List<AstarNode> openList = new List<AstarNode>(); // Ž���� ���� �ĺ����� ����
        Dictionary<Vector2Int,bool> closeList = new Dictionary<Vector2Int, bool>(); // �̹� Ž���� ���� ���� , Ž�� ���ο� ��ġ�� �˸� �Ǽ� ���� list�δ� �ȸ��� �� 
        path = new List<Vector2Int>();  // ��θ� ������ ����Ʈ 

        openList.Add(new AstarNode(stpos, null, 0, Heuristic(stpos,endpos)));  // ó������ Ž���� ������ ���� ����Ʈ�� �߰� , H ���� �Լ� �ʿ� 

        while (openList.Count> 0)  // �� �̻� Ž���� ������ ���� �� ����
        {
            // �������� Ž���� ���� ���� F, H�� ���ؼ� ���� 
            AstarNode nextNode = NextNode(openList);
            // ������ ���� �������� �ֺ� Ž�� 
            openList.Remove(nextNode);      // ���� Ž�� ������ �ĺ��� ����
            closeList.Add(nextNode.pos, true);  // Ž�� �Ϸ��� �����鿡 �߰� 

            // �������� Ž���� ������ �������� ��� Ž���� ���� =>path ��ȯ�ϸ� ���� 
            if (nextNode.pos == endpos) 
            {
               AstarNode current = nextNode;
                while (current != null) 
                {
                    path.Add(current.pos);
                    current = current.parent;
                }
                path.Add(stpos);
                path.Reverse();  // �������� ���ϱ� ������� �� 
                return true;
            }
            // �ֺ� ����(����)�� ������ ���  
            for (int i = 0; i < direction.Length; i++)  // ���� �ݺ�
            {
                Vector2Int pos = nextNode.pos + direction[i];  // ������ �־��� 

                // �̹� Ž���� �����̸� ��� x 
                if (closeList.ContainsKey(pos)) 
                {
                    continue;
                }
                // ������ ���� ��. ��ֹ� �Ǻ��� �������� ����� ����ؼ� �Ǻ� �ϸ� �� (����ĳ��Ʈ , ������ ������, Ÿ�ϸ� �ؽ�Ÿ�� ���)
                if (Physics2D.OverlapCircle(pos, 0.4f) != null)
                {
                    continue;
                }
                int g = nextNode.g + CostStraigth;  // �밢���̸� �ٲ����
                int h = Heuristic(pos, endpos);
                int f = g + h;

                // ���� ������ �ʿ� �� ��
                AstarNode findNode = FindNode(openList, pos);

                if (findNode == null) // ������ ���� �� 
                {
                    openList.Add(new AstarNode(pos, nextNode, g, h));
                }
                //f �� �� �۾��� �� 
                else if (findNode.f > f) 
                {
                    findNode.f = f;
                    findNode.g = g;
                    findNode.h = h;
                    findNode.parent = nextNode;
                }
        
            }
        }
        path = null;
        return false;
    }
    public const int CostStraigth = 10;  // ���� ���
    public const int CostDiagonal = 14;  // �밢�� ���
    public static int Heuristic(Vector2Int stpos, Vector2Int endpos) // �ֻ��� ��θ� �����ϴ� ���� ��(H), �� �Լ��� ���� AStar �˰����� ������ ������ 
    {
        int xSize = Mathf.Abs(stpos.x - endpos.x);
        int ySize = Mathf.Abs(stpos.y - endpos.y);

        // ����ư �Ÿ� : �����̵� (����, ����Ȯ)
        //return (xSize + ySize);
        // ��Ŭ���� : �밢�� �̵� (����, ��Ȯ)
        //return (int)Vector2Int.Distance(stpos, endpos);
        // Ÿ�ϸ� �Ÿ� : ���� + �밢�� (�߰�)
        int straightCount = Mathf.Abs(xSize - ySize);
        int diagonalCount = Mathf.Max(xSize - ySize)-straightCount;
        return CostStraigth * straightCount + CostDiagonal * diagonalCount;
    }
    public static AstarNode NextNode(List<AstarNode> openList) 
    {
        //F�� ���� ���ų� ���ٸ� H�� ���� ���� �� ����
        int curF = int.MaxValue; // �ʱⰪ�� ũ�� �����ؾ� ���ۺ��� �ȸ���
        int CurH = int.MaxValue ;
        AstarNode minNode= null;    
        for (int i = 0; i < openList.Count; i++) 
        {
            if (curF > openList[i].f)
            {
                curF = openList[i].f;
                CurH = openList[i].h;
                minNode = openList[i];
            }
            else if (curF == openList[i].f && CurH > openList[i].h) 
            {
                curF = openList[i].f;
                CurH = openList[i].h;
                minNode= openList[i];
            }
        }
        return minNode;
    }
    public static AstarNode FindNode(List<AstarNode> openList, Vector2Int pos) 
    {
        for (int i = 0; i < openList.Count; i++) 
        {
            if (openList[i].pos == pos) 
            {
                return openList[i]; 
            }
        }
        return null;
    }

    private void hastileChecker()  // Ÿ�ϸ��� �޾ƿͼ� Ÿ���� �ִ��� ������ Ȯ�� ����, �浹ü�� Ȯ������ ����  
    {
        for (int y = tilemap.origin.y; y < tilemap.size.y; y += (int)tilemap.cellSize.y)
        {
            for (int x = tilemap.origin.x; x < tilemap.size.x; x += (int)tilemap.cellSize.x)
            {
                bool hastile = tilemap.HasTile(new Vector3Int(x, y, 0));
                Debug.Log($"{x} : {y} = {hastile}");
            }
        }
    }
    private void PhysicsChecker() // �� �� �����ɸ����� Ÿ�ϸʸ� üũ�ϴ°� �ƴ� �浹ü(collider)�� üũ��
    {
        for (int y = tilemap.origin.y; y < tilemap.size.y; y += (int)tilemap.cellSize.y)
        {
            for (int x = tilemap.origin.x; x < tilemap.size.x; x += (int)tilemap.cellSize.x)
            {
                Collider2D collider = Physics2D.OverlapCircle(new Vector2(x, y), 0.4f);  // ����ĳ��Ʈ ó�� ���̾�� �����ؼ� üũ ����
                Debug.Log($"{x} : {y}  = {collider}");// Ư�� ����(��)�������� �ݰ�ȿ� �浹ü�� �ִ��� ������ üũ
            }
        }
    }
}

public class AstarNode 
{
    public Vector2Int pos; // ���� ������ ��ġ
    public AstarNode parent;//�� ������ Ž���� ���� 

    public int f; // ���� ���� �Ÿ�
    public int g; // �ɸ� �Ÿ�
    public int h; // ���� ���� �Ÿ�   

    public AstarNode(Vector2Int pos, AstarNode parent, int g, int h) // ������ 
    {
        this.pos = pos;
        this.parent = parent;
        this.f = g + h;
        this.g = g;
        this.h = h;
    }
}

