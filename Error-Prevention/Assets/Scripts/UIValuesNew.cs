using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIValuesNew : MonoBehaviour
{
    [Header("References")]
    // References the label from the UI.
    private Label _sceneNameLabel;
    
    // References the player movement script. (Contains movement as well as buffer and coyote time.
    [SerializeField] 
    private PlayerMovement _movementScript;

    // Fill all the reference variables and initialise the UI.
    private void OnEnable()
    {
        _movementScript = _movementScript.GetComponent<PlayerMovement>();

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement; 

        _sceneNameLabel = rootVisualElement.Q<Label>("SceneNameValue");

        SetValues();
    }

    // Sets the UI element to have the current scenes name.
    // Needed so the participant can fill out the questionnaire.
    private void SetValues()  
    {
        _sceneNameLabel.text = SceneManager.GetActiveScene().name;
    }
}