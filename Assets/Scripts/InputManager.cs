using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;


    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerLook look;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private Interactor interactor;

    [SerializeField] private UIManager uiManager;


    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;


        //Subscribe functions to action performed

        //PlayerMotor
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
        
        //PlayerInteract
        onFoot.Interact.performed += ctx => interactor.StartInteraction();


        //UIManager
        onFoot.Escape.performed += ctx => uiManager.Escape();
        onFoot.BuildPanel.performed += ctx => uiManager.BuildPanelUI();
        onFoot.Inventory.performed += ctx => uiManager.DisplayBackpack();

        //Hotbar
        onFoot.Hotbar0.performed += ctx => hotbar.SelectHotbar(0);
        onFoot.Hotbar1.performed += ctx => hotbar.SelectHotbar(1);
        onFoot.Hotbar2.performed += ctx => hotbar.SelectHotbar(2);
        onFoot.Hotbar3.performed += ctx => hotbar.SelectHotbar(3);
        onFoot.Hotbar4.performed += ctx => hotbar.SelectHotbar(4);
        onFoot.Hotbar5.performed += ctx => hotbar.SelectHotbar(5);
        onFoot.Hotbar6.performed += ctx => hotbar.SelectHotbar(6);
        onFoot.Hotbar7.performed += ctx => hotbar.SelectHotbar(7);
        onFoot.Hotbar8.performed += ctx => hotbar.SelectHotbar(8);
        onFoot.Hotbar9.performed += ctx => hotbar.SelectHotbar(9);

    }


    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }


}
