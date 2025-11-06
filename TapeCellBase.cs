using UnityEngine;
using TMPro;

public class TapeCellBase : MonoBehaviour
{
    public int value = 0;          
    public TextMeshPro text;       
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateVisual();
    }

    void OnMouseDown()
    {
        value = 1 - value;
        UpdateVisual();
    }

    public void SetValue(int val)
    {
        value = val;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (!rend) rend = GetComponent<Renderer>();

        if (value == 1)
            rend.material.color = new Color(0.6f, 0.3f, 0.9f); 
        else
            rend.material.color = new Color(0.6f, 0.9f, 0.9f); 

        if (text)
            text.text = value.ToString();
    }

    public int GetValue() => value;
}