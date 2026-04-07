using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{
    [Header("Wheels")]
    public WheelCollider[] frontWheels;
    public WheelCollider[] rearWheels;
    public Transform[] frontMeshes;
    public Transform[] rearMeshes;
    public Transform visualBody; // Coloque o modelo 3D aqui (filho do GameObject principal)

    [Header("Tuning")]
    public bool isMotorcycle = false;
    public float motorTorque = 1500f;
    public float brakeTorque = 3000f;
    public float maxSteerAngle = 35f;
    public float maxSpeed = 45f; // m/s
    public float leanAngle = 30f;
    public float leanSpeed = 6f;

    private Rigidbody rb;

    void Start() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        float accel = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        float speed = rb.velocity.magnitude;

        // Motor & Freio
        float motor = (speed < maxSpeed) ? accel * motorTorque : 0f;
        float brake = (accel < -0.1f) ? brakeTorque : 0f;

        foreach (var wc in rearWheels) { wc.motorTorque = motor; wc.brakeTorque = brake; }
        foreach (var wc in frontWheels) { wc.motorTorque = 0; wc.brakeTorque = 0; }

        // Direção
        foreach (var wc in frontWheels) wc.steerAngle = steer * maxSteerAngle;

        // Atualizar meshes visuais
        UpdateMeshes(frontWheels, frontMeshes);
        UpdateMeshes(rearWheels, rearMeshes);

        // Inclinação da moto (só na parte visual!)
        if (isMotorcycle && visualBody != null)
        {
            float targetLean = steer * leanAngle * Mathf.Clamp01(speed / 15f);
            Quaternion targetRot = Quaternion.Euler(0, 0, -targetLean);
            visualBody.localRotation = Quaternion.Slerp(visualBody.localRotation, targetRot, leanSpeed * Time.fixedDeltaTime);
        }
    }

    void UpdateMeshes(WheelCollider[] colliders, Transform[] meshes)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 pos; Quaternion rot;
            colliders[i].GetWorldPose(out pos, out rot);
            meshes[i].position = pos;
            meshes[i].rotation = rot;
        }
    }
}
