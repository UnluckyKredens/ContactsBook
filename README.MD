# ContactsBook

## Opis projektu

Projekt składa się z dwóch części:

-  **ContactsBookAPI** – .NET Core + EF Core + SQL Server + MediatR,

-  **ContactsBookSPA** – Angular + Angular Material.

Aplikacja pozwala tworzyć, edytować, usuwać i przeglądać kontakty z paginacją oraz filtrowaniem po emailu.

## Funkcje

- Dodawanie kontaktu. `POST /add`

- Edycja kontaktu. `PUT /update`

- Usuwanie kontaktu. `DELETE /delete?id={id}`

- Lista kontaktów z paginacją. `GET /list?pageNumber=1&pageSize=15&search=`

- Wyszukiwanie (po emailu).

- Walidacja formularzy po stronie frontendu.

## Stack technologiczny

### Backend

- .NET / ASP.NET Core Web API

- MediatR (CQRS)

- Entity Framework Core

- SQL Server

- Swagger 

### Frontend

- Angular (standalone components)

- Angular Material

- ngx-toastr

## Struktura repozytorium

```text

ContactsBook/

├── ContactsBookAPI/

│ ├── ContactsBookAPI/ # warstwa Web API

│ ├── ContactsBookAPI.Application/ # komendy, query, handlery

│ ├── ContactsBookAPI.Domain/ # encje, wyjątki domenowe

│ └── ContactsBookAPI.Infrastructure/ # EF Core, repozytoria, migracje

│ └── ContactsBookSPA/ # Angular

│ └── example_db.bak # Przykładowa baza danych

│ └── SQL_insert_records.sql # Skrypt zapełniający baze przykładowymi rekordami



```


## Startup

### 1) Baza Danych

1. Znajdź plik:

-  `ContactsBookAPI/ContactsBookAPI/appsettings.json`

2. Ustaw personalny connection string w `ConnectionStrings:DefaultConnection`.

Zamień `DB_USER` i `DB_PASSWD` na własne wartości

```json

"ConnectionStrings": {

"DefaultConnection": "Server=localhost;Database=ContactsBook;User Id=DB_USER;Password=DB_PASSWD;TrustServerCertificate=True;"

}

```

**Możesz zaimportować gotową baze danych z przykładowymi rekordami lub stworzyć własną (kroki niżej)** 


 Zastosuj migracje (z katalogu `ContactsBookAPI`):

```bash

dotnet ef database update --project ContactsBookAPI.Infrastructure --startup-project ContactsBookAPI

```

Przy utworzeniu nowej migracji użyj: 
```
dotnet ef migrations add NAZWA_MIGRACJI --project ContactsBookAPI.Infrastructure --startup-project ContactsBookAPI --output-dir Migrations
```



### 2) Uruchomienie backendu

Z katalogu `ContactsBookAPI`:

```bash

dotnet restore
dotnet run --project ContactsBookAPI

```

Domyślne adresy (profil `https`):

-  `https://localhost:7236`

-  `http://localhost:5177`

Swagger:

-  `https://localhost:7236/swagger`

### 3) Uruchomienie frontendu

Z katalogu `ContactsBookSPA`:

```bash

npm install
npm start

```

Domyślnie frontend korzysta z:

-  `https://localhost:4200`
