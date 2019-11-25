using Unity.Burst;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

[DisallowMultipleComponent, AddComponentMenu ("Animation Rigging/Custom/Foot IK")]
public class FootIKConstraint : RigConstraint<FootIKConstraintJob, FootIKConstraintData, FootIKConstraintBinder> { }

[BurstCompile]
public struct FootIKConstraintJob : IWeightedAnimationJob {

    public ReadWriteTransformHandle FootIKTarget;

    public ReadOnlyTransformHandle FootControllerBase;
    public ReadWriteTransformHandle FootRollCursor;

    public ReadOnlyTransformHandle ToeEnd;
    public ReadOnlyTransformHandle FootHeel;
    public ReadOnlyTransformHandle FootLeftSide;
    public ReadOnlyTransformHandle FootRightSide;

    public FloatProperty jobWeight { get; set; }

    public void ProcessRootMotion (AnimationStream stream) { }

    public void ProcessAnimation (AnimationStream stream) {
        float w = jobWeight.Get (stream);

        if (w > 0f) {
            var cursorPos = FootRollCursor.GetLocalPosition (stream);
            FootRollCursor.SetLocalPosition (stream, new Vector3 (cursorPos.x, cursorPos.y, 0));

            var pos = FootControllerBase.GetPosition (stream);
            var rot = FootControllerBase.GetRotation (stream);

            if (cursorPos.x < 0f) {
                var footRightSidePos = FootRightSide.GetPosition (stream);
                var axisZ = rot * Vector3.forward;
                var rotZ = Quaternion.AngleAxis (180 * cursorPos.x, axisZ);
                pos = footRightSidePos + rotZ * (pos - footRightSidePos);
                rot = rotZ * rot;

                if (cursorPos.y >= 0f) {
                    var toePos = footRightSidePos + rotZ * (ToeEnd.GetPosition (stream) - footRightSidePos);
                    var axisX = rot * Vector3.right;
                    var rotX = Quaternion.AngleAxis (180 * cursorPos.y, axisX);
                    pos = toePos + rotX * (pos - toePos);
                    rot = rotX * rot;
                } else {
                    var heelPos = footRightSidePos + rotZ * (FootHeel.GetPosition (stream) - footRightSidePos);
                    var axisX = rot * Vector3.right;
                    var rotX = Quaternion.AngleAxis (180 * cursorPos.y, axisX);
                    pos = heelPos + rotX * (pos - heelPos);
                    rot = rotX * rot;
                }
            } else {
                var footLeftSidePos = FootLeftSide.GetPosition (stream);
                var axisZ = rot * Vector3.forward;
                var rotZ = Quaternion.AngleAxis (180 * cursorPos.x, axisZ);
                pos = footLeftSidePos + rotZ * (pos - footLeftSidePos);
                rot = rotZ * rot;

                if (cursorPos.y >= 0f) {
                    var toePos = footLeftSidePos + rotZ * (ToeEnd.GetPosition (stream) - footLeftSidePos);
                    var axisX = rot * Vector3.right;
                    var rotX = Quaternion.AngleAxis (180 * cursorPos.y, axisX);
                    pos = toePos + rotX * (pos - toePos);
                    rot = rotX * rot;

                } else {
                    var heelPos = footLeftSidePos + rotZ * (FootHeel.GetPosition (stream) - footLeftSidePos);
                    var axisX = rot * Vector3.right;
                    var rotX = Quaternion.AngleAxis (180 * cursorPos.y, axisX);
                    pos = heelPos + rotX * (pos - heelPos);
                    rot = rotX * rot;
                }
            }

            var footIKTargetPos = FootIKTarget.GetPosition (stream);
            var footIKTargetRot = FootIKTarget.GetRotation (stream);
            FootIKTarget.SetPosition (stream, Vector3.Lerp (footIKTargetPos, pos, w));
            FootIKTarget.SetRotation (stream, Quaternion.Lerp (footIKTargetRot, rot, w));
        }
    }

}

[System.Serializable]
public struct FootIKConstraintData : IAnimationJobData {

    public Transform FootIKTarget;

    [SyncSceneToStream] public Transform FootControllerBase;
    [SyncSceneToStream] public Transform FootRollCursor;

    [SyncSceneToStream] public Transform ToeEnd;
    [SyncSceneToStream] public Transform FootHeel;
    [SyncSceneToStream] public Transform FootLeftSide;
    [SyncSceneToStream] public Transform FootRightSide;

    public bool IsValid () => !(FootIKTarget == null || FootControllerBase == null || FootRollCursor == null || ToeEnd == null || FootHeel == null || FootLeftSide == null || FootRightSide == null);

    public void SetDefaultValues () {
        FootIKTarget = null;
        FootControllerBase = null;
        FootRollCursor = null;
        ToeEnd = null;
        FootHeel = null;
        FootLeftSide = null;
        FootRightSide = null;
    }

}

public class FootIKConstraintBinder : AnimationJobBinder<FootIKConstraintJob, FootIKConstraintData> {

    public override FootIKConstraintJob Create (Animator animator, ref FootIKConstraintData data, Component component) {
        var job = new FootIKConstraintJob ();

        job.FootIKTarget = ReadWriteTransformHandle.Bind (animator, data.FootIKTarget);
        job.FootControllerBase = ReadOnlyTransformHandle.Bind (animator, data.FootControllerBase);
        job.FootRollCursor = ReadWriteTransformHandle.Bind (animator, data.FootRollCursor);
        job.ToeEnd = ReadOnlyTransformHandle.Bind (animator, data.ToeEnd);
        job.FootHeel = ReadOnlyTransformHandle.Bind (animator, data.FootHeel);
        job.FootLeftSide = ReadOnlyTransformHandle.Bind (animator, data.FootLeftSide);
        job.FootRightSide = ReadOnlyTransformHandle.Bind (animator, data.FootRightSide);

        return job;
    }

    public override void Destroy (FootIKConstraintJob job) { }

}
