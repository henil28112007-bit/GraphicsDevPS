using System;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private float[] data;
    private AudioSource musicSource;

    [SerializeField] private Material material;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        data = new float[512];
    }

    private void Update()
    {
        musicSource.GetSpectrumData(data, 0, FFTWindow.Blackman);
        material.SetFloatArray("_Frequency", data);
    }
}
