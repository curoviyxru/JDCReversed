using System.Numerics;
using System.Runtime.InteropServices;
using JDCReversed.Packets;
using Valve.VR;

class OpenVRHandler
{
    public const float GRAVITY = 9.80665f;
    public const float LIMIT = 4.0f;
    private const uint InvalidController = OpenVR.k_unMaxTrackedDeviceCount + 1;

    private uint FoundController = InvalidController;
    private Vector3 LastVelocity = new();
    public bool UpdateWasSuccessful { private set; get; } = false;
    public VRControllerState_t ControllerState { private set; get; } = new();
    public TrackedDevicePose_t TrackedDevicePose { private set; get; } = new();

    public void Start()
    {
        EVRInitError error = EVRInitError.None;
        OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);

        if (error != EVRInitError.None)
        {
            Console.WriteLine("OpenVR init error: " + error.ToString());
            return;
        }

        uint foundController = InvalidController;
        uint foundControllerRight = InvalidController;

        for (uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; ++i)
        {
            ETrackedDeviceClass cls = OpenVR.System.GetTrackedDeviceClass(i);
            if (cls != ETrackedDeviceClass.Controller) continue;

            ETrackedControllerRole role = OpenVR.System.GetControllerRoleForTrackedDeviceIndex(i);
            if (foundController == InvalidController) foundController = i;
            if (foundControllerRight == InvalidController && role == ETrackedControllerRole.RightHand) foundControllerRight = i;
        }

        FoundController = foundControllerRight == InvalidController ? foundController : foundControllerRight;
    }

    public void Stop()
    {
        OpenVR.Shutdown();
    }

    public void ResetLastVelocity()
    {
        LastVelocity = new();
    }

    private static Vector3 ApplyGravityAndRotation(HmdMatrix34_t matrix, Vector3 acceleration)
    {
        // Define the gravity vector
        Vector3 gravityVector = new(0, -GRAVITY, 0); // Gravity acts downwards in Y axis

        // Convert the HMDMatrix34_t to a rotation matrix (3x3)
        Matrix4x4 rotationMatrix = new(
            matrix.m0, matrix.m1, matrix.m2, 0,
            matrix.m4, matrix.m5, matrix.m6, 0,
            matrix.m8, matrix.m9, matrix.m10, 0,
            0, 0, 0, 1
        );

        // Rotate the acceleration vector by the controller's rotation matrix
        acceleration = Vector3.Transform(acceleration + gravityVector, rotationMatrix);

        return acceleration;
    }

    public void Update()
    {
        bool prevUpdateWasSuccessful = UpdateWasSuccessful;

        VRControllerState_t controllerState = new();
        TrackedDevicePose_t trackedDevicePose = new();

        UpdateWasSuccessful = OpenVR.System.GetControllerStateWithPose(ETrackingUniverseOrigin.TrackingUniverseStanding, FoundController, 
            ref controllerState, (uint)Marshal.SizeOf(typeof(VRControllerState_t)), ref trackedDevicePose);

        ControllerState = controllerState;
        TrackedDevicePose = trackedDevicePose;

        if (!UpdateWasSuccessful && prevUpdateWasSuccessful) {
            Console.WriteLine("Can't update controller state!");
        }
    }

    public void GetAccelValues(ref AccelDataItem accelDataItem, float delta)
    {
        // Calculate acceleration from velocity
        Vector3 velocity = new(TrackedDevicePose.vVelocity.v0, TrackedDevicePose.vVelocity.v1, TrackedDevicePose.vVelocity.v2);
        Vector3 acceleration = (velocity - LastVelocity) / delta;
        LastVelocity = velocity;

        // Apply gravity and rotation, convert from m/s/s to g
        acceleration = ApplyGravityAndRotation(TrackedDevicePose.mDeviceToAbsoluteTracking, acceleration);
        Vector3 accelerationInGs = acceleration / GRAVITY;

        // Set data message values
        accelDataItem.X = Math.Clamp(accelerationInGs.Y, -LIMIT, LIMIT);
        accelDataItem.Y = Math.Clamp(-accelerationInGs.Z, -LIMIT, LIMIT);
        accelDataItem.Z = Math.Clamp(-accelerationInGs.X, -LIMIT, LIMIT);
    }
}