using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 startHeight;
    private Vector3 targetHeight;
    private CatchPlayer sus;

    public GameObject susManager;
    public float timeBetweenPoints = .5f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        sus = susManager.GetComponent<CatchPlayer>();
        startHeight = new Vector3 (0,0,0) + new Vector3 (0,transform.position.y,0);
        targetHeight = startHeight + new Vector3(0,15,0);
    }

    // Update is called once per frame
    void Update()
    {
        var currentPosition = transform.position;
        var lerpInterpo = (float)sus.suspicionCount * .01f;
        var totalHeightToMove = Vector3.Lerp(startHeight, targetHeight, lerpInterpo);
        var newHeightToMove = new Vector3(transform.position.x, totalHeightToMove.y, transform.position.z);

        Debug.Log(lerpInterpo);

        transform.position = Vector3.SmoothDamp(currentPosition, newHeightToMove, ref velocity, timeBetweenPoints);
    }
}
