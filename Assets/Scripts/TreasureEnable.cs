using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureEnable : MonoBehaviour
{
    public GameObject enableObject;
    Animator animation;
    // Start is called before the first frame update
    void Start()
    {
        animation = this.gameObject.GetComponent<Animator>();
        StartCoroutine(WakeUp());
    }

    public float timeEnable = 47.1F;
    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(timeEnable);
        animation.enabled = false;
        enableObject.active = true;
    }
}
