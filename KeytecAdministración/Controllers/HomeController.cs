using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KeytecAdministración.Models;
using System.Net;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Routing;

namespace KeytecAdministración.Controllers
{
    public class HomeController : Controller
    {
      

        ProductionsContext productionContext = new ProductionsContext();
        TransaccionesContext transaccionesContext = new TransaccionesContext();
        

        private readonly ILog logger;

        public HomeController(ILog logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Sql(string Instancia,int pagina=1)
        {
            Func<Machines, bool> predicado = x => String.IsNullOrEmpty(Instancia) || Instancia.Equals(x.Instancia);

            var cantidadRegistrosPorPagina = 20;
            var maquinas = productionContext.Machines.Where(predicado).OrderBy(x => x.Id).Skip((pagina - 1) * cantidadRegistrosPorPagina).Take(cantidadRegistrosPorPagina).ToList();
            
            
            var totalDeRegistros = productionContext.Machines.Count();


            //var estadoDisp = transaccionesContext.EstadoDispositivos.Where(x=>x.EstSn.Contains(productionContext.Machines.Select(x=>x.Sn).ToString())).ToList();
            var estadoDisp = transaccionesContext.EstadoDispositivos.ToList();
            var transacciones = transaccionesContext.Transacciones.Where(x=>x.TraEstado > -1 && x.TraEstado < 2).ToList();
            

            List<TablaMaquinas> tablaFinal = new List<TablaMaquinas>();

            for (int i=0; i < maquinas.Count(); i++)
            {
                for(int j=0; j< estadoDisp.Count(); j++)
                {
                    while (!string.IsNullOrEmpty(maquinas[i].Sn)) 
                    { 
                        if (maquinas[i].Sn.Equals(estadoDisp[j].EstSn))
                        {
                            List<TablaMaquinas> listaMaquina = new List<TablaMaquinas>();
                            listaMaquina[i].Id = maquinas[i].Id;
                            listaMaquina[i].MachineAlias = maquinas[i].MachineAlias;
                            listaMaquina[i].Sn = maquinas[i].Sn;
                            listaMaquina[i].Instancia = maquinas[i].Instancia;
                            listaMaquina[i].IdSucursal = maquinas[i].IdSucursal;
                            listaMaquina[i].MachineNumber = maquinas[i].MachineNumber;
                            listaMaquina[i].EstCantHuellas = estadoDisp[j].EstCantHuellas;
                            listaMaquina[i].EstCantRostros = estadoDisp[j].EstCantRostros;
                            listaMaquina[i].EstCantUsuarios = estadoDisp[j].EstCantUsuarios;
                            listaMaquina[i].EstVersionFw = estadoDisp[j].EstVersionFw;
                            listaMaquina[i].EstUltimoReporte = estadoDisp[j].EstUltimoReporte;
                            string fecha_hoy = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                            var minutes = Convert.ToDateTime(fecha_hoy) - (Convert.ToDateTime(estadoDisp[j].EstUltimoReporte));
                            if (minutes.TotalMinutes > 10)
                            {
                                listaMaquina[i].Estado = 1;// desconectado
                            }
                            else
                            {
                                listaMaquina[i].Estado = 0;// conectado
                            }
                            for(int k = 0; k < transacciones.Count(); k++)
                            {
                                int countTraPendiente = 0;
                                int countPerfilPendiente = 0;
                                int countCarasPendiente = 0;
                                int countHuellasPendiente = 0;
                                int countReinicioPendiente = 0;
                                int countOtrasPendiente = 0;
                                if (maquinas[i].Sn.Equals(transacciones[k].TraSn))
                                {
                                    if (transacciones[k].TraTipo == 2 )
                                    {
                                        countReinicioPendiente ++;
                                    }
                                    else if(transacciones[k].TraTipo == 6 )
                                    {
                                        countPerfilPendiente ++;
                                    }
                                    else if(transacciones[k].TraTipo == 7)
                                    {
                                        countHuellasPendiente ++;
                                    }
                                    else if(transacciones[k].TraTipo == 8 )
                                    {
                                        countCarasPendiente ++;
                                    }
                                    else if(transacciones[k].TraTipo != 8 && transacciones[k].TraTipo != 7 && transacciones[k].TraTipo != 6 &&
                                        transacciones[k].TraTipo != 2)
                                    {
                                        countOtrasPendiente ++;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    countTraPendiente = countHuellasPendiente + countPerfilPendiente + countReinicioPendiente + countCarasPendiente
                                        + countOtrasPendiente;
                                }
                                listaMaquina[i].TraPendiente = countOtrasPendiente;
                                listaMaquina[i].PerfilPendiente = countPerfilPendiente;
                                listaMaquina[i].ReinicioPendiente = countReinicioPendiente;
                                listaMaquina[i].OtrasPendiente = countOtrasPendiente;
                                listaMaquina[i].CarasPendiente = countCarasPendiente;
                                tablaFinal.Add(listaMaquina[i]);                               
                            }
                        }
                        else
                        {
                            continue;
                        }                 
                    }
                    continue;
                }continue;
            }
            var modelo = new IndexViewModel();

            //modelo.Machiness = maquinas;
            //modelo.EstDispositivos = estadoDisp;
            modelo.tablaMaquinas = tablaFinal;
            modelo.PaginaActual = pagina;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["Instancia"] = Instancia;

            return View(modelo);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult DownloadFile()
        {
            //----------credenciales e información para conexion archivo FTP adms5-----------

            logger.Information("Nlog funcionando");
            string fecha_hoy = DateTime.Now.ToString(@"yyyy-MM-dd");
            string FtpServer = "ftp://waws-prod-cq1-011.ftp.azurewebsites.windows.net/site/wwwroot/logs/Warnings/";
            string username = @"key-adms5\$key-adms5";
            string password = "TRrpH02xwswp2rN3ZRboovc2B0lzPAn96dHet4M2opQbAYQlnngcjryBCXL7";
            string localpath = @"C:\Users\Alfonso\source\repos\KeytecAdministración\KeytecAdministración\DescargaFTP\descarga.txt";
            var returnValue = "Archivo ftp descargado pero con falla en lectura \n";

            try
            {
                //---------- Conexion FTP, metodo usado(descarga),listar directorio archivos----------
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FtpServer);
                request.Credentials = new NetworkCredential(username, password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fileName3 = streamReader.ReadLine();
                string aux = "Archivo_erroneo";
                List<string> directories = new List<string>();


                while (fileName3 != null && fileName3 != aux)
                {
                    //cambiar a la extension que quieras
                    if (fileName3.Contains("logADMS5_Warnings"))
                    {
                        if (Path.GetExtension(fileName3) == ".txt")//or .xlsx// .png // .jpg etc.
                        {
                            directories.Add(fileName3);
                        }

                        fileName3 = streamReader.ReadLine();
                    }
                    else
                    {
                        logger.Information("No hay directorio loggers del dia de hoy en el servidor o error en lectura ");
                        fileName3 = streamReader.ReadLine();
                    }
                }
                streamReader.Close();


                using (WebClient ftpClient = new WebClient())
                {
                    ftpClient.Credentials = new NetworkCredential(username, password);

                    for (int i = 0; i <= directories.Count - 1; i++)
                    {
                        if (directories[i].Contains("."))
                        {

                            string path = FtpServer + directories[i].ToString();
                            string trnsfrpth = localpath + directories[i].ToString();
                            ftpClient.DownloadFile(path, trnsfrpth);
                            if (!string.IsNullOrEmpty(trnsfrpth))
                            {
                                StreamReader sr = new StreamReader(trnsfrpth);
                                string line;

                                List<string> errores = new List<string>();
                                List<Tag> listaTag = new List<Tag>();


                                while ((line = sr.ReadLine()) != null)
                                {
                                    Tag tag = new Models.Tag();
                                    Console.WriteLine(line);
                                    errores.Add(line);
                                    string taglog = line.Substring(24);
                                    tag.Tags = taglog;
                                    string subFecha = line.Substring(0, 19);
                                    DateTime fecha = DateTime.Parse(subFecha);
                                    tag.Fecha = fecha;
                                    i++;
                                    listaTag.Add(tag);
                                }
                                sr.Close();
                                listaTag.Reverse();
                                List<string> listaMail = new List<string>();

                                return View(listaTag);
                            }
                            return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error al conectar al servidor FTP " + ex.Message);
                return View(returnValue);
            }
            return View(returnValue);
        }

        //-----------------------------------------------------------------------REQUEST ESTADO----------------------------------------------------
        public IActionResult GetRequest()
        {
            //------------ credenciales e información para conexion archivo FTP--------------------
            logger.Information("Logger request funcionando");
            string FtpServer = "ftp://waws-prod-cq1-011.ftp.azurewebsites.windows.net/site/wwwroot/logs/States/";
            string username = @"key-adms5\$key-adms5";
            string password = "TRrpH02xwswp2rN3ZRboovc2B0lzPAn96dHet4M2opQbAYQlnngcjryBCXL7";
            string localpath = @"C:\Users\Alfonso\source\repos\KeytecAdministración\KeytecAdministración\DescargaFTP\getrequest.txt";
            string localpathAnterior = @"C:\Users\Alfonso\source\repos\KeytecAdministración\KeytecAdministración\DescargaFTP\requestAnterior.txt";
            var returnValue = "Archivo ftp getrequest no descargado, falla en conexion servidor \n";

            try
            {

                // ----------Conexion FTP, método usado(descarga),listar directorio archivos----------
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FtpServer);
                request.Credentials = new NetworkCredential(username, password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fileName3 = streamReader.ReadLine();
                string aux = "Archivo_erroneo";
                List<string> directories = new List<string>();

                string stateFile = "logADMS5_States.txt";

                // ----------------validando nombre de archivo que queremos en el servidor ---------------


                FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(FtpServer);
                request1.Credentials = new NetworkCredential(username, password);
                request1.Method = WebRequestMethods.Ftp.ListDirectory;
                StreamReader streamReader1 = new StreamReader(request1.GetResponse().GetResponseStream());
                string fileName = streamReader1.ReadLine();
                string aux1 = "Archivo_erroneo";
                List<string> directories1 = new List<string>();
                int contdir = 0;

                string stateFile1 = "";
                int j = 0;
                bool existFile1 = false;
                while (fileName != null && fileName != aux1)
                {
                    if (fileName.Contains(".txt"))
                    {
                        existFile1 = true;
                        if (contdir == 1)
                        {
                            stateFile1 = fileName;
                            break;
                        }

                        contdir++;
                        fileName = streamReader1.ReadLine();

                    }
                    else
                    {
                        logger.Information("Archivo no es el que corresponde");
                        fileName = streamReader1.ReadLine();
                    }
                }
                streamReader1.Close();
                if (existFile1)
                {
                    using (WebClient ftpClient = new WebClient())
                    {
                        ftpClient.Credentials = new NetworkCredential(username, password);
                        string path = FtpServer + stateFile1;
                        string trnsfrpth = localpathAnterior + stateFile1;
                        ftpClient.DownloadFile(path, trnsfrpth);

                        if (!string.IsNullOrEmpty(trnsfrpth))
                        {
                            StreamReader sr = new StreamReader(trnsfrpth);
                            string line;
                            logger.Information("Entrando a archivo anterior en servidor FTP");
                            List<string> lineas = new List<string>();
                            List<EstadoSN> listaEstado = new List<EstadoSN>();
                            List<EstadoSN> stateListFinal = new List<EstadoSN>();
                            List<string> listSN = new List<string>();
                            //SN EN LA EL Base de datos SQL
                            string SQL_CONNECTION_TRANSACTIONS = "initial catalog=Transacciones; Data Source= keycloud-prod.database.windows.net; " +
                                "Connection Timeout=30; User Id = appkey; Password=Kkdbc36de$; Min Pool Size=20; Max Pool Size=200; " +
                                "MultipleActiveResultSets=True;";
                            using (SqlConnection connection = new SqlConnection(SQL_CONNECTION_TRANSACTIONS))
                            {
                                connection.Open();
                                string query = "SELECT EST_SN FROM ESTADO_DISPOSITIVOS WHERE EST_HOST='ADMS5T'";
                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    SqlDataReader response = command.ExecuteReader();
                                    try
                                    {
                                        if (response.HasRows)
                                        {
                                            while (response.Read())
                                                listSN.Add(response[0].ToString());//response[0] userinfo de cada uno insertar la transaccion                         
                                        }
                                        response.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error(ex.Message);
                                        logger.Error(ex.StackTrace);
                                        response.Close();
                                    }
                                }
                                connection.Close();
                            }

                            while ((line = sr.ReadLine()) != null)
                            {
                                EstadoSN estadosn = new Models.EstadoSN();
                                Console.WriteLine(line);
                                //lineas.Add(line); //agregar  a linea

                                // -------------lógica para sacar datos----------------                    
                                var sentences = new List<String>();
                                String[] parts = line.Split(" INFO ");
                                DateTime fecha = DateTime.Parse(parts[0]);
                                estadosn.Fecha = fecha;
                                estadosn.SN = parts[1];
                                estadosn.Estado = 1;
                                listaEstado.Add(estadosn);
                                j++;
                            }
                            sr.Close();

                            for (int i = listaEstado.Count - 1; i >= 0; i--)//lista de estado recorrer de atras hacia adelante
                            {
                                if (listSN.Count == 0)
                                    break;
                                foreach (string sn in listSN)
                                {
                                    if (sn == listaEstado[i].SN)
                                    {
                                        stateListFinal.Add(listaEstado[i]);
                                        listSN.Remove(sn);
                                        break;
                                    }
                                }
                            }
                            foreach (EstadoSN estado in stateListFinal)
                            {
                                var minutes = estado.Fecha.AddHours(-3) - DateTime.Now; //esto solo funciona cuando se corre de forma local
                                if (minutes.TotalMinutes > 25)
                                    estado.Estado = 0;
                            }
                            foreach (string sn in listSN)
                            {
                                EstadoSN estadosn = new EstadoSN();
                                estadosn.Fecha = new DateTime();
                                estadosn.SN = sn;
                                estadosn.Estado = 0;
                                stateListFinal.Add(estadosn);
                            }
                        }
                    }
                }
                //----------------archivo actual en servidor FTP---------------------
                bool existFile = false;
                while (fileName3 != null && fileName3 != aux)
                {
                    if (fileName3.Contains("logADMS5_States.txt"))
                    {
                        existFile = true;
                        break;
                    }
                    else
                    {
                        logger.Information("Archivo no es el que corresponde");
                        fileName3 = streamReader.ReadLine();
                    }
                }
                streamReader.Close();

                //--------------- lectura de archivo línea a línea--------------
                if (existFile)
                {
                    using (WebClient ftpClient = new WebClient())
                    {
                        ftpClient.Credentials = new NetworkCredential(username, password);
                        string path = FtpServer + stateFile;
                        string trnsfrpth = localpath + stateFile;
                        ftpClient.DownloadFile(path, trnsfrpth);

                        if (!string.IsNullOrEmpty(trnsfrpth))
                        {
                            StreamReader sr = new StreamReader(trnsfrpth);
                            string line;
                            List<string> lineas = new List<string>();
                            List<EstadoSN> listaEstado = new List<EstadoSN>();
                            List<EstadoSN> stateListFinal = new List<EstadoSN>();
                            List<string> listSN = new List<string>();

                            //--------------Conexion BD para comparar SN con los que salen en los logs----------------------
                            string SQL_CONNECTION_TRANSACTIONS = "initial catalog=Transacciones; Data Source= keycloud-prod.database.windows.net; " +
                                "Connection Timeout=30; User Id = appkey; Password=Kkdbc36de$; Min Pool Size=20; Max Pool Size=200; " +
                                "MultipleActiveResultSets=True;";
                            using (SqlConnection connection = new SqlConnection(SQL_CONNECTION_TRANSACTIONS))
                            {
                                connection.Open();
                                string query = "SELECT EST_SN FROM ESTADO_DISPOSITIVOS WHERE EST_HOST='ADMS5T'";
                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    SqlDataReader response = command.ExecuteReader();
                                    try
                                    {
                                        if (response.HasRows)
                                        {
                                            while (response.Read())
                                                listSN.Add(response[0].ToString());//response[0] userinfo de cada uno insertar la transaccion                         
                                        }
                                        response.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error(ex.Message);
                                        logger.Error(ex.StackTrace);
                                        response.Close();
                                    }
                                }
                                connection.Close();
                            }

                            while ((line = sr.ReadLine()) != null)
                            {
                                EstadoSN estadosn = new EstadoSN();
                                Console.WriteLine(line);

                                // -------------lógica para sacar datos----------------                    
                                var sentences = new List<String>();
                                String[] parts = line.Split(" INFO ");
                                DateTime fecha = DateTime.Parse(parts[0]);
                                estadosn.Fecha = fecha;
                                estadosn.SN = parts[1];
                                estadosn.Estado = 1;
                                listaEstado.Add(estadosn);
                                j++;
                            }
                            sr.Close();

                            for (int i = listaEstado.Count - 1; i >= 0; i--)//lista de estado recorrer de atras hacia adelante
                            {
                                if (listSN.Count == 0)
                                    break;
                                foreach (string sn in listSN)
                                {
                                    if (sn == listaEstado[i].SN)
                                    {
                                        stateListFinal.Add(listaEstado[i]);
                                        listSN.Remove(sn);
                                        break;
                                    }
                                }
                            }

                            foreach (EstadoSN estado in stateListFinal)//
                            {
                                var minutes = estado.Fecha.AddHours(-3) - DateTime.Now; //esto solo funciona cuando se corre de forma local
                                if (minutes.TotalMinutes > 25)
                                    estado.Estado = 0;
                            }

                            foreach (string sn in listSN)
                            {
                                EstadoSN estadosn = new EstadoSN();
                                estadosn.Fecha = new DateTime();
                                estadosn.SN = sn;
                                estadosn.Estado = 0;
                                stateListFinal.Add(estadosn);
                            }
                            List<string> snC = new List<string>();
                            List<string> snD = new List<string>();
                            ViewBag.countConectados = 0;
                            ViewBag.countDesconectados = 0;

                            foreach (EstadoSN estado in stateListFinal)
                            {
                                if (estado.Estado == 1)
                                {
                                    ViewBag.countConectados++;
                                    snC.Add(estado.SN);
                                }
                                else
                                {
                                    ViewBag.countDesconectados++;
                                    snD.Add(estado.SN);
                                }
                            }
                            ViewBag.conectados = snC;
                            ViewBag.desconectados = snD;

                            return View(stateListFinal);
                        }
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Conexion fallida" + ex.Message);
                return View(returnValue);
            }
            return View(returnValue);
        }
    }
}
