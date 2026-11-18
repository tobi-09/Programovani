# Kanban tasky

Řešitelé A–E jsou zástupná jména, nahraďte je členy týmu. Sloupec Skutečně se doplňuje po dokončení.

| ID | Úkol | Story | Řešitel | Reviewer | Odhad (h) | Skutečně (h) |
|---|---|---|---|---|---|---|
| T1 | Přidání hráče | 1, 2 | Tobi | B | 2 |  |
| T2 | Výpis hráčů | 3 | Tobi | B | 1 |  |
| T3 | Odebrání hráče | 4, 5 | Tobi | B | 2 |  |
| T4 | Založení turnaje | 6, 7 | B | C | 2 |  |
| T5 | Přihlášení a odhlášení hráče | 8, 9 | B | C | 2 |  |
| T6 | Vytvoření nového kola | 10 | B | C | 1.5 |  |
| T7 | Ruční určení dvojic | 11, 12 | C | Matěj | 3 |  |
| T8 | Volný los | 13 | C | Matěj | 2 |  |
| T9 | Zadání výsledku zápasu | 14, 17 | C | Matěj | 2 |  |
| T10 | Přepsání výsledku | 15 | B | C | 1 |  |
| T11 | Uzavření kola | 16 | B | C | 1.5 |  |
| T12 | Výpis kola a jeho zápasů | 10, 14 | Matěj | Matěj | 1.5 |  |
| T13 | Výpočet tabulky | 19, 20 | Matěj | Matěj | 3 |  |
| T14 | Zobrazení tabulky | 18 | Matěj | Matěj | 1.5 |  |
| T15 | Uložení turnaje | 21 | Matěj | Tobi | 4 |  |
| T16 | Načtení turnaje | 22 | Matěj | Tobi | 5 |  |
| T17 | Chybějící soubor | 23 | Tobi | B | 1 |  |
| T18 | Poškozený soubor | 24 | Matěj | Matěj | 3 |  |
| T19 | Ošetření vstupů z klávesnice | 25 | Tobi | B | 3 |  |
| T20 | Konzolové menu | 11, 25 | C | Matěj | 3 |  |
| T21 | Oddělení logiky od výpisu | 27 | Celý tým | podle PR | 2 |  |
| T22 | Ověření tabulky ručním výpočtem | 26 | Matěj | C | 3 |  |
| T23 | Repozitář, větve Dev/Stage/Main, šablona PR | 28 | B | C | 1.5 |  |
| T24 | Kanban board a naplnění tasky | 29 | C | Matěj | 1 |  |
| T25 | Dokument User-stories.md | 30 | Tobi | B | 2 |  |
| T26 | Dokument SPECIFIKACE.md | 30 | C | Matěj | 3 |  |
| T27 | Dokument NAVRH.md – třídy | 30 | Matěj | Matěj | 3 |  |
| T28 | Návrh vlastního formátu ukládání | 30 | Matěj | Tobi | 3 |  |
| T29 | Automatické testy logiky | 26, 27 | B | C | 3 |  |
| T30 | Testování na Stage a odevzdání do Main | 26, 30 | Celý tým (vede Tobi) | podle PR | 3 |  |

## Detail tasků

### T1: Přidání hráče
- Story: 1, 2
- Řešitel: Tobi, reviewer: B
- Odhad: 2 h, skutečně: 
- Akceptační kritéria:
  - [ ] Jde zadat jméno a přezdívku.
  - [ ] Prázdné (nebo jen mezery) jméno či přezdívka se odmítne s hláškou.
  - [ ] Existující přezdívka se odmítne (bez ohledu na velikost písmen).

### T2: Výpis hráčů
- Story: 3
- Řešitel: Tobi, reviewer: B
- Odhad: 1 h, skutečně: 
- Akceptační kritéria:
  - [ ] Vypíše všechny hráče se jménem a přezdívkou.
  - [ ] Při prázdném seznamu se zobrazí srozumitelná zpráva.

### T3: Odebrání hráče
- Story: 4, 5
- Řešitel: Tobi, reviewer: B
- Odhad: 2 h, skutečně: 
- Akceptační kritéria:
  - [ ] Hráče jde odebrat podle přezdívky.
  - [ ] Neexistující přezdívka vrátí chybu.
  - [ ] Hráče s odehraným zápasem nejde odebrat a program řekne proč.
  - [ ] Hráče přihlášeného do otevřeného turnaje nejde odebrat, dokud se neodhlásí.

### T4: Založení turnaje
- Story: 6, 7
- Řešitel: B, reviewer: C
- Odhad: 2 h, skutečně: 
- Akceptační kritéria:
  - [ ] Jde zadat název, disciplínu a počet kol.
  - [ ] Počet kol musí být kladné celé číslo, jinak se zadání odmítne.
  - [ ] Při existujícím otevřeném turnaji nejde založit další.

