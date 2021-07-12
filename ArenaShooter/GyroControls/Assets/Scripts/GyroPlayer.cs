#region Defines
#if UNITY_2020 || UNITY_2021 || UNITY_2022 || UNITY_2023 || UNITY_2024 || UNITY_2025
#define UNITY_2020_PLUS
#endif
#if UNITY_2019 || UNITY_2020_PLUS
#define UNITY_2019_PLUS
#endif
#if UNITY_2018 || UNITY_2019_PLUS
#define UNITY_2018_PLUS
#endif
#if UNITY_2017 || UNITY_2018_PLUS
#define UNITY_2017_PLUS
#endif
#if UNITY_5 || UNITY_2017_PLUS
#define UNITY_5_PLUS
#endif
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_1_PLUS
#endif
#if UNITY_5_2 || UNITY_5_3_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_2_PLUS
#endif
#if UNITY_5_3_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_3_PLUS
#endif
#if UNITY_5_4_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_4_PLUS
#endif
#if UNITY_5_5_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_5_PLUS
#endif
#if UNITY_5_6_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_6_PLUS
#endif
#if UNITY_5_7_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_7_PLUS
#endif
#if UNITY_5_8_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_8_PLUS
#endif
#if UNITY_5_9_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_9_PLUS
#endif
#pragma warning disable 0219
#pragma warning disable 0618
#pragma warning disable 0649
#pragma warning disable 0067
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;


public class GyroPlayer : MonoBehaviour
{
    Rigidbody rb;

    public Transform myCamera;
    public float MoveSpeed = 1f;
    public float Sensitivity = 2f;

    public CameraController cameraController;

    public bool Grounded;
    public float JumpForce;

    public float RisingMass;
    public float FallingMass;
    public Vector3 accelerometer;

    [SerializeField] private int playerId = 0;
    public Transform accelerometerTransform;

    private Player player;
    public bool gyroActive = false;
    private Quaternion previousGyroRot;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ReInput.isReady) return;

        input();
    }

    void input()
    {
        float horizontal = player.GetAxis("MoveHorizontal");
        float vertical = player.GetAxis("MoveVertical");

        transform.rotation = Quaternion.Euler(0, myCamera.rotation.eulerAngles.y, 0);

        rb.velocity = (transform.right * horizontal + transform.forward * vertical).normalized * MoveSpeed ;

        float mouseX = player.GetAxis("LookHorizontal");
        float mouseY = player.GetAxis("LookVertical");

        cameraController.AddRotation(-mouseY, mouseX, 0, Sensitivity);

        bool gyroDown = player.GetButtonDown("GyroToggle");
        if (player.GetButtonDown("GyroToggle"))
            gyroActive = true;
        if (player.GetButtonUp("GyroToggle"))
            gyroActive = false;
            
        if(player.GetButton("GyroToggle"))
        {
            var ds4 = GetFirstDS4(player);

            if (ds4 != null)
            {

                // Set the model's rotation to match the controller's     cameraController.transform.rotation
                Quaternion ps4Rot, totalRot;
                ps4Rot = ds4.GetOrientation();
                Vector3 sumVec = new Vector3(ps4Rot.eulerAngles.x + transform.rotation.eulerAngles.x,
                                            ps4Rot.eulerAngles.y + transform.rotation.eulerAngles.y,
                                             transform.rotation.eulerAngles.z);

                cameraController.transform.rotation = Quaternion.Euler(sumVec);
                Vector3 accelerometerValue = ds4.GetAccelerometerValue();
                accelerometer = accelerometerValue;
            }
        }
    }

    private IDualShock4Extension GetFirstDS4(Player player)
    {
        foreach (Joystick j in player.controllers.Joysticks)
        {
            // Use the interface because it works for both PS4 and desktop platforms
            IDualShock4Extension ds4 = j.GetExtension<IDualShock4Extension>();
            if (ds4 == null) continue;
            return ds4;
        }
        return null;
    }

}