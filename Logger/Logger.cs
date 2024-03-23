using System;
using System.IO;

public class Logger
{
    private readonly string _logFilePath;

    public Logger()
    {
        _logFilePath = "C:\\FormService\\log.txt";
    }

    public void WriteLog(string message, string type_error)
    {
        try
        {
            // Verificar si el archivo existe
            if (!File.Exists(_logFilePath))
            {
                // Crear el archivo
                using (StreamWriter writer = File.CreateText(_logFilePath))
                {
                    writer.WriteLine($"{type_error} | {DateTime.Now}: {message}");
                }
            }
            else
            {
                // Agregar el nuevo contenido al final del archivo
                using (StreamWriter writer = File.AppendText(_logFilePath))
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