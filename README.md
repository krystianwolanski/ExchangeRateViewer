# ExchangeRateViewer
Jest to API do pobierania kursów walut utworzone w .NET 5.
Do poprawnego działania aplikacji wymagane jest ustanowienie połączenia do relacyjnej bazy danych w pliku `appsettings.json` 
w sekcji `ConnectionStrings/ExchangeRateViewerDbConnection`.
Przykładowy connection string:

```json
  "ConnectionStrings": {
    "ExchangeRateViewerDbConnection": "Server=localhost\\SQLEXPRESS;Database=ExchangeRateViewerDb;Trusted_Connection=True;"
  },
```
Aplikacja do mapowania obiektowo-relacyjnego wykorzystuje `EntityFramework`.
Do utworzenia dokumentacji został użyty `Swagger API`. Definja endpointów znajduje się pod adresem `{baseUrl}/swagger`.

### Start aplikacji
Aby móc korzystać z API wystarczy ustanowić połączenie z bazą danych w pliku `appsettings.json` oraz uruchomić aplikację
korzystacjąć z profilu zdefiniowanego w `ExchangeRateViewer.API/Properties/launchSettings.json`. Utworzenie bazy danych oraz tabel zostanie zostanie wykonane automatycznie.

### Architektura
Aplikacja podzielona jest na 3 warstwy:
* `API` - warstwa prezentacji, odpowiada za komunikację z użytkownikiem. W niej znajduje się definicje endpointów oraz obsługa wyjątków pojawiających się w aplikacji.
* `Application` - warstwa, w której znajduje się cała logika aplikacji opierająca się na abstrakcjach.
* `Infrastructure` - wartwa, w której znajdują się implementację abstrakcji z warstwy `Application`.
