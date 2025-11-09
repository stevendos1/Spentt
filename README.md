# Spendn't

## Descripción general
Spendn't es una plataforma de gestión financiera personal compuesta por una API ASP.NET Core y un cliente web Blazor. El sistema permite a las personas registrar ingresos y egresos, planificar metas de ahorro, administrar recordatorios de gastos y visualizar un calendario de eventos financieros consolidado. La solución utiliza Entity Framework Core como ORM y expone documentación interactiva mediante Swagger para facilitar la exploración de los endpoints.

## Arquitectura y componentes
- **Spendnt.API:** API REST protegida con autenticación JWT. Organiza la lógica de negocio en controladores, servicios auxiliares, DTOs y entidades persistidas en SQL Server. Incluye inicialización de datos (`SeedDB`) y almacenamiento de archivos.
- **Spendnt.Shared:** Biblioteca de clases compartida con las entidades del dominio y los DTOs que permiten desacoplar la capa de transporte de la capa de datos.
- **Spendnt.WEB:** Aplicación cliente Blazor WebAssembly que consume la API para ofrecer experiencias como dashboards, formularios de registro de movimientos y seguimiento de metas.

## Objetivo principal
Brindar a las personas una herramienta centralizada para tomar decisiones informadas sobre sus finanzas personales mediante el registro estructurado de sus operaciones y la visualización de indicadores clave.

## Objetivos específicos
1. **Planificación y análisis:** Levantar los requisitos funcionales y no funcionales del caso de uso financiero, identificando entidades, flujos y reglas de negocio.
2. **Diseño de arquitectura y datos:** Definir una arquitectura en capas con contratos desacoplados (interfaces, DTOs) y modelar las relaciones entre entidades utilizando Entity Framework Core.
3. **Implementación de funcionalidades:** Desarrollar la API y el cliente web que materializan los casos de uso principales (autenticación, manejo de ingresos/egresos, metas de ahorro, recordatorios y calendario de eventos).
4. **Pruebas y aseguramiento de calidad:** Validar el funcionamiento de los endpoints y la interfaz mediante pruebas manuales y la documentación Swagger para garantizar la integridad de los datos y la experiencia del usuario.
5. **Despliegue y operación:** Preparar la solución para su publicación configurando la cadena de conexión, seeding inicial y políticas de seguridad (JWT, CORS) que permitan su operación en distintos entornos.

## Características destacadas
- Generación automática de un calendario financiero que integra transacciones, recordatorios y metas del usuario.
- Gestión completa del ciclo de vida de los ingresos, egresos, saldos, metas de ahorro y recordatorios.
- Autenticación y autorización basada en ASP.NET Core Identity con tokens JWT.
- Documentación Swagger UI disponible en entornos de desarrollo para explorar y probar la API.
- Separación clara entre entidades de dominio, DTOs y lógica de presentación para favorecer el mantenimiento y la escalabilidad.

## Tecnologías principales
- ASP.NET Core 9
- Entity Framework Core
- SQL Server
- Blazor WebAssembly
- ASP.NET Core Identity & JWT
- Swagger / OpenAPI

## Requisitos previos
- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- Instancia de SQL Server (LocalDB, Developer o una base compatible con SQL Server)
- Opcional: herramienta `dotnet-ef` instalada globalmente (`dotnet tool install --global dotnet-ef`) para ejecutar migraciones desde la terminal

## Configuración inicial
1. Clona el repositorio y ubícate en la carpeta raíz (`Spendnt/`).
2. Actualiza la cadena de conexión `DefaultConnection` en `Spendnt.API/appsettings.Development.json` (y/o `appsettings.json`) para que apunte a tu servidor SQL Server.
3. Verifica que los valores de JWT en `appsettings.Development.json` coincidan con las URLs que planeas utilizar (por defecto apuntan a los puertos de desarrollo descritos abajo).

## Migraciones y base de datos
La API incluye migraciones de Entity Framework Core y un seeder que crea datos de ejemplo. Para preparar la base de datos ejecuta:

```powershell
dotnet ef database update --project Spendnt.API/Spendnt.API.csproj --startup-project Spendnt.API/Spendnt.API.csproj
```

El comando anterior crea o actualiza la base de datos indicada en la cadena de conexión. Al arrancar la API en entorno de desarrollo se ejecuta el seeding (`SeedDB`) que crea roles, categorías predefinidas, movimientos de ejemplo y un usuario de prueba.

## Ejecución de los proyectos (solo `dotnet run`)
1. Abre una terminal en la raíz del repositorio (`Spendnt/`).
2. Inicia la API con `dotnet run`:

	```powershell
	dotnet run --project Spendnt.API/Spendnt.API.csproj
	```

	La API expone Swagger en `http://localhost:5230/swagger` mientras está en ejecución.

3. En una segunda terminal, inicia el cliente Blazor WebAssembly:

	```powershell
	dotnet run --project Spendnt.WEB/Spendnt.WEB.csproj
	```

	El cliente se conectará automáticamente a la API en los puertos por defecto.

Puertos por defecto configurados en los perfiles de desarrollo:
- API: `http://localhost:5230` (HTTPS opcional en `https://localhost:7000`).
- Cliente Blazor: `http://localhost:5047` (HTTPS opcional en `https://localhost:8000`).

## Usuario de prueba inicial
El seeder crea un usuario listo para iniciar sesión y explorar la aplicación:

- **Correo:** `testuser@example.com`
- **Contraseña:** `Password123!`
- **Rol:** `User` (los roles `User` y `Admin` se crean automáticamente).

Si necesitas un administrador, puedes promover el usuario semilla manualmente o crear uno nuevo usando las herramientas de Identity.
