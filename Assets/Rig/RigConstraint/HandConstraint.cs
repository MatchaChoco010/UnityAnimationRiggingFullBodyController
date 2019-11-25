using Unity.Burst;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

[DisallowMultipleComponent, AddComponentMenu ("Animation Rigging/Custom/Hand")]
public class HandConstraint : RigConstraint<HandConstraintJob, HandConstraintData, HandConstraintBinder> { }

[BurstCompile]
public struct HandConstraintJob : IWeightedAnimationJob {

  public ReadWriteTransformHandle ProximalThumb;
  public ReadWriteTransformHandle IntermediateThumb;
  public ReadWriteTransformHandle DistalThumb;

  public ReadWriteTransformHandle ProximalIndex;
  public ReadWriteTransformHandle IntermediateIndex;
  public ReadWriteTransformHandle DistalIndex;

  public ReadWriteTransformHandle ProximalMiddle;
  public ReadWriteTransformHandle IntermediateMiddle;
  public ReadWriteTransformHandle DistalMiddle;

  public ReadWriteTransformHandle ProximalRing;
  public ReadWriteTransformHandle IntermediateRing;
  public ReadWriteTransformHandle DistalRing;

  public ReadWriteTransformHandle ProximalLittle;
  public ReadWriteTransformHandle IntermediateLittle;
  public ReadWriteTransformHandle DistalLittle;

  public Vector2 ProximalThumbXRotationRange;
  public Vector2 ProximalThumbZRotationRange;
  public Vector2 IntermediateThumbXRotationRange;
  public Vector2 DistalThumbXRotationRange;

  public Vector2 ProximalIndexXRotationRange;
  public Vector2 ProximalIndexZRotationRange;
  public Vector2 IntermediateIndexXRotationRange;
  public Vector2 DistalIndexXRotationRange;

  public Vector2 ProximalMiddleXRotationRange;
  public Vector2 ProximalMiddleZRotationRange;
  public Vector2 IntermediateMiddleXRotationRange;
  public Vector2 DistalMiddleXRotationRange;

  public Vector2 ProximalRingXRotationRange;
  public Vector2 ProximalRingZRotationRange;
  public Vector2 IntermediateRingXRotationRange;
  public Vector2 DistalRingXRotationRange;

  public Vector2 ProximalLittleXRotationRange;
  public Vector2 ProximalLittleZRotationRange;
  public Vector2 IntermediateLittleXRotationRange;
  public Vector2 DistalLittleXRotationRange;

  public ReadWriteTransformHandle ThumbHandle;
  public ReadWriteTransformHandle IndexHandle;
  public ReadWriteTransformHandle MiddleHandle;
  public ReadWriteTransformHandle RingHandle;
  public ReadWriteTransformHandle LittleHandle;

  public ReadWriteTransformHandle HandleA;
  public ReadWriteTransformHandle HandleB;

  public FloatProperty jobWeight { get; set; }

  public void ProcessRootMotion (AnimationStream stream) { }

