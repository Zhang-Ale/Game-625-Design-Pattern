using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _testPlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    private CharacterController characterController;

    

    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        //player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection = transform.TransformDirection(moveDirection);

        characterController.SimpleMove(moveDirection * moveSpeed);
       
    }

   
}
