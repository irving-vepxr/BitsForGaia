using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]

public class AparatController : MonoBehaviour
{
    public GameObject On;
    public PlayerScore player;
    Vector3 inactiveSize;

    /// <summary>The material to use when this object is active (gazed at).</summary>
    Vector3 activeSize;

    private Vector3 startingPosition;
    private Vector3 mySize;
    bool load = false;

    /// <summary>Sets this instance's GazedAt state.</summary>
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
    public void SetGazedAt(bool gazedAt)
    {
        load = gazedAt;
        if (inactiveSize != null && activeSize != null)
        {
            mySize = gazedAt ? activeSize : inactiveSize;
            this.gameObject.transform.localScale = mySize;
            StartCoroutine(AparatOn());
            return;
        }
    }

    public int emisiones,comodidad,empatia;
    bool inCorrutine = false;
    IEnumerator AparatOn()
    {
        if (!inCorrutine)
        {
            inCorrutine = true;
            int i = 3;
            while (load && i > 0)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("i" + i);
                i--;
            }
            if (load)
            {
                player.emisiones += emisiones;
                player.comodidad += comodidad;
                player.empatia += empatia;
                On.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else
            {
                inCorrutine = false;
            }
        }
    }

    /// <summary>Resets this instance and its siblings to their starting positions.</summary>
    public void Reset()
    {
        int sibIdx = transform.GetSiblingIndex();
        int numSibs = transform.parent.childCount;
        for (int i = 0; i < numSibs; i++)
        {
            GameObject sib = transform.parent.GetChild(i).gameObject;
            sib.transform.localPosition = startingPosition;
            sib.SetActive(i == sibIdx);
        }
    }

    /// <summary>Calls the Recenter event.</summary>
    public void Recenter()
    {
#if !UNITY_EDITOR
            GvrCardboardHelpers.Recenter();
#else
        if (GvrEditorEmulator.Instance != null)
        {
            GvrEditorEmulator.Instance.Recenter();
        }
#endif  // !UNITY_EDITOR
    }

    /// <summary>Teleport this instance randomly when triggered by a pointer click.</summary>
    /// <param name="eventData">The pointer click event which triggered this call.</param>
    public void TeleportRandomly(BaseEventData eventData)
    {
        // Only trigger on left input button, which maps to
        // Daydream controller TouchPadButton and Trigger buttons.
        PointerEventData ped = eventData as PointerEventData;
        if (ped != null)
        {
            if (ped.button != PointerEventData.InputButton.Left)
            {
                return;
            }
        }

        // Pick a random sibling, move them somewhere random, activate them,
        // deactivate ourself.
        int sibIdx = transform.GetSiblingIndex();
        int numSibs = transform.parent.childCount;
        sibIdx = (sibIdx + Random.Range(1, numSibs)) % numSibs;
        GameObject randomSib = transform.parent.GetChild(sibIdx).gameObject;

        // Move to random new location ±90˚ horzontal.
        Vector3 direction = Quaternion.Euler(
            0,
            Random.Range(-90, 90),
            0) * Vector3.forward;

        // New location between 1.5m and 3.5m.
        float distance = (2 * Random.value) + 1.5f;
        Vector3 newPos = direction * distance;

        // Limit vertical position to be fully in the room.
        newPos.y = Mathf.Clamp(newPos.y, -1.2f, 4f);
        randomSib.transform.localPosition = newPos;

        randomSib.SetActive(true);
        gameObject.SetActive(false);
        SetGazedAt(false);
    }

    private void Start()
    {
        startingPosition = transform.localPosition;
        mySize = transform.localScale;
        inactiveSize = mySize;
        activeSize = new Vector3(inactiveSize.x * 2, inactiveSize.y * 2, inactiveSize.z * 2);
        SetGazedAt(false);
    }
}
