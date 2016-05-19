using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour {
    public float spawnTime;         //tempo de spawn dos circulos
    public float spawnTimeReduction;//a cada spawn o tempo de spawn diminui por essa medida
    private bool spawn;             //verifica se circulos podem ser spawnados

    public GameObject circlePrefab; //prefab do circulo
    public float CircleTime;
    public float CircleTimeReduction;
    public int CircleToLose;        //numero de circulos que devem explodir para o GameOver
    private int CircleLosed = 0;    //numero de circulos perdidos pelo player
    private Color initialColor;     //cor inicial da tela
    public Color finalColor;        //cor final da tela

    private int Points;
    public Text ScoreText;
	// Use this for initialization
	void Start () {
        Points = 0;
        initialColor = Camera.main.backgroundColor;
        spawn = true;
        StartCoroutine(SpawnCircle()); //Spawna circulos periodiamente
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) //se clicado na tela e nao spwanando mais objetos, reinicia o level
        {
            if (!spawn)
            {
                SceneManager.LoadScene("Main");
            }else
            {
                Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouse, mouse);
                if(hit.collider != null)
                {
                    if (hit.collider.CompareTag("Circle"))
                    {
                        hit.collider.GetComponent<CirculoController>().OnClick();
                    }
                }
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("Menu");
        }
	}

    IEnumerator SpawnCircle()
    {
        while (spawn)
        {
            //Spawna o circulo numa posicao aleatoria na tela
            CircleTime -= CircleTimeReduction;
            Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 10));
            circlePrefab.Spawn(pos).GetComponent<CirculoController>().changeTime = CircleTime;

            yield return new WaitForSeconds(spawnTime); //espera o tempo entre spawns
        }
    }

    public void GainPoint() //quando o player clica em algum circulo
    {
        Points++;
        ScoreText.text = ""+Points;
        spawnTime -= spawnTimeReduction;    //Quando um circulo e clicado diminui o tempo de spawn dos circulos
        if (spawnTime < 0) spawnTime = 0;
        CircleLosed -= 2;
        if (CircleLosed < 0) CircleLosed = 0; //diminui o numero de circulos (Minimo 0)
        float t = (float)CircleLosed / (float)CircleToLose; 
        Color end = Color.Lerp(initialColor, finalColor, t);  
        StartCoroutine(ChangeColor(Camera.main.backgroundColor, end, 0.5f));
    }

    public void LosePoint() //Quando algum circulo explode
    {
        CircleLosed++;  //conta o circulo perdido
        float t = (float)CircleLosed / (float)CircleToLose;
        Color end = Color.Lerp(initialColor, finalColor, t); //muda a cor de fundo da tela
        StartCoroutine(ChangeColor(Camera.main.backgroundColor, end, 0.5f));
        if(CircleLosed >= CircleToLose) //se o limite de circulos explodidos for alcancado
        {            
            GameOver();//Inicia o GameOver
        }
    }

    public void GameOver() //Fim de jogo
    {
        circlePrefab.RecycleAll();  //Retira todos os circulos da tela
        spawn = false;              //para de spawnar circulos
        Color init = Camera.main.backgroundColor;
        StartCoroutine(ChangeColor(init, Color.black, 3));
    }

    IEnumerator ChangeColor(Color init, Color end, float time)//muda a cor de fundo da tela
    {
        float t = 0;
        while (t < 1)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime / time;
            Camera.main.backgroundColor = Color.Lerp(init, end, t);
        }
    }


}
