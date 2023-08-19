using System;
using Infinity.Core;
using Infinity.Mathmatics;
using TerraFX.Interop.Gdiplus;

namespace Infinity.Graphics
{
    public struct RHIIndirectDispatchArgs
    {
        public uint GroupCountX;
        public uint GroupCountY;
        public uint GroupCountZ;
        public RHIIndirectDispatchArgs(in uint groupCountX, in uint groupCountY, in uint groupCountZ)
        {
            GroupCountX = groupCountX;
            GroupCountY = groupCountY;
            GroupCountZ = groupCountZ;
        }
    }

    public struct RHIIndirectDrawArgs
    {
        public uint VertexCount;
        public uint InstanceCount;
        public uint StartVertexLocation;
        public uint StartInstanceLocation;
        public RHIIndirectDrawArgs(in uint vertexCount, in uint instanceCount, in uint startVertexLocation, in uint startInstanceLocation)
        {
            VertexCount = vertexCount;
            InstanceCount = instanceCount;
            StartVertexLocation = startVertexLocation;
            StartInstanceLocation = startInstanceLocation;
        }
    }

    public struct RHIIndirectDrawIndexedArgs
    {
        public uint IndexCount;
        public uint InstanceCount;
        public uint StartIndexLocation;
        public int BaseVertexLocation;
        public uint StartInstanceLocation;
        public RHIIndirectDrawIndexedArgs(in uint indexCount, in uint instanceCount, in uint startIndexLocation, in int baseVertexLocation, in uint startInstanceLocation)
        {
            IndexCount = indexCount;
            InstanceCount = instanceCount;
            StartIndexLocation = startIndexLocation;
            BaseVertexLocation = baseVertexLocation;
            StartInstanceLocation = startInstanceLocation;
        }
    }

    public struct RHIBufferCopyDescriptor
    {
        public uint Offset;
        public uint RowPitch;
        public uint3 TextureHeight;
        public RHIBuffer Buffer;
    }

    public struct RHITextureCopyDescriptor
    {
        public uint MipLevel;
        public uint SliceBase;
        public uint SliceCount;
        public uint3 Origin;
        public RHITexture Texture;
    }

    public struct RHITimestampDescriptor
    {
        public RHIQuery Query;
        public uint BeginIndex;
        public uint EndIndex;
    }

    public struct RHIOcclusionDescriptor
    {
        public RHIQuery Query;
    }

    public struct RHIStatisticsDescriptor
    {
        public RHIQuery Query;
        public uint WriteIndex;
    }

    public struct RHISubpassDescriptor
    {
        public bool bUseDepthStencil;
        public ReadOnlyMemory<byte>? InputAttachmentIndex;
        public ReadOnlyMemory<byte>? OutputAttachmentIndex;
    }

    public struct RHIColorAttachmentDescriptor
    {
        public uint MipIndex;
        public uint ArraySlice;
        public float4 ClearValue;
        public ELoadAction LoadAction;
        public EStoreAction StoreAction;
        public RHITexture RenderTarget;
        public RHITexture ResolveTarget;
    }

    public struct RHIDepthStencilAttachmentDescriptor
    {
        public uint MipIndex;
        public uint ArraySlice;
        public bool DepthReadOnly;
        public float DepthClearValue;
        public ELoadAction DepthLoadOp;
        public EStoreAction DepthStoreOp;
        public bool StencilReadOnly;
        public int StencilClearValue;
        public ELoadAction StencilLoadOp;
        public EStoreAction StencilStoreOp;
        public RHITexture RenderTarget;
        public RHITexture ResolveTarget;
    }

    public struct RHITransferPassDescriptor
    {
        public string Name;
        public RHITimestampDescriptor? Timestamp;
    }

    public struct RHIComputePassDescriptor
    {
        public string Name;
        public RHITimestampDescriptor? Timestamp;
        public RHIStatisticsDescriptor? Statistics;
    }

    public struct RHIRayTracingPassDescriptor
    {
        public string Name;
        public RHITimestampDescriptor? Timestamp;
        public RHIStatisticsDescriptor? Statistics;
    }

