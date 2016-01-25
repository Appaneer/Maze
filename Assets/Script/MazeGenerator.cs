using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MazeGenerator : MonoBehaviour {

	/*
	 * this script is responsible for generating maze and pathfinding
	*/

    private System.Random ran;
    public string seed;
    public bool useRandomSeed;
    public bool breakMoreWalls;
    public GameObject wall;
	public GameObject darkWall;
    public float wallLength;
    [HideInInspector]
    public int xSize;// x size of the maze
    [HideInInspector]
    public int ySize;// y size of the maze
    [HideInInspector]
    public int totalCells;//total number of cells
    [HideInInspector]
    public Node[] cells;
    [HideInInspector]
    public GameObject wallHolder;
    private Vector2 initPos; //initial position
    private int currentCell = 0;
    private int visitedCells = 0;//number of cells visited
    private bool isStartedBuilding = false;
    private int currentNeighbour;
    private List<int> lastCells;
    private int backingUp = 0;//backing up a cell. DFS
    private int wallToBreak;
    public GameObject target;
    // Use this for initialization
    void Awake()
    {
		Debug.Log (xSize + "," + ySize);
        if (PlayerPrefs.GetInt("xSize") == 0)
        {
            PlayerPrefs.SetInt("xSize", (int)((Screen.width / Screen.height) * 10) + 1);
            PlayerPrefs.SetInt("ySize", 10);
        }
        xSize = PlayerPrefs.GetInt("xSize");
        ySize = PlayerPrefs.GetInt("ySize");
		if (PlayerPrefs.GetInt ("Level") <= 1)
			breakMoreWalls = false;
		else if(PlayerPrefs.GetInt ("Level")  >= 9){
			wall = darkWall;
		}
        CreateWalls();
        Instantiate(target, (cells[totalCells - 1].north.transform.position + cells[totalCells - 1].south.transform.position) / 2, Quaternion.identity);
    }

    void CreateWalls()
    {
        ran = new System.Random(useRandomSeed ? System.DateTime.Now.GetHashCode() : seed.GetHashCode());
        totalCells = xSize * ySize;
        wallHolder = new GameObject();
        wallHolder.name = "Maze";
        wallHolder.isStatic = true;
        initPos = new Vector2((-xSize / 2) + wallLength / 2, (-ySize / 2) + wallLength / 2);
        Vector2 myPos = initPos;// my position
        GameObject tempWall;

        //for x axis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector2(initPos.x + (j * wallLength) - wallLength / 2, initPos.y + (i * wallLength) - wallLength / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        //for y axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector2(initPos.x + (j * wallLength), initPos.y + (i * wallLength) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0.0f, 0.0f, 90.0f)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        CreateCells();
    }

    void CreateCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        int children = wallHolder.transform.childCount;
        GameObject[] allWalls = new GameObject[children];
        cells = new Node[totalCells];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;
        for (int i = 0; i < children; i++)
        { // get all children
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < cells.Length; i++)
        {
            if (termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }
            cells[i] = new Node();
            cells[i].west = allWalls[eastWestProcess];
            cells[i].south = allWalls[childProcess + (xSize + 1) * ySize];

            eastWestProcess++;

            termCount++;
            childProcess++;
            cells[i].east = allWalls[eastWestProcess];
            cells[i].north = allWalls[(childProcess + (xSize + 1) * ySize) + xSize - 1];
            cells[i].x = (cells[i].north.transform.position.x + cells[i].south.transform.position.x) / 2;
            cells[i].y = (cells[i].north.transform.position.y + cells[i].south.transform.position.y) / 2;
        }
        CreateMaze();
    }

    void CreateMaze()
    {
        while (visitedCells < totalCells)
        {
            if (isStartedBuilding)
            {
                GetNeighbour();
                if (cells[currentNeighbour].isVisited == false && cells[currentCell].isVisited == true)
                {
                    BreakWall();
                    cells[currentNeighbour].isVisited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else {
                currentCell = ran.Next(0, totalCells);//randomly choosing a cell to start
                cells[currentCell].isVisited = true;
                visitedCells++;
                isStartedBuilding = true;
            }
        }

        if (breakMoreWalls)
        {
            for (int i = 1; i < ySize - 1; i++)
            {
                int rand = ran.Next(i * xSize + 1, i * xSize + xSize - 2);
                switch (i % 6)
                {
                    case 0: { Destroy(cells[rand].west); Destroy(cells[rand].east); break; }
                    case 1: { Destroy(cells[rand].east); Destroy(cells[rand].south); break; }
                    case 2: { Destroy(cells[rand].south); Destroy(cells[rand].west); break; }
                    case 3: { Destroy(cells[rand].north); Destroy(cells[rand].west); break; }
                    case 4: { Destroy(cells[rand].south); Destroy(cells[rand].north); break; }
                    case 5: { Destroy(cells[rand].north); Destroy(cells[rand].east); break; }
                }
            }
        }

    }

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1: Destroy(cells[currentCell].north); break;
            case 2: Destroy(cells[currentCell].west); break;
            case 3: Destroy(cells[currentCell].east); break;
            case 4: Destroy(cells[currentCell].south); break;
        }

    }

    void GetNeighbour()
    {
        List<int> connectingWall = new List<int>();
        List<int> neighbours = new List<int>();
        int check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;

        //west
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].isVisited == false)
            {
                neighbours.Add(currentCell - 1);
                connectingWall.Add(2);
            }
        }
        //east
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].isVisited == false)
            {
                neighbours.Add(currentCell + 1);
                connectingWall.Add(3);
            }
        }
        //north
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].isVisited == false)
            {
                neighbours.Add(currentCell + xSize);
                connectingWall.Add(1);
            }
        }

        //south
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].isVisited == false)
            {
                neighbours.Add(currentCell - xSize);
                connectingWall.Add(4);
            }
        }

        if (neighbours.Count != 0)
        {
            int randomChosenNeightbour = ran.Next(neighbours.Count);
            currentNeighbour = neighbours[randomChosenNeightbour];// randomly choosing a neighbour
            wallToBreak = connectingWall[randomChosenNeightbour];
        }
        else {
            if (backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }

    }

    List<Node> GetNeighbour(int currentCell)
    {
        List<Node> neighbours = new List<Node>();
        int check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;

        //west
        if (currentCell - 1 >= 0 && currentCell != check)
            neighbours.Add(cells[currentCell - 1]);
        //east
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
            neighbours.Add(cells[currentCell + 1]);
        //north
        if (currentCell + xSize < totalCells)
            neighbours.Add(cells[currentCell + xSize]);
        //south
        if (currentCell - xSize >= 0)
            neighbours.Add(cells[currentCell - xSize]);

        return neighbours;
    }

    public List<Node> Astar(int startPos, int targetPos) {
        for (int i = 0; i < totalCells; i++)
        {
            cells[i].number = i;
        }
        Node startNode = cells[startPos];
        Node targetNode = cells[targetPos];
		Heap<Node> openSet = new Heap<Node>(totalCells);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst ();
            closedSet.Add(currentNode);

			if (currentNode.number == targetNode.number) 
                return RetracePath(startNode, targetNode);

            foreach (Node neighbourNode in GetNeighbour(currentNode.number)){
                if (closedSet.Contains(neighbourNode))
                    continue;
                else if (neighbourNode.number - currentNode.number == xSize && (neighbourNode.south != null || currentNode.north != null)) 
                    continue;//if the neighbour is at the north of current node and the wall is broken then continue
                else if (neighbourNode.number - currentNode.number == 1 && (neighbourNode.west != null || currentNode.east != null))
                    continue;//if the neighbour is at the east of current node and the wall is broken then continue
                else if (neighbourNode.number - currentNode.number == -1 && (neighbourNode.east != null || currentNode.west != null))
                    continue;//if the neighbour is at the west of current node and the wall is broken then continue
                else if (neighbourNode.number - currentNode.number == -xSize && (neighbourNode.north != null || currentNode.south != null))
                    continue;//if the neighbour is at the east of current node and the wall is broken then continue

                int newMovementCostToNeighbour = currentNode.gCost + NodeDistance(currentNode, neighbourNode);//quite mouthful but meaningful
                if (newMovementCostToNeighbour < neighbourNode.gCost || !openSet.Contains(neighbourNode)) {
                    neighbourNode.gCost = newMovementCostToNeighbour;
                    neighbourNode.hCost = NodeDistance(neighbourNode, targetNode);
                    neighbourNode.parent = currentNode;

					if (!openSet.Contains (neighbourNode))
						openSet.Add (neighbourNode);
					else
						openSet.UpdateItem (neighbourNode);
                }

            }
        }
        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode.number != startNode.number) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
		path.Insert (0, startNode);
        return path;
    }

    public int NodeDistance(Node a, Node b)
    {
        //return Vector2.Distance(a.north.transform.position, b.north.transform.position);
        return (int)(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y));
    }
}
