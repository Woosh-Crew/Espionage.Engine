  using UnityEngine;
    using System.Collections;
 
 namespace Espionage.Engine.Viewmodels{
    public class SetWeaponProjectionMatrix : Behaviour
    {
        public Camera SourceCam;
 
        private Matrix4x4 prevProjMatrix;
        private Matrix4x4 lastView;
 
        private Camera cam;
 
		void Update(){
			prevProjMatrix = SourceCam.projectionMatrix;
            lastView = SourceCam.transform.worldToLocalMatrix;
		}


        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            prevProjMatrix = SourceCam.projectionMatrix;
            lastView = SourceCam.worldToCameraMatrix;
 
            Shader.SetGlobalMatrix("_PrevCustomVP", prevProjMatrix * lastView);
            Shader.SetGlobalMatrix("_CustomProjMatrix", GL.GetGPUProjectionMatrix(SourceCam.projectionMatrix, false));
 
            Graphics.Blit(src, dest);
        }        
    }
 }
