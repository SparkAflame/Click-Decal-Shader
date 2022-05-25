// 
// Copyright © 2020-2022 Kevin Preece.
// All rights reserved.
// 

using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace SparkAflame.ClickShader
{
   /// <summary>
   ///    Shows the mouse click point by displaying a decal.  This can be a custom, animated decal shader in URP 12+.
   /// </summary>
   public sealed class AnimateClickDecal : MonoBehaviour
   {
      private const string TimeOffsetPropertyName = "_TimeOffset";


      #region > > > > >   Script Editor Properties   < < < < <

#pragma warning disable IDE0044 // Add readonly modifier

      [Tooltip( "How many times should the effect play?" )]
      [SerializeField]
      private float _showDuration = 1.0f;

      [Tooltip( "How fast to play the animation." )]
      [SerializeField]
      private float _speed = 1.0f;

#pragma warning restore IDE0044 // Add readonly modifier

      #endregion


      #region > > > > >   Private Fields   < < < < <

      private DecalProjector         _decal;
      private Material               _material;
      private Material               _savedMaterial;
      private BasicClickDecalManager _manager;
      private float                  _decalTimeOffset;
      private int                    _timeOffsetPropertyIndex;
      private bool                   _isRunning;

      #endregion


      #region > > > > >   MonoBehaviour Callbacks   < < < < <

      private void Awake()
      {
         if ( null == _savedMaterial )
         {
            _decal                   = GetComponent < DecalProjector >();
            _timeOffsetPropertyIndex = Shader.PropertyToID( TimeOffsetPropertyName );

            // While accessing Renderer.material creates a new instance of the material, DecalProjector.material does
            // not.  Don't you just love consistency?
            // We require each decal instance to have its own material instance so that they animate independently of
            // each other.

            _savedMaterial  = _decal.material;
            _material       = Instantiate( _savedMaterial );
            _decal.material = _material;
         }
      }


      private void OnDestroy()
      {
         if ( null != _savedMaterial )
         {
            _decal.material = _savedMaterial;

            Destroy( _material );

            _decal         = null;
            _manager       = null;
            _material      = null;
            _savedMaterial = null;
         }
      }


      private void LateUpdate()
      {
         _decalTimeOffset += Time.deltaTime * _speed;

         if ( _decalTimeOffset > _showDuration )
         {
            if ( null != _manager )
            {
               _isRunning = false;
               _manager.ReleaseDecal( this );
            }
         }
         else
         {
            _material.SetFloat( _timeOffsetPropertyIndex, _decalTimeOffset );
         }
      }

      #endregion


      #region > > > > >   Methods   < < < < <

      public void Move( Vector3 pos, Vector3 normal )
      {
         if ( _isRunning )
         {
            Transform decalTransform = _decal.transform;

            decalTransform.position = pos + ( normal * 0.5f );
            decalTransform.forward  = -normal;
         }
      }


      public void Reset()
      {
         _decalTimeOffset = 0.0f;
         _isRunning       = false;
      }


      /// <summary>
      ///    Call this method when the decal is to be shown.
      /// </summary>
      /// <param name="pos">
      ///    The decal's initial position.
      /// </param>
      /// <param name="normal">
      ///    The surface normal at <see cref="pos" />.  This is used to orient the decal to the surface.
      /// </param>
      /// <param name="manager">
      ///    The Decal Manager managing this decal.
      /// </param>
      public void Show( Vector3 pos, Vector3 normal, BasicClickDecalManager manager )
      {
         if ( !_isRunning )
         {
            Transform decalTransform = _decal.transform;

            decalTransform.position = pos + ( normal * 0.5f );
            decalTransform.forward  = -normal;
            _manager                = manager;
            _decalTimeOffset        = 0.0f;
            _isRunning              = true;
         }
      }

      #endregion
   }
}