### T5: Přihlášení a odhlášení hráče
- Story: 8, 9
- Řešitel: B, reviewer: C
- Odhad: 2 h, skutečně: 
- Akceptační kritéria:
  - [ ] Jde přihlásit jen existujícího hráče a jen jednou.
  - [ ] Odhlásit jde jen přihlášeného hráče.
  - [ ] Po začátku prvního kola obě akce skončí chybou.

### T6: Vytvoření nového kola
- Story: 10
- Řešitel: B, reviewer: C
- Odhad: 1.5 h, skutečně: 
- Akceptační kritéria:
  - [ ] Nové kolo jde vytvořit jen v otevřeném turnaji.
  - [ ] Nejde překročit zadaný počet kol.
  - [ ] Nejde vytvořit další kolo, dokud předchozí není uzavřené.
  - [ ] Vytvořením prvního kola turnaj 'začne' (zamknou se přihlášky).

### T7: Ruční určení dvojic
- Story: 11, 12
- Řešitel: C, reviewer: Matěj
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Dvojice se tvoří jen z přihlášených hráčů.
  - [ ] Hráč nejde zařadit do jednoho kola dvakrát.
  - [ ] Hráč nemůže hrát sám proti sobě.
  - [ ] Dvojice jde přidat jen do neuzavřeného kola.

### T8: Volný los
- Story: 13
- Řešitel: C, reviewer: Matěj
- Odhad: 2 h, skutečně: 
- Akceptační kritéria:
  - [ ] Hráč bez soupeře v kole je označen jako volný los (max. jeden na kolo).
  - [ ] Volný los nejde přidělit hráči, který už je v dvojici (a naopak).
  - [ ] Volný los je vidět ve výpisu kola.

### T9: Zadání výsledku zápasu
- Story: 14, 17
- Řešitel: C, reviewer: Matěj
- Odhad: 2 h, skutečně: 
- Akceptační kritéria:
  - [ ] Povolené výsledky jsou výhra 1., výhra 2. a remíza.
  - [ ] Výsledek jde zadat jen existujícímu zápasu.
  - [ ] Jiný vstup (text, číslo mimo rozsah, prázdno) se odmítne a program nespadne.

### T10: Přepsání výsledku
- Story: 15
- Řešitel: B, reviewer: C
- Odhad: 1 h, skutečně: 
- Akceptační kritéria:
  - [ ] Výsledek jde změnit, dokud kolo není uzavřené.
  - [ ] V uzavřeném kole změna skončí chybou.

### T11: Uzavření kola
- Story: 16
- Řešitel: B, reviewer: C
- Odhad: 1.5 h, skutečně: 
- Akceptační kritéria:
  - [ ] Kolo jde uzavřít, jen když mají všechny zápasy výsledek.
  - [ ] Jinak program vypíše, kolik zápasů chybí.
  - [ ] Po uzavření jde založit další kolo.

### T12: Výpis kola a jeho zápasů
- Story: 10, 14
- Řešitel: Matěj, reviewer: Matěj
- Odhad: 1.5 h, skutečně: 
- Akceptační kritéria:
  - [ ] Vypíše dvojice, výsledky a volný los daného kola.
  - [ ] Je vidět, zda je kolo uzavřené.

### T13: Výpočet tabulky
- Story: 19, 20
- Řešitel: Matěj, reviewer: Matěj
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Body se počítají podle SPECIFIKACE.md včetně volných losů.
  - [ ] Po změně výsledku se při dalším zobrazení přepočítá.
  - [ ] Řazení je podle bodů sestupně; shoda bodů = stejné pořadí, uvnitř abecedně podle přezdívky.

### T14: Zobrazení tabulky
- Story: 18
- Řešitel: Matěj, reviewer: Matěj
- Odhad: 1.5 h, skutečně: 
- Akceptační kritéria:
  - [ ] Zobrazí pořadí, hráče, body a počet odehraných zápasů.
  - [ ] Prázdný turnaj nevyvolá chybu.

### T15: Uložení turnaje
- Story: 21
- Řešitel: Matěj, reviewer: Tobi
- Odhad: 4 h, skutečně: 
- Akceptační kritéria:
  - [ ] Ukládá hráče, turnaj, přihlášky, kola, zápasy, volné losy a výsledky.
  - [ ] Používá vlastní formát z NAVRH.md bez JSON/XML knihoven.
  - [ ] Zápis jde přes dočasný soubor, takže chyba nezničí původní data.
  - [ ] Chyba při zápisu (nedostupný soubor, práva) se ošetří hláškou.

### T16: Načtení turnaje
- Story: 22
- Řešitel: Matěj, reviewer: Tobi
- Odhad: 5 h, skutečně: 
- Akceptační kritéria:
  - [ ] Po načtení jde plynule pokračovat v práci.
  - [ ] Načtený stav je shodný s uloženým (stejná tabulka, stejná kola).
  - [ ] Typ záznamu se pozná podle prvního pole řádku.

### T17: Chybějící soubor
- Story: 23
- Řešitel: Tobi, reviewer: B
- Odhad: 1 h, skutečně: 
- Akceptační kritéria:
  - [ ] Při chybějícím souboru program normálně nastartuje s prázdným stavem.
  - [ ] Uživatel dostane informativní zprávu, ne chybu.

