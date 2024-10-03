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

    [SerializeField] List<Vector2Int> path;  // 최종 경로, float 좌표로 하면 미세한 유격이 생길수도 있어서 

    private void Start()
    {
        Vector2Int st = new Vector2Int((int)startPos.position.x,(int)startPos.position.y);
        Vector2Int ed = new Vector2Int((int)endPos.position.x, (int)endPos.position.y);

       
        bool succ = Astar(st, ed, out path);
        if (succ)
        {
            Debug.Log("경로 탐색 성공");
        }
        else 
        {
            Debug.Log("경로 탐색 실패");
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
        //사전 세팅 
        List<AstarNode> openList = new List<AstarNode>(); // 탐색할 정점 후보들을 보관
        Dictionary<Vector2Int,bool> closeList = new Dictionary<Vector2Int, bool>(); // 이미 탐색한 정점 보관 , 탐색 여부와 위치만 알면 되서 굳이 list로는 안만들어도 됨 
        path = new List<Vector2Int>();  // 경로를 보관할 리스트 

        openList.Add(new AstarNode(stpos, null, 0, Heuristic(stpos,endpos)));  // 처음으로 탐색할 정점을 오픈 리스트에 추가 , H 계산용 함수 필요 

        while (openList.Count> 0)  // 더 이상 탐색할 정점이 없을 때 까지
        {
            // 다음으로 탐색할 정점 선택 F, H로 비교해서 선택 
            AstarNode nextNode = NextNode(openList);
            // 선택한 정점 기준으로 주변 탐색 
            openList.Remove(nextNode);      // 다음 탐색 정점을 후보중 제거
            closeList.Add(nextNode.pos, true);  // 탐색 완료한 정점들에 추가 

            // 다음으로 탐색할 정점이 도착지면 경로 탐생을 성공 =>path 반환하면 종료 
            if (nextNode.pos == endpos) 
            {
               AstarNode current = nextNode;
                while (current != null) 
                {
                    path.Add(current.pos);
                    current = current.parent;
                }
                path.Add(stpos);
                path.Reverse();  // 역순으로 가니까 뒤집어야 함 
                return true;
            }
            // 주변 정점(인접)의 점수를 계산  
            for (int i = 0; i < direction.Length; i++)  // 방향 반복
            {
                Vector2Int pos = nextNode.pos + direction[i];  // 점수를 넣어줌 

                // 이미 탐색한 정점이면 계산 x 
                if (closeList.ContainsKey(pos)) 
                {
                    continue;
                }
                // 못가는 곳일 때. 장애물 판별은 여러가지 방법을 사용해서 판별 하면 됨 (래이캐스트 , 피직스 오버랩, 타일맵 해스타일 등등)
                if (Physics2D.OverlapCircle(pos, 0.4f) != null)
                {
                    continue;
                }
                int g = nextNode.g + CostStraigth;  // 대각선이면 바꿔야함
                int h = Heuristic(pos, endpos);
                int f = g + h;

                // 점수 갱신이 필요 할 때
                AstarNode findNode = FindNode(openList, pos);

                if (findNode == null) // 점수가 없을 때 
                {
                    openList.Add(new AstarNode(pos, nextNode, g, h));
                }
                //f 가 더 작아질 때 
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
    public const int CostStraigth = 10;  // 직선 비용
    public const int CostDiagonal = 14;  // 대각선 비용
    public static int Heuristic(Vector2Int stpos, Vector2Int endpos) // 최상의 경로를 추정하는 순위 값(H), 이 함수에 의해 AStar 알고리즘의 성능이 결정됨 
    {
        int xSize = Mathf.Abs(stpos.x - endpos.x);
        int ySize = Mathf.Abs(stpos.y - endpos.y);

        // 맨해튼 거리 : 직선이동 (빠름, 부정확)
        //return (xSize + ySize);
        // 유클리드 : 대각선 이동 (느림, 정확)
        //return (int)Vector2Int.Distance(stpos, endpos);
        // 타일맵 거리 : 직선 + 대각선 (중간)
        int straightCount = Mathf.Abs(xSize - ySize);
        int diagonalCount = Mathf.Max(xSize - ySize)-straightCount;
        return CostStraigth * straightCount + CostDiagonal * diagonalCount;
    }
    public static AstarNode NextNode(List<AstarNode> openList) 
    {
        //F가 가장 낮거나 같다면 H가 가장 낮은 걸 선택
        int curF = int.MaxValue; // 초기값을 크게 설정해야 시작부터 안막힘
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

    private void hastileChecker()  // 타일맵을 받아와서 타일이 있는지 없는지 확인 가능, 충돌체를 확인하진 않음  
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
    private void PhysicsChecker() // 좀 더 오래걸리지만 타일맵만 체크하는게 아닌 충돌체(collider)도 체크함
    {
        for (int y = tilemap.origin.y; y < tilemap.size.y; y += (int)tilemap.cellSize.y)
        {
            for (int x = tilemap.origin.x; x < tilemap.size.x; x += (int)tilemap.cellSize.x)
            {
                Collider2D collider = Physics2D.OverlapCircle(new Vector2(x, y), 0.4f);  // 래이캐스트 처럼 레이어로 구분해서 체크 가능
                Debug.Log($"{x} : {y}  = {collider}");// 특정 지점(점)기준으로 반경안에 충돌체가 있는지 없는지 체크
            }
        }
    }
}

public class AstarNode 
{
    public Vector2Int pos; // 현재 정점의 위치
    public AstarNode parent;//이 정점을 탐색한 정점 

    public int f; // 예상 최종 거리
    public int g; // 걸린 거리
    public int h; // 예상 남은 거리   

    public AstarNode(Vector2Int pos, AstarNode parent, int g, int h) // 생성자 
    {
        this.pos = pos;
        this.parent = parent;
        this.f = g + h;
        this.g = g;
        this.h = h;
    }
}

