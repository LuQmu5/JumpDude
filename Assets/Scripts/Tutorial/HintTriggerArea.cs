using UnityEngine;

public class HintTriggerArea : MonoBehaviour
{
    [SerializeField][TextArea] private string _hintText;
    [SerializeField] private TutorialActions _action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            TutorialManager.Singleton.ShowHint(_hintText, _action);
            gameObject.SetActive(false);
        }
    }
}
