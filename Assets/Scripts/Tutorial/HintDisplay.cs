using TMPro;
using UnityEngine;

public class HintDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Show(string message)
    {
        _canvasGroup.alpha = 1;
        _text.text = message;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
    }
}
