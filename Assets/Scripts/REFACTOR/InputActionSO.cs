using UnityEngine;

[CreateAssetMenu(menuName = "Input/Action")]
public class InputActionSO : ScriptableObject
{
    public string actionName;
    public KeyCode pcKey;
    public Sprite mobileIcon;
    public string mobileDescription;
}
