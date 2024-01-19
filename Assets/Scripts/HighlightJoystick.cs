using UnityEngine;

public class HighlightJoystick : MonoBehaviour
{
    [SerializeField]
    GameObject normal, highlight;

    public void Switch()
    {
        normal.SetActive(!normal.activeSelf);
        highlight.SetActive(!highlight.activeSelf);
    }
}
