using System;
using UnityEngine.Rendering.Universal;

namespace CustomRendering
{
    using UnityEngine;
    using System.Runtime.InteropServices;

    public class NewBehaviourScript : MonoBehaviour
    {
        private bool _textureSent;
        
        [DllImport("__Internal")]
        private static extern void BindWebGLTexture(int texture);
        
        [DllImport("__Internal")]
        private static extern void CallExternal(int texture);
        

        void Update()
        {
            TrySendTexture();
            transform.Rotate(Time.deltaTime * 2f, Time.deltaTime * -22f, Time.deltaTime * 11f);
        }

        private void TrySendTexture()
        {
            try
            {
                if (!_textureSent)
                {
                    if (StaticProps.TextureId == -1)
                        return;

                    _textureSent = true;
                    
                    Debug.Log("Sending Texture Native Pointer " + StaticProps.TextureId);
                    
                    CallExternal(StaticProps.TextureId);
                    
                    Debug.Log("Sent successfully");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}