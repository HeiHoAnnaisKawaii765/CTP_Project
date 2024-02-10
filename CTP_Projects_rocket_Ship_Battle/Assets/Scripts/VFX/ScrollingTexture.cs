using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    [SerializeField]
    float scrollSpeedX, scrollSpeedY;
    MeshRenderer meshRenderer;
    float currentSpeedX, currentSpeedY,riseRateX = 0.008f, riseRateY = 0.008f;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup*currentSpeedX, Time.realtimeSinceStartup * currentSpeedY);
        currentSpeedX += riseRateX*Time.deltaTime;
        currentSpeedY += riseRateY * Time.deltaTime;
        if (currentSpeedX>scrollSpeedX)
        {
            riseRateX *= -1;
        }
        if (currentSpeedX < -scrollSpeedX)
        {
            riseRateX *= -1;
        }
        if (currentSpeedY > scrollSpeedY)
        {
            riseRateY *= -1;
        }
        if (currentSpeedY < -scrollSpeedY)
        {
            riseRateY *= -1;
        }
    }
}
