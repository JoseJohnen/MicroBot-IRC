using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BOT_IRC_SINGLE_INSTANCE
{
    public class CommsDAO
    {
        private MongoClient mongoClient;
        private IMongoCollection<Comm> CommsCollection;

        public static List<Comm> l_comms_sended = new List<Comm>();
        public static string PathToApp = Environment.CurrentDirectory + "\\HistoryLogs\\History_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

        public CommsDAO(string server, string db, string collection)
        {
            try
            {
                mongoClient = new MongoClient(server);
                IMongoDatabase database = mongoClient.GetDatabase(db);
                CommsCollection = database.GetCollection<Comm>(collection);
                Console.WriteLine("Conexión exitosa con Base de Datos: "+db+" en colección "+collection);
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n"+"Conexión no establecida debido a error: "+"\n" + ex.ToString());
            }
        }

        public static bool PrepareLoadChat()
        {
            try
            {
                if (!File.Exists(PathToApp))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\HistoryLogs");
                    File.WriteAllText(PathToApp, "");
                    return true;
                }
                else if (File.Exists(PathToApp))
                {
                    if(File.ReadLines(PathToApp) != null)
                    {
                        if (File.ReadLines(PathToApp).Count() > 0)
                        {
                            foreach (string item in File.ReadLines(PathToApp).ToList())
                            {
                                if(!string.IsNullOrWhiteSpace(item))
                                {
                                    //Comm.JSONtoComm(item)
                                    //l_comms_sended.Add(JsonConvert.DeserializeObject<Comm>(item));
                                }
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error en prepareLoadChat(): \n" + ex.ToString());
                Console.ReadLine();
                return false;
            }
        }

        public List<Comm> FindAllUpdates()
        {
            List<Comm> comms = new List<Comm>();
            try
            {
                comms.AddRange(CommsCollection.AsQueryable<Comm>().Where(I => I.FechaHora.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).ToList());
                l_comms_sended = l_comms_sended.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                comms.AddRange(l_comms_sended);
                comms = comms.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                //foreach (Comm comm in comms)
                //{
                //    Console.WriteLine("id: " + comm.Id);
                //    Console.WriteLine("name: " + comm.Name);
                //    Console.WriteLine("comment: " + comm.Comment);
                //    Console.WriteLine("origin: " + comm.Origen);
                //    Console.WriteLine("status: " + comm.FechaHora.ToString());
                //    Console.WriteLine("================================");
                //}
                return comms;
            }
            catch (Exception)
            {
                return comms;
            }
        }

        public bool AgregarComunicacion(string userName, string comment, string Origen = null)
        {
            try
            {
                string strOrigen = userName;
                bool boolPublic = false;
                //Si NO es de un mensaje privado
                if (!string.IsNullOrWhiteSpace(Origen))
                {
                    strOrigen = Origen;
                    boolPublic = true;
                }

                Comm comm = new Comm()
                {
                    Name = userName,
                    Comment = comment,
                    Origen = strOrigen,
                    isPublic = boolPublic,
                    FechaHora = DateTime.Now
                };
                
                CommsCollection.InsertOne(comm);

                using (StreamWriter writer = new StreamWriter(PathToApp))
                {
                    string hl = comm.ToJSON();
                    writer.WriteLine(comm.ToJSON());
                    writer.Flush();
                    writer.Close();
                }

                l_comms_sended.Add(comm);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        
        public static CommsDAO PrepareDBConnection()
        {
            Restart:
            string PathToApp = Environment.CurrentDirectory + "\\ConfigMongoConn.txt";
            string[] someString = new string[3];
            if (File.Exists(PathToApp))
            {
                Console.WriteLine("El archivo de configuracion para la base de datos Existe!" + "\n" + "Connectando a:");
                someString = File.ReadAllLines(PathToApp);
                Console.WriteLine("Servidor MongoDB: " + someString[0]);
                Console.WriteLine("DataBase: " + someString[1]);
                Console.WriteLine("Coleccion: " + someString[2]);
                Console.WriteLine();
                return new CommsDAO(someString[0], someString[1], someString[2]);
            }
            else
            {
                Console.WriteLine("NO EXISTE el archivo de configuración, creando uno con parametros estandar...");

                someString[0] = "mongodb://adminduoc:duoc2018@ds259820.mlab.com:59820/virtualizacion"; //"mongodb://localhost:27017/";
                someString[1] = "virtualizacion";//"test";
                someString[2] = "botchats";//"comms";

                File.WriteAllLines(PathToApp, someString);
                Console.Write("Archivo creado exitosamente, Comenzando conexión con parametros estandar");
                Console.WriteLine();
                goto Restart;
            }
        }
    }
}
