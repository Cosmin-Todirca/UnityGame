using System.Collections.Generic;
using UnityEngine;

public class SeekerController_TC : MonoBehaviour
{
    public Vector2 seekerActualPoz = new Vector2();
    public Vector2 playerActualPoz = new Vector2();
    public Vector2 seekerOldPoz = new Vector2();
    public Vector2 playerOldPoz = new Vector2();
    private int[,] space = new int[80, 50];
    private bool playerFound = false;
    public float SeekerSpeed = 3f;
    public Rigidbody2D rigidbody;

    //eficientizare mapare
    GameObject[] BeefyBirdObjects;//marcat cu -1
    GameObject[] SeekerObjects;//marcat cu -10
    GameObject[] PlayerObjects;//marcat cu -2

    //pentru a repara faptul ca se blocheaza deasupra unei pasari.
    private bool goLeft = false;

    private void Awake()
    {
        BeefyBirdObjects = GameObject.FindGameObjectsWithTag("BeefyBird");
        SeekerObjects = GameObject.FindGameObjectsWithTag("Seeker");
        PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        space = matrixPopulation();
    }

    void Update()
    {
        space = matrixPopulation();
        if (playerFound)
        {
            playerFound = false;
            Vector2 deplasament = Lee(space, seekerActualPoz, playerActualPoz);

            if (deplasament.x != 0 || deplasament.y != 0)
                BirdMovement(deplasament * SeekerSpeed);
        }

    }

    //AI Algorithm

    struct elem
    {
        public int linii;
        public int coloane;
    }

    private Vector2 Lee(int[,] a, Vector2 seeker, Vector2 player)
    {
        int[] di = { 1, 1, -1, -1, 0, 0, 1, -1 };
        int[] dj = { 1, -1, 1, -1, 1, -1, 0, 0 };
        int n = a.GetLength(0);
        int m = a.GetLength(1);
        Queue<elem> coada = new Queue<elem>();
        coada.Enqueue(new elem() { linii = (int)playerActualPoz.x, coloane = (int)playerActualPoz.y });
        a[(int)playerActualPoz.x, (int)playerActualPoz.y] = 1;
        while (coada.Count > 0)
        {
            elem aux = coada.Dequeue();
            for (int t = 0; t <= 7; t++)
            {
                int i = aux.linii + di[t];
                int j = aux.coloane + dj[t];
                if (i >= 0 && i < n && j >= 0 && j < m)
                {
                    if (a[i, j] == -10)
                    {
                        if (-di[t] < 0)
                            goLeft = true;
                        else
                            goLeft = false;
                        return new Vector2(-di[t], -dj[t]);//noi cum mapam de la player, mergem invers pentru pasare.

                    }
                    if (a[i, j] == 0)
                    {
                        a[i, j] = a[aux.linii, aux.coloane] + 1;
                        coada.Enqueue(new elem() { linii = i, coloane = j });
                    }

                }
            }
        }
        return new Vector2(0, 0);//asta merge cu explicatia de mai sus, nu e stock
    }

    private int[,] matrixPopulation()
    {
        int[,] matrice = new int[80, 50];
        for (int i = 0; i < 80; i++)
            for (int j = 0; j < 50; j++)
                matrice[i, j] = 0;

        foreach (GameObject go in BeefyBirdObjects)
        {
            //Debug.Log((int)go.transform.position.x + " " + (int)go.transform.position.y + "BIRD");
            if ((int)go.transform.position.x > 70 && (int)go.transform.position.x < 150 &&
                (int)go.transform.position.y > 50 && (int)go.transform.position.y < 100)
            {
                matrice[(int)go.transform.position.x - 70, (int)go.transform.position.y - 50] = -1;
                //som testing here
                matrice[(int)go.transform.position.x - 69 >= 80 ? 79 : (int)go.transform.position.x - 69, (int)go.transform.position.y - 50] = -1;
                matrice[(int)go.transform.position.x - 71 < 0 ? 0 : (int)go.transform.position.x - 71, (int)go.transform.position.y - 50] = -1;
                matrice[(int)go.transform.position.x - 70, (int)go.transform.position.y - 49 >= 50 ? 49 : (int)go.transform.position.y - 49] = -1;
                matrice[(int)go.transform.position.x - 70, (int)go.transform.position.y - 51 < 0 ? 0 : (int)go.transform.position.y - 51] = -1;
            }
        }
        foreach (GameObject go in SeekerObjects)
        {
            //Debug.Log((int)go.transform.position.x + " " + (int)go.transform.position.y + "SEEKER");
            if ((int)go.transform.position.x > 70 && (int)go.transform.position.x < 150 &&
                (int)go.transform.position.y > 50 && (int)go.transform.position.y < 100)
            {
                matrice[(int)go.transform.position.x - 70, (int)go.transform.position.y - 50] = -10;
                seekerActualPoz = new Vector2((int)go.transform.position.x - 70, (int)go.transform.position.y - 50);
                seekerOldPoz = new Vector2((int)go.transform.position.x - 70, (int)go.transform.position.y - 50);
            }
        }
        foreach (GameObject go in PlayerObjects)
        {
            //noi suprascriem pozitia jucatorului ca sa il gasim mereu
            //Debug.Log((int)go.transform.position.x + " " + (int)go.transform.position.y +"PLAYER");
            if ((int)go.transform.position.x > 70 && (int)go.transform.position.x < 150 &&
                (int)go.transform.position.y > 50 && (int)go.transform.position.y < 100)
            {
                playerActualPoz = new Vector2((int)go.transform.position.x - 70, (int)go.transform.position.y - 50);
                playerOldPoz = new Vector2((int)go.transform.position.x - 70, (int)go.transform.position.y - 50);
                matrice[(int)go.transform.position.x - 70, (int)go.transform.position.y - 50] = -2;
                playerFound = true;
            }
        }
        return matrice;
    }

    private void BirdMovement(Vector2 direction)
    {
        //rigidbody.velocity = new Vector2(direction.x,direction.y);
        rigidbody.AddForce(direction * Time.deltaTime * 1000f);
        //capping out the speed
        if (rigidbody.velocity.y > 2.5f)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 2.1f);
        if (rigidbody.velocity.y < -2.5f)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, -2.1f);
        if (rigidbody.velocity.x > 2.5f)
            rigidbody.velocity = new Vector2(2.1f, rigidbody.velocity.y);
        if (rigidbody.velocity.x < -2.5f)
            rigidbody.velocity = new Vector2(-2.1f, rigidbody.velocity.y);
        //Debug.Log("Deplasamentul este asa: "+direction.x+" "+direction.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //pentru a nu se mai bloca pe pasare, ii dau un mic ajutor.
        if (collision.gameObject.CompareTag("BeefyBird"))
        {
            float xDir = 1f;
            if (goLeft)
                xDir *= -1f;
            Vector2 directie = new Vector2(xDir, 0);
            if (this.transform.position.y > collision.transform.position.y)
            {
                directie.y = 1f;
                //Debug.Log("Am intrat in coliziune cu o pasare care e sub");
            }
            else
            {
                directie.y = -1f;
                //Debug.Log("Am intrat in coliziune cu o pasare care e deasupra");
            }
            directie *= Time.deltaTime * 10000f;
            rigidbody.AddForce(directie);
        }
    }

}
