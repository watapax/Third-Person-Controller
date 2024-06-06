using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPlayer : MonoBehaviour
{
    public CharacterController controller;
    public Transform camTransform;
    public float speed;
    public float alturaSalto;
    public float gravedad;
    public float velocidadAlResbalar;

    float speedRotation = 0.1f;
    float velocityRotation;
    Vector3 moveDirection;
    Vector3 velocidad;
    Vector3 hitNormal;
    bool isGrounded;

    void Mover()
    {
        
        isGrounded = controller.isGrounded;

        if(isGrounded && velocidad.y < 0)
        {
            velocidad.y = -2;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direccion = new Vector3(h, 0, v).normalized;


        if (direccion.magnitude >= 0.1f)
        {
            float anguloTarget = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloTarget, ref velocityRotation, speedRotation);
            transform.rotation = Quaternion.Euler(0, angulo, 0);

            moveDirection =  Quaternion.Euler(0, anguloTarget, 0) * Vector3.forward;
            moveDirection.Normalize();
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        if(isGrounded)
        {
            if(Vector3.Angle(Vector3.up, hitNormal) > controller.slopeLimit)
            {
                moveDirection.x += (1 - hitNormal.y) * hitNormal.x * velocidadAlResbalar;
                moveDirection.z += (1 - hitNormal.y) * hitNormal.z * velocidadAlResbalar;
            }
        }
        



    }

    void Salto()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocidad.y = Mathf.Sqrt(alturaSalto * -2 * gravedad);
        }
    }

    void Gravedad()
    {
        velocidad.y += gravedad * Time.deltaTime;
        controller.Move(velocidad * Time.deltaTime);
    }

    private void Update()
    {
        Mover();
        Salto();
        Gravedad();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

}
