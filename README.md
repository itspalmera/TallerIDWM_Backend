# Taller IDWM: Backend

Este proyecto corresponde al backend desarrollado para el taller de la asignatura Introducción al Desarrollo Web Móvil.  
El sistema expone una API REST desarrollada con ASP.NET Core, diseñada para gestionar entidades del dominio definido en el contexto del taller.

## 🧑‍💻 Desarrolladores
- Pamela Vera 21.564.004-3  pamela.vera@alumnos.ucn.cl
- Yamir Castillo 21.220.241-K  yamir.castillo@alumnos.ucn.cl

## 📌 Descripción del Proyecto

Este backend tiene como propósito servir como API REST para una aplicación web, permitiendo operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre los recursos definidos.  
Está construido con tecnologías modernas y emplea buenas prácticas de desarrollo como separación de capas, uso de Entity Framework Core para la persistencia, y Serilog para el manejo de logs.

## 💻 Tecnologías
El proyecto utiliza las siguientes tecnologías y herramientas:
- C#: Lenguaje de programación.
- .NET 9: Framework para construir la API REST.
- SQLite: Base de datos para almacenar los datos del proyecto.
- Postman: Herramienta para pruebas y documentación de los endpoints.


## 🛠 Requisitos Previos

Antes de ejecutar el sistema, asegúrate de tener instalado lo siguiente:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0) 
- [SQLite](https://www.sqlite.org/download.html) 
- [Postman](https://www.postman.com/downloads/)
- [Git](https://git-scm.com)

## 📥 Instrucciones de Instalación

Sigue los siguientes pasos para configurar el proyecto en tu entorno local:

1. Clona el repositorio:

   ```bash
   git clone https://github.com/itspalmera/TallerIDWM_Backend.git
   ```

2. Navega al directorio del proyecto:
   
   ```bash
   cd TallerIDWM_Backend
   ```
   

3. Restaura las dependencias:

   ``` bash
   dotnet restore
   ```
   

4. *Crea el archivo appsettings.json en la raíz del proyecto con la siguiente configuración:*
   ```bash
    {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },

    "Serilog": {
      "MinimumLevel": {
        "Default": "Information",
        "Override": {
          "Microsoft": "Warning",
          "Microsoft.AspNetCore.Hosting.Diagnostics": "Error",
          "Microsoft.Hosting.Lifetime": "Information",
          "System": "Error"
          
        }
      },

      "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
      "WriteTo": [
        { "Name": "Console" },
        {
          "Name": "File",
          "Args": {
            "path": "logs/log-.txt",
            "rollingInterval": "Day",
            "restrictedToMinimumLevel": "Information"
          }
        }
      ]
    },
    "AllowedHosts": "*",

    "ConnectionStrings": {
      "DefaultConnection": "Data Source=database.db"
    },

    "JWT": {
      "SignInKey": "vbKcmEtIGxwsn8fGWnhcMMBd3/rLqFYlA54jHF4Fri7pYli4DDF55OYPW1Sd/aR0faqomaeFoKmMK6+buUYNwA==",
      "Issuer": "http://localhost:5162",
      "Audience": "http://localhost:5162"
    },

    "CloudinarySettings": {
        "CloudName": "dnsxvmlto",
        "ApiKey": "644639564517695",
        "ApiSecret": "msaDP0V_-sH4pMsYv-xOijpd2GQ"
      },
      "CorsSettings": {
          "AllowedOrigins": [ "https://localhost:7188", "http://localhost:3000" ],
          "AllowedMethods": [ "GET", "POST", "PUT", "DELETE" ],
          "AllowedHeaders": [ "Content-Type", "Authorization"]
      }
   }   
   ```
   

## ▶ Ejecutar la API

Para ejecutar el servidor en modo desarrollo local, usa el siguiente comando:

```bash
dotnet run
```


Esto levantará el backend y la API quedará disponible en http://localhost:5000 o https://localhost:5001 según tu configuración.
