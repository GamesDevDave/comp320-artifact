using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIValuesNew : MonoBehaviour
{

    private Label _sceneNameLabel;


    [SerializeField]
    private PlayerMovement _movementScript;

    private void OnEnable()
    {
        _movementScript = _movementScript.GetComponent<PlayerMovement>();

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        _sceneNameLabel = rootVisualElement.Q<Label>("SceneNameValue");

        SetValues();
    }

    private void SetValues()  
    {
        _sceneNameLabel.text = SceneManager.GetActiveScene().name;
    }
}