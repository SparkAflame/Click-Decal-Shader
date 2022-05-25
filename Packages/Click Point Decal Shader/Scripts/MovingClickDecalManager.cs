// 
// Copyright © 2020-2022 Kevin Preece.
// All rights reserved.
// 

using UnityEngine;


namespace SparkAflame.ClickShader
{
   public class MovingClickDecalManager : BasicClickDecalManager
   {
      #region > > > > >   Private Fields   < < < < <

      private AnimateClickDecal _decal;

      #endregion


      #region > > > > >   MonoBehaviour Callbacks   < < < < <

      protected override void Awake()
      {
         base.Awake();

         MaximumDecals = 1;
         _decal        = base.SpawnAt( Vector3.zero, Vector3.up );

         HideDecal( _decal );
      }

      #endregion


      #region > > > > >   Methods   < < < < <

      public override void Cancel()
      {
         base.Cancel();
         
         if ( _decal.isActiveAndEnabled )
         {
            ReleaseDecal( _decal );
         }
      }


      /// <summary>
      ///    Hides the decal - this implementation hides it but leaves it in the scene so it can be reused.
      /// </summary>
      /// <param name="decal">
      ///    The decal to be hidden.
      /// </param>
      protected override void HideDecal( AnimateClickDecal decal )
      {
         decal.gameObject.SetActive( false ); // Disable so it doesn't consume CPU cycles when the decal is hidden.

         decal.Reset();
      }


      /// <summary>
      ///    Spawn a decal at the given position oriented using the given surface normal.  There is only one decal so
      ///    the behaviour is:
      ///
      ///      - If the decal is inactive then activate it at the given position.
      ///      - If the decal is active then move it to the given position without interrupting the animation.
      ///
      ///    The effect stays active for as long as the method is called, e.g. for as long as a mouse button is pressed.
      /// </summary>
      /// <param name="pos">
      ///    The decal's position.
      /// </param>
      /// <param name="normal">
      ///    The surface normal at the given position.
      /// </param>
      public override AnimateClickDecal SpawnAt( Vector3 pos, Vector3 normal )
      {
         if ( !_decal.isActiveAndEnabled )
         {
            return ShowDecal( _decal, pos, normal );
         }

         _decal.Move( pos, normal );

         return _decal;
      }

      #endregion
   }
}
