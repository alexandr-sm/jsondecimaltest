using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonDecimalTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testobj = new TestObject { _int = 3, _decimalp0 = 5M, _decimalp1 = 6.0M, _doublep0 = 7D, _str = "hello" };
            string serialized = JsonConvert.SerializeObject(testobj);
            Console.WriteLine(serialized);
            var deserialazed = JsonConvert.DeserializeObject<TestObject>(serialized);

            string serialized1 = JsonConvert.SerializeObject(testobj, new DecimalFormatConverter());
            Console.WriteLine(serialized1);
            var deserialazed1 = JsonConvert.DeserializeObject<TestObject>(serialized1);
        }
    }

    class TestObject
    {
        public int _int { get; set; }
        public decimal _decimalp0 { get; set; }
        public decimal _decimalp1 { get; set; }
        public double _doublep0 { get; set; }
        public string _str { get; set; }
    }

    public class DecimalFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal));
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            //writer.WriteValue(value.ToString()); //This will work to but serialize nubmers as stirngs 5 => "5". 
            if (value.GetType() == typeof(decimal))
            {
                var precision = (Decimal.GetBits((decimal)value)[3] >> 16) & 0x000000FF;
                if(precision == 0)
                {
                    writer.WriteValue(Convert.ToInt64(value));
                }
                else
                {
                    writer.WriteValue(value);
                }
            }
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType,
                                     object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