### T18: Poškozený soubor
- Story: 24
- Řešitel: Matěj, reviewer: Matěj
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Neplatný záznam (neznámý typ, špatný počet polí, duplicita, odkaz na neexistujícího hráče) program nezhroutí.
  - [ ] Uživatel dostane srozumitelnou zprávu s číslem řádku.
  - [ ] Stávající data se nepřepíšou bez upozornění (poškozený soubor se zazálohuje).

### T19: Ošetření vstupů z klávesnice
- Story: 25
- Řešitel: Tobi, reviewer: B
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Text místo čísla, číslo mimo rozsah a prázdný vstup jsou ošetřené všude.
  - [ ] Program se po špatném vstupu vrátí k opakování zadání.
  - [ ] Existuje společná třída pro čtení vstupu (žádné přímé Console.ReadLine v menu).

### T20: Konzolové menu
- Story: 11, 25
- Řešitel: C, reviewer: Matěj
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Hlavní menu zpřístupní všechny funkce a umí je opakovat.
  - [ ] Špatná volba nezpůsobí pád.
  - [ ] Menu volá jen veřejné metody logiky, sám žádná pravidla nekontroluje.

### T21: Oddělení logiky od výpisu
- Story: 27
- Řešitel: Celý tým, reviewer: podle PR
- Odhad: 2 h, skutečně: 
- Akceptační kritéria:
  - [ ] Turnajová logika neobsahuje Console.WriteLine ani Console.ReadLine.
  - [ ] Logika vrací výsledek operace (OK / chyba + text), výpis je v samostatné části.
  - [ ] Ověřuje se při každém pull requestu (checklist v šabloně PR).

### T22: Ověření tabulky ručním výpočtem
- Story: 26
- Řešitel: Matěj, reviewer: C
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Připravený turnaj má 4–5 hráčů, min. 3 kola, remízu a volný los.
  - [ ] Ruční výpočet a výstup programu jsou v dokumentaci vedle sebe.
  - [ ] Případné rozdíly jsou vysvětlené nebo opravené.

### T23: Repozitář, větve Dev/Stage/Main, šablona PR
- Story: 28
- Řešitel: B, reviewer: C
- Odhad: 1.5 h, skutečně: 
- Akceptační kritéria:
  - [ ] Existují větve Dev, Stage, Main; Main je chráněná (merge jen při odevzdání).
  - [ ] Šablona PR obsahuje checklist: akceptační kritéria, žádné Console v logice, kdo testoval.
  - [ ] README s postupem spuštění.

### T24: Kanban board a naplnění tasky
- Story: 29
- Řešitel: C, reviewer: Matěj
- Odhad: 1 h, skutečně: 
- Akceptační kritéria:
  - [ ] Board (GitHub Projects/Trello) má sloupce Backlog, In progress, Review/Test, Done.
  - [ ] Všechny tasky mají kritéria, řešitele, odhad a odkaz na story.
  - [ ] Po dokončení se doplňuje skutečný čas.

### T25: Dokument User-stories.md
- Story: 30
- Řešitel: Tobi, reviewer: B
- Odhad: 2 h, skutečně: 
- Akceptační kritéria:
  - [ ] Obsahuje role a všechny stories ve tvaru 'Jako … chci …, abych …'.
  - [ ] Každá story má číslo a odkaz na tasky.

### T26: Dokument SPECIFIKACE.md
- Story: 30
- Řešitel: C, reviewer: Matěj
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Obsahuje slovník pojmů, bodování (výhra, remíza, volný los) a seznam funkcí.
  - [ ] Bodování je potvrzeno zákazníkem (vyučujícím).

### T27: Dokument NAVRH.md – třídy
- Story: 30
- Řešitel: Matěj, reviewer: Matěj
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Seznam tříd a co drží.
  - [ ] Popis oddělení logiky a UI.
  - [ ] Aktualizuje se při změnách architektury.

### T28: Návrh vlastního formátu ukládání
- Story: 30
- Řešitel: Matěj, reviewer: Tobi
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Popsán typ záznamu, oddělování záznamů, escapování, vyhledávání, duplicity, verze.
  - [ ] Odůvodnění formátu v NAVRH.md.
  - [ ] Schváleno týmem před začátkem T15/T16.

### T29: Automatické testy logiky
- Story: 26, 27
- Řešitel: B, reviewer: C
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Testy pokrývají bodování, dvojice, volný los, uzavření kola.
  - [ ] Testy jdou spustit jedním příkazem.

### T30: Testování na Stage a odevzdání do Main
- Story: 26, 30
- Řešitel: Celý tým (vede Tobi), reviewer: podle PR
- Odhad: 3 h, skutečně: 
- Akceptační kritéria:
  - [ ] Stage je prošlá kompletním testováním podle kritérií všech ticketů.
  - [ ] Dokumentace je aktuální a board odpovídá stavu.
  - [ ] Merge Stage → Main až při odevzdání.
