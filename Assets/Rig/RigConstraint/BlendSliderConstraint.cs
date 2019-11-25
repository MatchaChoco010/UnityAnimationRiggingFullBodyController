using Unity.Burst;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

[DisallowMultipleComponent, AddComponentMenu ("Animation Rigging/Custom/Blend Slider")]
public class BlendSliderConstraint : RigConstraint<BlendSliderConstraintJob, BlendSliderConstraintData, BlendSliderConstraintBinder> { }

[BurstCompile]
public struct BlendSliderConstraintJob : IWeightedAnimationJob {

    public ReadWriteTransformHandle Target;

    public ReadOnlyTransformHandle SourceA;
    public ReadOnlyTransformHandle SourceB;

    public ReadWriteTransformHandle Slider;

    public bool Position;
    public bool Rotation;

    public Vector3 PositionOffset;
    public Quaternion RotationOffset;

    public FloatProperty jobWeight { get; set; }

    public void ProcessRootMotion (AnimationStream stream) { }

    public void ProcessAnimation (AnimationStream stream) {
        float w = jobWeight.Get (stream);

        if (w > 0f) {
            var sliderPos = Slider.GetLocalPosition (stream);
            var t = Mathf.Clamp01 (sliderPos.y);
            Slider.SetLocalPosition (stream, new Vector3 (0, t, 0));

            if (Rotation) {
                var rot = Quaternion.Lerp (
                    SourceA.GetRotation (stream),
                    SourceB.GetRotation (stream),
                    t
                );

                var targetRot = Target.GetRotation (stream);
                Target.SetRotation (stream, Quaternion.Lerp (targetRot, rot, w));
            }

            if (Position) {
                var pos = Vector3.Lerp (
                    SourceA.GetPosition (stream),
                    SourceB.GetPosition (stream),
                    t
                );

                var targetPos = Target.GetPosition (stream);
                Target.SetPosition (stream, Vector3.Lerp (targetPos, pos, w));
            }
        }
    }

}

[System.Serializable]
public struct BlendSliderConstraintData : IAnimationJobData {

    public Transform Target;

    [SyncSceneToStream] public Transform SourceA;
    [SyncSceneToStream] public Transform SourceB;

    [SyncSceneToStream] public Transform Slider;

    [NotKeyable] public bool Position;
    [NotKeyable] public bool Rotation;

    public bool IsValid () => !(Target == null || SourceA == null || SourceB == null || Slider == null);

    public void SetDefaultValues () {
        Target = null;
        SourceA = null;
        SourceB = null;
        Slider = null;
        Position = true;
        Rotation = true;
    }

}

public class BlendSliderConstraintBinder : AnimationJobBinder<BlendSliderConstraintJob, BlendSliderConstraintData> {

    public override BlendSliderConstraintJob Create (Animator animator, ref BlendSliderConstraintData data, Component component) {
        var job = new BlendSliderConstraintJob ();

        job.Target = ReadWriteTransformHandle.Bind (animator, data.Target);

        job.SourceA = ReadOnlyTransformHandle.Bind (animator, data.SourceA);
        job.SourceB = ReadOnlyTransformHandle.Bind (animator, data.SourceB);

        job.Slider = ReadWriteTransformHandle.Bind (animator, data.Slider);

        job.Position = data.Position;
        job.Rotation = data.Rotation;

        return job;
    }

    public override void Destroy (BlendSliderConstraintJob job) { }

}
