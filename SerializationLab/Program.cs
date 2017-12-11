using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Bird bird = new Bird() { Category = "Eagle", Color = "gray" };
            string file = "bird.bin";

            try
            {
                SerializeObj(file, bird);
                Bird bird1 = (Bird)DeserializeFrom(file);

                if (bird.Category == bird1.Category)
                    Console.WriteLine("same");
                else
                    Console.WriteLine("error!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("using DataContractSerializer...");
            Console.WriteLine();
            //File.Delete(file);
            string dcFile = "bird.xml";
            try
            {
                SerializeWithDataContract(dcFile, bird);
                Bird bird1 = (Bird)DataContractDeserializeFrom(dcFile, typeof(Bird));

                if (bird.Category == bird1.Category)
                    Console.WriteLine("DataContract same");
                else
                    Console.WriteLine("DataContract error!!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static Object DataContractDeserializeFrom(string fileName, Type type)
        {
            DataContractSerializer dcs = new DataContractSerializer(type);
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                Object obj = dcs.ReadObject(fs);
                return obj;
            }
        }

        static void SerializeWithDataContract(string fileName, Object obj)
        {
            DataContractSerializer dcs = new DataContractSerializer(obj.GetType());

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                dcs.WriteObject(fs, obj);
            }
        }

        static Object DeserializeFrom(string file)
        {
            IFormatter formater = new BinaryFormatter();
            using(Stream bs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                Object obj = formater.Deserialize(bs);
                return obj;
            }
        }

        static void SerializeObj(string fileName, object obj)
        {
            IFormatter bf = new BinaryFormatter();
            using (Stream bs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                
                bf.Serialize(bs, obj);
            }
        }
    }

    //[Serializable]
    [DataContract]
    class Bird
    {   
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Color { get; set; }
    }
}
