using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathFinishPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _deathFinishText;
    [SerializeField] private TextMeshProUGUI _loopValueText;
    [SerializeField] private TextMeshProUGUI _timeValueText;
    [SerializeField] private TextMeshProUGUI _killValueText;

    public void ShowPanel(bool death)
    {
        gameObject.SetActive(true);
        _deathFinishText.text = "YOU " + (death ? "DIED" : "FINISHED");
        _loopValueText.text = GameManager.instance.loopCount.ToString();
        _timeValueText.text = TimeFormatter(GameManager.instance.completionTime);
        _killValueText.text = GameManager.instance.killCount.ToString();
    }

    private string TimeFormatter(float time)
    {
        var intTime = (int)time;
        var minutes = intTime / 60;
        var seconds = intTime % 60;
        var fraction = time * 1000;
        fraction -= 0.01f;
        fraction = fraction % 1000;
        return $"{minutes:00} : {seconds:00} : {fraction / 10:00}";
    }
}
