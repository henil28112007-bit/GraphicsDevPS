using UnityEngine;
using System.Collections.Generic;

public class TapeManager : MonoBehaviour
{
    [Header("Tape Setup")]
    public GameObject tapeCellPrefab;
    public float cellSpacing = 1.1f;

    [HideInInspector]
    public List<TapeCellBase> tapeCells = new List<TapeCellBase>();


    public void ClearTape()
    {
        List<GameObject> oldCells = new List<GameObject>();
        foreach (Transform child in transform)
            oldCells.Add(child.gameObject);

        foreach (GameObject obj in oldCells)
        {
            if (obj != null)
            {
                if (Application.isPlaying)
                    DestroyImmediate(obj);
                else
                    DestroyImmediate(obj);
            }
        }

        tapeCells.Clear();
    }
    public void GenerateTape(string binary)
    {
        ClearTape();

        binary = binary.Trim();

        for (int i = 0; i < binary.Length; i++)
        {
            Vector3 pos = new Vector3(i * cellSpacing, 0, 0);
            GameObject cellObj = Instantiate(tapeCellPrefab, pos, Quaternion.identity, transform);

            TapeCellBase cell = cellObj.GetComponent<TapeCellBase>();
            int bit = binary[i] == '1' ? 1 : 0;
            cell.SetValue(bit);
            tapeCells.Add(cell);
        }

        CenterTape(binary.Length);
    }

    private void CenterTape(int length)
    {
        float totalWidth = (length - 1) * cellSpacing;
        transform.localPosition = new Vector3(-totalWidth / 2f, 0, 0);
    }
}