using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 4f;
    private Rigidbody2D rb2D;

    private Vector2 movement;
    private Camera playerCamera;
    private Vector2 mousePos;


    void Awake()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        playerCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movement * movementSpeed * Time.fixedDeltaTime);

        Vector2 lookDirection = mousePos - rb2D.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb2D.rotation = angle;
    }
}
