﻿using System;
using System.Diagnostics;
using TerraFX.Interop.DirectX;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using static TerraFX.Interop.Windows.Windows;

namespace Infinity.Graphics
{
#pragma warning disable CS8600, CS8602, CS8604, CS8618, CA1416
    internal unsafe class Dx12CommandBuffer : RHICommandBuffer
    {
        public ID3D12CommandAllocator* NativeCommandAllocator
        {
            get
            {
                return m_NativeCommandAllocator;
            }
        }
        public ID3D12GraphicsCommandList5* NativeCommandList
        {
            get
            {
                return m_NativeCommandList;
            }
        }

        private Dx12BlitEncoder m_BlitEncoder;
        private Dx12ComputeEncoder m_ComputeEncoder;
        private Dx12MeshletEncoder m_MeshletEncoder;
        private Dx12GraphicsEncoder m_GraphicsEncoder;
        private Dx12RaytracingEncoder m_RaytracingEncoder;
        private ID3D12CommandAllocator* m_NativeCommandAllocator;
        private ID3D12GraphicsCommandList5* m_NativeCommandList;

        public Dx12CommandBuffer(Dx12CommandQueue commandQueue)
        {
            m_CommandQueue = commandQueue;

            ID3D12CommandAllocator* commandAllocator;
            bool success = SUCCEEDED(commandQueue.Dx12Device.NativeDevice->CreateCommandAllocator(Dx12Utility.ConvertToDx12QueueType(commandQueue.Type), __uuidof<ID3D12CommandAllocator>(), (void**)&commandAllocator));
            Debug.Assert(success);
            m_NativeCommandAllocator = commandAllocator;

            ID3D12GraphicsCommandList5* commandList;
            success = SUCCEEDED(commandQueue.Dx12Device.NativeDevice->CreateCommandList(0, Dx12Utility.ConvertToDx12QueueType(commandQueue.Type), m_NativeCommandAllocator, null, __uuidof<ID3D12GraphicsCommandList5>(), (void**)&commandList));
            Debug.Assert(success);
            m_NativeCommandList = commandList;

            m_BlitEncoder = new Dx12BlitEncoder(this);
            m_ComputeEncoder = new Dx12ComputeEncoder(this);
            m_MeshletEncoder = new Dx12MeshletEncoder(this);
            m_GraphicsEncoder = new Dx12GraphicsEncoder(this);
            m_RaytracingEncoder = new Dx12RaytracingEncoder(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Begin(string name)
        {
            m_NativeCommandAllocator->Reset();
            m_NativeCommandList->Reset(m_NativeCommandAllocator, null);

            IntPtr namePtr = Marshal.StringToHGlobalUni(name);
            m_NativeCommandList->BeginEvent(0, namePtr.ToPointer(), (uint)name.Length * 2);
            Marshal.FreeHGlobal(namePtr);

            Dx12CommandQueue commandQueue = m_CommandQueue as Dx12CommandQueue;
            ID3D12DescriptorHeap** resourceBarriers = stackalloc ID3D12DescriptorHeap*[2];
            resourceBarriers[0] = commandQueue.Dx12Device.SamplerHeap.DescriptorHeap;
            resourceBarriers[1] = commandQueue.Dx12Device.CbvSrvUavHeap.DescriptorHeap;
            m_NativeCommandList->SetDescriptorHeaps(2, &*resourceBarriers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIBlitEncoder BeginBlitPass(string name)
        {
            m_BlitEncoder.BeginPass(name);
            return m_BlitEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void EndBlitPass()
        {
            m_BlitEncoder.EndPass();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIComputeEncoder BeginComputePass(string name)
        {
            m_ComputeEncoder.BeginPass(name);
            return m_ComputeEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void EndComputePass()
        {
            m_ComputeEncoder.EndPass();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIRaytracingEncoder BeginRaytracingPass(string name)
        {
            m_RaytracingEncoder.BeginPass(name);
            return m_RaytracingEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void EndRaytracingPass()
        {
            m_RaytracingEncoder.EndPass();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIMeshletEncoder BeginMeshletPass(in RHIMeshletPassDescriptor descriptor)
        {
            m_MeshletEncoder.BeginPass(descriptor);
            return m_MeshletEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void EndMeshletPass()
        {
            m_MeshletEncoder.EndPass();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIGraphicsEncoder BeginGraphicsPass(in RHIGraphicsPassDescriptor descriptor)
        {
            m_GraphicsEncoder.BeginPass(descriptor);
            return m_GraphicsEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void EndGraphicsPass()
        {
            m_GraphicsEncoder.EndPass();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void End()
        {
            m_NativeCommandList->EndEvent();
            m_NativeCommandList->Close();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIBlitEncoder GetBlitEncoder()
        {
            return m_BlitEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIComputeEncoder GetComputeEncoder()
        {
            return m_ComputeEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIRaytracingEncoder GetRaytracingEncoder()
        {
            return m_RaytracingEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIMeshletEncoder GetMeshletEncoder()
        {
            return m_MeshletEncoder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override RHIGraphicsEncoder GetGraphicsEncoder()
        {
            return m_GraphicsEncoder;
        }

        /*public override void Commit(RHIFence? fence)
        {
            Dx12CommandAllocator dx12CommandPool = m_CommandPool as Dx12CommandAllocator;
            Dx12CommandQueue commandQueue = m_CommandPool.CommandQueue as Dx12CommandQueue;

            ID3D12CommandList** ppCommandLists = stackalloc ID3D12CommandList*[1] { (ID3D12CommandList*)m_NativeCommandList };
            commandQueue.NativeCommandQueue->ExecuteCommandLists(1, ppCommandLists);

            if (fence != null)
            {
                Dx12Fence dx12Fence = fence as Dx12Fence;
                dx12Fence.Reset();
                commandQueue.NativeCommandQueue->Signal(dx12Fence.NativeFence, 1);
            }
        }*/

        protected override void Release()
        {
            m_NativeCommandList->Release();
            m_NativeCommandAllocator->Release();
        }
    }

    internal unsafe class Dx12IndirectCommandBuffer : RHIIndirectCommandBuffer
    {
        public Dx12IndirectCommandBuffer(Dx12Device device, in RHIIndirectCommandBufferDescription descriptor)
        {

        }

        protected override void Release()
        {

        }
    }
#pragma warning restore CS8600, CS8602, CS8604, CS8618, CA1416
}
