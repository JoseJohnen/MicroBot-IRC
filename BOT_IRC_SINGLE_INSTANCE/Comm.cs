using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace BOT_IRC_SINGLE_INSTANCE
{
    public class Comm
    {
        [BsonId]
        public ObjectId Id
        {
            get;
            set;
        }

        [BsonElement("name")]
        public string Name
        {
            get;
            set;
        }

        [BsonElement("comment")]
        public string Comment
        {
            get;
            set;
        }

        [BsonElement("origen")]
        public string Origen
        {
            get;
            set;
        }

        [BsonElement("Public")]
        public bool isPublic
        {
            get;
            set;
        }

        [BsonElement("FechaHora")]
        public DateTime FechaHora
        {
            get;
            set;
        }

        public string ToJSON()
        {
            try
            {                
                string strTemp =
                "{"+
                "\"Id\" : "+ this.Id.ToJson()+
                "\"Name\" : " + this.Name +
                "\"Comment\" : " + this.Comment +
                "\"Origen\" : " + this.Origen +
                "\"isPublic\" : " + this.isPublic +
                "\"FechaHora\" : " + this.FechaHora.ToString() +
                "}";
                return strTemp;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error en ToJSON(): "+"\n"+ ex.ToString());
                return string.Empty;
            }
        }

        internal static Comm JSONtoComm(string item)
        {
            try
            {
                Comm newComm = new Comm();
                //string strTemp =
                //"{" +
                //"\"Id\" : " + this.Id.ToJson() +
                //"\"Name\" : " + this.Name +
                //"\"Comment\" : " + this.Comment +
                //"\"Origen\" : " + this.Origen +
                //"\"isPublic\" : " + this.isPublic +
                //"\"FechaHora\" : " + this.FechaHora.ToString() +
                //"}";

                //JsonConvert.SerializeObject<ObjectId>("ObjectId(\"5b2425f2749bd2307c5f3aaa\")")

                //"{
                //\"Id\" : ObjectId(\"5b2425f2749bd2307c5f3aaa\")
                //\"Name\" : Obsdark
                //\"Comment\" : hgg
                //\"Origen\" : Obsdark
                //\"isPublic\" : False
                //\"FechaHora\" : 15-06-2018 16:47:46
                //}"
                return newComm;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en ToJSON(): " + "\n" + ex.ToString());
                return null;
            }
        }
    }
}
