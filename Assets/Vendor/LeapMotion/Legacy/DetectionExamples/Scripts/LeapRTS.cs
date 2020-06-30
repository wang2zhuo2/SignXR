/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2018.                                 *
 * Leap Motion proprietary and confidential.                                  *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/

using UnityEngine;
using System;

namespace Leap.Unity {

  /// <summary>
  /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
  /// allows rotation, translation, and scale of the object (RTS).
  /// </summary>
  public class LeapRTS : MonoBehaviour {

    public enum RotationMethod {
      None,
      Single,
      Full
    }

    [SerializeField]
    private PinchDetector _pinchDetectorA;
    public PinchDetector PinchDetectorA {
      get {
        return _pinchDetectorA;
      }
      set {
        _pinchDetectorA = value;
      }
    }

    [SerializeField]
    private PinchDetector _pinchDetectorB;
    public PinchDetector PinchDetectorB {
      get {
        return _pinchDetectorB;
      }
      set {
        _pinchDetectorB = value;
      }
    }

    [SerializeField]
    private RotationMethod _oneHandedRotationMethod;

    [SerializeField]
    private RotationMethod _twoHandedRotationMethod;

    [SerializeField]
    private bool _allowScale = true;

    [Header("GUI Options")]
    [SerializeField]
    private KeyCode _toggleGuiState = KeyCode.None;

    [SerializeField]
    private bool _showGUI = true;

    private Transform _anchor;

    private float _defaultNearClip;

        // public Transform handleSphere;
        // public Transform planeReference1;
        // public Transform planeReference2;
        // public Transform planeReference3;
        // public GameObject scaleLimitation;
        public Transform rotationAnchor;


        void Start() {
            //      if (_pinchDetectorA == null || _pinchDetectorB == null) {
            //        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
            //        enabled = false;
            //      }

            GameObject pinchControl = new GameObject("RTS Anchor");
      _anchor = pinchControl.transform;
      _anchor.transform.parent = transform.parent;
      transform.parent = _anchor; // model's parent = RTS Anchor's transform
        }

    void Update() {
      if (Input.GetKeyDown(_toggleGuiState)) {
        _showGUI = !_showGUI;
      }

      bool didUpdate = false;
      if(_pinchDetectorA != null)
        didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
      if(_pinchDetectorB != null)
        didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

      if (didUpdate) {
        transform.SetParent(null, true);
      }

      if (_pinchDetectorA != null && _pinchDetectorA.IsActive && 
          _pinchDetectorB != null &&_pinchDetectorB.IsActive) {
        transformDoubleAnchor();
      } /*else if (_pinchDetectorA != null && _pinchDetectorA.IsActive) {
        transformSingleAnchor(_pinchDetectorA);
      } else if (_pinchDetectorB != null && _pinchDetectorB.IsActive) {
        transformSingleAnchor(_pinchDetectorB);
      }*/

      if (didUpdate) {
        transform.SetParent(_anchor, true);
      }
    }

    void OnGUI() {
      if (_showGUI) {
        GUILayout.Label("One Handed Settings");
        doRotationMethodGUI(ref _oneHandedRotationMethod);
        GUILayout.Label("Two Handed Settings");
        doRotationMethodGUI(ref _twoHandedRotationMethod);
        _allowScale = GUILayout.Toggle(_allowScale, "Allow Two Handed Scale");
      }
    }

    private void doRotationMethodGUI(ref RotationMethod rotationMethod) {
      GUILayout.BeginHorizontal();

      GUI.color = rotationMethod == RotationMethod.None ? Color.green : Color.white;
      if (GUILayout.Button("No Rotation")) {
        rotationMethod = RotationMethod.None;
      }

      GUI.color = rotationMethod == RotationMethod.Single ? Color.green : Color.white;
      if (GUILayout.Button("Single Axis")) {
        rotationMethod = RotationMethod.Single;
      }

      GUI.color = rotationMethod == RotationMethod.Full ? Color.green : Color.white;
      if (GUILayout.Button("Full Rotation")) {
        rotationMethod = RotationMethod.Full;
      }

      GUI.color = Color.white;

      GUILayout.EndHorizontal();
    }

    private void transformDoubleAnchor() {

            /*
            //define the anchor position
            var plane = new Plane(planeReference1.position, planeReference2.position, planeReference3.position);//new
            Vector3 pinchAnchor = (_pinchDetectorA.Position + _pinchDetectorB.Position) / 2.0f;//new
            Vector3 projectionPoint = plane.ClosestPointOnPlane(pinchAnchor);//new      
            _anchor.position = (pinchAnchor + projectionPoint) / 2.0f;//new */

            _anchor.position = (_pinchDetectorA.Position + _pinchDetectorB.Position) / 2.0f; //rotationAnchor.transform.position;

            //this function is only activated in the scope of circle and fist is not detected;
         /*   if (Vector3.Distance(transform.position, pinchAnchor) < 
          Vector3.Distance(transform.position,handleSphere.transform.position)*1.1f
          && !DetectionManager.Get().GetHand(EHand.eLeftHand).IsClosed(0.8f)
          && !DetectionManager.Get().GetHand(EHand.eRightHand).IsClosed(0.8f) )  
            { */

            switch (_twoHandedRotationMethod)
            {
        case RotationMethod.None:
          break;
        case RotationMethod.Single:
          Vector3 p = _pinchDetectorA.Position;
          p.y = _anchor.position.y;
          _anchor.LookAt(p);
          break;
        case RotationMethod.Full:
          Quaternion pp = Quaternion.Lerp(_pinchDetectorA.Rotation, _pinchDetectorB.Rotation, 0.5f);
          Vector3 u = pp * Vector3.up;
          _anchor.LookAt(_pinchDetectorA.Position, u);
          break;
            }



                if (_allowScale)
                {
                    /* Vector3 oldScale = _anchor.localScale;

                     float newDistance = Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);*/
                    _anchor.localScale = Vector3.one * Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);

 //                   if (transform.localScale.x < 1000.0f)
 //                   {
 //                       scaleLimitation.SetActive(false); //warning text   
 //                   }
 //                   else
 //                   {
  //                      scaleLimitation.SetActive(true);

                       /* if (newDistance > oldDistance)
                        {
                            _anchor.localScale = oldScale;
                            Debug.Log("bigger");
                        }
                        if (newDistance <= oldDistance)
                        {
                            _anchor.localScale = Vector3.one * Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);
                            Debug.Log("smaller");
                        }
                         oldDistance = newDistance;*/
  //                  }
 //               }
                }
        }

        /*  private void transformSingleAnchor(PinchDetector singlePinch) {
            _anchor.position = singlePinch.Position;

            switch (_oneHandedRotationMethod) {
              case RotationMethod.None:
                break;
              case RotationMethod.Single:
                Vector3 p = singlePinch.Rotation * Vector3.right;
                p.y = _anchor.position.y;
                _anchor.LookAt(p);
                break;
              case RotationMethod.Full:
                _anchor.rotation = singlePinch.Rotation;
                break;
            }

            _anchor.localScale = Vector3.one;
          }*/
    }
}
