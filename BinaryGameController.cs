using System;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class BinaryGameController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI feedbackText;   
    public TextMeshProUGUI hintText;       

    [Header("Managers")]
    public TapeManager tapeManager;        

    [Header("Level Settings")]
    public int currentLevel = 1;           
    private string currentBinary;          
    private string correctAnswer;          

    void Start()
    {
        GenerateLevel(currentLevel);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
            CheckAnswer();

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Y))
            ShowSolution();
    }

    void GenerateLevel(int level)
    {
        int length = (level == 1) ? Random.Range(4, 6) : Random.Range(6, 9);
        do
        {
            currentBinary = GenerateRandomBinary(length);
        } while (!IsValidBinary(currentBinary));

        correctAnswer = AddTwoToBinary(currentBinary);

        hintText.text = $"<b><color=#E5E5E5>Binary:</color></b> <color=#FFD700>{currentBinary}</color>";
        feedbackText.text = "";

        tapeManager.GenerateTape(currentBinary);

        var head = FindFirstObjectByType<TuringHead>();
        if (head != null)
            head.ResetToRightmost();
    }


    bool IsValidBinary(string bin)
    {
        int num = System.Convert.ToInt32(bin, 2);
        int result = num + 2;
        string resultBin = System.Convert.ToString(result, 2);

        return resultBin.Length == bin.Length;
    }

    string GenerateRandomBinary(int length)
    {
        string bin = "";
        for (int i = 0; i < length; i++)
            bin += Random.value > 0.5f ? "1" : "0";
        return bin.Trim();
    }

    string AddTwoToBinary(string bin)
    {
        int num = System.Convert.ToInt32(bin, 2);
        num += 2;
        return System.Convert.ToString(num, 2);
    }

    void CheckAnswer()
    {
        string playerAnswer = GetPlayerAnswer();
        Debug.Log($"Player Answer: {playerAnswer}, Correct: {correctAnswer}");

        if (Convert.ToInt32(playerAnswer, 2) == Convert.ToInt32(correctAnswer, 2))

        {
            feedbackText.text =
                $"<b><color=#00FF99>✔ Correct!</color></b>\n<size=24>Binary: <color=#FFD700>{currentBinary}</color>\nAnswer: <color=#00FF99>{playerAnswer}</color></size>";
            Invoke(nameof(NextLevel), 2f);
        }
        else
        {
            feedbackText.text =
                $"<b><color=#FF4444>✘ Wrong!</color></b>\n<size=24>Your: <color=#FFAAAA>{playerAnswer}</color>\nCorrect: <color=#FFD700>{correctAnswer}</color>\nBinary: <color=#AAAAFF>{currentBinary}</color></size>";
        }
    }

    void ShowSolution()
    {
        feedbackText.text = $"<b><color=#FFD700>🧠 Solution:</color></b>\n<size=24>{correctAnswer}</size>";
    }

    string GetPlayerAnswer()
    {
        if (tapeManager == null || tapeManager.tapeCells == null)
            return "";

        string answer = "";
        foreach (var cell in tapeManager.tapeCells)
            answer += cell.GetValue().ToString();

        return answer.Trim();
    }

    void NextLevel()
    {
        currentLevel++;
        if (currentLevel > 2)
        {
            feedbackText.text = "<b><color=#00FFFF>🎉 All levels complete!</color></b>\n<size=22>Great work, machine whisperer.</size>";
        }
        else
        {
            GenerateLevel(currentLevel);
        }
    }

}
