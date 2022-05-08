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

    [SerializeField] private float sodaDuration;
    public float sodaTimer;
    // Start is called before the first frame update
    void Start()
    {
        sodaEffect.profile.TryGet<LensDistortion>( out ld );
        sodaEffect.profile.TryGet<Vignette>( out vnt );

        sodaTimer = sodaDuration + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (sodaTimer < sodaDuration)
        {
            ld.intensity.value = Mathf.Lerp(-0.628f, 0f, sodaTimer / sodaDuration);
            vnt.intensity.value = Mathf.Lerp(1, 0f, sodaTimer / sodaDuration);
            
            sodaTimer += Time.deltaTime;
        }
    }
}
