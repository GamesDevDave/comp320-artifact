using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIValues : MonoBehaviour
{

    private Label _coyoteTimeLabel;
    private Label _jumpBufferLabel;
    private Label _sceneNameLabel;



    [SerializeField]
    private PlayerMovement _movementScript;

    private void OnEnable()
    {
        _movementScript = _movementScript.GetComponent<PlayerMovement>();

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        _coyoteTimeLabel = rootVisualElement.Q<Label>("CoyoteTimeAmount");
        _jumpBufferLabel = rootVisualElement.Q<Label>("JumpBufferAmount");
        _sceneNameLabel = rootVisualElement.Q<Label>("SceneNameValue");

        SetValues();
    }

    private void SetValues()
    {
        _coyoteTimeLabel.text = _movementScript.HangTime.ToString();
        _jumpBufferLabel.text = _movementScript.JumpBufferLength.ToString();
        _sceneNameLabel.text = SceneManager.GetActiveScene().name;
    }
}