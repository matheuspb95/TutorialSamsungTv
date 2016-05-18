using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
    public float time;
	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyNow());
	}
	
    IEnumerator DestroyNow()
    {
        yield return new WaitForSeconds(time);
        gameObject.Recycle();
    }
}
