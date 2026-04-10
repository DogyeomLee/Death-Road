using UnityEngine;
using UnityEngine.InputSystem;

//입력 값을 받아, carMovemnt 와 연동하여, 캡슐화된 입력값들로 이동을 구현.
public class CarInput : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction boosterAction;

    private float movementInput;
    private float rotationInput;

    public float Movement => movementInput;
    public float Rotation => rotationInput;

    public bool IsOnBooster => HandleBooster();

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        boosterAction = playerInput.actions["Booster"];
    }

    private void Update()
    {
        HandleMovement();
        HandleBooster();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        movementInput = inputVector.y;
        rotationInput = inputVector.x;
    }

    private bool HandleBooster()
    {
        if(boosterAction.IsPressed())
        {
            return true;
        }

        return false;
    }
}
