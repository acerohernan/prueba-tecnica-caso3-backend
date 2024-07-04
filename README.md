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

## Video Demo

https://github.com/acerohernan/prueba-tecnica-caso3-frontend/assets/73747878/8a71d4c2-506c-4c30-ba6b-787403e5117a

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

- Agrega las siguientes credenciales al appsettings.json:

```json
"ConnectionStrings": {
   "DefaultConnection": "Host=localhost;Database=mantenimiento_simple_dev;Username=mantenimiento_simple_dev_user;Password=password"
 },
 "JWT": {
   "ValidIssuer": "mantenimientosimple.api",
   "Secret": "8DNEHF3tW2nVybEZVCc1xwbNinRpX7Dc"
 }
```
 o a los user-secrets de .NET SDK para que el proyecto pueda funcionar

```bash
dotnet user-secrets init

dotnet user-secrets set "JWT:ValidIssuer" "mantenimientosimple.api"
dotnet user-secrets set "JWT:Secret" "8DNEHF3tW2nVybEZVCc1xwbNinRpX7Dc"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=mantenimiento_simple_dev;Username=mantenimiento_simple_dev_user;Password=password"

dotnet user-secrets list
### Output
JWT:ValidIssuer = mantenimientosimple.api
JWT:Secret = 8DNEHF3tW2nVybEZVCc1xwbNinRpX7Dc
ConnectionStrings:DefaultConnection = Host=localhost;Database=mantenimiento_simple_dev;Username=mantenimiento_simple_dev_user;Password=password
```

- Cuando la base de datos esté corriendo, ejecuta las migraciones del proyecto. Lo puedes hacer desde la terminal de VisualStudio

```bash
Update-Database
```

o con .NET SDK

```bash
dotnet ef database update
```

- Inicia el servidor de desarrollo con VisualStudio

![image](https://github.com/acerohernan/prueba-tecnica-caso3-backend/assets/73747878/2a310c25-d9b9-4d96-8a71-edb936cb259b)

- Para crear el usuario admin que hará login, por el momento no hay UI para este registro, pero se pretende implementar en un futuro. Hasta que no esté esa vista, podemos crear el usuario admin de la siguiente forma:
Nos vamos a la ruta de la documentación generado por Swagger

https://localhost:7202/swagger/index.html

En esa ruta encontraremos la primera ruta '/api/Auth/Register'
![image](https://github.com/acerohernan/prueba-tecnica-caso3-backend/assets/73747878/494f951c-f158-4744-a2d9-4bf4cd13672a)

Hacemos click y le damos a 'Try it out'
![image](https://github.com/acerohernan/prueba-tecnica-caso3-backend/assets/73747878/22cd859d-122a-47a1-be9b-cb1ec84a2d3f)

En el campo de texto pegamos los siguientes valores

```json
{
  "username": "admindemo",
  "email": "demo@demo.com",
  "password": "@Password123",
  "confirmPassword": "@Password123",
  "role": "Admin"
}
```
Y ejecutamos la petición. Nos debe de salir el siguiente mensaje:
![image](https://github.com/acerohernan/prueba-tecnica-caso3-backend/assets/73747878/0fca4abc-f279-485e-b43d-1843a23ef287)

- Luego de esto, ya podremos ingresar al frontend y hacer login con las credenciales creadas anteriormente

Correo: demo@demo.com
Contraseña: @Password123
![image](https://github.com/acerohernan/prueba-tecnica-caso3-backend/assets/73747878/693e8f9e-4762-4a1b-809e-fea39a1c92fc)

