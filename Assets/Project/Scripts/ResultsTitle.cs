using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ResultsTitle : MonoBehaviour
{
    private void OnEnable()
    {
        TMP_Text title = GetComponent<TMP_Text>();

        if (!RunResult.Won) title.text = "GAME OVER";
        else title.text = GameState.IsTimeAttack ? "TIME UP" : "LEVEL CLEAR";
    }
}