using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class MazeCreating : MonoBehaviour
{
    public Cell CellPrefab;
    public GameObject Floor;

    public GameObject Player;
    private GameObject player;
    Player _player;

    public GameObject PointEnd;
    public Vector3 CellSize = new Vector3(1, 1, 0);
    public Maze maze;
    public GameObject panelStop;

    [HideInInspector]
    public GameObject pointE;

    [HideInInspector]
    public bool gameOver = false;

    private GameObject map;
    private GameObject walls;
    private Animation animation;
    private float time;
    private void Awake()
    {
        MazeGenerator generator = new MazeGenerator();
        animation = GameObject.Find("stopPunel").GetComponent<Animation>();
        panelStop.SetActive(false);
        map = GameObject.Find("map");
        walls = GameObject.Find("walls");
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);
                c.transform.SetParent(walls.transform);
                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
            }
        }

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                if (x != maze.cells.GetLength(0) - 1 && y != maze.cells.GetLength(1) - 1)
                {
                    GameObject floor = Instantiate(Floor, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);
                    floor.transform.SetParent(map.transform);
                }
                if (x == maze.cells.GetLength(0) - 2 && y == maze.cells.GetLength(1) - 2)
                {
                    pointE = Instantiate(PointEnd, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);
                }
            }
        }
        map.transform.GetChild(0).GetComponent<NavMeshSurface>().BuildNavMesh();
        player = Instantiate(Player, new Vector3(0, 1, 0), Quaternion.identity);
        _player = player.GetComponent<Player>();
    }
    private void Update()
    {
        if (gameOver == true)
        {
            player = Instantiate(Player, new Vector3(0, 1, 0), Quaternion.identity);
            gameOver = false;
        }
        if (panelStop.activeInHierarchy == true) {

            time += Time.deltaTime;
            if (time >= 1f)
            {
                time = 0;
                Time.timeScale = 0;

            }
        }
    }
    public void playerDownShield()
    {
        _player.shieldAct = true;
    }
    public void playerUpShield()
    {
        _player.shieldAct = false;
        player.GetComponent<Renderer>().material.color = _player.hero.color;
        player.GetComponent<BoxCollider>().enabled = true;
        _player.shieldTime = 0;
    }
    public void Pause()
    {
        if (panelStop.activeInHierarchy == true)
        {
            panelStop.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            panelStop.SetActive(true);
            animation.Play("animationStopGame_0");
        }
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif

    }

}