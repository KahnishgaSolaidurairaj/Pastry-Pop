// shoot
// using __ imports namespace
// Namespaces are collection of classes, data types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// MonoBehavior is the base class from which every Unity Script Derives
public class PlayerMovement : MonoBehaviour
{
    public float speed = 25.0f;
    public float rotationSpeed = 90;
    public float force = 700f;

    public GameObject cannon;
    public GameObject bullet;

    Rigidbody rb;
    Transform t;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Time.deltaTime represents the time that passed since the last frame
        //the multiplication below ensures that GameObject moves constant speed every frame
        if (Keyboard.current != null && Keyboard.current.wKey.isPressed)
            rb.linearVelocity += this.transform.forward * speed * Time.deltaTime;
        else if (Keyboard.current != null && Keyboard.current.sKey.isPressed)
            rb.linearVelocity -= this.transform.forward * speed * Time.deltaTime;

        // Quaternion returns a rotation that rotates x degrees around the x axis and so on
        if (Keyboard.current != null && Keyboard.current.dKey.isPressed)
            t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        else if (Keyboard.current != null && Keyboard.current.aKey.isPressed)
            t.rotation *= Quaternion.Euler(0, - rotationSpeed * Time.deltaTime, 0);
        
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            rb.AddForce(t.up * force);

        // https://docs.unity3d.com/ScriptReference/Input.html
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame){
            GameObject newBullet = GameObject.Instantiate(bullet, cannon.transform.position, cannon.transform.rotation) as GameObject;
            newBullet.GetComponent<Rigidbody>().linearVelocity += Vector3.up * 2;
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * 1500);
        }

    }
}