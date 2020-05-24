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

    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(47.1f);
        animation.enabled = false;
        enableObject.active = true;
    }
}
