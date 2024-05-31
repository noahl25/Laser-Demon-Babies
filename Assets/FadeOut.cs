using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image overlay;
    // Start is called before the first frame update
    void Start()
    {
        overlay = GetComponent<Image>();
        StartCoroutine("Fade");
    }

    IEnumerator Fade() {
        yield return new WaitForSeconds(2f);
        while (overlay.color.a >= 0) {
            overlay.CrossFadeAlpha(0, 3f, true);
            yield return null;
        }
    }
    
}
