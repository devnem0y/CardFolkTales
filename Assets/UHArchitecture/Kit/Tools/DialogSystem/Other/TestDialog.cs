using UnityEngine;

public class TestDialog : MonoBehaviour
{
    [SerializeField] private DialogData _dialogTest;
    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            new Dialog(_dialogTest, () =>
            {
                Debug.Log("Dialog finish");
            });
        }
    }
}
