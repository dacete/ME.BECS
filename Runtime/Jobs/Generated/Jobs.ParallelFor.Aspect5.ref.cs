namespace ME.BECS.Jobs {
    
    using static Cuts;
    using Unity.Jobs;
    using Unity.Jobs.LowLevel.Unsafe;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Burst;

    public static unsafe partial class QueryAspectScheduleExtensions {
        
        public static JobHandle ScheduleParallelFor<T, T0,T1,T2,T3,T4>(this QueryBuilder builder, in T job = default) where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect {
            builder.WithAspect<T0>(); builder.WithAspect<T1>(); builder.WithAspect<T2>(); builder.WithAspect<T3>(); builder.WithAspect<T4>();
            builder.builderDependsOn = builder.SetEntities(builder.commandBuffer, builder.builderDependsOn);
            builder.builderDependsOn = job.ScheduleParallelFor<T, T0,T1,T2,T3,T4>(in builder.commandBuffer, builder.parallelForBatch, builder.builderDependsOn);
            builder.builderDependsOn = builder.Dispose(builder.builderDependsOn);
            return builder.builderDependsOn;
        }
        
        public static JobHandle ScheduleParallelFor<T, T0,T1,T2,T3,T4>(this Query staticQuery, in T job, in SystemContext context) where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect {
            return staticQuery.ScheduleParallelFor<T, T0,T1,T2,T3,T4>(in job, in context.world, context.dependsOn);
        }
        
        public static JobHandle ScheduleParallelFor<T, T0,T1,T2,T3,T4>(this Query staticQuery, in T job, in World world, JobHandle dependsOn = default) where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect {
            var state = world.state;
            var query = API.MakeStaticQuery(QueryContext.Create(state, world.id), dependsOn).FromQueryData(state, world.id, state->queries.GetPtr(state, staticQuery.id));
            return query.ScheduleParallelFor<T, T0,T1,T2,T3,T4>(in job);
        }

        public static JobHandle ScheduleParallelFor<T, T0,T1,T2,T3,T4>(this QueryBuilderDisposable staticQuery, in T job) where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect {
            staticQuery.builderDependsOn = job.ScheduleParallelFor<T, T0,T1,T2,T3,T4>(in staticQuery.commandBuffer, staticQuery.parallelForBatch, staticQuery.builderDependsOn);
            staticQuery.builderDependsOn = staticQuery.Dispose(staticQuery.builderDependsOn);
            return staticQuery.builderDependsOn;
        }
        
    }

    public static partial class EarlyInit {
        public static void DoParallelForAspect<T, T0,T1,T2,T3,T4>()
                where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect
                where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> => JobParallelForAspectExtensions.JobEarlyInitialize<T, T0,T1,T2,T3,T4>();
    }

    [JobProducerType(typeof(JobParallelForAspectExtensions.JobProcess<,,,,,>))]
    public interface IJobParallelForAspect<T0,T1,T2,T3,T4> : IJobParallelForAspectBase where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect {
        void Execute(ref T0 c0,ref T1 c1,ref T2 c2,ref T3 c3,ref T4 c4);
    }

    public static unsafe partial class JobParallelForAspectExtensions {
        
        public static void JobEarlyInitialize<T, T0,T1,T2,T3,T4>() where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> => JobProcess<T, T0,T1,T2,T3,T4>.Initialize();
        
        private static System.IntPtr GetReflectionData<T, T0,T1,T2,T3,T4>() where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> {
            JobProcess<T, T0,T1,T2,T3,T4>.Initialize();
            System.IntPtr reflectionData = JobProcess<T, T0,T1,T2,T3,T4>.jobReflectionData.Data;
            return reflectionData;
        }

        public static JobHandle ScheduleParallelFor<T, T0,T1,T2,T3,T4>(this T jobData, in CommandBuffer* buffer, uint innerLoopBatchCount, JobHandle dependsOn = default)
            where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect
            where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> {
            
            if (innerLoopBatchCount == 0u) innerLoopBatchCount = JobUtils.GetScheduleBatchCount(buffer->count);

            buffer->sync = false;
            var data = new JobData<T, T0,T1,T2,T3,T4>() {
                jobData = jobData,
                buffer = buffer,
                c0 = buffer->state->aspectsStorage.Initialize<T0>(buffer->state),c1 = buffer->state->aspectsStorage.Initialize<T1>(buffer->state),c2 = buffer->state->aspectsStorage.Initialize<T2>(buffer->state),c3 = buffer->state->aspectsStorage.Initialize<T3>(buffer->state),c4 = buffer->state->aspectsStorage.Initialize<T4>(buffer->state),
            };
            
            var parameters = new JobsUtility.JobScheduleParameters(_address(ref data), GetReflectionData<T, T0,T1,T2,T3,T4>(), dependsOn, ScheduleMode.Parallel);
            return JobsUtility.ScheduleParallelForDeferArraySize(ref parameters, (int)innerLoopBatchCount, (byte*)buffer, null);

        }

        private struct JobData<T, T0,T1,T2,T3,T4>
            where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect
            where T : struct {
            [NativeDisableUnsafePtrRestriction]
            public T jobData;
            [NativeDisableUnsafePtrRestriction]
            public CommandBuffer* buffer;
            public T0 c0;public T1 c1;public T2 c2;public T3 c3;public T4 c4;
        }

        internal struct JobProcess<T, T0,T1,T2,T3,T4>
            where T0 : unmanaged, IAspect where T1 : unmanaged, IAspect where T2 : unmanaged, IAspect where T3 : unmanaged, IAspect where T4 : unmanaged, IAspect
            where T : struct, IJobParallelForAspect<T0,T1,T2,T3,T4> {

            internal static readonly Unity.Burst.SharedStatic<System.IntPtr> jobReflectionData = Unity.Burst.SharedStatic<System.IntPtr>.GetOrCreate<JobProcess<T, T0,T1,T2,T3,T4>>();

            [BurstDiscard]
            public static void Initialize() {
                if (jobReflectionData.Data == System.IntPtr.Zero) {
                    jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(JobData<T, T0,T1,T2,T3,T4>), typeof(T), (ExecuteJobFunction)Execute);
                }
            }

            private delegate void ExecuteJobFunction(ref JobData<T, T0,T1,T2,T3,T4> jobData, System.IntPtr bufferPtr, System.IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

            private static void Execute(ref JobData<T, T0,T1,T2,T3,T4> jobData, System.IntPtr bufferPtr, System.IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex) {

                var aspect0 = jobData.c0;var aspect1 = jobData.c1;var aspect2 = jobData.c2;var aspect3 = jobData.c3;var aspect4 = jobData.c4;
                while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out var begin, out var end) == true) {
                    
                    jobData.buffer->BeginForEachRange((uint)begin, (uint)end);
                    for (uint i = (uint)begin; i < end; ++i) {
                        var entId = *(jobData.buffer->entities + i);
                        var gen = jobData.buffer->state->entities.GetGeneration(jobData.buffer->state, entId);
                        aspect0.ent = new Ent(entId, gen, jobData.buffer->worldId);aspect1.ent = new Ent(entId, gen, jobData.buffer->worldId);aspect2.ent = new Ent(entId, gen, jobData.buffer->worldId);aspect3.ent = new Ent(entId, gen, jobData.buffer->worldId);aspect4.ent = new Ent(entId, gen, jobData.buffer->worldId);
                        jobData.jobData.Execute(ref aspect0,ref aspect1,ref aspect2,ref aspect3,ref aspect4);
                    }
                    jobData.buffer->EndForEachRange();
                    
                }

            }
        }
    }
    
}