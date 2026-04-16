using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class marnoldMover : MonoBehaviour
{
    float desired_acceleration;
    float desired_strafe;
    public float impulse;
    public float turnrate;
    //public TextMeshProUGUI timelbl;
    //float starttime;

    void OnMove(InputValue action)
    {
        var movement = action.Get<Vector2>();
        desired_acceleration = movement.y;
        desired_strafe = movement.x;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //starttime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddRelativeForce(desired_acceleration*impulse, 0, 0);
        GetComponent<Rigidbody>().AddRelativeForce(0, 0, -desired_strafe*impulse);
        float dx = (Mouse.current.position.x.value - Screen.width / 2) / turnrate;
        if (Mathf.Abs(dx) > 0.01f)
        {
            transform.Rotate(0, dx, 0);
        }
        // updates UI to record how long the race has been going on for
        //timelbl.text = string.Format("Current time: {0:F2} seconds", (Time.time - starttime));
    }
}
