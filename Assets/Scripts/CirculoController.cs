using UnityEngine;
using System.Collections;

public class CirculoController : MonoBehaviour
{
    public GameObject explosion;
    float lerpValue;
    public float changeTime;
    public Color initialColor, finalColor;
    SpriteRenderer sprite;
    bool reducing;
    private MainSceneManager scenemanager;

    // Use this for initialization
    void Start()
    {
        scenemanager = GameObject.FindObjectOfType<MainSceneManager>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnEnable()
    {
        transform.localScale = Vector3.one;
        reducing = false;
        lerpValue = 0;
    }



    // Update is called once per frame
    void Update()
    {
        if (!reducing)
        {
            lerpValue += Time.deltaTime / changeTime;

            sprite.color = Color.Lerp(initialColor, finalColor, lerpValue);

            if (lerpValue >= 1)
            {

                explosion.Spawn(transform.position);
                //Utility.GetInstance().DestroyByTime(exp, 1);

                scenemanager.LosePoint();

                gameObject.Recycle();
            }
        }        
    }

    IEnumerator Reduce(float time)
    {
        float size = 1;
        
        while (size > 0)
        {
            yield return new WaitForFixedUpdate();

            size -= Time.deltaTime / time;
            transform.localScale = new Vector3(size, size, size);

        }
        if(size <= 0)
        {
            gameObject.Recycle();
        }
    }

    public void OnClick()
    {
        if (!reducing)
        {
            StartCoroutine(Reduce(0.4f));
            scenemanager.GainPoint();
            reducing = true;
        }
    }

    
    void OnMouseDown()
    {
        StartCoroutine(Reduce(0.4f));
        scenemanager.GainPoint();
        reducing = true;
    }

}
