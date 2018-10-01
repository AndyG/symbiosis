// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DoorManager : MonoBehaviour
// {

//   [SerializeField]
//   private CameraTrack cameraTrack;

//   // Use this for initialization
//   void Start()
//   {

//   }

//   // Update is called once per frame
//   void Update()
//   {

//   }

//   public void Enter(Door door, Transform actor)
//   {
//     Door exitDoor = door.GetOtherSide();
//     Vector3 exitDoorPosition = exitDoor.transform.position;
//     Vector3 targetPosition = new Vector3(exitDoorPosition.x, exitDoorPosition.y + exitDoor.GetOffsetFromGround(), exitDoorPosition.z);
//     MoveTo(actor.transform, targetPosition.x, targetPosition.y);
//     if (actor.transform.gameObject.tag == "Player")
//     {
//       cameraTrack.SnapImmediate(targetPosition.x, targetPosition.y);
//     }
//   }

//   private void MoveTo(Transform transform, float x, float y)
//   {
//     transform.position = new Vector3(x, y, transform.position.z);
//   }
// }
