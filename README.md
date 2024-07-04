# Evaluación Técnica – Developer .NET - Caso 03 - Backend

## Indicaciones

### Proyecto: Mantenimiento simple

- Crear una ventana de login y un mantenimiento de usuarios
- El mantenimiento de usuarios debe tener al menos 3 tipos diferentes de campos
  (Ejemplos: Texto, password, selector de fecha, select, multi select, checkboxes, etc).
- El mantenimiento debe permitir crear, editar y eliminar usuarios.
- Los usuarios creados en el mantenimiento deben poder hacer uso del login.
- Los servicios del mantenimiento deben tener seguridad.

Al finalizar debe entregarse:

- Código Frontend / Backend en .zip
- Scripts de BD (de requerirse)
- Usuario/Password de prueba
- Otra información necesaria para levantar el sistema

### Video demo

https://github.com/acerohernan/prueba-tecnica-caso3-backend/assets/73747878/8cfb908f-82f0-409a-9494-25ec548b07e2

## Guía para levantar el sistema completo (Frontend + Backend)

### Frontend

- Clona el repositorio de código

```bash
git clone https://github.com/acerohernan/prueba-tecnica-caso3-frontend
```

- Instala las dependencias

```bash
yarn install
```

- Crear el archivo .env basado en el .env.example

```bash
cp .env.example .env
```

- Agrega las varibles de entorno

```bash
  VITE_API_URL = https://localhost:7202/api
```

- Inicia el servidor de desarrollo. (asegurate de que se inicie en el puerto 3000, de otra forma los CORS del backend no le permitirán hacer las peticiones)

```bash
  yarn dev
```

### Backend

- Clona el repositorio de código

```bash
git clone https://github.com/acerohernan/prueba-tecnica-caso3-backend
```

- Ingresa a la carpeta 'MantenimientoSimple.Api'

```bash
cd ./MantenimientoSimple.Api
```

- Antes de iniciar el entorno de docker, crearemos un archivo '.env' con los siguientes datos:

```bash
POSTGRES_USER=mantenimiento_simple_dev_user
POSTGRES_PASSWORD=password
POSTGRES_DB=mantenimiento_simple_dev
```

- Inicia el entorno de Docker. Este levantará la base de datos Postgres con la que trabajará el backend.

```bash
docker compose up -d
```

- Cuando la base de datos esté corriendo, ejecuta las migraciones del proyecto. Lo puedes hacer desde la terminal de VisualStudio

```bash
Update-Database
```

o con .NET SDK

```bash
dotnet ef database update
```

- Agrega las credenciales al appsettings.json o a los user-secrets de .NET SDK para que el proyecto pueda funcionar

```bash
dotnet user-secrets list
### Output
JWT:ValidIssuer = mantenimientosimple.api
JWT:Secret = 8DNEHF3tW2nVybEZVCc1xwbNinRpX7Dc
ConnectionStrings:DefaultConnection = Host=localhost;Database=mantenimiento_simple_dev;Username=mantenimiento_simple_dev_user;Password=password
```

- Después de este paso, inicia el servidor de desarrollo y el sistema estará listo para ser probado
