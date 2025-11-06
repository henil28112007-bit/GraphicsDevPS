using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]
public class TuringHead : MonoBehaviour
{
    [Header("References")]
    public TapeManager tapeManager;             
    public Camera mainCamera;                   

    [Header("Head Movement")]
    public int currentCellIndex = 0;            
    public float moveSpeed = 5f;                
    public float headHeight = 1f;               

    [Header("Camera Follow")]
    public bool cameraFollow = true;            
    [Range(0.01f, 1f)]
    public float cameraLerp = 0.12f;            

    private bool isMoving = false;

    void Start()
    {
        if (tapeManager == null)
        {
            Debug.LogError("TuringHead: tapeManager is not assigned! Drag the TapeManager object into the inspector.");
            return;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (tapeManager.tapeCells != null && tapeManager.tapeCells.Count > 0)
        {
            currentCellIndex = tapeManager.tapeCells.Count - 1;
            Vector3 startPos = tapeManager.tapeCells[currentCellIndex].transform.position + new Vector3(0f, headHeight, 0f);
            transform.position = startPos;
        }
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                MoveHead(1);

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                MoveHead(-1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            WriteCell(0);

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            WriteCell(1);

        if (Input.GetKeyDown(KeyCode.R))
            ReadCell();
    }

    public void MoveHead(int direction)
    {
        if (tapeManager == null || tapeManager.tapeCells == null) return;

        int newIndex = currentCellIndex + direction;
        if (newIndex < 0 || newIndex >= tapeManager.tapeCells.Count)
            return; 

        currentCellIndex = newIndex;

        Vector3 targetPos = tapeManager.tapeCells[currentCellIndex].transform.position + new Vector3(0f, headHeight, 0f);
        StartCoroutine(SmoothMove(targetPos));
    }

    public void ResetToRightmost()
    {
        if (tapeManager != null && tapeManager.tapeCells.Count > 0)
        {
            currentCellIndex = tapeManager.tapeCells.Count - 1;
            Vector3 newPos = tapeManager.tapeCells[currentCellIndex].transform.position + new Vector3(0, 1f, 0);
            transform.position = newPos;

            // also reset camera to this position
            if (mainCamera != null)
            {
                mainCamera.transform.position = new Vector3(newPos.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
        }
    }


    IEnumerator SmoothMove(Vector3 targetPos)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float t = 0f;
        float speed = Mathf.Max(0.01f, moveSpeed); 

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, t));

            if (cameraFollow && mainCamera != null)
            {
                Vector3 camTarget = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camTarget, cameraLerp);
            }

            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    public void ReadCell()
    {
        if (tapeManager == null || tapeManager.tapeCells == null) return;
        var cell = tapeManager.tapeCells[currentCellIndex];
        int value = cell != null ? cell.GetValue() : 0;
        Debug.Log($"TuringHead: Read value at index {currentCellIndex}: {value}");
    }

    public void WriteCell(int newValue)
    {
        if (tapeManager == null || tapeManager.tapeCells == null) return;
        var cell = tapeManager.tapeCells[currentCellIndex];
        if (cell == null) return;
        cell.SetValue(Mathf.Clamp(newValue, 0, 1));
        Debug.Log($"TuringHead: Wrote value {newValue} at index {currentCellIndex}");
    }

    public void SetHeadIndex(int index, bool animate = true)
    {
        if (tapeManager == null || tapeManager.tapeCells == null) return;
        index = Mathf.Clamp(index, 0, tapeManager.tapeCells.Count - 1);
        currentCellIndex = index;
        Vector3 targetPos = tapeManager.tapeCells[index].transform.position + new Vector3(0f, headHeight, 0f);

        if (animate) StartCoroutine(SmoothMove(targetPos));
        else transform.position = targetPos;
    }
}
