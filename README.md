# Prueba-Técnica-Leonardo-Quinones-

# 🎰 El Juego de la Ruleta

---

## 📜 Descripción

**El Juego de la Ruleta** es un proyecto que permite simular una ruleta de casino utilizando una API implementada con .NET 8 (MVC/Controller web api) y frontend desarrollado con Vue3.
---

## 🚀 Características

- **🎯 Seguridad**: Manejo de sesión de usuario utilizando un token de único uso. Este fue implementado utilizando un custom Authentication Scheme. Endpoints de la API protegidos mediante política de autorización. El sistema garantiza que un solo cliente pueda acceder al juego (solo una pestaña/ventana de navegador)
- **💸 Apuestas**: Se maneja el balance en la cuenta del usuario utilizando un esquema de transacciones, cada juego apostado representa una transacción que se puede sumar o no a los créditos en la cuenta del usuario.
- **🎉 Implementación de principios SOLID**: Se desarrollan dos variantes del backend: uno que utiliza REDIS como base de datos para las sesiones activas (variante 0.1a) y otra que utiliza PostgreSQL + EF (variante 0.1b). Se realiza el intercambio de bases de datos utilizando la interfaz ISessionDatabase.

---
## 🛠️ Prerequisitos
1. Tener instalado .NET 8.0 SDK (https://dotnet.microsoft.com/es-es/download)
2. Tener instalado Vue (https://vuejs.org/guide/quick-start.html)
3. Tener instalado Bootstrap (npm install bootstrap@5.3.5)
4. Tener una instancia postgreSQL > v15 funcionando (https://www.postgresql.org/download/windows/)
5. Ejecutar el script SQL db.sql que encontrarás en el proyecto backend para cargar las tablas del modelo a la instancia postgreSQL

## 🛠️ Instalación

Para ejecutar **El Juego de la Ruleta** en tu máquina local, sigue estos pasos:

1. **Clona el repositorio**:
   ```bash
   git clone https://github.com/leon-quinones/Prueba-Tecnica-Leonardo-Quinones-.git
2. **Estructura del proyecto**
   Encontrarás dos carpetas una destinada para el backend y otra el frontend.
3. **Instalación backend**
   Esta guía esta basada en el tutorial: https://tecadmin.net/how-to-deploy-dotnet-application-on-iis/ \
   El primer paso será publicar la aplicación .NET utilizando el siguiente comando en una terminal que se encuentre apuntando a la carpeta ./roulette-backend:
   ```bash
   dotnet publish -c Release
   ```
   Finalizado el paso anterior, deberas alojar la carpeta ./bin/Release/net8.0/publish en el IIS copiandola a la carpeta pública de sitios web por defecto que generalmente tiene dirección X:\inetpub\wwwroot\dist, donde X es el identificador del disco donde esta alojado el IIS. \
   Luego deberás crear dos elementos: Grupo de aplicaciones y un sitio web. \
   Para crear el grupo de aplicaciones irás al administrador del IIS en la parte de conexiones, seleccionas Grupo de aplicaciones, das click derecho y seleccionas crear un nuevo grupo de aplicaciones. Aparecerá el siguiente cuadro de diálogo: \
   ![image](https://github.com/user-attachments/assets/fa600995-bb77-4ec0-80c8-20b7dfe5e3ee) \
   Importante seleccionar Sin código no administrado. \
   Luego en conexiones nuevamente y seleccionando sitios, con click derecho desplegarás el menú que tiene la opción agregar sitio web: \
   ![image](https://github.com/user-attachments/assets/d90ec1bb-d706-4b84-ab37-b030e1af03f0) \
   Es importante activar la opción Https ya que la API require de esta conexión para servir la información. Asegurate de seleccionar un certificado SSL válido.

   _Posterior a la creación del sitio web, se deberán crear las siguientes variables de entorno_:
   Para esto debes entrar en el administrador del IIS, seleccionar el  sitio web que creaste e ir a Editor de configuración que aparece en el panel de la derecha. En la sección que se despliega debes configurar la siguiente sección (parte superior) y campo "de" de la siguiente manera: \
   ![image](https://github.com/user-attachments/assets/46acdfff-d2cb-4b50-ad88-633a695fc1ac) \
   Ahora en la opción EnvironmentVariables dar al pequeño botón que se encuentra en la derecha y deberás agregar las siguientes variables de entorno:   
   _Variable 0.1b.x - PostgreSQL como base de dato de sesiones activas_
   ```Go
     APIVersion__Major = 1 
     APIVersion__Minor = 0
     GameDatabase__Password
     GameDatabase__Host
     GameDatabase__Port
     GameDatabase__Database
     GameDatabase__Username 
     GameDatabase__PoolMode 
   ```
   _Variable 0.1b.x - REDIS como base de dato de sesiones activas_
      ```Go
     APIVersion__Major = 1 
     APIVersion__Minor = 0
     GameDatabase__Password
     GameDatabase__Host
     GameDatabase__Port
     GameDatabase__Database
     GameDatabase__Username 
     GameDatabase__PoolMode
    SessionDatabase__Host
    SessionDatabase__Port
    SessionDatabase__Username
    SessionDatabase__Password
    SessionDatabase__DatabaseName
    SessionDatabase__SessionDbName = key of list that stores active sessions hash keys
    SessionDatabase__SessionTimeout      
   ```

5. **Instalación frontend**
   Esta guía se basa en las instrucciones de Vue (https://router.vuejs.org/guide/essentials/history-mode.html#Internet-Information-Services-IIS-)
   En la carpeta roulette-frontend deberás construir los archivos mediante el siguiente comando:
   ```bash
      npm install
      npm run build
   ```
   Deberas copiar la carpeta dist que se ha generado luego de la construcción en el directorio X:\inetpub\wwwroot del IIS. Deberás crear un sitio web desde el Administrador del IIS apuntando a la ruta X:\inetpub\wwwroot\dist, o la carpeta por defecto que estes usando para los sitios alojados en IIS. \
![image](https://github.com/user-attachments/assets/97f08c7a-fcbd-48b5-a916-663de6f913ed) \
   Debes crear el sitio web para el frontend siguiendo los mismos pasos dados anteriormente para el proyecto de backend. \
   Ahora debe instalar IIS UrlRewrite (https://www.iis.net/downloads/microsoft/url-rewrite) y reiniciar tu servidor IIS.   
   En el servidor IIS deberás también adicionar la siguiente variable de entorno:
   ```Go
      VITE_ROULETTE_DOMAIN = "url to roulette-backend provided by your IIS (i.e https://localhost:443/api/v1.0, where 1.0 is the version defined in your backend through Major and Minor variables) 
   ```
   Finalmente, dentro de la carpeta X:\inetpub\wwwroot\dist debes crear un archivo web.config de acuerdo con las instrucciones de vue:
  ```Go
  <?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="Handle History Mode and custom 404/500" stopProcessing="true">
          <match url="(.*)" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
  ```
