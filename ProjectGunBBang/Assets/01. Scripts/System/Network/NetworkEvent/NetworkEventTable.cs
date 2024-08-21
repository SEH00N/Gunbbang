using System;
using System.Collections.Generic;
using Unity.Collections;

namespace GB.NetworkEvents
{
    internal static class NetworkEventTable
    {
        private static Dictionary<ulong, Dictionary<ulong, INetworkEvent>> eventTable;
        private static Dictionary<FixedString128Bytes, Func<byte[], NetworkEventParams>> paramsFactories;

        static NetworkEventTable()
        {
            eventTable = new Dictionary<ulong, Dictionary<ulong, INetworkEvent>>();
            paramsFactories = new Dictionary<FixedString128Bytes, Func<byte[], NetworkEventParams>>();

            RegisterParamsFactory<NoneParams>();
            RegisterParamsFactory<IntParams>();
            RegisterParamsFactory<FloatParams>();
            RegisterParamsFactory<UlongParams>();
            RegisterParamsFactory<BoolParams>();
            RegisterParamsFactory<Vector3Params>();
            RegisterParamsFactory<TransformParams>();
            RegisterParamsFactory<AttackParams>();
            RegisterParamsFactory<StateParams>();
        }

        public static void RegisterParamsFactory<T>() where T : NetworkEventParams, new() => paramsFactories.Add(typeof(T).ToString(), ParamsFactory<T>);
        public static INetworkEvent GetEvent(ulong instanceID, ulong eventID) 
        {
            if(eventTable.ContainsKey(instanceID) == false)
                return null;
            
            if(eventTable[instanceID].ContainsKey(eventID) == false)
                return null;

            return eventTable[instanceID][eventID];
        }
        public static NetworkEventParams GetEventParams(FixedString128Bytes paramsID, byte[] buffer) => paramsFactories[paramsID]?.Invoke(buffer);

        public static void RegisterEvent(ulong instanceID, INetworkEvent networkEvent)
        {
            if(eventTable.ContainsKey(instanceID) == false)
                eventTable.Add(instanceID, new Dictionary<ulong, INetworkEvent>());

            eventTable[instanceID][networkEvent.EventID] = networkEvent;
        }

        public static void UnregisterEvent(ulong instanceID, INetworkEvent networkEvent)
        {
            if(eventTable.ContainsKey(instanceID) == false)
                return;

            eventTable[instanceID].Remove(networkEvent.EventID);
            if(eventTable[instanceID].Count <= 0)
                eventTable.Remove(instanceID);
        }

        public static NetworkEventParams ParamsFactory<T>(byte[] buffer) where T : NetworkEventParams, new()
        {
            T eventParams = new T();
            eventParams.Deserialize(buffer);

            return eventParams;
        }

        public static ulong StringToHash(string key)
        {
            const ulong FNV_offset_basis = 14695981039346656037UL;
            const ulong FNV_prime = 1099511628211UL;

            ulong hash = FNV_offset_basis;

            foreach (char c in key)
            {
                hash ^= c;
                hash *= FNV_prime;
            }

            return hash;
        }
    }
}
