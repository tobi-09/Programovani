# Návrh

## Architektura
Tři vrstvy. Logika nezná konzoli, vrací výsledky operací.

| Vrstva | Třídy |
|---|---|
| Model + logika | Player, Tournament, Round, Match, MatchResult, StandingRow, PlayerRegistry, TournamentService, StandingsCalculator, OperationResult |
| Ukládání | TournamentFileStore (zápis a čtení), LineCodec (escapování) |
| UI | ConsoleMenu, InputReader, ConsoleView |

## Třídy a co drží
- **Player**: Name, Nickname, PlayedInPastTournament.
- **PlayerRegistry**: Dictionary<string, Player> podle přezdívky (klíč v malých písmenech). Přidat, odebrat, najít, vypsat.
- **MatchResult** (enum): None, FirstWins, SecondWins, Draw.
- **Match**: FirstNick, SecondNick, Result.
- **Round**: Number, List<Match>, ByeNick (nebo null), IsClosed. Kontroluje, že hráč je v kole jen jednou.
- **Tournament**: Name, Discipline, RoundCount, HashSet<string> přihlášených přezdívek, List<Round>, vlastnost Started (je alespoň jedno kolo).
- **StandingRow**: Rank, Nickname, Points, Played.
- **StandingsCalculator**: z Tournament spočítá List<StandingRow> podle SPECIFIKACE.md. Tabulka se neukládá, vždy se počítá znovu.
- **OperationResult**: Ok, Message. Každá akce logiky ho vrací místo výjimky nebo výpisu.
- **TournamentService**: veřejné metody pro všechny funkce, drží PlayerRegistry a aktuální Tournament.
- **InputReader**: čtení čísla v rozsahu, neprázdného textu, volby; opakuje dotaz při chybě.
- **ConsoleMenu / ConsoleView**: jediné místo s Console.

## Vlastní formát ukládání (textový, řádkový)
Soubor `turnaj.dat`, UTF-8. **Jeden záznam = jeden řádek**, pole odděleno znakem `|`. **Typ záznamu je první pole**.

```
TURNAJ-DAT|2
P|jana|Jana Nováková|0
P|petr|Petr Dvořák|0
P|karel|Karel Novák|0
T|Jarní turnaj|šachy|3|1
E|jana
E|petr
E|karel
R|1|1
M|1|jana|petr|1
B|1|karel
END|10
```

| Typ | Pole | Význam |
|---|---|---|
| TURNAJ-DAT | verze | hlavička, musí být první řádek |
| P | přezdívka, jméno, odehrál zápas v předchozím turnaji (0/1) | hráč |
| T | název, disciplína, počet kol, otevřený (0/1) | turnaj, nejvýše jeden |
| E | přezdívka | přihlášený hráč |
| R | číslo kola, uzavřeno (0/1) | kolo |
| M | číslo kola, přezdívka 1, přezdívka 2, výsledek (0 nic, 1, 2, D) | zápas |
| B | číslo kola, přezdívka | volný los |
| END | počet záznamů | konec, kontrola úplnosti |

Program čte i verzi 1, kde záznam P obsahuje jen přezdívku a jméno. Při založení dalšího turnaje zapamatuje účastníky odehraných zápasů. Hráče na soupisce právě uloženého turnaje chrání i při volném losu.

### Escapování
V textových polích se `\` zapisuje `\\`, `|` jako `\p`, nový řádek jako `\n`. Díky tomu text s oddělovačem nerozbije záznam.

### Načítání
1. Čte se po řádcích, `Split` podle neescapovaného `|`, typ určí `switch` na prvním poli.
2. Hlavička a verze se zkontrolují jako první, `END` s počtem záznamů jako poslední (detekce useknutého souboru).
3. Hráči se při čtení ukládají do slovníku podle přezdívky: vyhledání je O(1) a duplicita se pozná okamžitě.
4. Odkazy (E, M, B) se ověřují vůči slovníku hráčů. Neznámá přezdívka, duplicitní P, špatný počet polí, neznámý typ, zápas v neexistujícím kole = poškozený soubor, chybová hláška s číslem řádku.

### Duplicity
Duplicitní přezdívka (P), duplicitní kolo (R se stejným číslem), hráč dvakrát v jednom kole nebo duplicitní E se považují za poškození. Načtení se přeruší a stávající stav v paměti se nepřepíše.

### Bezpečný zápis
Zapíše se do `turnaj.dat.tmp`, po úspěchu se přesune přes `turnaj.dat`. Poškozený soubor se při nahrazení přejmenuje na `turnaj.dat.bad`.

### Chybějící a poškozený soubor
- Chybí: start s prázdným stavem.
- Poškozený: zpráva s důvodem, start s prázdným stavem, soubor se nemaže ani nepřepisuje bez potvrzení.

### Odůvodnění
Řádkový formát je čitelný člověkem, jde ho ladit v textovém editoru, nevyžaduje stromový parser a hodí se na zadané omezení (jen soubory a řetězce). Typ na začátku řádku usnadní přidání nových záznamů při změně zadání (stačí nový typ a verze).

## Rozšiřitelnost
Změny pravidel bodování jsou jen v StandingsCalculator (konstanty). Nové typy záznamů se přidají do čtečky bez změny existujících.