  public void ProcessAnimation (AnimationStream stream) {
    var w = jobWeight.Get (stream);

    var thumbHandlePos = ThumbHandle.GetLocalPosition (stream);
    var thumbT = Mathf.Clamp01 (thumbHandlePos.y);
    ThumbHandle.SetLocalPosition (stream, new Vector3 (0, thumbT, 0));

    var indexHandlePos = IndexHandle.GetLocalPosition (stream);
    var indexT = Mathf.Clamp01 (indexHandlePos.y);
    IndexHandle.SetLocalPosition (stream, new Vector3 (0, indexT, 0));

    var middleHandlePos = MiddleHandle.GetLocalPosition (stream);
    var middleT = Mathf.Clamp01 (middleHandlePos.y);
    MiddleHandle.SetLocalPosition (stream, new Vector3 (0, middleT, 0));

    var ringHandlePos = RingHandle.GetLocalPosition (stream);
    var ringT = Mathf.Clamp01 (ringHandlePos.y);
    RingHandle.SetLocalPosition (stream, new Vector3 (0, ringT, 0));

    var littleHandlePos = LittleHandle.GetLocalPosition (stream);
    var littleT = Mathf.Clamp01 (littleHandlePos.y);
    LittleHandle.SetLocalPosition (stream, new Vector3 (0, littleT, 0));

    var handleAPos = HandleA.GetLocalPosition (stream);
    var aT = Mathf.Clamp01 (handleAPos.y);
    HandleA.SetLocalPosition (stream, new Vector3 (0, aT, 0));

    var handleBPos = HandleB.GetLocalPosition (stream);
    var bT = Mathf.Clamp01 (handleBPos.y);
    HandleB.SetLocalPosition (stream, new Vector3 (0, bT, 0));

    if (w > 0) {
      // Thumb
      var proximalThumbHandleRot = Quaternion.Euler (
        Mathf.Lerp (ProximalThumbXRotationRange.x, ProximalThumbXRotationRange.y, thumbT * aT),
        0,
        Mathf.Lerp (ProximalThumbZRotationRange.x, ProximalThumbZRotationRange.y, bT)
      );
      var proximalThumbRot = ProximalThumb.GetLocalRotation (stream);
      ProximalThumb.SetLocalRotation (stream, Quaternion.Lerp (
        proximalThumbRot,
        proximalThumbRot * proximalThumbHandleRot,
        w
      ));

      var intermediateThumbHandleRot = Quaternion.Euler (
        Mathf.Lerp (IntermediateThumbXRotationRange.x, IntermediateThumbXRotationRange.y, thumbT * aT),
        0,
        0
      );
      var intermediateThumbRot = IntermediateThumb.GetLocalRotation (stream);
      IntermediateThumb.SetLocalRotation (stream, Quaternion.Lerp (
        intermediateThumbRot,
        intermediateThumbRot * intermediateThumbHandleRot,
        w
      ));

      var distalThumbHandleRot = Quaternion.Euler (
        Mathf.Lerp (DistalThumbXRotationRange.x, DistalThumbXRotationRange.y, thumbT * aT),
        0,
        0
      );
      var distalThumbRot = DistalThumb.GetLocalRotation (stream);
      DistalThumb.SetLocalRotation (stream, Quaternion.Lerp (
        distalThumbRot,
        distalThumbRot * distalThumbHandleRot,
        w
      ));

      // Index
      var proximalIndexHandleRot = Quaternion.Euler (
        Mathf.Lerp (ProximalIndexXRotationRange.x, ProximalIndexXRotationRange.y, indexT * aT),
        0,
        Mathf.Lerp (ProximalIndexZRotationRange.x, ProximalIndexZRotationRange.y, bT)
      );
      var proximalIndexRot = ProximalIndex.GetLocalRotation (stream);
      ProximalIndex.SetLocalRotation (stream, Quaternion.Lerp (
        proximalIndexRot,
        proximalIndexRot * proximalIndexHandleRot,
        w
      ));

      var intermediateIndexHandleRot = Quaternion.Euler (
        Mathf.Lerp (IntermediateIndexXRotationRange.x, IntermediateIndexXRotationRange.y, indexT * aT),
        0,
        0
      );
      var intermediateIndexRot = IntermediateIndex.GetLocalRotation (stream);
      IntermediateIndex.SetLocalRotation (stream, Quaternion.Lerp (
        intermediateIndexRot,
        intermediateIndexRot * intermediateIndexHandleRot,
        w
      ));

      var distalIndexHandleRot = Quaternion.Euler (
        Mathf.Lerp (DistalIndexXRotationRange.x, DistalIndexXRotationRange.y, indexT * aT),
        0,
        0
      );
      var distalIndexRot = DistalIndex.GetLocalRotation (stream);
      DistalIndex.SetLocalRotation (stream, Quaternion.Lerp (
        distalIndexRot,
        distalIndexRot * distalIndexHandleRot,
        w
      ));

      // Middle
      var proximalMiddleHandleRot = Quaternion.Euler (
        Mathf.Lerp (ProximalMiddleXRotationRange.x, ProximalMiddleXRotationRange.y, middleT * aT),
        0,
        Mathf.Lerp (ProximalMiddleZRotationRange.x, ProximalMiddleZRotationRange.y, bT)
      );
      var proximalMiddleRot = ProximalMiddle.GetLocalRotation (stream);
      ProximalMiddle.SetLocalRotation (stream, Quaternion.Lerp (
        proximalMiddleRot,
        proximalMiddleRot * proximalMiddleHandleRot,
        w
      ));

      var intermediateMiddleHandleRot = Quaternion.Euler (
        Mathf.Lerp (IntermediateMiddleXRotationRange.x, IntermediateMiddleXRotationRange.y, middleT * aT),
        0,
        0
      );
      var intermediateMiddleRot = IntermediateMiddle.GetLocalRotation (stream);
      IntermediateMiddle.SetLocalRotation (stream, Quaternion.Lerp (
        intermediateMiddleRot,
        intermediateMiddleRot * intermediateMiddleHandleRot,
        w
      ));

      var distalMiddleHandleRot = Quaternion.Euler (
        Mathf.Lerp (DistalMiddleXRotationRange.x, DistalMiddleXRotationRange.y, middleT * aT),
        0,
        0
      );
      var distalMiddleRot = DistalMiddle.GetLocalRotation (stream);
      DistalMiddle.SetLocalRotation (stream, Quaternion.Lerp (
        distalMiddleRot,
        distalMiddleRot * distalMiddleHandleRot,
        w
      ));

      // Ring
      var proximalRingHandleRot = Quaternion.Euler (
        Mathf.Lerp (ProximalRingXRotationRange.x, ProximalRingXRotationRange.y, ringT * aT),
        0,
        Mathf.Lerp (ProximalRingZRotationRange.x, ProximalRingZRotationRange.y, bT)
      );
      var proximalRingRot = ProximalRing.GetLocalRotation (stream);
      ProximalRing.SetLocalRotation (stream, Quaternion.Lerp (
        proximalRingRot,
        proximalRingRot * proximalRingHandleRot,
        w
      ));

      var intermediateRingHandleRot = Quaternion.Euler (
        Mathf.Lerp (IntermediateRingXRotationRange.x, IntermediateRingXRotationRange.y, ringT * aT),
        0,
        0
      );
      var intermediateRingRot = IntermediateRing.GetLocalRotation (stream);
      IntermediateRing.SetLocalRotation (stream, Quaternion.Lerp (
        intermediateRingRot,
        intermediateRingRot * intermediateRingHandleRot,
        w
      ));

      var distalRingHandleRot = Quaternion.Euler (
        Mathf.Lerp (DistalRingXRotationRange.x, DistalRingXRotationRange.y, ringT * aT),
        0,
        0
      );
      var distalRingRot = DistalRing.GetLocalRotation (stream);
      DistalRing.SetLocalRotation (stream, Quaternion.Lerp (
        distalRingRot,
        distalRingRot * distalRingHandleRot,
        w
      ));

      // Little
      var proximalLittleHandleRot = Quaternion.Euler (
        Mathf.Lerp (ProximalLittleXRotationRange.x, ProximalLittleXRotationRange.y, littleT * aT),
        0,
        Mathf.Lerp (ProximalLittleZRotationRange.x, ProximalLittleZRotationRange.y, bT)
      );
      var proximalLittleRot = ProximalLittle.GetLocalRotation (stream);
      ProximalLittle.SetLocalRotation (stream, Quaternion.Lerp (
        proximalLittleRot,
        proximalLittleRot * proximalLittleHandleRot,
        w
      ));

      var intermediateLittleHandleRot = Quaternion.Euler (
        Mathf.Lerp (IntermediateLittleXRotationRange.x, IntermediateLittleXRotationRange.y, littleT * aT),
        0,
        0
      );
      var intermediateLittleRot = IntermediateLittle.GetLocalRotation (stream);
      IntermediateLittle.SetLocalRotation (stream, Quaternion.Lerp (
        intermediateLittleRot,
        intermediateLittleRot * intermediateLittleHandleRot,
        w
      ));

      var distalLittleHandleRot = Quaternion.Euler (
        Mathf.Lerp (DistalLittleXRotationRange.x, DistalLittleXRotationRange.y, littleT * aT),
        0,
        0
      );
      var distalLittleRot = DistalLittle.GetLocalRotation (stream);
      DistalLittle.SetLocalRotation (stream, Quaternion.Lerp (
        distalLittleRot,
        distalLittleRot * distalLittleHandleRot,
        w
      ));
    }
  }

}

