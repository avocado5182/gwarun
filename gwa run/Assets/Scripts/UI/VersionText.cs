using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        GetComponent<TMP_Text>().text = $"v{Application.version}";
    }
}
