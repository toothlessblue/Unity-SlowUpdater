using System;

namespace Unity_SlowUpdater
{
    public struct RateFunctionPair
    {
        public RateFunctionPair(uint rate, Action function) {
            this.function = function;
            this.rate = rate;
        }

        public Action function;
        public uint rate;
    }
}