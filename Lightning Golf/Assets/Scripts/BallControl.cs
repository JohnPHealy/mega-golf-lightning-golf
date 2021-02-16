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
    public Transform cam;
    public Transform ActualCam;

    Vector3 target;

    bool isAiming = true;

    public Slider power;
    public Slider AngleSlider;
    public Slider CameraAngle;
    public Button ShootButton;
    public Text Shots;
    public Text Percentage;
    public Text AngleText;

    public float Power;

    Rigidbody rb;

    int shots;

    public Vector3 Offset;

    float cameraZ = -12;
    float cameraYValue = 3;
    AnimationCurve cameraY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
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
            
            IndicatorLine.LookAt(Indicator.position);
            
        }
        IndicatorLine.position = transform.position;
        IndicatorLine.rotation = Quaternion.Slerp(IndicatorLine.rotation, Quaternion.Euler(-AngleSlider.value, IndicatorLine.rotation.eulerAngles.y, IndicatorLine.rotation.eulerAngles.z), 0.1f);


        Shots.text = "Shots: " + shots;

        Percentage.text = (power.value * 100).ToString("0") + "%";

        AngleText.text = AngleSlider.value.ToString("0") + " degrees";

        cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, Quaternion.Euler(0, -CameraAngle.value, 0), 0.1f);
        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position, 0.1f);

        Debug.Log(Input.mouseScrollDelta);
        if (Input.mouseScrollDelta.y > 0 && cameraZ < -2)
        {
            cameraZ += Input.mouseScrollDelta.magnitude;
        }

        if (Input.mouseScrollDelta.y < 0 && cameraZ > -35)
        {
            cameraZ -= Input.mouseScrollDelta.magnitude;
        }

        cameraYValue = cameraY.Evaluate(cameraZ);

        Vector3 CamNewPos = new Vector3(0, cameraYValue, cameraZ);
        ActualCam.localPosition = Vector3.Lerp(ActualCam.localPosition, CamNewPos, 0.2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Die"))
        {
            Reset();
        }
    }
    public void Shoot()
    {
        rb.drag = 0.2f;
        transform.rotation = IndicatorLine.rotation;
        rb.AddForce(transform.forward * power.value * Power, ForceMode.Impulse);
        isAiming = false;

        shots++;
    }


    public void Reset()
    {
        rb.drag = 10;
        transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position + new Vector3(0, 1, 0);
    }
}
