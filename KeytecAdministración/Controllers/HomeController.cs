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
using Newtonsoft.Json.Linq;
using RestSharp;

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
        public IActionResult Sql(string Instancia,string SN,int pagina=1)
        {
            Func<TablaMaquinas, bool> predicado = x => String.IsNullOrEmpty(Instancia) || Instancia.Equals(x.Instancia);
            Func<TablaMaquinas, bool> predicado1 = x => String.IsNullOrEmpty(SN) || SN.Equals(x.Sn);

            var cantidadRegistrosPorPagina = 30;
            //var maquinas = productionContext.Machines.Where(predicado).Where(y => !string.IsNullOrEmpty(y.Sn)).OrderBy(x => x.Id).Skip((pagina - 1) * cantidadRegistrosPorPagina).Take(cantidadRegistrosPorPagina).ToList();
            var maquinas = productionContext.Machines.Where(y => !string.IsNullOrEmpty(y.Sn)).Where(x => x.Sn.Contains("CGJ")).OrderBy(x => x.Id).ToList();
            var estadoDisp = transaccionesContext.EstadoDispositivos.Where(x => x.EstSn.Contains("CGJ")).ToList();
            var totalDeRegistros = maquinas.Count();

           
           
            List<Tra_aux> listaTransacciones = new List<Tra_aux>();


            string SQL_CONNECTION_TRANSACTIONS = "initial catalog=Transacciones; Data Source= keycloud-prod.database.windows.net; " +
                                "Connection Timeout=30; User Id = appkey; Password=Kkdbc36de$; Min Pool Size=20; Max Pool Size=200; " +
                                "MultipleActiveResultSets=True;";
            using (SqlConnection connection = new SqlConnection(SQL_CONNECTION_TRANSACTIONS))
            {
                string query;
                connection.Open();
                query = ("SELECT TRA_SN,TRA_TIPO FROM TRANSACCIONES WHERE TRA_ESTADO >-1 AND TRA_ESTADO <2 AND TRA_SN LIKE 'CGJ%'");

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int intAux;
                    SqlDataReader response;
                    response = command.ExecuteReader();
                    if (response.HasRows)
                    {
                        
                        while (response.Read()) {
                            Tra_aux transacciones_aux = new Tra_aux();
                            transacciones_aux.TRA_SN = response[0].ToString();
                            string aux = response[1].ToString();
                            if (!Int32.TryParse(aux, out intAux))
                                logger.Information("ERROR NOT PARSE.");
                            else
                                transacciones_aux.TRA_TIPO = intAux;
                            transacciones_aux.TRA_TIPO = Int32.Parse(aux);
                            listaTransacciones.Add(transacciones_aux);
                        }
                    }
                    response.Close();
                }
                connection.Close();
            }
            //var transacciones = transaccionesContext.Transacciones.Where(x => x.TraSn.Contains("CGJ")).ToList();

            //var estadoDisp = transaccionesContext.EstadoDispositivos.Where(x=>x.EstSn.Contains(maquinas.Select(x=>x.Sn).ToString())).ToList();

            List<TablaMaquinas> tablamaquina = new List<TablaMaquinas>();
            foreach (var i in maquinas)
            {
                foreach(var j in estadoDisp)
                {                    
                    if (i.Sn.Equals(j.EstSn))
                    { 
                        int countTraPendiente = 0;
                        int countPerfilPendiente = 0;
                        int countCarasPendiente = 0;
                        int countHuellasPendiente = 0;
                        int countReinicioPendiente = 0;
                        int countOtrasPendiente = 0;
                        int countEliminadosPendiente = 0;
                        int countDescargaPendientes = 0;



                        TablaMaquinas tablaFinal = new TablaMaquinas();
                        tablaFinal.Id = i.Id;
                        tablaFinal.MachineAlias = i.MachineAlias;
                        tablaFinal.Sn = i.Sn;
                        tablaFinal.Instancia = i.Instancia;
                        tablaFinal.IdSucursal = i.IdSucursal;
                        tablaFinal.MachineNumber = i.MachineNumber;
                        tablaFinal.EstCantHuellas = j.EstCantHuellas;
                        tablaFinal.EstCantRostros = j.EstCantRostros;
                        tablaFinal.EstCantUsuarios = j.EstCantUsuarios;
                        tablaFinal.EstVersionFw = j.EstVersionFw;
                        tablaFinal.EstUltimoReporte = j.EstUltimoReporte;
                        string fecha_hoy = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        var minutes = (Convert.ToDateTime(fecha_hoy)) - (Convert.ToDateTime(j.EstUltimoReporte));
                        if (minutes.TotalMinutes > 10)
                        {
                            tablaFinal.Estado = 0;// desconectado
                        }
                        else
                        {
                            tablaFinal.Estado = 1;// conectado
                        }
                    
                        tablaFinal.TraPendiente = countTraPendiente;
                        tablaFinal.PerfilPendiente = countPerfilPendiente;
                        tablaFinal.ReinicioPendiente = countReinicioPendiente;
                        tablaFinal.OtrasPendiente = countOtrasPendiente;
                        tablaFinal.CarasPendiente = countCarasPendiente;
                        tablaFinal.HuellasPendiente = countHuellasPendiente;
                        tablaFinal.EliminadosPendiente = countEliminadosPendiente;
                        tablaFinal.DescargaPendiente = countDescargaPendientes;
                        tablaFinal.IdRegion = i.IdRegion;

                        tablamaquina.Add(tablaFinal);
                    }                  
                }
            }

            for (int i = 0; i < tablamaquina.Count(); i++)
            {
                tablamaquina[i].TraPendiente = 0;//countTraPendiente;
                tablamaquina[i].PerfilPendiente = 0;//countPerfilPendiente;
                tablamaquina[i].ReinicioPendiente = 0;//countReinicioPendiente;
                tablamaquina[i].OtrasPendiente = 0;//countOtrasPendiente;
                tablamaquina[i].CarasPendiente = 0;//countCarasPendiente;
                tablamaquina[i].HuellasPendiente = 0;//countHuellasPendiente;
                tablamaquina[i].EliminadosPendiente = 0;//countEliminadoPendiente;

                foreach (var t in listaTransacciones)
                {
                    if (tablamaquina[i].Sn.Equals(t.TRA_SN))
                    {
                        if (t.TRA_TIPO == 2)
                        {
                            tablamaquina[i].EliminadosPendiente++;
                        }
                        if (t.TRA_TIPO == 12)
                        {
                            tablamaquina[i].DescargaPendiente++;
                        }
                        if (t.TRA_TIPO == 10)
                        {
                            tablamaquina[i].ReinicioPendiente++;
                        }
                        else if (t.TRA_TIPO == 6)
                        {
                            tablamaquina[i].PerfilPendiente++;
                        }
                        else if (t.TRA_TIPO == 7)
                        {
                            tablamaquina[i].HuellasPendiente++;
                        }
                        else if (t.TRA_TIPO == 8)
                        {
                            tablamaquina[i].CarasPendiente++;
                        }
                        else if (t.TRA_TIPO != 8 && t.TRA_TIPO != 7 && t.TRA_TIPO != 6 && t.TRA_TIPO != 2 && t.TRA_TIPO != 12 && t.TRA_TIPO != 10)
                        {
                            tablamaquina[i].OtrasPendiente++;
                        }
                        tablamaquina[i].TraPendiente = tablamaquina[i].ReinicioPendiente + tablamaquina[i].PerfilPendiente +
                            tablamaquina[i].HuellasPendiente + tablamaquina[i].CarasPendiente + tablamaquina[i].OtrasPendiente + tablamaquina[i].DescargaPendiente + 
                            tablamaquina[i].EliminadosPendiente;

                    }
                }
            }

            var table = tablamaquina.Where(predicado).Where(predicado1).Where(y => !string.IsNullOrEmpty(y.Sn))
            .OrderBy(x => x.Id).Skip((pagina - 1) * cantidadRegistrosPorPagina).Take(cantidadRegistrosPorPagina).ToList();

            var modelo = new IndexViewModel();

            
            modelo.TableMaquinas = table;
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
        public IActionResult Warnings()
        {
            //----------credenciales e información para conexion archivo FTP adms5-----------

            logger.Information("Nlog funcionando");
            string fecha_hoy = DateTime.Now.ToString(@"yyyy-MM-dd");
            string FtpServer = "ftp://waws-prod-cq1-011.ftp.azurewebsites.windows.net/site/wwwroot/logs/Warnings/";
            string username = @"key-adms5\$key-adms5";
            string password = "TRrpH02xwswp2rN3ZRboovc2B0lzPAn96dHet4M2opQbAYQlnngcjryBCXL7";
            Ini iniFile = new Ini("config.ini");
            string localpath = iniFile.GetValue("descarga", "path");
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
                    if (fileName3.Contains("logADMS5_Warnings") && fileName3!=aux)
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


        public IActionResult WarningsADMS6()
        {
            //----------credenciales e información para conexion archivo FTP adms5-----------

            logger.Information("Nlog funcionando");
            string fecha_hoy = DateTime.Now.ToString(@"yyyy-MM-dd");
            string FtpServer = "ftp://waws-prod-cq1-017.ftp.azurewebsites.windows.net/site/wwwroot/logs/Warnings/";
            string username = @"key-adms6\$key-adms6";
            string password = "xDjnZsds4yfSzxx96s4uCFokMqpz5TsnFR1iyyxnxuRW7h0LqNa61tNz8zvo";
            Ini iniFile = new Ini("config.ini");
            string localpath = iniFile.GetValue("descargaadms6", "path");
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
                    if (fileName3.Contains("logADMS6_Warnings") && fileName3 != aux)
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
        public IActionResult GetADMS5T(string? filtro,string? filtrosn, int pagina = 1)
        {
            Func<EstadoSN, bool> predicado = x => String.IsNullOrEmpty(filtro) || filtro.Equals(x.SN);
            Func<EstadoSN, bool> predicado1 = x => String.IsNullOrEmpty(filtrosn) || filtrosn.Equals(x.SN);

            var cantidadRegistrosPorPagina = 20;
            
            //------------ credenciales e información para conexion archivo FTP--------------------
            logger.Information("Logger request funcionando");
            string FtpServer = "ftp://waws-prod-cq1-011.ftp.azurewebsites.windows.net/site/wwwroot/logs/States/";
            string username = @"key-adms5\$key-adms5";
            string password = "TRrpH02xwswp2rN3ZRboovc2B0lzPAn96dHet4M2opQbAYQlnngcjryBCXL7";
            Ini iniFile = new Ini("config.ini");
            string localpath = iniFile.GetValue("getrequest", "path");
            string localpathAnterior = iniFile.GetValue("anterior", "path");

            
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
                                EstadoSN estadosn = new EstadoSN();
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

                                // -------------lógica para sacar datos----------------                    
                                var sentences = new List<String>();
                                String[] parts = line.Split(" INFO ");
                                DateTime fecha = DateTime.Parse(parts[0]);
                                estadosn.Fecha = fecha;
                                estadosn.SN = parts[1];
                                estadosn.Estado = 1; //estado 1 = conectado
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

                            var modelo1 = new IndexViewModel();
                            var totalDeRegistros = stateListFinal.Count();

                            var table1 = stateListFinal.Where(predicado).Where(predicado1).Where(y => !string.IsNullOrEmpty(y.SN))
                .OrderBy(x => x.Estado).Skip((pagina - 1) * cantidadRegistrosPorPagina).Take(cantidadRegistrosPorPagina).ToList();
                            
                            modelo1.Estadosn = table1;
                            modelo1.PaginaActual = pagina;
                            modelo1.RegistrosPorPagina = cantidadRegistrosPorPagina;
                            modelo1.TotalDeRegistros = totalDeRegistros;
                            modelo1.ValoresQueryString = new RouteValueDictionary();
                            modelo1.ValoresQueryString["SN"] = filtro;

                            return View(modelo1);
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


        [HttpPost]
        public JsonResult Reinicio(string sn)
        {
            try
            {
                string SQL_CONNECTION_TRANSACTIONS = "initial catalog=Transacciones; Data Source= keycloud-prod.database.windows.net; Connection Timeout=30; User Id = appkey; Password=Kkdbc36de$; Min Pool Size=20; Max Pool Size=200; MultipleActiveResultSets=True;";
                using (SqlConnection connection = new SqlConnection(SQL_CONNECTION_TRANSACTIONS))
                {
                    string query;
                    connection.Open();
                    query = string.Format("INSERT INTO TRANSACCIONES (TRA_TIPO,TRA_ESTADO,TRA_DETALLE, TRA_SN, TRA_HORA_INICIO) VALUES({0},{1},'{2}','{3}',CONVERT(datetime, '{4}'))", 10, 0, "{}", sn, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss"));
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return Json("Reiniciado equipo con sn: " + sn);
            } catch (Exception ex) {
                return Json("Error : " + ex.Message);
            }
           

        }
        [HttpPost]
        public JsonResult Eliminar(string sn)
        {
            try
            {
                string SQL_CONNECTION_PRODUCTIONS = "initial catalog=Produccion; Data Source= keycloud-prod.database.windows.net; Connection Timeout=30; User Id = appkey; Password=Kkdbc36de$; Min Pool Size=20; Max Pool Size=200; MultipleActiveResultSets=True;";
                using (SqlConnection connection = new SqlConnection(SQL_CONNECTION_PRODUCTIONS))
                {
                    string query;
                    string id = "";
                    connection.Open();
                    query = string.Format("SELECT id FROM machines WHERE SN='{0}'", sn);
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader response;
                        response = command.ExecuteReader();
                        if (response.HasRows)
                        {                           
                            if (response.Read())
                                id = response[0].ToString();
                        }
                        response.Close();
                    }
                    query = string.Format("DELETE KEY_MODELO_DISPOSITIVO WHERE MOD_DISPOSITIVO={0}", id);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    query = string.Format("DELETE Machines WHERE ID={0}", id);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                    return Json("Eliminado modelo con SN: " + sn); 
            } 
            catch(Exception ex) {
                return Json("Error :" + ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Modificar(string sn)
        {
            return Json("Modificado zona horaria equipo con SN: " + sn);
        }
        [HttpPost]
        public JsonResult Descargar(string sn, string fecha_inicio, string fecha_final)
        {
            DateTime fecha_ini = Convert.ToDateTime(fecha_inicio);
            fecha_ini.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime fecha_fin = Convert.ToDateTime(fecha_final);
            fecha_fin.ToString("yyyy-MM-dd HH:mm:ss");

            try {
                JObject dateJSON = new JObject(new JProperty("Fec_inicio", fecha_ini), new JProperty("Fec_fin", fecha_final));
                string query = string.Format("INSERT INTO TRANSACCIONES (TRA_TIPO,TRA_ESTADO,TRA_DETALLE, TRA_SN, TRA_HORA_INICIO) VALUES({0},{1},'{2}','{3}',CONVERT(datetime, '{4}')", 17, 0, dateJSON.ToString(), sn, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Json("Descarga equipo con SN: " + sn);
            }
            catch(Exception ex) {
                return Json("Error al descargar equipo con SN: " + sn + ex.Message);
            }
        }
        [HttpPost]
        public JsonResult Proxy(string sn, string host, int n=1)
        {
            try {
                /*JObject proxyJSON = new JObject(new JProperty("id", sn), new JProperty("host", host), new JProperty("business_id", n));
                string ruta = "";
                RestClient restClient;
                restClient = new RestClient( ruta);
                var restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("Authorization", "1234");
                restRequest.AddParameter("application/json", proxyJSON, ParameterType.RequestBody);
                var response = restClient.Execute(restRequest);
                */
                return Json("Equipo con  SN: " + sn + "añadido a nemo proxy" + host);

            }
            catch (Exception ex) {
                return Json("Error al agregar a Nemo: "+ ex.Message);
            }

        }


        public IActionResult GetADMS6(string? filtro, string? filtrosn, int pagina = 1)
        {
            Func<EstadoSN, bool> predicado = x => String.IsNullOrEmpty(filtro) || filtro.Equals(x.SN);
            Func<EstadoSN, bool> predicado1 = x => String.IsNullOrEmpty(filtrosn) || filtrosn.Equals(x.SN);

            var cantidadRegistrosPorPagina = 20;

            //------------ credenciales e información para conexion archivo FTP--------------------
            logger.Information("Logger request funcionando");
            string FtpServer = "ftp://waws-prod-cq1-017.ftp.azurewebsites.windows.net/site/wwwroot/logs/States/";
            string username = @"key-adms6\$key-adms6";
            string password = "xDjnZsds4yfSzxx96s4uCFokMqpz5TsnFR1iyyxnxuRW7h0LqNa61tNz8zvo";
            Ini iniFile = new Ini("config.ini");
            string localpath = iniFile.GetValue("getadms6", "path");
            

            string localpathAnterior = iniFile.GetValue("getanterior6", "path");
            
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

                string stateFile = "logADMS6_States.txt";

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
                                string query = "SELECT EST_SN FROM ESTADO_DISPOSITIVOS WHERE EST_HOST='ADMS6'";
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
                    if (fileName3.Contains("logADMS6_States.txt"))
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
                                string query = "SELECT EST_SN FROM ESTADO_DISPOSITIVOS WHERE EST_HOST='ADMS6'";
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

                                // -------------lógica para sacar datos----------------                    
                                var sentences = new List<String>();
                                String[] parts = line.Split(" INFO ");
                                DateTime fecha = DateTime.Parse(parts[0]);
                                estadosn.Fecha = fecha;
                                estadosn.SN = parts[1];
                                estadosn.Estado = 1; //estado 1 = conectado
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

                            var modelo1 = new IndexViewModel();
                            var totalDeRegistros = stateListFinal.Count();

                            var table1 = stateListFinal.Where(predicado).Where(predicado1).Where(y => !string.IsNullOrEmpty(y.SN))
                .OrderBy(x => x.Estado).Skip((pagina - 1) * cantidadRegistrosPorPagina).Take(cantidadRegistrosPorPagina).ToList();

                            modelo1.Estadosn = table1;
                            modelo1.PaginaActual = pagina;
                            modelo1.RegistrosPorPagina = cantidadRegistrosPorPagina;
                            modelo1.TotalDeRegistros = totalDeRegistros;
                            modelo1.ValoresQueryString = new RouteValueDictionary();
                            modelo1.ValoresQueryString["SN"] = filtro;

                            return View(modelo1);
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
