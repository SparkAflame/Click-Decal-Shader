// 
// Copyright © 2020-2022 Kevin Preece.
// All rights reserved.
// 

using UnityEngine;


namespace SparkAflame.ClickShader.Demo
{
   public class SimpleMouseButtonHandler : MonoBehaviour
   {
      [SerializeField]
      private Camera _mainCamera;

      [SerializeField]
      private BasicClickDecalManager _decalManager;


      private void Start()
      {
         if ( ( null == _mainCamera ) || ( null == _decalManager ) )
         {
            enabled = false;
         }
      }


      private void Update()
      {
         bool pressedThisFrame = Input.GetMouseButtonDown( 0 );
         bool moveWithPointer  = Input.GetMouseButton( 0 ) && _decalManager is MovingClickDecalManager;

         if ( pressedThisFrame || moveWithPointer )
         {
            Ray ray = _mainCamera.ScreenPointToRay( Input.mousePosition );

            if ( Physics.Raycast( ray, out RaycastHit hit, float.PositiveInfinity, ~0, QueryTriggerInteraction.Ignore ) )
            {
               if ( pressedThisFrame )
               {
                  _decalManager.Cancel();
               }
               _decalManager.SpawnAt( hit.point, hit.normal );
            }
         }
      }
   }
}
