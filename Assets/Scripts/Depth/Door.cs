// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Door : MonoBehaviour
// {

//   [SerializeField]
//   private Door otherSide;

//   [SerializeField]
//   private float offsetFromGround;

//   private DoorManager doorManager;

//   // Use this for initialization
//   void Start()
//   {
//     doorManager = FindObjectOfType<DoorManager>();
//   }

//   // Update is called once per frame
//   void Update()
//   {

//   }

//   public void Enter(Transform actor)
//   {
//     doorManager.Enter(this, actor);
//   }

//   public Door GetOtherSide()
//   {
//     return otherSide;
//   }

//   public float GetOffsetFromGround()
//   {
//     return offsetFromGround;
//   }
// }
