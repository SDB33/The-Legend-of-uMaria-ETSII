using System.Net;
using System.Threading;
using UnityEngine;
using System.IO;

public class ServidorRest : MonoBehaviour
{
    public API api; // Referencia al componente que maneja los comandos recibidos desde fuera

    private HttpListener servidorHttp; // Escucha las peticiones HTTP entrantes
    private Thread hiloEscucha;        // Hilo que ejecuta el bucle del servidor para no bloquear Unity
    private bool enFuncionamiento = true; // Controla si el servidor sigue activo

    void Start()
    {
        // Se configura el servidor para escuchar en la ruta http://localhost:8080/comando/
        servidorHttp = new HttpListener();
        servidorHttp.Prefixes.Add("http://*:8080/comando/");

        // Se inicia el hilo que escuchará las peticiones
        hiloEscucha = new Thread(EscucharPeticiones);
        hiloEscucha.Start();

        // Mensaje en consola para saber que el servidor está en marcha
        Debug.Log("Servidor iniciado en: http://localhost:8080/comando/");
    }

    // Este método se ejecuta en un hilo aparte y se encarga de recibir y procesar las peticiones
    void EscucharPeticiones()
    {
        servidorHttp.Start();

        while (enFuncionamiento)
        {
            // Espera hasta que llega una petición (bloquea el hilo hasta entonces)
            var contexto = servidorHttp.GetContext();

            var peticion = contexto.Request;   // La petición que ha llegado (por ejemplo un POST)
            var respuesta = contexto.Response; // Lo que vamos a devolverle al cliente

            if (peticion.HttpMethod == "POST")
            {
                // Leemos el cuerpo de la petición (normalmente un comando en texto)
                using var lector = new StreamReader(peticion.InputStream);
                string contenido = lector.ReadToEnd();

                // Enviamos el contenido leído al sistema de comandos (la clase API)
                api?.EnviarComando(contenido.Trim());

                // Preparamos la respuesta "OK" para que el cliente sepa que se recibió correctamente
                byte[] respuestaOk = System.Text.Encoding.UTF8.GetBytes("OK");
                respuesta.ContentLength64 = respuestaOk.Length;
                respuesta.OutputStream.Write(respuestaOk, 0, respuestaOk.Length);
            }
            else
            {
                // Si el método no es POST, respondemos con error 405 (no permitido)
                respuesta.StatusCode = 405;
            }

            // Cerramos el canal de salida de la respuesta
            respuesta.OutputStream.Close();
        }
    }

    void OnApplicationQuit()
    {
        // Se indica que el servidor debe dejar de escuchar
        enFuncionamiento = false;

        // Se detiene el servidor HTTP
        servidorHttp.Stop();

        // Se detiene el hilo de escucha
        hiloEscucha.Abort(); // ⚠️ Forzar abortar un hilo no es lo más limpio, pero funciona en este contexto
    }
}
