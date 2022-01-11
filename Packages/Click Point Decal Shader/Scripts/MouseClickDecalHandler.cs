/*
 * MIT License
 *
 * Copyright (c) 2022 SparkAflame
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace SparkAflame.ClickShader
{
   /// <summary>
   ///   Shows the mouse click point by displaying a decal using a custom decal shader.
   /// </summary>
   public class MouseClickDecalHandler : MonoBehaviour
   {
      private const string TimeOffsetPropertyName = "_TimeOffset";


#pragma warning disable IDE0044 // Add readonly modifier

      [Tooltip( "Should the decal be shown when a mouse click is detected?" )]
      [SerializeField]
      private bool _showClick = true;

      [Tooltip( "How many times should the effect play if the mouse button is pressed momentarily?" )]
      [SerializeField]
      private float _showDuration = 1.0f;

      [Tooltip( "How fast to play the animation." )]
      [SerializeField]
      private float _speed = 1.0f;

      [Tooltip(
         "If enabled the effect plays continuously while the mouse button is pressed, otherwise it only plays once."
      )]
      [SerializeField]
      private bool _playContinuously;

      // [Header( "For Debugging Only" )]
      // [Tooltip(
      //    "If enabled the effect plays even if the mouse button is not pressed."
      // )]
      // [SerializeField]
      private bool _alwaysShow;

#pragma warning restore IDE0044 // Add readonly modifier


      private DecalProjector _decal;
      private Material       _material;                // URP decal projector's material.
      private Camera         _camera;                  // Main camera.
      private float          _decalTimeOffset;         // How far through the effect we are.
      private int            _timeOffsetPropertyIndex; // Shader property ID: "_TimeOffset".
      private bool           _wasMousePressed;         // Used to control one-shot or continuous mode.


      private void Start()
      {
         _decal                   = GetComponent < DecalProjector >();
         _material                = _decal.material;
         _decal.enabled           = false;
         _timeOffsetPropertyIndex = Shader.PropertyToID( TimeOffsetPropertyName );
         _camera                  = Camera.main;
      }


      private void Update()
      {
         if ( !_showClick )
         {
            // There's nothing to do so just bail out.

            return;
         }

         // First update the effect if the decal projector is projecting.

         if ( _decal.enabled )
         {
            _decalTimeOffset += Time.deltaTime * _speed;

            if ( _decalTimeOffset > _showDuration )
            {
               _decal.enabled   = false;
               _decalTimeOffset = 0.0f;
            }

            _material.SetFloat( _timeOffsetPropertyIndex, _decalTimeOffset );
         }

         // Now deal with any mouse input, update the effect for as long as the mouse button is pressed.

         if ( _alwaysShow || Input.GetMouseButton( 0 ) )
         {
            // Convert the mouse position (in screen coordinates) to the corresponding world position so we can...

            Vector3 screenPos = Input.mousePosition;
            Ray     worldPos  = _camera.ScreenPointToRay( screenPos );

            // ... cast a ray through that point to see whether the "terrain" was clicked on and not empty space.

            if ( Physics.Raycast( worldPos, out RaycastHit hit ) )
            {
               // 'twas terrain.

               Play( hit.point, hit.normal, _wasMousePressed );
            }

            _wasMousePressed = true;
         }
         else
         {
            _wasMousePressed = false;
         }
      }


      /// <summary>
      ///   Called when the object is destroyed by Unity.  Unregister all event callbacks.
      /// </summary>
      private void OnDestroy()
      {
         if ( null != _material )
         {
            _material.SetFloat( _timeOffsetPropertyIndex, 0.0f );
         }
      }


      /// <summary>
      ///   Called by the events system when an item tagged as ground is clicked on.
      /// </summary>
      /// <param name="clickPoint">
      ///   The world-space position of the mouse click.
      /// </param>
      /// <param name="clickNormal">
      ///   The normal to the surface at the click point.
      /// </param>
      /// <param name="wasMousePressed">
      ///   Was the mouse button down during the last frame.  Used to limit the effect to running only once regardless
      ///   of how long the mouse button is pressed.
      /// </param>
      private void Play( Vector3 clickPoint, Vector3 clickNormal, bool wasMousePressed )
      {
         Transform decalTransform      = _decal.transform;
         float     halfProjectionDepth = _decal.size.z * 0.5f;

         // Move the decal projector if the mouse button is kept pressed.
         // The following code expects the terrain to be reasonably smooth (with the default decal projector Projection
         // Depth) or that the projection depth has been adjusted to cover the changes in terrain height.  It also
         // rotates the decal projector to match clickNormal - i.e. the normal to the surface at the click point.

         decalTransform.position = clickPoint + ( clickNormal * halfProjectionDepth );
         decalTransform.forward  = -clickNormal;

         if ( !_decal.enabled && ( _playContinuously || _alwaysShow || !wasMousePressed ) )
         {
            _decal.enabled   = _showClick;
            _decalTimeOffset = 0.0f;
         }
      }
   }
}
