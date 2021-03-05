using UnityEngine.Rendering.Universal;

namespace CustomRendering
{
    public class CustomRenderPassFeature : ScriptableRendererFeature
    {
        CustomRenderPass _scriptablePass;

        public override void Create()
        {
            _scriptablePass = new CustomRenderPass();

            // Configures where the render pass should be injected.
            _scriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        }

        // Here you can inject one or multiple render passes in the renderer.
        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_scriptablePass);
        }
    }
}

