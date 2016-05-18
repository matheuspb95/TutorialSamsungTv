using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour {
    public Text Title;
    public Transform greenButton, redButton;
    public Texture2D cursorTexture;
    // Use this for initialization
    void Start () {
        SamsungTV.touchPadMode = SamsungTV.TouchPadMode.Mouse;  //modifica a forma como o touchpad do controle da tv vai funcionar
        SamsungTV.gestureMode = SamsungTV.GestureMode.Mouse;    //Habilita controles por gestos 

        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouse, mouse);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("StartCircle"))
                {
                    StartCoroutine(FadeTitle(1));
                    StartCoroutine(ReduceButton(1, greenButton));
                    StartCoroutine(ReduceButton(1, redButton));
                    StartCoroutine(StartMain(2.5f));
                }
                else if (hit.collider.CompareTag("ExitCircle"))
                {
                    StartCoroutine(FadeTitle(0.5f));
                    StartCoroutine(ReduceButton(0.5f, greenButton));
                    StartCoroutine(ReduceButton(0.5f, redButton));
                    StartCoroutine(ExitGame(1));
                }
            }
        }
	}

    IEnumerator StartMain(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Main");
    }

    IEnumerator ExitGame(float time)
    {
        yield return new WaitForSeconds(time);
        Application.Quit();
    }

    IEnumerator FadeTitle(float time)
    {
        Color init = Title.color;
        float t = 0;
        while(Title.color.a > 0)
        {
            yield return new WaitForFixedUpdate();
            t += Time.deltaTime / time;
            Title.color = Color.Lerp(init, Color.clear, t);
        }
    }

    IEnumerator ReduceButton(float time, Transform obj)
    {
        Vector3 init = obj.localScale;

        float t = 0;
        while(obj.localScale.x > 0)
        {
            yield return new WaitForFixedUpdate();

            t += Time.deltaTime / time;
            obj.localScale = Vector3.Lerp(init, Vector3.zero, t);
        }
    }
}
