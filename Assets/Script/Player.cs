using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    NavMeshAgent Agent;
    bool StartGame = true;
    MazeCreating mazeCreating;

    public GameObject redzone;
    [HideInInspector]
    public bool shieldAct = false;
    [HideInInspector]
    public float shieldTime;

    public GameObject particleDestroy;
    public GameObject fireWorks;

    private float time = 2f;
    private float timer;
    private GameObject REDZONE;
    private bool win = false;

    public Material hero;
    public Material shield;
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        mazeCreating = GameObject.Find("GameManager").GetComponent<MazeCreating>();
        StartCoroutine(Delay());

        REDZONE = GameObject.Find("REDZONE");
    }
    private void Update()
    {
        if (win == true) {
            time -= Time.deltaTime;
            if(time <= -1)
            SceneManager.LoadScene(0);
        }
        if (!StartGame)
        {
            Agent.destination = mazeCreating.pointE.transform.position;
        }
        if (win == false) {
            timer += Time.deltaTime;
            if (time <= timer)
            {
                GameObject rz = Instantiate(redzone, new Vector3(Random.Range(transform.position.x + 6f, transform.position.x + 7f),
                    0.5f, Random.Range(transform.position.x + 6f, transform.position.x + 7f)), Quaternion.identity);
                rz.transform.SetParent(REDZONE.transform);
                timer = 0;
            }
        }
        if (shieldAct)
        {
            shieldTime += Time.deltaTime;
            if (shieldTime >= 2)
            {
                gameObject.GetComponent<Renderer>().material.color = hero.color;
                gameObject.GetComponent<BoxCollider>().enabled = true;
                shieldTime = 0;
                shieldAct = false;
            }
            else
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Renderer>().material.color = shield.color;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "redzone")
        {
            for (int i = 0; i < REDZONE.transform.childCount; i++)
            {
                Destroy(REDZONE.transform.GetChild(i).gameObject);
            }
            mazeCreating.gameOver = true;
            Instantiate(particleDestroy, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.tag == "end")
        {
            win = true;
            GameObject.Find("PunelWin").GetComponent<Animation>().Play("animationStopGame_0");
            Instantiate(fireWorks, new Vector3(gameObject.transform.position.x, 4f, gameObject.transform.position.z),
        Quaternion.identity);
        }
    }
    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        StartGame = !StartGame;
    }
}