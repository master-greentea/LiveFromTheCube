using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SodaCanEffect : MonoBehaviour
{
    public Volume sodaEffect;
    LensDistortion ld;
    Vignette vnt;
    // Start is called before the first frame update
    void Start()
    {
        sodaEffect.profile.TryGet<LensDistortion>( out ld );
        sodaEffect.profile.TryGet<Vignette>( out vnt );

        StartCoroutine(SodaFade());
    }

    IEnumerator SodaFade()
    {
        yield return new WaitForSeconds(2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
