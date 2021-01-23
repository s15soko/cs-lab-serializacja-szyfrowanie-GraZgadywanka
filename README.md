# Gra w "Za dużo, za mało" - dodanie funkcjonalności

* Autor: _Krzysztof Molenda_
* Wersja: 2019-12-10

Celem ćwiczenia jest nabycie umiejętności w czytaniu (ze zrozumieniem) obcego kodu, wprowadzanie w nim korekt z zachowaniem istniejącej funkcjonalności oraz dodanie nowych funkcjonalności.

Głównym zadaniem jest zastosowanie **mechanizmów serializacji do pliku** stanu aplikacji, w celu późniejszego odtworzenia przy ponownym uruchomieniu.

Po wykonaniu ćwiczenia powinieneś:

* umieć przeprowadzić serializację i deserializację binarną obiektu i kolekcji obiektów,
* umieć przeprowadzić serializację i deserializację _via_ `DataContract` obiektu i kolekcji obiektów do XML
* umieć zaszyfrować i odszyfrować informacje.

⚠️ ZASTRZEŻENIE: serializacja binarna jest aktualnie **silnie niezalecana** ze względów bezpieczeństwa kodu ([BinaryFormatter security guide](https://docs.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-security-guide)). W ramach niniejszego ćwiczenia wykonywana jest wyłącznie w celach edukacyjnych.

---

## Problem

W załączonych plikach zawarto realizację konsolową (jednowątkową) prostej gry w "Za dużo, za mało", tworzonej na kursie podstawowym C#. Aplikację zrealizowano wzorując się na wzorcu MVP (Model-Widok-Prezenter):

* plik `Gra.cs` - opisuje logikę gry (model)
* plik `KontrolerCLI.cs` - zapewnia łączność między modelem a widokiem
* plik `WidokCLI.cs` - dostarcza funkcjonalność związaną z obsługą konsoli.

**Zadanie**: Skompiluj, uruchom aplikację, zapoznaj się z jej działaniem. Zapoznaj się z kodem aplikacji i jej architekturą.

Aplikacja jeszcze nie jest skończona. O ile, ogólnie, działa poprawnie, o tyle funkcjonalność zakończenia jej w dowolnym momencie (wybranie `X`) nie została jeszcze zaimplementowana.

## Zadanie

Wprowadź poprawki do kodu.

Dostarcz funkcjonalność przerwania działania aplikacji (wybranie `X`) działające według następującego scenariusza:

1. Użytkownik kończy działanie aplikacji - poddaje się - odpowiadając na stosowne pytanie wyborem `X` lub `x`.
2. Stan aplikacji zostaje zapamiętany na dysku, w pliku zlokalizowanym w folderze, w którym znajduje się program uruchomieniowy aplikacji.
3. Aplikacja kończy działanie.
4. Przy ponownym uruchomieniu aplikacji, wykrywa ona istnienie pliku, próbuje go odczytać.
5. Jeśli odczyt jest poprawny, aplikacja komunikuje użytkownikowi, że jest możliwe odtworzenie stanu z poprzedniego uruchomienia, wyświetlając sumaryczne informacje dotyczące przerwanej rozgrywki (oczywiście bez poszukiwanej wartości).
6. Jeśli użytkownik zdecyduje się na kontynuowanie poprzedniej rozgrywki, stan aplikacji sprzed zamknięcia zostaje przywrócony i rozgrywka toczy się dalej. Plik z zapamiętanym stanem poprzedniej rozgrywki zostaje usunięty.
7. Jeśli użytkownik chce rozpocząć rozgrywkę od nowa, plik ze stanem gry zostaje usunięty i uruchamiana jest nowa gra.

Wprowadzone poprawki **muszą** być odporne na pojawienie się ewentualnych błędów (brak pliku, nie można zapisać pliku, plik uszkodzony, zapis stanu gry niewłaściwy, ...). Wprowadź odpowiednie przechwytywanie wyjątków i reakcje na nie, ale tak, aby nie kończyć działania aplikacji.

Skoryguj wyświetlanie historii gry. Wprowadź nowy stan gry `Zawieszona`. Skoryguj obliczanie czasu trwania rozgrywki (nie uwzględnianie czasu jej zawieszenia).

Zadanie wykonaj w dwóch wariantach:

1. Wykorzystaj serializację binarną
2. Wykorzystaj serializację do XML _via_ `DataContract`.

W tym drugim przypadku zapewnij odpowiedni poziom "tajności" zapisanego pliku poprzez zaszyfrowanie albo całego pliku, albo przynajmniej wartości odgadywanej. Ręczne poprawki wprowadzone do pliku XML mogłyby zafałszować kontynuowaną rozgrywkę.

Jeśli chcesz, dokonaj poprawek kosmetycznych kodu, wykonaj jego refaktoryzację według własnego punktu widzenia (np. stosując LINQ w niektórych sytuacjach), utrzymując ogólną funkcjonalność aplikacji.

### Zadanie dodatkowe

Zmodyfikuj działanie aplikacji tak, aby co ustaloną liczbę sekund aplikacja automatycznie zrzucała swój stan do pliku. Zbudujesz wtedy automatyczny backup danych aplikacji. Nawet przerwanie awaryjne (Ctrl-C) nie pozbawi użytkownika możliwości kontynuowania tej niezmiernie ciekawej gry.

Podpowiedź: wielowątkowość.

## Referencje

1. [Binary serialization](https://docs.microsoft.com/en-us/dotnet/standard/serialization/binary-serialization)
2. [C# Serialization & Deserialization with Example](https://www.guru99.com/c-sharp-serialization.html)
3. [Walkthrough: persisting an object using C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/walkthrough-persisting-an-object-in-visual-studio)
4. [WCF: Serialization and Deserialization](https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/serialization-and-deserialization)
5. [Serialization guidelines](https://docs.microsoft.com/en-us/dotnet/standard/serialization/serialization-guidelines)
6. [.NET Serializers Comparison Chart](https://manski.net/2014/10/net-serializers-comparison-chart/)
7. [XML serialization in depth](https://docs.microsoft.com/en-us/dotnet/standard/serialization/introducing-xml-serialization)
8. [Cryptographic Services](https://docs.microsoft.com/en-us/dotnet/standard/security/cryptographic-services)
9. [How to: Encrypt XML Elements with Symmetric Keys](https://docs.microsoft.com/pl-pl/dotnet/standard/security/how-to-encrypt-xml-elements-with-symmetric-keys)