[System.Serializable]
public struct HandConstraintData : IAnimationJobData {

  public Transform ProximalThumb;
  public Transform IntermediateThumb;
  public Transform DistalThumb;

  public Transform ProximalIndex;
  public Transform IntermediateIndex;
  public Transform DistalIndex;

  public Transform ProximalMiddle;
  public Transform IntermediateMiddle;
  public Transform DistalMiddle;

  public Transform ProximalRing;
  public Transform IntermediateRing;
  public Transform DistalRing;

  public Transform ProximalLittle;
  public Transform IntermediateLittle;
  public Transform DistalLittle;

  public Vector2 ProximalThumbXRotationRange;
  public Vector2 ProximalThumbZRotationRange;
  public Vector2 IntermediateThumbXRotationRange;
  public Vector2 DistalThumbXRotationRange;

  public Vector2 ProximalIndexXRotationRange;
  public Vector2 ProximalIndexZRotationRange;
  public Vector2 IntermediateIndexXRotationRange;
  public Vector2 DistalIndexXRotationRange;

  public Vector2 ProximalMiddleXRotationRange;
  public Vector2 ProximalMiddleZRotationRange;
  public Vector2 IntermediateMiddleXRotationRange;
  public Vector2 DistalMiddleXRotationRange;

  public Vector2 ProximalRingXRotationRange;
  public Vector2 ProximalRingZRotationRange;
  public Vector2 IntermediateRingXRotationRange;
  public Vector2 DistalRingXRotationRange;

