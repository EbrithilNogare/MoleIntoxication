using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerParallax : MonoBehaviour
{
    private RawImage _rawImage;
    public float speedX;

    private void Start()
    {
        _rawImage = GetComponent<RawImage>();
    }
    // Update is called once per frame
    void Update()
    {
        _rawImage.uvRect = new Rect(_rawImage.uvRect.position + new Vector2(speedX, 0) * Time.deltaTime, _rawImage.uvRect.size);
    }
}
