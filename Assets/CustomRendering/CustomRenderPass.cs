using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CustomRendering
{
    public class CustomRenderPass : ScriptableRenderPass
    {
        private RTHandle _buffer1;

        public void SetUp()
        {
            _buffer1 = RTHandles.Alloc(1024, 1024, 1,
                dimension: TextureXR.dimension,
                colorFormat: GraphicsFormat.R8G8B8A8_UNorm,
                    
                useDynamicScale: false, name: "_SharedRenderTexture"
            );
                
            StaticProps.TextureId = _buffer1.rt.GetNativeTexturePtr().ToInt32();
        }
        
        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            if (_buffer1 == null)
                return;

            ConfigureTarget(_buffer1.nameID);
            ConfigureClear(ClearFlag.All, Color.black);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_buffer1 == null)
                return;

            
            if (renderingData.cameraData.isSceneViewCamera)
                return;
            
            CommandBuffer cmd = CommandBufferPool.Get("RenderCustom++");
            
            Profiler.BeginSample("+++ Custom Pass");
            //CoreUtils.SetRenderTarget(cmd, _buffer1, ClearFlag.Color);
            var rd = renderingData;
            var drs = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref rd, SortingCriteria.CommonOpaque);
            
            FilteringSettings filterSettings = new FilteringSettings();
            filterSettings.renderQueueRange = RenderQueueRange.all;
            filterSettings.layerMask = -1;
            filterSettings.renderingLayerMask = 0xFFFFFFFF;
            filterSettings.sortingLayerRange = SortingLayerRange.all;
            
            context.ExecuteCommandBuffer(cmd);
            
            context.DrawRenderers(renderingData.cullResults, ref drs, ref filterSettings);
            Profiler.EndSample();
            
            
            CommandBufferPool.Release(cmd);
        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {

        }
    }
}