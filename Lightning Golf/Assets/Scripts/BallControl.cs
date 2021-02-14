using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControl : MonoBehaviour
{
    public float RotSpeed;
    public Transform Indicator;
    public Transform IndicatorLine;

    Vector3 point;
    Camera cam;

    Vector3 target;

    bool isAiming;

    public Slider power;
    public Button ShootButton;
    public Text Shots;

    public float Power;

    Rigidbody rb;

    int shots;

    public Vector3 Offset;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (isAiming)
            {
                isAiming = false;
            }
            else
            {
                isAiming = true;
            }
        }
    }

    private void FixedUpdate()
    {

        if (rb.velocity.magnitude > 0.01f)
        {
            ShootButton.interactable = false;
        }
        else
        {
            ShootButton.interactable = true;
        }

        

        if (isAiming)
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;

            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                target = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Indicator.position = target;
            IndicatorLine.position = transform.position;
            IndicatorLine.LookAt(Indicator.position);
            IndicatorLine.rotation = Quaternion.Euler(0, IndicatorLine.rotation.eulerAngles.y, IndicatorLine.rotation.eulerAngles.z);
        }

        Shots.text = "Shots: " + shots;

        cam.transform.position = transform.position + Offset;
    }

    public void Shoot()
    {
        transform.rotation = IndicatorLine.rotation;
        rb.AddForce(transform.forward * power.value * Power, ForceMode.Impulse);
        isAiming = false;

        shots++;
    }
}
