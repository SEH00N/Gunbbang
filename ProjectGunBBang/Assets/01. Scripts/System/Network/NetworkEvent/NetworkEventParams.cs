using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace GB.NetworkEvents
{
    internal struct NetworkEventPacket : INetworkSerializable
    {
        public ulong InstanceID;
        public ulong EventID;
        public FixedString128Bytes ParamsID;
        public byte[] Buffer;

        public NetworkEventPacket(ulong instanceID, ulong eventID, FixedString128Bytes paramsID, byte[] buffer)
        {
            InstanceID = instanceID;
            EventID = eventID;
            ParamsID = paramsID;
            Buffer = buffer;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ParamsID);
            serializer.SerializeValue(ref InstanceID);
            serializer.SerializeValue(ref EventID);
            serializer.SerializeValue(ref Buffer);
        }
    }

    public abstract class NetworkEventParams
    {
        protected abstract ushort Size { get; }

        public void Deserialize(byte[] buffer)
        {
            FastBufferReader bufferReader = new FastBufferReader(buffer, Allocator.Temp);
            if(bufferReader.TryBeginRead(Size) == false)
                throw new OverflowException("Not enough space in the buffer");

            Deserialize(bufferReader);
        }

        public byte[] Serialize()
        {
            FastBufferWriter bufferWriter = new FastBufferWriter(Size, Allocator.Temp);
            if(bufferWriter.TryBeginWrite(Size) == false)
                throw new OverflowException("Not enough space in the buffer");

            Serialize(bufferWriter);
            return bufferWriter.ToArray();
        }

        protected abstract void Deserialize(FastBufferReader reader);
        protected abstract void Serialize(FastBufferWriter writer);
    }

    public class NoneParams : NetworkEventParams, IConvertible<NoneParams>
    {
        protected override ushort Size => 0;

        public NoneParams Convert() => this;

        protected override void Deserialize(FastBufferReader reader) { }
        protected override void Serialize(FastBufferWriter writer) { }
    }

    public class IntParams : NetworkEventParams, IConvertible<IntParams>, IConvertible<int>
    {
        protected override ushort Size => sizeof(int);
        public int Value;

        IntParams IConvertible<IntParams>.Convert() => this;
        int IConvertible<int>.Convert() => Value;

        public IntParams() { }
        public IntParams(int value) { Value = value; }
        protected override void Deserialize(FastBufferReader reader) { reader.ReadValue(out Value); }
        protected override void Serialize(FastBufferWriter writer) { writer.WriteValue(Value); }

        public static implicit operator int(IntParams left) => left.Value;
        public static implicit operator IntParams(int left) => new IntParams(left);
    }

    public class FloatParams : NetworkEventParams, IConvertible<FloatParams>, IConvertible<float>
    {
        protected override ushort Size => sizeof(float);
        public float Value;

        FloatParams IConvertible<FloatParams>.Convert() => this;
        float IConvertible<float>.Convert() => Value;

        public FloatParams() { }
        public FloatParams(float value) { Value = value; }
        protected override void Deserialize(FastBufferReader reader) { reader.ReadValue(out Value); }
        protected override void Serialize(FastBufferWriter writer) { writer.WriteValue(Value); }

        public static implicit operator float(FloatParams left) => left.Value;
        public static implicit operator FloatParams(float left) => new FloatParams(left);
    }

    public class UlongParams : NetworkEventParams, IConvertible<UlongParams>, IConvertible<ulong>
    {
        protected override ushort Size => sizeof(ulong);
        public ulong Value;

        UlongParams IConvertible<UlongParams>.Convert() => this;
        ulong IConvertible<ulong>.Convert() => Value;

        public UlongParams() { }
        public UlongParams(ulong value) { Value = value; }
        protected override void Deserialize(FastBufferReader reader) { reader.ReadValue(out Value); }
        protected override void Serialize(FastBufferWriter writer) { writer.WriteValue(Value); }

        public static implicit operator ulong(UlongParams left) => left.Value;
        public static implicit operator UlongParams(ulong left) => new UlongParams(left);
    }

    public class BoolParams : NetworkEventParams, IConvertible<BoolParams>, IConvertible<bool>
    {
        protected override ushort Size => sizeof(bool);
        public bool Value;

        BoolParams IConvertible<BoolParams>.Convert() => this;
        bool IConvertible<bool>.Convert() => Value;

        public BoolParams() { }
        public BoolParams(bool value) { Value = value; }
        protected override void Deserialize(FastBufferReader reader) { reader.ReadValue(out Value); }
        protected override void Serialize(FastBufferWriter writer) { writer.WriteValue(Value); }

        public static implicit operator bool(BoolParams left) => left.Value;
        public static implicit operator BoolParams(bool left) => new BoolParams(left);
    }

    public class Vector3Params : NetworkEventParams, IConvertible<Vector3Params>, IConvertible<Vector3>
    {
        protected override ushort Size => sizeof(float) * 3;
        public Vector3 Value;

        Vector3Params IConvertible<Vector3Params>.Convert() => this;
        Vector3 IConvertible<Vector3>.Convert() => Value;

        public Vector3Params() { }
        public Vector3Params(Vector3 value) { Value = value; }
        protected override void Deserialize(FastBufferReader reader) { reader.ReadValue(out Value); }
        protected override void Serialize(FastBufferWriter writer) { writer.WriteValue(Value); }

        public static implicit operator Vector3(Vector3Params left) => left.Value;
        public static implicit operator Vector3Params(Vector3 left) => new Vector3Params(left);
    }

    public class TransformParams : NetworkEventParams, IConvertible<TransformParams>
    {
        protected override ushort Size => sizeof(float) * 6;
        public Vector3 Position;
        public Vector3 Rotation;

        public TransformParams Convert() => this;

        public TransformParams() { }
        public TransformParams(Vector3 position, Vector3 rotation) 
        { 
            Position = position; 
            Rotation = rotation; 
        }

        protected override void Deserialize(FastBufferReader reader) 
        { 
            reader.ReadValue(out Position);
            reader.ReadValue(out Rotation); 
        }

        protected override void Serialize(FastBufferWriter writer)
        {
            writer.WriteValue(Position);
            writer.WriteValue(Rotation);
        }

        public static implicit operator TransformParams(Transform left) => new TransformParams(left.position, left.eulerAngles);
    }

    public class AttackParams : NetworkEventParams, IConvertible<AttackParams>
    {
        protected override ushort Size => sizeof(ulong) + sizeof(float) + sizeof(float) * 3 * 3 + sizeof(int);
        public ulong AttackerID;
        public int EffectType;
        public float Damage;
        public Vector3 Point;
        public Vector3 Dir;
        public Vector3 Normal;

        public AttackParams Convert() => this;

        public AttackParams() { }
        public AttackParams(ulong attackerID, int effectType, float damage, Vector3 point, Vector3 dir, Vector3 normal) 
        {
            AttackerID = attackerID;
            Damage = damage;
            Point = point;
            Dir = dir;
            Normal = normal;
            EffectType = effectType;
        }

        protected override void Deserialize(FastBufferReader reader) 
        { 
            reader.ReadValue(out AttackerID);
            reader.ReadValue(out Damage);
            reader.ReadValue(out EffectType);
            reader.ReadValue(out Point);
            reader.ReadValue(out Dir);
            reader.ReadValue(out Normal);
        }

        protected override void Serialize(FastBufferWriter writer)
        {
            writer.WriteValue(AttackerID);
            writer.WriteValue(Damage);
            writer.WriteValue(EffectType);
            writer.WriteValue(Point);
            writer.WriteValue(Dir);
            writer.WriteValue(Normal);
        }
    }

    public class StateParams : NetworkEventParams, IConvertible<StateParams>
    {
        protected override ushort Size => sizeof(int) + sizeof(ulong);

        public int State = 0;
        public ulong ClientID = 0;

        public StateParams Convert() => this;

        public StateParams() {}
        public StateParams(int state, ulong clientID)
        {
            State = state;
            ClientID = clientID;
        }

        protected override void Deserialize(FastBufferReader reader)
        {
            reader.ReadValue(out State);
            reader.ReadValue(out ClientID);
        }

        protected override void Serialize(FastBufferWriter writer)
        {
            writer.WriteValue(State);
            writer.WriteValue(ClientID);
        }
    }
}
