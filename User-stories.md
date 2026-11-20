# User stories

## Role
- **Pořadatel**: hlavní uživatel, ovládá celý turnaj.
- **Program / systém**: technické požadavky, které nejsou vázané na pořadatele.
- **Zákazník (vyučující)**: zadává a mění požadavky, potvrzuje pravidla.
- **Vývojář**: člen týmu, požadavky na kvalitu kódu a proces.

## Hráči
| # | Story | Tasky |
|---|---|---|
| 1 | Jako pořadatel chci přidat hráče se jménem a přezdívkou, abych je mohl přihlásit do turnaje. | T1 |
| 2 | Jako pořadatel chci, aby byla přezdívka jedinečná, abych hráče vždy jednoznačně rozlišil. | T1 |
| 3 | Jako pořadatel chci vypsat všechny hráče, abych viděl, koho mám k dispozici. | T2 |
| 4 | Jako pořadatel chci odebrat hráče, abych mohl opravit chybně zadané záznamy. | T3 |
| 5 | Jako pořadatel chci, aby nešel smazat hráč s odehraným zápasem, aby se nezkreslila historie a tabulka. | T3 |

## Turnaj
| # | Story | Tasky |
|---|---|---|
| 6 | Jako pořadatel chci založit turnaj s názvem, disciplínou a počtem kol, abych mohl začít organizovat soutěž. | T4 |
| 7 | Jako pořadatel chci mít otevřený vždy jen jeden turnaj, abych se nepletl mezi více soutěžemi. | T4 |
| 8 | Jako pořadatel chci přihlašovat a odhlašovat hráče do začátku prvního kola, abych mohl upravit soupisku. | T5 |
| 9 | Jako pořadatel chci, aby po začátku prvního kola nešlo měnit přihlášené hráče, aby turnaj zůstal konzistentní. | T5, T6 |

## Kola a zápasy
| # | Story | Tasky |
|---|---|---|
| 10 | Jako pořadatel chci vytvořit nové kolo, abych turnaj posunul dál. | T6, T12 |
| 11 | Jako pořadatel chci ručně určit dvojice z přihlášených hráčů, abych kontroloval, kdo s kým hraje (losování zatím dělám já). | T7, T20 |
| 12 | Jako pořadatel chci, aby hráč mohl být v kole nejvýše jednou, abych nezadal nesmyslné dvojice. | T7 |
| 13 | Jako pořadatel chci, aby hráč bez soupeře dostal volný los, aby nebyl znevýhodněn lichým počtem hráčů. | T8 |
| 14 | Jako pořadatel chci zadat výsledek zápasu (výhra prvního, výhra druhého, remíza), abych zaznamenal průběh turnaje. | T9, T12 |
| 15 | Jako pořadatel chci přepsat výsledek, dokud kolo není uzavřené, abych opravil překlep. | T10 |
| 16 | Jako pořadatel chci uzavřít kolo, až mají všechny zápasy výsledek, aby byla data úplná. | T11 |
| 17 | Jako pořadatel chci, aby nešlo zadat výsledek neexistujícího zápasu, abych nepoškodil data. | T9 |

## Tabulka
| # | Story | Tasky |
|---|---|---|
| 18 | Jako pořadatel chci zobrazit tabulku s pořadím, hráčem, body a počtem odehraných zápasů, abych viděl stav turnaje. | T14 |
| 19 | Jako pořadatel chci, aby se tabulka po zadání výsledků přepočítala, abych měl vždy aktuální pořadí. | T13 |
| 20 | Jako pořadatel chci, aby se do bodů počítaly i volné losy, aby tabulka byla spravedlivá. | T13 |

## Uložení a načtení
| # | Story | Tasky |
|---|---|---|
| 21 | Jako pořadatel chci uložit rozdělaný turnaj (hráči, kola, zápasy, výsledky), abych mohl program vypnout. | T15, T28 |
| 22 | Jako pořadatel chci při dalším spuštění načíst turnaj a pokračovat, kde jsem skončil. | T16 |
| 23 | Jako pořadatel chci, aby program zvládl chybějící soubor, abych se nedostal do chyby při prvním spuštění. | T17 |
| 24 | Jako pořadatel chci, aby program zvládl poškozený soubor, abych nepřišel o kontrolu nad programem. | T18 |

## Odolnost
| # | Story | Tasky |
|---|---|---|
| 25 | Jako pořadatel chci, aby program ošetřil špatný vstup (text místo čísla, číslo mimo rozsah, prázdný vstup), aby nespadl. | T19, T20 |

## Ověření, kvalita a proces
| # | Story | Tasky |
|---|---|---|
| 26 | Jako zákazník chci doložit, že tabulka odpovídá ručnímu výpočtu, abych programu věřil. | T22, T29, T30 |
| 27 | Jako vývojář chci oddělit logiku od výpisu, aby šla logika testovat a měnit nezávisle. | T21, T29 |
| 28 | Jako vývojář chci mít větve Dev, Stage a Main a pull requesty s kontrolou jiným členem, aby byla práce ověřená a odevzdání přehledné. | T23 |
| 29 | Jako člen týmu chci mít kanban board odpovídající skutečnému stavu a se skutečnými časy, abychom mohli porovnat odhad a realitu. | T24 |
| 30 | Jako tým chceme mít aktuální dokumentaci (user stories, specifikace, návrh), abychom při změně zadání věděli, co upravit. | T25–T28, T30 |

## Otevřené otázky pro zákazníka
1. Kolik bodů za výhru, remízu a volný los? (návrh ve SPECIFIKACE.md)
2. Jak se turnaj ukončí a jak se otevře další? Zadání říká "jen jeden otevřený", ale nic o ukončení.
3. Je hráč s "odehraným zápasem" ten, kdo má zadaný výsledek, nebo už ten, kdo je v dvojici?
4. Smí mít hráč volný los v turnaji víckrát?
5. Hraje se do konce všech kol, nebo jde turnaj ukončit dřív?
