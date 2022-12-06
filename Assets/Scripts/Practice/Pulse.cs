using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pulse : MonoBehaviour 
{
    public GameObject pulse;
    private float range = 0.85f;
    private Image pulseImage;
    private float transparency;

    // Set these values in inspector
    public float pulseSpeed;
    public float rangeMax;
    public float fadeRange;

    private void OnEnable() 
    {
        pulseImage = pulse.GetComponent<Image>();
        range = 0.85f;
        pulseImage.color = new Color(pulseImage.color.r, pulseImage.color.g, pulseImage.color.b, 255);
    }

    private void Update() 
    {
        range += rangeMax * (Time.deltaTime * pulseSpeed);
        if (range > rangeMax) 
        {
            range = 0.85f;
            gameObject.SetActive(false);
        }
        if (range > rangeMax - fadeRange)
        {
            transparency = Mathf.Lerp(0f, 1f, (rangeMax - range) / fadeRange);
        }
        pulse.GetComponent<RectTransform>().localScale = new Vector3(range, range, 1);
        pulseImage.color = new Color(pulseImage.color.r, pulseImage.color.g, pulseImage.color.b, transparency);
    }
}