    public struct RHIMeshletPassDescriptor
    {
        public string Name;
        public uint ArrayLength;
        public uint MultiViewCount;
        public ESampleCount SampleCount;
        public RHITimestampDescriptor? Timestamp;
        public RHIStatisticsDescriptor? Statistics;
        public RHITexture ShadingRateTexture;
        //public Memory<RHISubpassDescriptor>? SubpassDescriptors;
        public Memory<RHIColorAttachmentDescriptor> ColorAttachments;
        public RHIDepthStencilAttachmentDescriptor? DepthStencilAttachment;
    }

    public struct RHIGraphicsPassDescriptor
    {
        public string Name;
        public uint ArrayLength;
        public uint MultiViewCount;
        public ESampleCount SampleCount;
        public RHITimestampDescriptor? Timestamp;
        public RHIOcclusionDescriptor? Occlusion;
        public RHIStatisticsDescriptor? Statistics;
        public RHITexture ShadingRateTexture;
        //public Memory<RHISubpassDescriptor>? SubpassDescriptors;
        public Memory<RHIColorAttachmentDescriptor> ColorAttachments;
        public RHIDepthStencilAttachmentDescriptor? DepthStencilAttachment;
    }

    public abstract class RHITransferEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;

        internal abstract void BeginPass(in RHITransferPassDescriptor descriptor);
        public abstract void BeginQuery(RHIQuery query, in uint index);
        public abstract void EndQuery(RHIQuery query, in uint index);
        public abstract void ResolveQuery(RHIQuery query, in uint index, in uint count);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void ResourceBarrier(in RHIBarrier barrier);
        public abstract void ResourceBarriers(in Memory<RHIBarrier> barriers);
        public abstract void CopyBufferToBuffer(RHIBuffer srcBuffer, in int srcOffset, RHIBuffer dstBuffer, in int dstOffset, in int size);
        public abstract void CopyBufferToTexture(in RHIBufferCopyDescriptor src, in RHITextureCopyDescriptor dst, in int3 size);
        public abstract void CopyTextureToBuffer(in RHITextureCopyDescriptor src, in RHIBufferCopyDescriptor dst, in int3 size);
        public abstract void CopyTextureToTexture(in RHITextureCopyDescriptor src, in RHITextureCopyDescriptor dst, in int3 size);
        public abstract void EndPass();
    }

    public abstract class RHIComputeEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;
        protected RHIPipelineLayout? m_PipelineLayout;
        protected RHIComputePipeline? m_Pipeline;

        internal abstract void BeginPass(in RHIComputePassDescriptor descriptor);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void BeginQuery(RHIQuery query, in uint index);
        public abstract void EndQuery(RHIQuery query, in uint index);
        public abstract void ResourceBarrier(in RHIBarrier barrier);
        public abstract void ResourceBarriers(in Memory<RHIBarrier> barriers);
        public abstract void SetPipelineLayout(RHIPipelineLayout pipelineLayout);
        public abstract void SetPipeline(RHIComputePipeline pipeline);
        public abstract void SetBindTable(RHIBindTable bindTable, in uint tableIndex);
        public abstract void Dispatch(in uint groupCountX, in uint groupCountY, in uint groupCountZ);
        public abstract void DispatchIndirect(RHIBuffer argsBuffer, in uint argsOffset);
        // TODO public abstract void ExecuteBundles(RHIIndirectCommandBuffer indirectCommandBuffer);
        public abstract void EndPass();
    }

    public abstract class RHIRaytracingEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;
        protected RHIPipelineLayout? m_PipelineLayout;
        protected RHIRaytracingPipeline? m_Pipeline;

        internal abstract void BeginPass(in RHIRayTracingPassDescriptor descriptor);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void BeginQuery(RHIQuery query, in uint index);
        public abstract void EndQuery(RHIQuery query, in uint index);
        public abstract void ResourceBarrier(in RHIBarrier barrier);
        public abstract void ResourceBarriers(in Memory<RHIBarrier> barriers);
        public abstract void SetPipelineLayout(RHIPipelineLayout pipelineLayout);
        public abstract void SetPipeline(RHIRaytracingPipeline pipeline);
        public abstract void SetBindTable(RHIBindTable bindTable, in uint tableIndex);
        public abstract void BuildAccelerationStructure(RHITopLevelAccelStruct tlas);
        public abstract void BuildAccelerationStructure(RHIBottomLevelAccelStruct blas);
        public abstract void Dispatch(in uint width, in uint height, in uint depth, RHIFunctionTable functionTable);
        public abstract void DispatchIndirect(RHIBuffer argsBuffer, in uint argsOffset, RHIFunctionTable functionTable);
        // TODO public abstract void ExecuteBundles(RHIIndirectCommandBuffer indirectCommandBuffer);
        public abstract void EndPass();
    }
    
    public abstract class RHIMeshletEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;
        protected RHIPipelineLayout? m_PipelineLayout;
        protected RHIMeshletPipeline? m_Pipeline;

        internal abstract void BeginPass(in RHIMeshletPassDescriptor descriptor);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void BeginQuery(RHIQuery query, in uint index);
        public abstract void EndQuery(RHIQuery query, in uint index);
        public abstract void ResourceBarrier(in RHIBarrier barrier);
        public abstract void ResourceBarriers(in Memory<RHIBarrier> barriers);
        public abstract void SetScissor(in Rect rect);
        public abstract void SetScissors(in Memory<Rect> rects);
        public abstract void SetViewport(in Viewport viewport);
        public abstract void SetViewports(in Memory<Viewport> viewports);
        public abstract void SetStencilRef(in uint value);
        public abstract void SetBlendFactor(in float4 value);
        //public abstract void NextSubpass();
        public abstract void SetPipelineLayout(RHIPipelineLayout pipelineLayout);
        public abstract void SetPipeline(RHIMeshletPipeline pipeline);
        public abstract void SetBindTable(RHIBindTable bindTable, in uint tableIndex);
        public abstract void SetShadingRate(in EShadingRate shadingRate, in EShadingRateCombiner shadingRateCombiner);
        public abstract void Dispatch(in uint groupCountX, in uint groupCountY, in uint groupCountZ);
        public abstract void DispatchIndirect(RHIBuffer argsBuffer, in uint argsOffset);
        // TODO public abstract void ExecuteBundles(RHIIndirectCommandBuffer indirectCommandBuffer);
        public abstract void EndPass();
    }

    public abstract class RHIGraphicsEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;
        protected RHIPipelineLayout? m_PipelineLayout;
        protected RHIGraphicsPipeline? m_Pipeline;

        internal abstract void BeginPass(in RHIGraphicsPassDescriptor descriptor);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void BeginQuery(RHIQuery query, in uint index);
        public abstract void EndQuery(RHIQuery query, in uint index);
        public abstract void ResourceBarrier(in RHIBarrier barrier);
        public abstract void ResourceBarriers(in Memory<RHIBarrier> barriers);
        public abstract void SetScissor(in Rect rect);
        public abstract void SetScissors(in Memory<Rect> rects);
        public abstract void SetViewport(in Viewport viewport);
        public abstract void SetViewports(in Memory<Viewport> viewports);
        public abstract void SetStencilRef(in uint value);
        public abstract void SetBlendFactor(in float4 value);
        //public abstract void NextSubpass();
        public abstract void SetPipelineLayout(RHIPipelineLayout pipelineLayout);
        public abstract void SetPipeline(RHIGraphicsPipeline pipeline);
        public abstract void SetBindTable(RHIBindTable bindTable, in uint tableIndex);
        public abstract void SetIndexBuffer(RHIBuffer buffer, in uint offset, in EIndexFormat format);
        public abstract void SetVertexBuffer(RHIBuffer buffer, in uint slot, in uint offset);
        public abstract void SetShadingRate(in EShadingRate shadingRate, in EShadingRateCombiner shadingRateCombiner);
        public abstract void Draw(in uint vertexCount, in uint instanceCount, in uint firstVertex, in uint firstInstance);
        public abstract void DrawIndexed(in uint indexCount, in uint instanceCount, in uint firstIndex, in uint baseVertex, in uint firstInstance);
        public abstract void DrawIndirect(RHIBuffer argsBuffer, in uint offset);
        public abstract void DrawIndexedIndirect(RHIBuffer argsBuffer, in uint offset);
        // TODO public abstract void ExecuteBundles(RHIIndirectCommandBuffer indirectCommandBuffer);
        public abstract void EndPass();
    }
}
