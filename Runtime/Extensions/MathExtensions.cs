namespace ME.BECS {
    
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    using Unity.Mathematics;

    public static class Math {

        [INLINE(256)]
        public static float3 MoveTowards(float3 current, float3 target, float maxDistanceDelta) {
            var delta = target - current;
            var sqDist = math.dot(delta, delta);
            if (sqDist <= maxDistanceDelta * maxDistanceDelta) return target;
            var dist = math.sqrt(sqDist);
            return current + delta / dist * maxDistanceDelta;
        }

    }

}