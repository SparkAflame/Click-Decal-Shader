// 
// Copyright © 2020-2022 Kevin Preece.
// All rights reserved.
// 

using System.Text;

using UnityEngine;


namespace SparkAflame.ClickShader
{
   /// <summary>
   ///    Manages showing decals to indicate the position of a mouse click; multiple decals may be spawned.  This
   ///    implementation creates and destroys objects on demand.  A subclass could provide object pooling.
   /// </summary>
   public class BasicClickDecalManager : MonoBehaviour
   {
      #region > > > > >   Editor Exposed Fields   < < < < <

      [Tooltip( "The decal prefab." )]
      [SerializeField]
      private AnimateClickDecal _decalPrefab;

      [Tooltip( "The maximum number of simultaneous decals this manager can spawn." )]
      [SerializeField]
      private int _maximumDecals = 3;

#if DEBUG
      [SerializeField]
      private bool _showDebug;
#endif

      #endregion


      #region > > > > >   Properties   < < < < <

      protected bool CanShowDecal => ( _numberAllocated < _maximumDecals ) || ( _maximumDecals < 0 );

      protected int MaximumDecals
      {
         get => _maximumDecals;
         set => _maximumDecals = value;
      }

      #endregion


      #region > > > > >   Private Fields   < < < < <

      private int _numberAllocated;  // Number currently allocated.
      private int _totalAllocations; // Total number allocated.
      private int _totalReleases;    // Total number released.

      #endregion


      #region > > > > >   MonoBehaviour Callbacks   < < < < <

      protected virtual void Awake()
      {
         // Nothing here.
      }


#if DEBUG
      private void OnDestroy()
      {
         if ( _showDebug )
         {
            Debug.Log(
               new StringBuilder()
                  .Append( name )
                  .Append( " : Currently Active = " )
                  .Append( _numberAllocated.ToString() )
                  .Append( ", Total Allocations = " )
                  .Append( _totalAllocations.ToString() )
                  .Append( ", Total Releases = " )
                  .Append( _totalReleases.ToString() )
                  .ToString()
            );
         }
      }
#endif

      #endregion


      #region > > > > >   Methods   < < < < <

      public virtual void Cancel()
      {
         // Do nothing.
      }


      /// <summary>
      ///    Hides the decal - this implementation deletes it from the scene.
      /// </summary>
      /// <param name="decal">
      ///    The decal to be removed.
      /// </param>
      protected virtual void HideDecal( AnimateClickDecal decal )
      {
         Destroy( decal.gameObject );
      }


      /// <summary>
      ///    Must be called by the prefab script when the decal finishes playing to release it back to this manager.
      ///    This implementation just destroys the object created in <see cref="SpawnAt" />.
      /// </summary>
      /// <param name="decal">
      ///    The decal being released.
      /// </param>
      public void ReleaseDecal( AnimateClickDecal decal )
      {
         ++_totalReleases;
         --_numberAllocated;

         HideDecal( decal );
      }


      /// <summary>
      ///    Call when a decal is to be shown.
      /// </summary>
      /// <param name="decal">
      ///    The decal instance.
      /// </param>
      /// <param name="pos">
      ///    The decal's position.
      /// </param>
      /// <param name="normal">
      ///    The surface normal at <see cref="pos" />.
      /// </param>
      /// <returns></returns>
      protected AnimateClickDecal ShowDecal( AnimateClickDecal decal, Vector3 pos, Vector3 normal )
      {
         ++_numberAllocated;
         ++_totalAllocations;

         decal.gameObject.SetActive( true );
         decal.Show( pos, normal, this );

         return decal;
      }


      /// <summary>
      ///    Spawn a decal at the given position oriented using the given surface normal.
      /// </summary>
      /// <param name="pos">
      ///    The decal's spawn position.
      /// </param>
      /// <param name="normal">
      ///    The surface normal at the spawn point.
      /// </param>
      public virtual AnimateClickDecal SpawnAt( Vector3 pos, Vector3 normal )
      {
         if ( null == _decalPrefab )
         {
            Debug.LogError( "Unable to spawn the mouse click decal - no prefab provided." );
         }
         else
         {
            if ( CanShowDecal )
            {
               AnimateClickDecal decal = Instantiate( _decalPrefab, pos, Quaternion.identity, transform );

               if ( null != decal )
               {
                  return ShowDecal( decal, pos, normal );
               }

               Debug.LogError( "Unable to spawn the mouse click decal - instantiation failed." );
            }
         }

         return null;
      }

      #endregion
   }
}
