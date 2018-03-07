using System;
using System.Collections.Immutable;
using Orleans.CodeGeneration;
using Orleans.Serialization;

namespace Rides.Grains
{
    [Serializer(typeof(ImmutableList<Car>))]
    public class ImmutableListSerializer
    {
        [CopierMethod]
        public static object DeepCopier(object original, ICopyContext context)
        {
            //throw new Exception();
            var input = (ImmutableList<Car>) original;
            var result = ImmutableList<Car>.Empty;
            context.RecordCopy(original, result);
            result.AddRange(input);
            return result;
        }

        [SerializerMethod]
        public static void Serializer(object untypedInput, ISerializationContext context, Type expected)
        {
            var input = (ImmutableList<Car>) untypedInput;
            SerializationManager.SerializeInner(input.Count, context, expected);
            foreach (var car in input)
            {
                SerializationManager.SerializeInner(car, context, expected);
            }
        }

        [DeserializerMethod]
        public static object Deserializer(Type expected, IDeserializationContext context)
        {
            var result = ImmutableList<Car>.Empty;
            context.RecordObject(result);
            var count = SerializationManager.DeserializeInner<int>(context);
            for (var i = 0; i < count; i++)
            {
                var car = SerializationManager.DeserializeInner<Car>(context);
                result.Add(car);
            }

            return result;
        }
    }
}