  public Vector2 ProximalLittleXRotationRange;
  public Vector2 ProximalLittleZRotationRange;
  public Vector2 IntermediateLittleXRotationRange;
  public Vector2 DistalLittleXRotationRange;

  [SyncSceneToStream] public Transform ThumbHandle;
  [SyncSceneToStream] public Transform IndexHandle;
  [SyncSceneToStream] public Transform MiddleHandle;
  [SyncSceneToStream] public Transform RingHandle;
  [SyncSceneToStream] public Transform LittleHandle;

  [SyncSceneToStream] public Transform HandleA;
  [SyncSceneToStream] public Transform HandleB;

  public bool IsValid () => !(
    ProximalThumb == null ||
    IntermediateThumb == null ||
    DistalThumb == null ||

    ProximalIndex == null ||
    IntermediateIndex == null ||
    DistalIndex == null ||

    ProximalMiddle == null ||
    IntermediateMiddle == null ||
    DistalMiddle == null ||

    ProximalRing == null ||
    IntermediateRing == null ||
    DistalRing == null ||

    ProximalLittle == null ||
    IntermediateLittle == null ||
    DistalLittle == null ||

    ProximalThumbXRotationRange == null ||
    ProximalThumbZRotationRange == null ||
    IntermediateThumbXRotationRange == null ||
    DistalThumbXRotationRange == null ||

    ProximalIndexXRotationRange == null ||
    ProximalIndexZRotationRange == null ||
    IntermediateIndexXRotationRange == null ||
    DistalIndexXRotationRange == null ||

    ProximalMiddleXRotationRange == null ||
    ProximalMiddleZRotationRange == null ||
    IntermediateMiddleXRotationRange == null ||
    DistalMiddleXRotationRange == null ||

    ProximalRingXRotationRange == null ||
    ProximalRingZRotationRange == null ||
    IntermediateRingXRotationRange == null ||
    DistalRingXRotationRange == null ||

    ProximalLittleXRotationRange == null ||
    ProximalLittleZRotationRange == null ||
    IntermediateLittleXRotationRange == null ||
    DistalLittleXRotationRange == null ||

    ThumbHandle == null ||
    IndexHandle == null ||
    MiddleHandle == null ||
    RingHandle == null ||
    LittleHandle == null ||

    HandleA == null ||
    HandleB == null);

  public void SetDefaultValues () {
    ProximalThumb = null;
    IntermediateThumb = null;
    DistalThumb = null;

    ProximalIndex = null;
    IntermediateIndex = null;
    DistalIndex = null;

    ProximalMiddle = null;
    IntermediateMiddle = null;
    DistalMiddle = null;

    ProximalRing = null;
    IntermediateRing = null;
    DistalRing = null;

    ProximalLittle = null;
    IntermediateLittle = null;
    DistalLittle = null;

    ProximalThumbXRotationRange = Vector2.zero;
    ProximalThumbZRotationRange = Vector2.zero;
    IntermediateThumbXRotationRange = Vector2.zero;
    DistalThumbXRotationRange = Vector2.zero;

    ProximalIndexXRotationRange = Vector2.zero;
    ProximalIndexZRotationRange = Vector2.zero;
    IntermediateIndexXRotationRange = Vector2.zero;
    DistalIndexXRotationRange = Vector2.zero;

    ProximalMiddleXRotationRange = Vector2.zero;
    ProximalMiddleZRotationRange = Vector2.zero;
    IntermediateMiddleXRotationRange = Vector2.zero;
    DistalMiddleXRotationRange = Vector2.zero;

    ProximalRingXRotationRange = Vector2.zero;
    ProximalRingZRotationRange = Vector2.zero;
    IntermediateRingXRotationRange = Vector2.zero;
    DistalRingXRotationRange = Vector2.zero;

    ProximalLittleXRotationRange = Vector2.zero;
    ProximalLittleZRotationRange = Vector2.zero;
    IntermediateLittleXRotationRange = Vector2.zero;
    DistalLittleXRotationRange = Vector2.zero;

    ThumbHandle = null;
    IndexHandle = null;
    MiddleHandle = null;
    RingHandle = null;
    LittleHandle = null;

    HandleA = null;
    HandleB = null;
  }

}

