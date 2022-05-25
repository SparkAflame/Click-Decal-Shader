// 
// Copyright © 2020-2022 Kevin Preece.
// All rights reserved.
// 

using System.Collections.Generic;

using UnityEngine;


namespace SparkAflame.ClickShader
{
   /// <summary>
   ///    A very simple click decal manager that pools the decal instances.
   /// </summary>
   public sealed class PoolingClickDecalManager : BasicClickDecalManager
   {
      #region > > > > >   Private Fields   < < < < <

      private Queue < AnimateClickDecal > _decals;

      #endregion


      #region > > > > >   MonoBehaviour Callbacks   < < < < <

      protected override void Awake()
      {
         base.Awake();

         _decals =
            MaximumDecals > 0
               ? new Queue < AnimateClickDecal >( MaximumDecals )
               : new Queue < AnimateClickDecal >();
      }

      #endregion


      #region > > > > >   Methods   < < < < <

      /// <summary>
      ///    Hides the decal - this implementation hides it but leaves it in the scene so it can be reused.
      /// </summary>
      /// <param name="decal">
      ///    The decal to be hidden.
      /// </param>
      protected override void HideDecal( AnimateClickDecal decal )
      {
         decal.gameObject.SetActive( false ); // Disable so it doesn't consume CPU cycles when the decal is hidden.

         _decals.Enqueue( decal );
      }


      /// <summary>
      ///    Spawn a decal at the given position oriented using the given surface normal.  If there are decals available
      ///    in the pool then one of those is used, otherwise a new game object is spawned; it will get added to the
      ///    pool when it releases itself.
      /// </summary>
      /// <param name="pos">
      ///    The decal's spawn position.
      /// </param>
      /// <param name="normal">
      ///    The surface normal at the spawn point.
      /// </param>
      public override AnimateClickDecal SpawnAt( Vector3 pos, Vector3 normal )
      {
         if ( !CanShowDecal )
         {
            return null;
         }

         return _decals.Count > 0 ? ShowDecal( _decals.Dequeue(), pos, normal ) : base.SpawnAt( pos, normal );
      }

      #endregion
   }
}
