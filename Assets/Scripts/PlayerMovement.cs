using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public float speed = 10.0f;
    public float rotationSpeed = 90f;
    public float jumpForce = 700f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public GameObject cannon;
    public GameObject bullet;

    private Rigidbody rb;
    private Transform t;

    void Start(){
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();

        // Helps reduce drifting
        rb.linearDamping = 5f;
        rb.angularDamping = 5f;
    }

    void FixedUpdate() {
        if (Keyboard.current == null) return;

        // MOVEMENT (NO DRIFTING)
        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            move += t.forward;

        if (Keyboard.current.sKey.isPressed)
            move -= t.forward;

        // Set velocity directly (prevents infinite acceleration)
        rb.linearVelocity = new Vector3(
            move.x * speed,
            rb.linearVelocity.y,   // keep vertical velocity (jump/gravity)
            move.z * speed
        );

        // ROTATION
        if (Keyboard.current.dKey.isPressed)
            t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.fixedDeltaTime, 0);

        if (Keyboard.current.aKey.isPressed)
            t.rotation *= Quaternion.Euler(0, -rotationSpeed * Time.fixedDeltaTime, 0);

        // BETTER FALLING
        if (rb.linearVelocity.y < 0) {
            // Falling → make it faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        } else if (rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed) {
            // Short hop if jump key released early
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void Update() {
        if (Keyboard.current == null) return;

        // JUMP
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            rb.AddForce(t.up * jumpForce);
        }

        // SHOOT
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) {
            GameObject newBullet = Instantiate(
                bullet,
                cannon.transform.position,
                cannon.transform.rotation
            );

            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();

            // Give bullet forward force
            bulletRb.linearVelocity = newBullet.transform.forward * 20f;

            // Optional slight upward boost
            bulletRb.AddForce(Vector3.up * 2, ForceMode.Impulse);
        }
    }
}