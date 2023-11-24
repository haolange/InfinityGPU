﻿using System;
using SharpMetal.Metal;
using SharpMetal.ObjectiveCCore;

namespace Infinity.Graphics
{
#pragma warning disable CS8600, CS8602
    internal unsafe class MtlCommandQueue : RHICommandQueue
    {
        public MTLCommandQueue NativeQueue
        {
            get
            {
                return m_NativeQueue;
            }
        }
        public override ulong Frequency
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private MtlDevice m_MtlDevice;
        private MTLCommandQueue m_NativeQueue;

        public MtlCommandQueue(MtlDevice device, in ERHIPipeline pipeline)
        {
            m_MtlDevice = device;
            m_PipelineType = pipeline;
        }

        public override RHICommandBuffer CreateCommandBuffer()
        {
            throw new NotImplementedException();
        }

        public override void MapTiledTexture(in RHITiledTextureRegions tiledTextureRegions)
        {
            throw new NotImplementedException();
        }

        public override void UnMapTiledTexture(in RHITiledTextureRegions tiledTextureRegions)
        {
            throw new NotImplementedException();
        }

        public override void MapPackedMips(in RHITiledTexturePackedMips tiledTexturePackedMips)
        {
            throw new NotImplementedException();
        }

        public override void UnMapPackedMips(in RHITiledTexturePackedMips tiledTexturePackedMips)
        {
            throw new NotImplementedException();
        }

        public override void Submit(RHICommandBuffer cmdBuffer, RHIFence signalFence, RHISemaphore waitSemaphore, RHISemaphore signalSemaphore)
        {
            throw new NotImplementedException();
        }

        public override void Submits(RHICommandBuffer[] cmdBuffers, RHIFence signalFence, RHISemaphore[] waitSemaphores, RHISemaphore[] signalSemaphores)
        {
            throw new NotImplementedException("ToDo : batch submit");
        }

        protected override void Release()
        {
            ObjectiveCRuntime.release(m_NativeQueue.NativePtr);
        }
    }
#pragma warning restore CS8600, CS8602
}
