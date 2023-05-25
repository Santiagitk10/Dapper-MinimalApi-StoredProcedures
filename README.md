# Dapper-MinimalApi-StoredProcedures


Parte 1

https://www.youtube.com/watch?v=dwMFg6uxQ0I

Para convertir la clase de block scoped a file scoped seleccionar la clase y ctrl .

Paquetes a instalar para usar Dapper

- Dapper
- System.Data.SqlClient
- Microsoft.Extensions.Configuration.Abstractions


Lo bueno del diseño del proyecto:

- Ningún proyecto tiene que saber de Sql ni de Dapper. Si se requiere cambiar la Base de datos simplemente se modifica SqlDataAccess. Gracias a la Injección de Dependencias (Extraimos todas las clases a interfaces).
- Si se tuvieran más tablas simplemente se hacen más clases para cada tabla en Data ya que la clase SqlDataAccess es genérica
