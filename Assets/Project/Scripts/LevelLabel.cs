using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LevelLabel : MonoBehaviour
{
    private void OnEnable()
    {
        TMP_Text label = GetComponent<TMP_Text>();

        if (GameState.IsClassic)
            label.text = "LEVEL " + GameState.SelectedLevel + " - CLASSIC";
        else
            label.text = GameState.SelectedMode;
    }
}