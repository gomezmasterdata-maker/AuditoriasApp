Sistema de Gestión de Auditorías Internas
- Solución Full Stack para la planificación, ejecución y reporte de auditorías.

Descripción Técnica de la Solución
- La solución implementa una arquitectura desacoplada basada en servicios RESTful, separando la lógica de negocio, el acceso a datos y la presentación.

Arquitectura y Patrones
- Backend (API): ASP.NET Core 8 Web API.
- Clean Architecture (Simplificada): Separación en capas de Controladores, Servicios (Lógica de Negocio), Repositorios (Acceso a Datos) y Modelos/DTOs.
- Repository Pattern: Abstracción de la capa de persistencia para facilitar el testing y mantenimiento.
- Dependency Injection: Uso extensivo del contenedor nativo de .NET.
- Validaciones: Reglas de negocio encapsuladas en la capa de Servicios.
- Frontend (Web): ASP.NET Core Razor Pages.
- Rendering: Server-Side Rendering (SSR) complementado con JavaScript vanilla para interactividad (Modales, Gráficos).
- Estilos: Bootstrap 5 para un diseño responsivo y profesional.
- Reportes: Generación de gráficos con Chart.js
- Base de Datos: SQL Server.
- Uso de Constraints (CHECK) para integridad de datos a nivel de motor.
- Vistas SQL para agregación de datos en reportes complejos.

Tecnologías Clave
- .NET 8 SDK
- Entity Framework Core (Code-First approach con validaciones SQL)
- Swagger / OpenAPI (Documentación de API)
- GitFlow (Estrategia de ramas)

Supuestos Realizados
- Para la implementación de las reglas de negocio, se asumieron las siguientes premisas lógicas basadas en el enunciado:
- Integridad de Auditoría: Una auditoría solo puede editarse (Título, Fechas) mientras esté en estado "Pendiente". Una vez iniciada ("En Proceso"), la cabecera se bloquea para preservar la trazabilidad.
- Gestión de Hallazgos: Los hallazgos solo pueden agregarse, modificarse o eliminarse durante la ejecución ("En Proceso"). Si la auditoría está "Pendiente" (aún no inicia) o "Finalizada" (cerrada), los hallazgos son de solo lectura.
- Integridad de Responsables: No se permite editar los datos de un Responsable (Nombre, Área) si este ya tiene auditorías históricas asignadas, para evitar alterar registros pasados.
- Concurrencia: Se asume un entorno de despliegue local monousuario para la prueba, por lo que no se implementó manejo de concurrencia optimista (ETags).
- Flujo de Estados: El flujo es lineal: Pendiente -> En Proceso -> Finalizada. No se permite el retroceso de estados.

Instrucciones de Despliegue Local
Sigue estos pasos para ejecutar la solución en tu entorno local.

Prerrequisitos
- .NET 8.0 SDK instalado.
- SQL Server (Express o Developer) en ejecución.
- Git.

Paso 1: Configuración de Base de Datos
- Abra SQL Server Management Studio (SSMS).
- Ejecute el script Database/Script_Creacion_BD.sql incluido en este repositorio.
- Verifique que se creó la base de datos GestionAuditoriasDB y la vista vw_ResumenAuditoriasFinalizadas.

Paso 2: Ejecución del Backend (API)
- Navegue a la carpeta del API:
- Bash: cd AuditoriasAPI
- Abra appsettings.json y verifique la cadena de conexión. Por defecto apunta a localhost o .\SQLEXPRESS. Ajuste según su instancia.

Ejecute el proyecto:
- Bash: dotnet run
- La API iniciará (por defecto en http://localhost:5000 o similar). Tome nota del puerto.

Paso 3: Ejecución del Frontend (Web)
- Abra una nueva terminal y navegue a la carpeta Web:
- Bash:cd AuditoriasWeb
- Abra Program.cs y asegúrese de que la URL del HttpClient coincida con el puerto de la API del paso anterior:
- C#:client.BaseAddress = new Uri("http://localhost:5000/"); // Ajustar puerto si es necesario

Ejecute el proyecto:
- Bash: dotnet run
- Abra el navegador en la URL indicada (ej: http://localhost:5044).

Evidencias de Pruebas Unitarias
- Se implementaron pruebas unitarias utilizando xUnit y Moq para validar las reglas de negocio críticas en la capa de Servicios.

Cobertura Principal
- Validación de Edición: Verificar que AuditoriaService lanza excepción si se intenta editar una auditoría que no está "Pendiente".
- Validación de Borrado: Verificar que HallazgoService impide borrar hallazgos si la auditoría no está "En Proceso".

Ejecución de Pruebas
- Ejecutar el siguiente comando en la raíz de la solución:
- Bash: dotnet test
- Ver Reporte de Ejecución

Estructura del Repositorio (GitFlow)
- El proyecto sigue el flujo de trabajo GitFlow:
- main: Código estable y listo para producción (Releases).
- develop: Rama principal de integración.
- feature/*: Ramas de características específicas (ej: feature/reportes-pdf).
