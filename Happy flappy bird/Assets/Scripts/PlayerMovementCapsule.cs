using UnityEngine;

public class PlayerMovementCapsule : MonoBehaviour {
public float speed = 5;
public Rigidbody rb;

float horizontalInput;
public float horizontalMultiplier = 2;

private void FixedUpdate ()
{
Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier; //horizontally moves twice faster than forward

rb.MovePosition(rb.position + forwardMove + horizontalMove); 
}

 private void Update () {
  horizontalInput = Input.GetAxis("Horizontal");
  
}
}
