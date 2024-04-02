using FormService.Utilities;
using System;
using System.IO;

public class Logger
{
    private readonly string _logFilePath;

    public Logger()
    {
        _logFilePath = "C:\\FormService\\Logs\\";
    }

    public void WriteLog(string message, string type_error)
    {
        try
        {
            Files.createDirectoryIfNotExists(_logFilePath);
            DateTime now = DateTime.Now;
            string fileName = _logFilePath + "log" + now.ToString("yyyy-MM-dd") + ".txt";
            // Verificar si el archivo existe
            if (!File.Exists(fileName))
            {
                // Crear el archivo
                using (StreamWriter writer = File.CreateText(fileName))
                {
                    writer.WriteLine($"{type_error} | {DateTime.Now}: {message}");
                }
            }
            else
            {
                // Agregar el nuevo contenido al final del archivo
                using (StreamWriter writer = File.AppendText(fileName))
                {
                    writer.WriteLine($"{type_error} | {DateTime.Now}: {message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al escribir en el archivo de log: {ex.Message}");
        }
    }
}