# Programovani
Repozitář pro školní programování

## Turnajový systém

Pořadatel školního turnaje eviduje hráče, ručně sestaví dvojice, zapíše výsledky a zobrazí průběžné pořadí. Rozehraný turnaj lze uložit a znovu otevřít.

### Spuštění na Windows

1. Ve Visual Studio Installer aktualizuj Visual Studio 2022 (pro .NET 8 alespoň 17.8, ideálně poslední 17.x). V **Modify** vyber **.NET desktop development** a ověř instalaci .NET 8 SDK.
2. Ve Visual Studiu zvol **Open a project or solution** a otevři `Turnaj.sln` z kořene repozitáře. Počkej na obnovu balíčků z NuGet.
3. V **Solution Explorer** klikni pravým na projekt **Turnaj → Set as Startup Project**. Zvol **Build → Build Solution** (Ctrl+Shift+B) a **Debug → Start Without Debugging** (Ctrl+F5).
4. V konzoli vybírej čísla nabídky. Volba 1 přidává hráče do evidence, 4 zakládá turnaj, 5 přihlašuje a 6 odhlašuje hráče. Po volbě 7 se přihlášky zamknou. Páry vybereš volbou 8, volný los volbou 9. Volba 10 zapisuje i opravuje výsledek, 13 ukáže tabulku a 11 uzavře kolo. Volba 16 uloží stav, 0 ukončí program.

### Příkazy z kořene repozitáře

```sh
dotnet build
dotnet test
dotnet run --project Turnaj/Turnaj.csproj
```

Pro čtení týmového příkladu spusť `dotnet run --project Turnaj/Turnaj.csproj -- Turnaj/ukazka.dat`, vyber 13 a při odchodu zvol 0 a n. Bez argumentu aplikace používá `turnaj.dat` v pracovním adresáři. Ve Visual Studiu spustíš testy přes **Test → Test Explorer → Run All Tests**.

### Struktura

| Umístění | Úloha |
|---|---|
| Turnaj.sln | Společné řešení aplikace a testů pro Visual Studio 2022 |
| Turnaj/Program.cs, Ui/ | Start, načtení stavu, konzolové menu, čtení vstupů a výpis |
| Turnaj/Model/, Logic/ | Hráči, turnaj, kola, zápasy a jejich pravidla |
| Turnaj/Storage/ | Vlastní řádkový formát pro uložení a načtení |
| Turnaj.Tests/ | xUnit testy pro .NET 8 nad skutečnou logikou aplikace |
| SPECIFIKACE.md, NAVRH.md, User-stories.md, TASKY.md | Pravidla, návrh a rozdělení práce |

### Spolupráce

Vzdálené větve mají názvy `dev`, `stage` a `main`. Větší změnu připrav na feature větvi z aktuálního `dev`. V PR do `dev` uveď task, story, kritéria a průběh testu. Reviewer odlišný od autora změnu vyzkouší. Logika pracuje s daty a vrací výsledek, výpis zajistí UI. Tým integruje větší celky z `dev` do `stage`. Po společném ověření na `stage` přichází při odevzdání PR do `main`.

Na boardu přesuň rozpracovaný task do **In progress**, při předání na kontrolu do **Review/Test**. Do **Done** patří po ověření kritérií jiným členem a začlenění přes PR podle pravidel odevzdání. Do pole **Skutečně (h)** vyplní řešitel vlastní naměřený čas.