public class HandConstraintBinder : AnimationJobBinder<HandConstraintJob, HandConstraintData> {

  public override HandConstraintJob Create (Animator animator, ref HandConstraintData data, Component component) {
    var job = new HandConstraintJob ();

    job.ProximalThumb = ReadWriteTransformHandle.Bind (animator, data.ProximalThumb);
    job.IntermediateThumb = ReadWriteTransformHandle.Bind (animator, data.IntermediateThumb);
    job.DistalThumb = ReadWriteTransformHandle.Bind (animator, data.DistalThumb);

    job.ProximalIndex = ReadWriteTransformHandle.Bind (animator, data.ProximalIndex);
    job.IntermediateIndex = ReadWriteTransformHandle.Bind (animator, data.IntermediateIndex);
    job.DistalIndex = ReadWriteTransformHandle.Bind (animator, data.DistalIndex);

    job.ProximalMiddle = ReadWriteTransformHandle.Bind (animator, data.ProximalMiddle);
    job.IntermediateMiddle = ReadWriteTransformHandle.Bind (animator, data.IntermediateMiddle);
    job.DistalMiddle = ReadWriteTransformHandle.Bind (animator, data.DistalMiddle);

    job.ProximalRing = ReadWriteTransformHandle.Bind (animator, data.ProximalRing);
    job.IntermediateRing = ReadWriteTransformHandle.Bind (animator, data.IntermediateRing);
    job.DistalRing = ReadWriteTransformHandle.Bind (animator, data.DistalRing);

    job.ProximalLittle = ReadWriteTransformHandle.Bind (animator, data.ProximalLittle);
    job.IntermediateLittle = ReadWriteTransformHandle.Bind (animator, data.IntermediateLittle);
    job.DistalLittle = ReadWriteTransformHandle.Bind (animator, data.DistalLittle);

    job.ProximalThumbXRotationRange = data.ProximalThumbXRotationRange;
    job.ProximalThumbZRotationRange = data.ProximalThumbZRotationRange;
    job.IntermediateThumbXRotationRange = data.IntermediateThumbXRotationRange;
    job.DistalThumbXRotationRange = data.DistalThumbXRotationRange;

    job.ProximalIndexXRotationRange = data.ProximalIndexXRotationRange;
    job.ProximalIndexZRotationRange = data.ProximalIndexZRotationRange;
    job.IntermediateIndexXRotationRange = data.IntermediateIndexXRotationRange;
    job.DistalIndexXRotationRange = data.DistalIndexXRotationRange;

    job.ProximalMiddleXRotationRange = data.ProximalMiddleXRotationRange;
    job.ProximalMiddleZRotationRange = data.ProximalMiddleZRotationRange;
    job.IntermediateMiddleXRotationRange = data.IntermediateMiddleXRotationRange;
    job.DistalMiddleXRotationRange = data.DistalMiddleXRotationRange;

    job.ProximalRingXRotationRange = data.ProximalRingXRotationRange;
    job.ProximalRingZRotationRange = data.ProximalRingZRotationRange;
    job.IntermediateRingXRotationRange = data.IntermediateRingXRotationRange;
    job.DistalRingXRotationRange = data.DistalRingXRotationRange;

    job.ProximalLittleXRotationRange = data.ProximalLittleXRotationRange;
    job.ProximalLittleZRotationRange = data.ProximalLittleZRotationRange;
    job.IntermediateLittleXRotationRange = data.IntermediateLittleXRotationRange;
    job.DistalLittleXRotationRange = data.DistalLittleXRotationRange;

    job.ThumbHandle = ReadWriteTransformHandle.Bind (animator, data.ThumbHandle);
    job.IndexHandle = ReadWriteTransformHandle.Bind (animator, data.IndexHandle);
    job.MiddleHandle = ReadWriteTransformHandle.Bind (animator, data.MiddleHandle);
    job.RingHandle = ReadWriteTransformHandle.Bind (animator, data.RingHandle);
    job.LittleHandle = ReadWriteTransformHandle.Bind (animator, data.LittleHandle);

    job.HandleA = ReadWriteTransformHandle.Bind (animator, data.HandleA);
    job.HandleB = ReadWriteTransformHandle.Bind (animator, data.HandleB);

    return job;
  }

  public override void Destroy (HandConstraintJob job) { }

}
