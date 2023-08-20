﻿using System;
using Infinity.Core;

namespace Infinity.Graphics
{
    public struct RHIDeviceProperty
    {
        public EDeviceType Type;
        public uint VendorId;
        public uint DeviceId;
    }

    public abstract class RHIDevice : Disposal
    {
        public abstract bool IsRaytracingSupported { get; }
        public abstract bool IsRaytracingQuerySupported { get; }
        public abstract bool IsFlipProjectionRequired { get; }
        public abstract EClipDepth ClipDepth { get; }
        public abstract EMatrixMajorness MatrixMajorness { get; }
        public abstract EMultiviewStrategy MultiviewStrategy { get; }

        public abstract RHIDeviceProperty GetDeviceProperty();
        public abstract RHIFence CreateFence();
        public abstract RHISemaphore CreateSemaphore();
        public abstract RHIQuery CreateQuery(in RHIQueryDescriptor descriptor);
        public abstract RHIHeap CreateHeap(in RHIHeapDescription descriptor);
        public abstract RHIBuffer CreateBuffer(in RHIBufferDescriptor descriptor);
        public abstract RHITexture CreateTexture(in RHITextureDescriptor descriptor);
        public abstract RHISampler CreateSampler(in RHISamplerDescriptor descriptor);
        public abstract RHICommandQueue CreateCommandQueue(in EQueueType type);
        public abstract RHITopLevelAccelStruct CreateAccelerationStructure(in RHITopLevelAccelStructDescriptor descriptor);
        public abstract RHIBottomLevelAccelStruct CreateAccelerationStructure(in RHIBottomLevelAccelStructDescriptor descriptor);
        public abstract RHIFunction CreateFunction(in RHIFunctionDescriptor descriptor);
        public abstract RHISwapChain CreateSwapChain(in RHISwapChainDescriptor descriptor);
        public abstract RHIBindTableLayout CreateBindTableLayout(in RHIBindTableLayoutDescriptor descriptor);
        public abstract RHIBindTable CreateBindTable(in RHIBindTableDescriptor descriptor);
        public abstract RHIPipelineLayout CreatePipelineLayout(in RHIPipelineLayoutDescriptor descriptor);
        public abstract RHIComputePipelineState CreateComputePipelineState(in RHIComputePipelineStateDescriptor descriptor);
        public abstract RHIRaytracingPipelineState CreateRaytracingPipelineState(in RHIRaytracingPipelineStateDescriptor descriptor);
        public abstract RHIMeshletPipelineState CreateMeshletPipelineState(in RHIMeshletPipelineStateDescriptor descriptor);
        public abstract RHIGraphicsPipelineState CreateGraphicsPipelineState(in RHIGraphicsPipelineStateDescriptor descriptor);
    }
}
