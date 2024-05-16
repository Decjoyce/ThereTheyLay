using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Testcamera : MonoBehaviour
{
    [SerializeField] private RawImage _image = default;

    private WebCamTexture _webCamTexture;
    void Start()
    {
        _webCamTexture = new WebCamTexture();
        if (!_webCamTexture.isPlaying) _webCamTexture.Play();
        {
            _image.texture = _webCamTexture;
        }
    }
    
}
